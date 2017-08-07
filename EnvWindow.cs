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
        public EnvWindow( int fontSize ) : base ( fontSize ) {
        }

        override public void Awake()
        {
            try
            {
                this.allBackgrounds = new Dictionary<string, string>();
                this.allBackgrounds.Add("", "");
                foreach( var kvp in ConstantValues.Background )
                    this.allBackgrounds.Add(kvp.Key, kvp.Value);
                string[] bgFiles = GameUty.FileSystem.GetList("bg", AFileSystemBase.ListType.AllFile)
                    .Where(f => f.EndsWith("bg")).ToArray();

                foreach( var bg in bgFiles ) {
                    string name = bg.Remove(0, 3);
                    name = Path.GetFileNameWithoutExtension(name);
                    this.allBackgrounds.Add(name, name);
                }

                this.allModels = GameUty.FileSystem.GetList("model", AFileSystemBase.ListType.AllFile)
                    .Where(f => f.EndsWith("model")).ToList();

                this.addLightButton = new CustomButton();
                this.addLightButton.Text = "Add Light";
                this.addLightButton.Click += AddLightButtonPressed;
                this.ChildControls.Add( this.addLightButton );

                this.addModelButton = new CustomButton();
                this.addModelButton.Text = "Add Model";
                this.addModelButton.Click += AddModelButtonPressed;
                this.ChildControls.Add( this.addModelButton );

                this.bgButton = new CustomButton();
                this.bgButton.Text = "bg";
                this.bgButton.Click += BGButtonPressed;
                this.ChildControls.Add( this.bgButton );

                this.backgroundBox = new CustomComboBox( this.allBackgrounds.Keys.ToArray() );
                this.backgroundBox.FontSize = this.FontSize;
                this.backgroundBox.Text = "Background";
                this.backgroundBox.SelectedIndex = 0;
                this.backgroundBox.SelectedIndexChanged += this.ChangeBackground;
                this.ChildControls.Add( this.backgroundBox );

                this.modelBox = new CustomComboBox( this.allModels.ToArray() );
                this.modelBox.FontSize = this.FontSize;
                this.modelBox.Text = "Model Type";
                this.modelBox.SelectedIndex = 0;
                this.ChildControls.Add( this.modelBox );

                this.addedLightInstance = new Dictionary<String, GameObject>();
                this.addedModelInstance = new Dictionary<string, GameObject>();
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
            this.backgroundBox.Left = this.Left + ControlBase.FixedMargin;
            this.backgroundBox.Top = this.Top + ControlBase.FixedMargin;
            this.backgroundBox.Width = this.Width / 2 - ControlBase.FixedMargin / 4;
            this.backgroundBox.Height = this.ControlHeight;
            this.backgroundBox.OnGUI();

            // GUIUtil.AddGUICheckbox( this, this.bgButton, this.addLightButton );
            GUIUtil.AddGUICheckbox( this, this.addLightButton, this.backgroundBox );
            // GUIUtil.AddGUICheckbox( this, this.modelBox, this.backgroundBox );
            // GUIUtil.AddGUICheckbox( this, this.addModelButton, this.modelBox );

            ControlBase prev = this.addLightButton;
            foreach( LightPane pane in this.lightPanes )
            {
                GUIUtil.AddGUICheckbox( this, pane, prev );
                prev = pane;
            }

            this.Height = GUIUtil.GetHeightForParent(this);
        }

        private void ChangeBackground( object sender, EventArgs args )
        {
            if( String.IsNullOrEmpty( this.backgroundBox.SelectedItem ) == false )
            {
                if( this.allBackgrounds.ContainsKey( this.backgroundBox.SelectedItem ) )
                {
                    if( this.backgroundBox.SelectedItem == "非表示") {
                        GameMain.Instance.BgMgr.DeleteBg();
                    }
                    else
                    {
                        GameMain.Instance.BgMgr.ChangeBg( this.allBackgrounds[ this.backgroundBox.SelectedItem ] );
                    }
                }
            }
        }

        private void AddModelButtonPressed( object sender, EventArgs args )
        {
            string modelName = this.modelBox.SelectedItem;
            AFileBase file = GameUty.FileOpen(modelName);
            Debug.Log("size " + file.GetSize());

            if( !file.IsValid() || file.GetSize() == 0 )
            {
                string name = modelName.Replace(@"model\", "");
                Debug.Log("try " + name);
                file = GameUty.FileOpen(name);
                if( file.GetSize() == 0 || !file.IsValid() )
                {
                    Debug.LogError("File not valid");
                    return;
                }
            }
            GameObject model = LoadMesh(file.ReadAll());
            model.AddComponent<GizmoRenderTarget>().Visible = true;
            model.GetComponent<GizmoRenderTarget>().eRotate = false;
            model.GetComponent<GizmoRenderTarget>().eAxis = true;

            // NOTE
            // public void Load(GameObject srcbody, GameObject body1, string bonename, string filename, string slotname, string AttachSlot, int layer)
            // public void AddItem(string slotname, string filename, string AttachSlot = "", string AttachName = "")
            // ClickCallback in SceneEdit
            // ProcItem(MaidProp mp)

            // model.AddComponent<Cloth>().enabled = true;
            // model.GetComponent<Cloth>().useGravity = true;
            // model.AddComponent<Rigidbody>().useGravity = true;
            // model.GetComponent<Rigidbody>().isKinematic = true;
            // this.Collidify(model);

            model.transform.localScale = new Vector3(1,1,1);
            model.transform.position = new Vector3(0, 1.5f, -1f);

            this.addedModelInstance.Add(this.modelsAdded.ToString(), model);
            this.modelsAdded++;
        }

        private void AddLightButtonPressed( object sender, EventArgs args )
        {
            AddLight();
            this.SetLightInstances();
        }

        private void Report( GameObject o )
        {
            foreach(Component c in o.GetComponents<Component>())
            {
                Debug.Log(c);
            }
        }

        private void Collidify( GameObject o )
        {
            report(o);
            Debug.Log("Now");
            foreach (Transform child in o.transform) {
                GameObject obj = child.gameObject;
                report(obj);
                Debug.Log(obj);
                foreach(SkinnedMeshRenderer R in obj.GetComponents<SkinnedMeshRenderer>())
                {
                    Debug.Log("getet");
                    CompoundCollider BC = R.gameObject.AddComponent<CompoundCollider>();
                }
            }
        }

        private void BGButtonPressed( object sender, EventArgs args )
        {
            if (GameMain.Instance.CharacterMgr.GetMaidCount() > 0)
            {
                foreach(TBodySkin skin in GameMain.Instance.CharacterMgr.GetMaid(0).body0.goSlot){
                    Debug.Log(skin.m_strModelFileName);
                    AFileBase file = GameUty.FileOpen(skin.m_strModelFileName);
                    if(file.IsValid()) {
                        Debug.Log(file.GetSize());
                    }
                }
            }
            // foreach(var a in )
            // {
            //     var open = GameUty.FileOpen(a);
            //     Debug.Log(a + " " + open.GetSize());
            // }

            foreach (Transform child in GameMain.Instance.BgMgr.current_bg_object.transform) {
                GameObject o = child.gameObject;
                Debug.Log(o);
                MeshFilter mes = o.GetComponent<MeshFilter>();
                if(mes != null) {
                    Debug.Log("GET");
                    o.AddComponent<MeshCollider>().sharedMesh = mes.sharedMesh;
                }
            }


            // foreach (var t in GameUty.BgFiles)
            // {
            //     Debug.Log("r " + t);
            // }
            // string[] allFiles = GameUty.FileSystem.GetList("", AFileSystemBase.ListType.AllFile);
            // string[] bgFiles = allFiles.Where(f => f.EndsWith("bg")).ToArray();
            // string[] anmFiles = allFiles.Where(f => f.EndsWith(".model")).ToArray();
            // foreach(string file in anmFiles) Console.WriteLine("{0}", file);
            // foreach(string file in bgFiles) Console.WriteLine("{0}", file);

            // // report(GameMain.Instance.BgMgr.current_bg_object);
            // GameMain.Instance.BgMgr.DeleteBg();
            // // byte[] bytes = File.ReadAllBytes(ConstantValues.ConfigDir + @"\model.model");
            // bg = LoadMesh();
            // report(bg);
            // // connect texture to material of GameObject this script is attached to
            // UnityEngine.Object.DontDestroyOnLoad(bg);

            // bg.transform.localScale = new Vector3(1,1,1);
            // bg.transform.position = new Vector3(0, 1.5f, -1f);

            // // foreach (Component componentsInChild in bg.GetComponentsInChildren<Transform>(true))
            // // {
            // //     Renderer component = componentsInChild.GetComponent<Renderer>();
            // //     if (component != null && component.materials != null)
            // //     {
            // //         foreach (Material material in component.materials)
            // //         {
            // //             material.color = new Color32(255, 192, 192, 192);
            // //             material.mainTexture = texture;
            // //         }
            // //     }
            // // }
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
                    String lightName = ConstantValues.AddLightName + this.lightsAdded;
                    this.lightsAdded++;

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

        public static GameObject LoadMesh(byte[] data) {
            using (BinaryReader r = new BinaryReader(new MemoryStream(data)) )
            {
                int layer = 0;
                // TBodySkin.OriVert oriVert = bodyskin.m_OriVert;
                GameObject gameObject1 = UnityEngine.Object.Instantiate(Resources.Load("seed")) as GameObject;
                gameObject1.layer = layer;
                GameObject gameObject2 = (GameObject) null;
                Hashtable hashtable = new Hashtable();

                string str1 = r.ReadString();
                Debug.Log("str1 " + str1);
                if (str1 != "CM3D2_MESH") {
                    return null;
                }
                r.ReadInt32();
                string str2 = r.ReadString();
                Debug.Log("str2 " + str2);
                string str3 = r.ReadString();
                Debug.Log("str3 " + str3);
                int num = r.ReadInt32();
                List<GameObject> gameObjectList = new List<GameObject>();
                for (int index = 0; index < num; ++index)
                {
                    GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("seed")) as GameObject;
                    gameObject3.layer = layer;
                    gameObject3.name = r.ReadString();
                    Debug.Log("name " + gameObject3.name);
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

                // NOTE
                // bodyskin.listDEL.Add((UnityEngine.Object) gameObject2);
                Transform[] transformArray = new Transform[length3];
                for (int index = 0; index < length3; ++index)
                {
                    string str4 = r.ReadString();
                    Debug.Log("str4 " + str4);
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
                Debug.Log("reach");
                component.materials = materialArray;
                r.Close();
                return gameObject1;
                while (true)
                {
                    string str4;
                    do
                    {
                        str4 = r.ReadString();
                        Debug.Log("str4 " + str4);
                        if (str4 == "end")
                            goto label_68;
                    }
                    while (!(str4 == "morph"));
                }
            label_68:
                r.Close();
                return gameObject1;
            }
        }


        public static Material ReadMaterial(BinaryReader r, Material existmat)
        {
            Debug.Log("ReadMaterial");
            string str1 = r.ReadString();
            Debug.Log(str1);
            string str2 = r.ReadString();
            Debug.Log(str2);
            string path = "DefMaterial/" + r.ReadString();
            Debug.Log(path);
            Material material = existmat;
            if ((UnityEngine.Object) existmat == (UnityEngine.Object) null)
            {
                Material original = Resources.Load(path, typeof (Material)) as Material;
                if ((UnityEngine.Object) original == (UnityEngine.Object) null)
                    NDebug.Assert("DefMaterialが見つかりません。" + path);
                material = UnityEngine.Object.Instantiate<Material>(original);
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
                    Debug.Log("str3 " + str3);
                    if (!(str3 == "end"))
                    {
                        propertyName = r.ReadString();
                        Debug.Log("propertyName " + propertyName);
                        if (str3 == "tex")
                        {
                            str4 = r.ReadString();
                            Debug.Log("str4 " + str4);
                            if (str4 == "null")
                                material.SetTexture(propertyName, (Texture) null);
                            else if (str4 == "tex2d")
                            {
                                string str5 = r.ReadString();
                                Debug.Log("str5 " + str5);
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
                                Debug.Log("offset " + offset);
                                Vector2 scale;
                                scale.x = r.ReadSingle();
                                scale.y = r.ReadSingle();
                                material.SetTextureScale(propertyName, scale);
                                Debug.Log("scale " + scale);
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

        #region Fields
        private CustomButton addLightButton = null;
        private CustomButton addModelButton = null;
        private CustomButton bgButton = null;
        private CustomComboBox backgroundBox = null;
        private CustomComboBox modelBox = null;
        private List<LightPane> lightPanes = null;
        private int lightsAdded = 0;
        private int modelsAdded = 0;
        private Dictionary<String, GameObject> addedLightInstance = null;
        private Dictionary<string, string> allBackgrounds = null;
        private List<string> allModels = null;
        private Dictionary<string, GameObject> addedModelInstance = null;
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
