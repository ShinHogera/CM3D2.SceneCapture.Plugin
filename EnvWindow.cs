using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.SceneCapture.Plugin
{
    internal class EnvWindow : ScrollablePane
    {
        public EnvWindow( int fontSize ) : base ( fontSize ) {}

        override public void Awake()
        {
            try
            {
                this.addLightButton = new CustomButton();
                this.addLightButton.Text = "Add";
                this.addLightButton.Click += AddLightButtonPressed;
                this.ChildControls.Add( this.addLightButton );

                this.bgButton = new CustomButton();
                this.bgButton.Text = "bg";
                this.bgButton.Click += BGButtonPressed;
                this.ChildControls.Add( this.bgButton );

                this.addedLightInstance = new Dictionary<String, GameObject>();
                this.lightPanes = new List<LightPane>();

                InitMainLight();

                if( GameMain.Instance.MainLight.GetComponent<Light>() != null )
                {
                    this.SetLightInstances();
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        private void InitMainLight()
        {
            if( GameMain.Instance.MainLight.GetComponent<Light>() != null )
            {
                LightPane pane = new LightPane( this.FontSize, GameMain.Instance.MainLight.GetComponent<Light>() );
                pane.Text = ConstantValues.MainLightName;
                this.ChildControls.Add( pane );
                this.lightPanes.Add( pane );
            }
        }

        override public void Update()
        {
            try
            {
                this.UpdateChildControls();
                this.CheckForLightUpdates();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void ShowPane()
        {
            this.addLightButton.Left = this.Left + ControlBase.FixedMargin;
            this.addLightButton.Top = this.Top + ControlBase.FixedMargin;
            this.addLightButton.Width = this.Width / 2 - ControlBase.FixedMargin / 4;
            this.addLightButton.Height = this.ControlHeight;
            this.addLightButton.OnGUI();

            GUIUtil.AddGUICheckbox( this, this.bgButton, this.addLightButton );

            ControlBase prev = this.bgButton;
            foreach( LightPane pane in this.lightPanes )
            {
                GUIUtil.AddGUICheckbox( this, pane, prev );
                prev = pane;
            }

            this.Height = GUIUtil.GetHeightForParent(this);
        }

        private void AddLightButtonPressed( object sender, EventArgs args )
        {
            AddLight();
            this.SetLightInstances();
        }

        private void BGButtonPressed( object sender, EventArgs args )
        {
            // report(GameMain.Instance.BgMgr.current_bg_object);
            GameMain.Instance.BgMgr.DeleteBg();
            // byte[] bytes = File.ReadAllBytes(ConstantValues.ConfigDir + @"\model.model");
            bg = LoadMesh();
            report(bg);
            // // Texture2D tex = ImportCM.CreateTexture(ConstantValues.ConfigDirSybaris + @"\tex.tex");

            // Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);

            // set the pixel values
            texture.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 0.5f));
            texture.SetPixel(1, 0, Color.clear);
            texture.SetPixel(0, 1, Color.white);
            texture.SetPixel(1, 1, Color.black);

            // Apply all SetPixel calls
            texture.Apply();

            // connect texture to material of GameObject this script is attached to
            UnityEngine.Object.DontDestroyOnLoad(bg);

            bg.GetComponent<Transform>().GetComponent<Renderer>().material.color = new Color32(255, 192, 192, 192);
            bg.GetComponent<Transform>().GetComponent<Renderer>().material.mainTexture = texture;
            bg.AddComponent<Light>().color = new Color32(255, 192, 192, 192);
            bg.GetComponent<Light>().type = LightType.Point;
            bg.GetComponent<Light>().intensity = 0.5f;
            bg.GetComponent<Light>().range = 8.0f;
            bg.GetComponent<Light>().transform.position = new Vector3(0, 0, 0);
            bg.GetComponent<Light>().enabled = true;
            bg.transform.localScale = new Vector3(1,1,1);
            bg.transform.position = new Vector3(0, 1.5f, -1f);

            // foreach (Component componentsInChild in bg.GetComponentsInChildren<Transform>(true))
            // {
            //     Renderer component = componentsInChild.GetComponent<Renderer>();
            //     if (component != null && component.materials != null)
            //     {
            //         foreach (Material material in component.materials)
            //         {
            //             material.color = new Color32(255, 192, 192, 192);
            //             material.mainTexture = texture;
            //         }
            //     }
            // }
        }

        private void report(GameObject o)
        {
            Component[] allComponents = o.GetComponents<Component>();
            foreach (Component component in allComponents) {
                Debug.Log(component);
            }
            Debug.Log("all");
            var all = o.GetComponentsInChildren<Component>();
            foreach (Component component in all) {
                Debug.Log(component);
            }
        }

        private void AddLight()
        {
            try
            {
                Debug.Log("Addlight");
                if( this.lightPanes.Count < ConstantValues.MaxLightCount )
                {
                    String lightName = ConstantValues.AddLightName + this.lightPanes.Count.ToString();

                    // 光源追加し、選択中の設定をコピー
                    GameObject newObject = new GameObject( "Light" );
                    if( newObject != null )
                    {
                        newObject.AddComponent<Light>();
                        this.addedLightInstance[ lightName ] = newObject;

                        Light currentLight = this.lightPanes.Last().LightValue;

                        if( newObject.GetComponent<Light>() != null )
                        {
                            newObject.GetComponent<Light>().type = currentLight.type;
                            newObject.GetComponent<Light>().intensity = currentLight.intensity;
                            newObject.GetComponent<Light>().range = currentLight.range;
                            newObject.GetComponent<Light>().color = new Color( currentLight.color.r,
                                                                               currentLight.color.g,
                                                                               currentLight.color.b,
                                                                               currentLight.color.a );
                            newObject.GetComponent<Light>().spotAngle = currentLight.spotAngle;
                            newObject.GetComponent<Light>().transform.rotation = new Quaternion( currentLight.transform.rotation.x,
                                                                                                 currentLight.transform.rotation.y,
                                                                                                 currentLight.transform.rotation.z,
                                                                                                 currentLight.transform.rotation.w );
                            newObject.GetComponent<Light>().transform.position = new Vector3( currentLight.transform.position.x,
                                                                                              currentLight.transform.position.y,
                                                                                              currentLight.transform.position.z );
                            newObject.GetComponent<Light>().enabled = true;

                            LightPane pane =  new LightPane( this.FontSize, newObject.GetComponent<Light>() );
                            pane.Text = lightName;
                            this.ChildControls.Add( pane );
                            this.lightPanes.Add( pane );
                        }
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        private void CheckForLightUpdates()
        {
            if ( Instances.needLightReload )
            {
                List<LightInfo> lightInfos = Instances.GetLights();

                this.ClearLights(true);
                this.lightPanes.Clear();
                for(int i = 0; i < lightInfos.Count; i++)
                {
                    Light light;
                    LightInfo lightInfo = lightInfos[i];
                    if(i != 0)
                    {
                        AddLight();
                        light = this.lightPanes[i].LightValue;
                        lightInfo.UpdateLight(light);

                        // inform light pane of changed value
                        this.lightPanes[i].UpdateFromLight();
                    }
                    else
                    {
                        light = GameMain.Instance.MainLight.GetComponent<Light>();
                        lightInfo.UpdateLight(light);
                        LightPane pane = new LightPane( this.FontSize, light );
                        pane.Text = ConstantValues.MainLightName;
                        this.ChildControls.Add( pane );
                        this.lightPanes.Add( pane );

                        pane.UpdateFromLight();
                    }

                }
                this.SetLightInstances();
                Instances.needLightReload = false;
            }
            else
            {
                bool changed = false;
                for (int i = this.lightPanes.Count - 1; i >= 0; i--)
                {
                    LightPane pane = this.lightPanes[i];
                    if( pane.IsDeleteRequested )
                    {
                        this.lightPanes.RemoveAt(i);
                        this.ChildControls.Remove( pane );
                        GameObject.Destroy( this.addedLightInstance[ pane.Text ] );
                        this.addedLightInstance.Remove( pane.Text );
                        changed = true;
                    }
                    else if( pane.WasChanged )
                    {
                        changed = true;
                        pane.WasChanged = false;
                    }
                }

                if( changed )
                {
                    this.SetLightInstances();
                }
            }
        }

        public void SetLightInstances()
        {
            List<LightInfo> lights = new List<LightInfo>();
            foreach(LightPane pane in this.lightPanes)
            {
                lights.Add(new LightInfo(pane.LightValue));
            }
            Debug.Log("lights set: " + this.lightPanes.Count);
            Instances.SetLights(lights);
        }

        public void ClearLights(bool clearMain)
        {
            try
            {
                // 光源一覧クリア
                LightPane[] panes = this.lightPanes.ToArray();
                foreach( LightPane pane in panes )
                {
                    if( clearMain || pane.Text != ConstantValues.MainLightName )
                    {
                        pane.StopDrag();
                        this.ChildControls.Remove( pane );
                        this.lightPanes.Remove( pane );
                    }
                }

                // 追加した光源を破棄
                foreach( GameObject obj in this.addedLightInstance.Values )
                {
                    GameObject.Destroy( obj );
                }

                // 追加光源オブジェクトクリア
                this.addedLightInstance.Clear();
                this.SetLightInstances();

                // デフォルト値設定
                // this.lightTypeComboBox.SelectedItem = DefaultLightInfo.Type.ToString();
                // this.lightEnableButton.Value = true;
                // this.lightIntensitySlider.Value = DefaultLightInfo.LightIntensity;
                // this.lightRangeSlider.Value = DefaultLightInfo.LightRange;
                // this.spotLightAngleSlider.Value = DefaultLightInfo.LightAngle;
                // this.rSlider.Value = DefaultLightInfo.LightColor.r * 255;
                // this.gSlider.Value = DefaultLightInfo.LightColor.g * 255;
                // this.bSlider.Value = DefaultLightInfo.LightColor.b * 255;
                // this.SelectedLight.transform.rotation = DefaultLightInfo.LightRotation;
                // this.SelectedLight.transform.position = DefaultLightInfo.LightPosition;
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        public static GameObject LoadMesh() {
            using (BinaryReader r = new BinaryReader(File.Open(ConstantValues.ConfigDir + @"\model.model", FileMode.Open)) )
            {
                int layer = 0;
                // TBodySkin.OriVert oriVert = bodyskin.m_OriVert;
                GameObject gameObject1 = UnityEngine.Object.Instantiate(Resources.Load("seed")) as GameObject;
                gameObject1.layer = layer;
                GameObject gameObject2 = (GameObject) null;
                Hashtable hashtable = new Hashtable();

                string str1 = r.ReadString();
                Debug.Log(str1);
                if (str1 != "CM3D2_MESH") {
                    Console.WriteLine("LoadSkinMesh_R 例外 : ヘッダーファイルが不正です。" + str1);
                    return null;
                }
                r.ReadInt32();
                string str2 = r.ReadString();
                Console.WriteLine("Name: " + "_SM_" + str2);
                string str3 = r.ReadString();
                Console.WriteLine(str3);
                int num = r.ReadInt32();
                Console.WriteLine(num);
                List<GameObject> gameObjectList = new List<GameObject>();
                for (int index = 0; index < num; ++index)
                {
                    Console.WriteLine(index);
                    GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("seed")) as GameObject;
                    gameObject3.layer = layer;
                    gameObject3.name = r.ReadString();
                    gameObjectList.Add(gameObject3);
                    if (gameObject3.name == str3)
                        gameObject2 = gameObject3;
                    hashtable[(object) gameObject3.name] = (object) gameObject3;
                    if ((int) r.ReadByte() != 0)
                    {
                        Console.WriteLine("Get extra");
                        GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load("seed")) as GameObject;
                        gameObject4.name = gameObject3.name + "_SCL_";
                        Console.WriteLine(gameObject4.name);
                        gameObject4.transform.parent = gameObject3.transform;
                        hashtable[(object) (gameObject3.name + "&_SCL_")] = (object) gameObject4;
                    }
                }
                for (int index1 = 0; index1 < num; ++index1)
                {
                    int index2 = r.ReadInt32();
                    gameObjectList[index1].transform.parent = index2 < 0 ? gameObject1.transform : gameObjectList[index2].transform;
                    Console.WriteLine(index1 + " " + index2);
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
                    Console.WriteLine("Item: " + x1 + "," + y1 + "," + z1 + " " + x2 + "," + y2 + "," + z2 + " " + w);
                }
                int length1 = r.ReadInt32();
                int length2 = r.ReadInt32();
                int length3 = r.ReadInt32();
                Console.WriteLine("len " + length1 + " " + length2 +  " " + length3);
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

                // NOTE
                // bodyskin.listDEL.Add((UnityEngine.Object) gameObject2);
                Transform[] transformArray = new Transform[length3];
                for (int index = 0; index < length3; ++index)
                {
                    string str4 = r.ReadString();
                    Console.WriteLine("Bone " + str4);
                    if (!hashtable.ContainsKey((object) str4))
                    {
                        Console.WriteLine((object) ("nullbone= " + str4));
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
                    // Console.WriteLine(new_x1 + "," + new_y1 + "," + new_z1 + " " + new_x2 + "," + new_y2 + "," + new_z2 + " " + new_x3 + "," + new_y3);
                }
                mesh2.vertices = vector3Array1;
                mesh2.normals = vector3Array2;
                mesh2.uv = vector2Array;
                // oriVert.vOriVert = vector3Array1;
                // oriVert.vOriNorm = vector3Array2;
                int length4 = r.ReadInt32();
                Console.WriteLine("length4 " + length4);
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
                        Console.WriteLine("vec4 " + x + "," + y + "," + z + "," + w);
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
                    // Console.WriteLine("Next");
                    // Console.WriteLine(r.ReadUInt16());
                    // Console.WriteLine(r.ReadUInt16());
                    // Console.WriteLine(r.ReadUInt16());
                    // Console.WriteLine(r.ReadUInt16());
                    // Console.WriteLine(r.ReadSingle());
                    // Console.WriteLine(r.ReadSingle());
                    // Console.WriteLine(r.ReadSingle());
                    // Console.WriteLine(r.ReadSingle());
                    // Console.WriteLine(r.ReadSingle());
                    // Console.WriteLine("Done");
                }
                mesh2.boneWeights = boneWeightArray;
                mesh2.subMeshCount = length2;
                // oriVert.bwWeight = boneWeightArray;
                // oriVert.nSubMeshCount = length2;
                // oriVert.nSubMeshOriTri = new int[length2][];
                for (int submesh = 0; submesh < length2; ++submesh)
                {
                    int length5 = r.ReadInt32();
                    Console.WriteLine("length5 " + length5);
                    int[] triangles = new int[length5];
                    for (int index = 0; index < length5; ++index)
                        triangles[index] = (int) r.ReadUInt16();
                    Console.WriteLine("tri " + triangles);
                    // oriVert.nSubMeshOriTri[submesh] = triangles;
                    mesh2.SetTriangles(triangles, submesh);
                }
                int length6 = r.ReadInt32();
                Console.WriteLine("length6 " + length6);
                Material[] materialArray = new Material[length6];
                for (int index = 0; index < length6; ++index)
                {
                    Material material = ReadMaterial(r, null);
                    materialArray[index] = material;
                }
                component.materials = materialArray;
                while (true)
                {
                    string str4;
                    do
                    {
                        str4 = r.ReadString();
                        Console.WriteLine(str4);
                        if (str4 == "end")
                            goto label_68;
                    }
                    while (!(str4 == "morph"));
                    LoadMoprhData2(r);
                }
            label_68:
                r.Close();
                return gameObject1;
            }
        }


        public static Material ReadMaterial(BinaryReader r, Material existmat)
        {
            Console.WriteLine("ReadMaterial");
            string str1 = r.ReadString();
            string str2 = r.ReadString();
            string path = "DefMaterial/" + r.ReadString();
            Console.WriteLine(str1 + " " + str2 + " " + path);
            Material material;
            if ((UnityEngine.Object) existmat == (UnityEngine.Object) null)
            {
                Material original = Resources.Load(path, typeof (Material)) as Material;
                if ((UnityEngine.Object) original == (UnityEngine.Object) null)
                    Console.WriteLine("DefMaterialが見つかりません。" + path);
                material = UnityEngine.Object.Instantiate<Material>(original);
                // if (bodyskin != null)
                //   bodyskin.listDEL.Add((UnityEngine.Object) material);
            }
            else
            {
                material = existmat;
                // NDebug.Assert(material.shader.name == str2, "マテリアル入れ替えエラー。違うシェーダーに入れようとしました。 " + str2 + " -> " + material.shader.name);
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
                    Console.WriteLine("str3 " + str3);
                    if (!(str3 == "end"))
                    {
                        propertyName = r.ReadString();
                        Console.WriteLine("prop " + propertyName);
                        if (str3 == "tex")
                        {
                            str4 = r.ReadString();
                            Console.WriteLine(str4);
                            if (str4 == "null")
                                material.SetTexture(propertyName, (Texture) null);
                            else if (str4 == "tex2d")
                            {
                                string str5 = r.ReadString();
                                Console.WriteLine(str5);
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
                    Console.WriteLine("color " + color);
                    material.SetColor(propertyName, color);
                }
                else if (str3 == "vec")
                {
                    Vector4 vector;
                    vector.x = r.ReadSingle();
                    vector.y = r.ReadSingle();
                    vector.z = r.ReadSingle();
                    vector.w = r.ReadSingle();
                    Console.WriteLine("vector " + vector);
                    material.SetVector(propertyName, vector);
                }
                else if (str3 == "f")
                {
                    float num = r.ReadSingle();
                    Console.WriteLine("f " + num);
                    material.SetFloat(propertyName, num);
                }
                else
                    Debug.LogError((object) ("マテリアルが読み込めません。不正なマテリアルプロパティ型です " + str3));
            }
        label_43:
            Console.WriteLine("ReadMaterial DONE");
            return material;
        }

        public static void LoadMoprhData2(BinaryReader r)
        {
            Console.WriteLine("LoadMoprhData2");
            string str = r.ReadString();
            Console.WriteLine("str " + str);;
            // int count = this.BlendDatas.Count;
            // this.hash[(object) str] = (object) count;
            // BlendData blendData = new BlendData();
            // blendData.name = str;
            int length = r.ReadInt32();
            Console.WriteLine("length " + length);;
            // blendData.vert = new Vector3[length];
            // blendData.norm = new Vector3[length];
            // blendData.v_index = new int[length];
            for (int index = 0; index < length; ++index)
            {
                Console.WriteLine("vert " + r.ReadUInt16());
                // blendData.v_index[index] = (int) r.ReadUInt16();
                Console.WriteLine("" + r.ReadSingle());
                Console.WriteLine("" + r.ReadSingle());
                Console.WriteLine("" + r.ReadSingle());
                Console.WriteLine("" + r.ReadSingle());
                Console.WriteLine("" + r.ReadSingle());
                Console.WriteLine("" + r.ReadSingle());
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

        #region Fields
        private CustomButton addLightButton = null;
        private CustomButton bgButton = null;
        private List<LightPane> lightPanes = null;
        private Dictionary<String, GameObject> addedLightInstance = null;
        private GameObject bg = null;
        #endregion
    }

    #region InsideClasses
    ///=========================================================================
    /// <summary>光源設定値</summary>
    ///=========================================================================
    public class LightInfo
    {
        public LightInfo() { }

        public LightInfo( Light light )
        {
            this.type = light.type;
            this.enabled = light.enabled;
            this.intensity = light.intensity;
            this.range = light.range;
            this.spotAngle = light.spotAngle;
            this.color = light.color;
            this.position = light.transform.position;
            this.eulerAngles = light.transform.eulerAngles;
        }

        public void UpdateLight(Light light)
        {
            light.type = this.type;
            light.enabled = this.enabled;
            light.intensity = this.intensity;
            light.range = this.range;
            light.spotAngle = this.spotAngle;
            light.color = this.color;
            light.transform.position = this.position;
            light.transform.eulerAngles = this.eulerAngles;
        }

        /// <summary>光源種別</summary>
        public LightType type = LightType.Directional;

        /// <summary>有効無効</summary>
        public bool enabled = false;

        /// <summary>光量</summary>
        public float intensity = 0.0f;

        /// <summary>光源範囲</summary>
        public float range = 0.0f;

        /// <summary>スポットライト角度</summary>
        public float spotAngle = 0.0f;

        /// <summary>光源色</summary>
        public Color color = new Color();

        public Vector3 position;

        public Vector3 eulerAngles;
    }
    #endregion
}
