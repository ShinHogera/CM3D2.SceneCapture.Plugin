using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Xml;

namespace CM3D2.SceneCapture.Plugin
{
    public class MenuInfo
    {
        public MenuInfo() { }

        public string menuName { get; set; }
        public string menuNameInColorSet { get; set; }
        public string menuInfo { get; set; }
        public string modelName { get; set; }
        public bool delOnly { get; set; }
        public bool isMan { get; set; }
        public float priority { get; set; }
        public string iconTextureName { get; set; }
        public MPN partCategory { get; set; }
        public MPN colorSetCategory { get; set; }
        public MaidParts.PARTS_COLOR partsColor { get; set; }
    }

    public static class AssetLoader
    {

        public static Texture2D LoadTexture(string textureName)
        {
            Texture2D texture = null;

            if (textureName != null)
            {
                if (textureName != string.Empty)
                {
                    try
                    {
                        texture = ImportCM.CreateTexture(textureName);
                    }
                    catch
                    {
                        try
                        {
                            textureName = textureName.Replace(@"tex\", "");
                            texture = ImportCM.CreateTexture(textureName);
                        }
                        catch (Exception ex)
                        {
                            UnityEngine.Debug.LogError("Error: " + ex.ToString());
                        }
                    }
                }
            }

            return texture;
        }

        private static string readCom(BinaryReader binaryReader)
        {
            string sss;
            do
            {
                int num3 = (int) binaryReader.ReadByte();
                // str2 = sss;
                sss = string.Empty;
                if (num3 != 0)
                {
                    for (int index = 0; index < num3; ++index)
                        sss = sss + "\"" + binaryReader.ReadString() + "\" ";
                }
                else
                    return null;
            }
            while (sss == string.Empty);
            return sss;
        }

        private static bool readMenuProp(string stringCom, string[] stringList, ref MaidParts.PARTS_COLOR partsColor, ref MenuInfo mi)
        {
            if (stringCom == "name")
            {
                string str3 = stringList[1];
                string empty2 = string.Empty;
                string empty3 = string.Empty;

                int index;
                for (index = 0; index < str3.Length && (int) str3[index] != 12288 && (int) str3[index] != 32; ++index)
                    empty2 += str3[index];
                for (; index < str3.Length; ++index)
                    empty3 += str3[index];
                mi.menuName = empty2;
            }
            else if (stringCom == "setumei")
            {
                mi.menuInfo = stringList[1];
                mi.menuInfo = mi.menuInfo.Replace("《改行》", "\n");
            }
            else if (stringCom == "category")
            {
                string lower = stringList[1].ToLower();
                try
                {
                    mi.partCategory = (MPN) Enum.Parse(typeof (MPN), lower);
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("カテゴリがありません。" + stringList[1]);
                    mi.partCategory = MPN.null_mpn;
                }
            }
            else if (stringCom == "color_set")
            {
                try
                {
                    mi.colorSetCategory = (MPN) Enum.Parse(typeof (MPN), stringList[1].ToLower());
                }
                catch
                {
                    UnityEngine.Debug.LogWarning((object) ("カテゴリがありません。" + stringList[1].ToLower()));
                }
                if (stringList.Length >= 3)
                    mi.menuNameInColorSet = stringList[2].ToLower();
            }
            else if (stringCom == "tex" || stringCom == "テクスチャ変更")
                partsColor = MaidParts.PARTS_COLOR.NONE;
            else
                return true;

            return false;
        }

        public static MenuInfo LoadMenu(string menuFileName)
        {
            AFileBase file = GameUty.FileOpen(menuFileName);

            if( !file.IsValid() || file.GetSize() == 0 )
            {
                string name = menuFileName.Replace(@"menu\", "");
                file = GameUty.FileOpen(name);
                if( file.GetSize() == 0 || !file.IsValid() )
                {
                    name = menuFileName.Replace(@"man\", "");
                    file = GameUty.FileOpen(name);
                    if( file.GetSize() == 0 || !file.IsValid() )
                    {
                        throw new FileNotFoundException(name);
                    }
                }
            }

            MenuInfo mi = new MenuInfo();

            BinaryReader binaryReader = new BinaryReader(new MemoryStream(file.ReadAll()), System.Text.Encoding.UTF8);

            string str1 = binaryReader.ReadString();
            NDebug.Assert(str1 == "CM3D2_MENU", "ProcScriptBin 例外 : ヘッダーファイルが不正です。" + str1);
            binaryReader.ReadInt32();
            string path = binaryReader.ReadString();

            binaryReader.ReadString();
            binaryReader.ReadString();
            binaryReader.ReadString();

            long num1 = (long) binaryReader.ReadInt32();

            string sss = string.Empty;
            string str2 = string.Empty;
            string empty1 = string.Empty;
            try
            {
                while (true)
                {
                    string stringCom;
                    string[] stringList;
                    do
                    {
                        do
                        {
                            do
                            {
                                MaidParts.PARTS_COLOR partsColor = MaidParts.PARTS_COLOR.NONE;
                                do
                                {
                                    sss = readCom(binaryReader);
                                    if (sss == null)
                                    {
                                        goto label_61;
                                    }

                                    stringCom = UTY.GetStringCom(sss);
                                    stringList = UTY.GetStringList(sss);

                                    if (readMenuProp(stringCom, stringList, ref partsColor, ref mi) == true)
                                    {
                                        goto label_40;
                                    }
                                }
                                while (stringList.Length != 6);

                                string str4 = stringList[5];
                                try
                                {
                                    partsColor = (MaidParts.PARTS_COLOR) Enum.Parse(typeof (MaidParts.PARTS_COLOR), str4.ToUpper());
                                }
                                catch
                                {
                                    NDebug.Assert("無限色IDがありません。" + str4);
                                }
                                mi.partsColor = partsColor;
                                continue;

                            label_40:
                                if (stringCom == "icon" || stringCom == "icons")
                                    mi.iconTextureName = stringList[1];
                            }
                            while (stringCom == "iconl" || stringCom == "setstr" || stringCom == "アイテムパラメータ");

                            if (stringCom == "saveitem")
                            {
                                string str3 = stringList[1];
                                if (str3 == string.Empty)
                                    UnityEngine.Debug.LogError( "err SaveItem \"" + str3);
                                if (str3 == null)
                                    UnityEngine.Debug.LogError( "err SaveItem null=\"" + str3);
                            }
                        }
                        while (stringCom == "catno");

                        if (stringCom == "additem")
                            mi.modelName = stringList[1];

                        else if (stringCom == "delitem" || stringCom == "unsetitem")
                            mi.delOnly = true;

                        else if (stringCom == "priority")
                            mi.priority = float.Parse(stringList[1]);
                    }
                    while (!(stringCom == "メニューフォルダ") || !(stringList[1].ToLower() == "man"));
                    mi.isMan = true;
                }
            }
            catch
            {
                UnityEngine.Debug.LogError("Failed to parse menu file " + Path.GetFileName(path));
                // UnityEngine.Debug.LogError(("Exception " + Path.GetFileName(path) + " 現在処理中だった行 = " + sss + " 以前の行 = " + str2 + "   " + ex.Message + "StackTrace：\n" + ex.StackTrace));
                throw;
            }
        label_61:

            binaryReader.Close();
            return mi;
        }

        public static GameObject LoadMesh(string modelName) {
            AFileBase file = GameUty.FileOpen(modelName);

            if( !file.IsValid() || file.GetSize() == 0 )
            {
                string name = modelName.Replace(@"model\", "");
                file = GameUty.FileOpen(name);
                if( file.GetSize() == 0 || !file.IsValid() )
                {
                    Debug.LogError("File not valid");
                    return null;
                }
            }

            using (BinaryReader r = new BinaryReader(new MemoryStream(file.ReadAll())) )
            {
                int layer = 0;
                // TBodySkin.OriVert oriVert = bodyskin.m_OriVert;
                GameObject gameObject1 = UnityEngine.Object.Instantiate(Resources.Load("seed")) as GameObject;
                gameObject1.layer = layer;
                GameObject gameObject2 = (GameObject) null;
                Hashtable hashtable = new Hashtable();

                string str1 = r.ReadString();
                if (str1 != "CM3D2_MESH") {
                    return null;
                }
                r.ReadInt32();
                string str2 = r.ReadString();
                string str3 = r.ReadString();
                int num = r.ReadInt32();
                List<GameObject> gameObjectList = new List<GameObject>();
                for (int index = 0; index < num; ++index)
                {
                    GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("seed")) as GameObject;
                    gameObject3.layer = layer;
                    gameObject3.name = r.ReadString();
                    gameObjectList.Add(gameObject3);
                    if (gameObject3.name == str3)
                        gameObject2 = gameObject3;
                    hashtable[(object) gameObject3.name] = (object) gameObject3;
                    if ((int) r.ReadByte() != 0)
                    {
                        GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load("seed")) as GameObject;
                        gameObject4.name = gameObject3.name + "_SCL_";
                        gameObject4.transform.parent = gameObject3.transform;
                        hashtable[(object) (gameObject3.name + "&_SCL_")] = (object) gameObject4;
                    }
                }
                for (int index1 = 0; index1 < num; ++index1)
                {
                    int index2 = r.ReadInt32();
                    gameObjectList[index1].transform.parent = index2 < 0 ? gameObject1.transform : gameObjectList[index2].transform;
                }
                for (int index = 0; index < num; ++index)
                {
                    Transform transform = gameObjectList[index].transform;
                    float x1 = r.ReadSingle();
                    float y1 = r.ReadSingle();
                    float z1 = r.ReadSingle();
                    transform.localPosition = new Vector3(x1, y1, z1);
                    float x2 = r.ReadSingle();
                    float y2 = r.ReadSingle();
                    float z2 = r.ReadSingle();
                    float w = r.ReadSingle();
                    transform.localRotation = new Quaternion(x2, y2, z2, w);
                }
                int length1 = r.ReadInt32();
                int length2 = r.ReadInt32();
                int length3 = r.ReadInt32();
                // oriVert.VCount = length1;
                // oriVert.nSubMeshCount = length2;
                gameObject2.AddComponent(typeof (SkinnedMeshRenderer));
                gameObject1.AddComponent(typeof (Animation));
                SkinnedMeshRenderer component = gameObject2.GetComponent<Renderer>() as SkinnedMeshRenderer;
                component.updateWhenOffscreen = true;

                // if (!bodyskin.body.boMAN)
                // {
                //     if (slotname == "head")
                //         component.castShadows = false;
                //     if (bodyskin.Category == "chikubi")
                //         component.castShadows = false;
                //     if (bodyskin.Category.IndexOf("seieki_") == 0)
                //         component.castShadows = false;
                // }

                // bodyskin.listDEL.Add((UnityEngine.Object) gameObject2);
                Transform[] transformArray = new Transform[length3];
                for (int index = 0; index < length3; ++index)
                {
                    string str4 = r.ReadString();
                    if (!hashtable.ContainsKey((object) str4))
                    {
                        Debug.LogError((object) ("nullbone= " + str4));
                    }
                    else
                    {
                        GameObject gameObject3 = !hashtable.ContainsKey((object) (str4 + "&_SCL_")) ? (GameObject) hashtable[(object) str4] : (GameObject) hashtable[(object) (str4 + "&_SCL_")];
                        transformArray[index] = gameObject3.transform;
                    }
                }
                component.bones = transformArray;
                Mesh mesh1 = new Mesh();
                component.sharedMesh = mesh1;
                Mesh mesh2 = mesh1;
                // bodyskin.listDEL.Add((UnityEngine.Object) mesh2);
                Matrix4x4[] matrix4x4Array = new Matrix4x4[length3];
                for (int index1 = 0; index1 < length3; ++index1)
                {
                    for (int index2 = 0; index2 < 16; ++index2)
                        matrix4x4Array[index1][index2] = r.ReadSingle();
                }
                mesh2.bindposes = matrix4x4Array;
                Vector3[] vector3Array1 = new Vector3[length1];
                Vector3[] vector3Array2 = new Vector3[length1];
                Vector2[] vector2Array = new Vector2[length1];
                BoneWeight[] boneWeightArray = new BoneWeight[length1];
                for (int index = 0; index < length1; ++index)
                {
                    float new_x1 = r.ReadSingle();
                    float new_y1 = r.ReadSingle();
                    float new_z1 = r.ReadSingle();
                    vector3Array1[index].Set(new_x1, new_y1, new_z1);
                    float new_x2 = r.ReadSingle();
                    float new_y2 = r.ReadSingle();
                    float new_z2 = r.ReadSingle();
                    vector3Array2[index].Set(new_x2, new_y2, new_z2);
                    float new_x3 = r.ReadSingle();
                    float new_y3 = r.ReadSingle();
                    vector2Array[index].Set(new_x3, new_y3);
                }
                mesh2.vertices = vector3Array1;
                mesh2.normals = vector3Array2;
                mesh2.uv = vector2Array;
                // oriVert.vOriVert = vector3Array1;
                // oriVert.vOriNorm = vector3Array2;
                int length4 = r.ReadInt32();
                if (length4 > 0)
                {
                    Vector4[] vector4Array = new Vector4[length4];
                    for (int index = 0; index < length4; ++index)
                    {
                        float x = r.ReadSingle();
                        float y = r.ReadSingle();
                        float z = r.ReadSingle();
                        float w = r.ReadSingle();
                        vector4Array[index] = new Vector4(x, y, z, w);
                    }
                    mesh2.tangents = vector4Array;
                }
                for (int index = 0; index < length1; ++index)
                {
                    boneWeightArray[index].boneIndex0 = (int) r.ReadUInt16();
                    boneWeightArray[index].boneIndex1 = (int) r.ReadUInt16();
                    boneWeightArray[index].boneIndex2 = (int) r.ReadUInt16();
                    boneWeightArray[index].boneIndex3 = (int) r.ReadUInt16();
                    boneWeightArray[index].weight0 = r.ReadSingle();
                    boneWeightArray[index].weight1 = r.ReadSingle();
                    boneWeightArray[index].weight2 = r.ReadSingle();
                    boneWeightArray[index].weight3 = r.ReadSingle();
                }
                mesh2.boneWeights = boneWeightArray;
                mesh2.subMeshCount = length2;
                // oriVert.bwWeight = boneWeightArray;
                // oriVert.nSubMeshCount = length2;
                // oriVert.nSubMeshOriTri = new int[length2][];
                for (int submesh = 0; submesh < length2; ++submesh)
                {
                    int length5 = r.ReadInt32();
                    int[] triangles = new int[length5];
                    for (int index = 0; index < length5; ++index)
                        triangles[index] = (int) r.ReadUInt16();
                    // oriVert.nSubMeshOriTri[submesh] = triangles;
                    mesh2.SetTriangles(triangles, submesh);
                }
                int length6 = r.ReadInt32();
                Material[] materialArray = new Material[length6];
                for (int index = 0; index < length6; ++index)
                {
                    Material material = ReadMaterial(r, null);
                    materialArray[index] = material;
                }
                component.materials = materialArray;
                r.Close();
                return gameObject1;
            }
        }

        public static Material ReadMaterial(BinaryReader r, Material existmat)
        {
            string str1 = r.ReadString();
            string str2 = r.ReadString();
            string path = "DefMaterial/" + r.ReadString();
            Material material = existmat;
            if ( existmat ==  null)
            {
                Material original = Resources.Load(path, typeof (Material)) as Material;
                if ((UnityEngine.Object) original == (UnityEngine.Object) null)
                    NDebug.Assert("DefMaterialが見つかりません。" + path);
                material = UnityEngine.Object.Instantiate<Material>(original);
            }
            else
            {
                material = existmat;
                NDebug.Assert(material.shader.name == str2, "マテリアル入れ替えエラー。違うシェーダーに入れようとしました。 " + str2 + " -> " + material.shader.name);
            }
            material.name = str1;
            int hashCode = material.name.GetHashCode();
            // if (ImportCM.m_hashPriorityMaterials != null && ImportCM.m_hashPriorityMaterials.ContainsKey(hashCode))
            // {
            //   KeyValuePair<string, float> priorityMaterial = ImportCM.m_hashPriorityMaterials[hashCode];
            //   if (priorityMaterial.Key == material.name)
            //   {
            //     material.SetFloat("_SetManualRenderQueue", priorityMaterial.Value);
            //     material.renderQueue = (int) priorityMaterial.Value;
            //   }
            // }
            while (true)
            {
                string str3;
                string propertyName;
                string str4;
                do
                {
                    str3 = r.ReadString();
                    if (!(str3 == "end"))
                    {
                        propertyName = r.ReadString();
                        if (str3 == "tex")
                        {
                            str4 = r.ReadString();
                            if (str4 == "null")
                                material.SetTexture(propertyName, (Texture) null);
                            else if (str4 == "tex2d")
                            {
                                string str5 = r.ReadString();
                                r.ReadString();
                                Texture2D texture = ImportCM.CreateTexture(str5 + ".tex");
                                texture.name = str5;
                                texture.wrapMode = TextureWrapMode.Clamp;
                                material.SetTexture(propertyName, (Texture) texture);
                                // if (bodyskin != null)
                                //   bodyskin.listDEL.Add((UnityEngine.Object) texture);
                                Vector2 offset;
                                offset.x = r.ReadSingle();
                                offset.y = r.ReadSingle();
                                material.SetTextureOffset(propertyName, offset);
                                Vector2 scale;
                                scale.x = r.ReadSingle();
                                scale.y = r.ReadSingle();
                                material.SetTextureScale(propertyName, scale);
                            }
                        }
                        else
                            goto label_36;
                    }
                    else
                        goto label_43;
                }
                while (!(str4 == "texRT"));
                r.ReadString();
                r.ReadString();
                continue;
            label_36:
                if (str3 == "col")
                {
                    Color color;
                    color.r = r.ReadSingle();
                    color.g = r.ReadSingle();
                    color.b = r.ReadSingle();
                    color.a = r.ReadSingle();
                    material.SetColor(propertyName, color);
                }
                else if (str3 == "vec")
                {
                    Vector4 vector;
                    vector.x = r.ReadSingle();
                    vector.y = r.ReadSingle();
                    vector.z = r.ReadSingle();
                    vector.w = r.ReadSingle();
                    material.SetVector(propertyName, vector);
                }
                else if (str3 == "f")
                {
                    float num = r.ReadSingle();
                    material.SetFloat(propertyName, num);
                }
                else
                    Debug.LogError((object) ("マテリアルが読み込めません。不正なマテリアルプロパティ型です " + str3));
            }
        label_43:
            return material;
        }

        public static void LoadMoprhData2(BinaryReader r)
        {
            string str = r.ReadString();
            // int count = this.BlendDatas.Count;
            // this.hash[(object) str] = (object) count;
            // BlendData blendData = new BlendData();
            // blendData.name = str;
            int length = r.ReadInt32();
            // blendData.vert = new Vector3[length];
            // blendData.norm = new Vector3[length];
            // blendData.v_index = new int[length];
            for (int index = 0; index < length; ++index)
            {
                // blendData.v_index[index] = (int) r.ReadUInt16();
                // blendData.vert[index].x = r.ReadSingle();
                // blendData.vert[index].y = r.ReadSingle();
                // blendData.vert[index].z = r.ReadSingle();
                // blendData.norm[index].x = r.ReadSingle();
                // blendData.norm[index].y = r.ReadSingle();
                // blendData.norm[index].z = r.ReadSingle();
            }
            // ++this.MorphCount;
            // this.BlendDatas.Add(blendData);
            // this.BlendValues = new float[this.MorphCount + 1];
            // this.BlendValuesCHK = new float[this.MorphCount + 1];
        }
    }
}
