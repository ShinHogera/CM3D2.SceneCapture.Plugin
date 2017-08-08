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
                this.addLightButton = new CustomButton();
                this.addLightButton.Text = "Add Light";
                this.addLightButton.Click += AddLightButtonPressed;
                this.ChildControls.Add( this.addLightButton );

                this.bgButton = new CustomButton();
                this.bgButton.Text = "Add Model";
                this.bgButton.Click += BGButtonPressed;
                this.ChildControls.Add( this.bgButton );

                this.backgroundBox = new CustomComboBox( new string[0] );
                this.backgroundBox.FontSize = this.FontSize;
                this.backgroundBox.Text = "Background";
                this.backgroundBox.SelectedIndex = 0;
                this.backgroundBox.SelectedIndexChanged += this.ChangeBackground;
                this.ChildControls.Add( this.backgroundBox );

                this.addedLightInstance = new Dictionary<String, GameObject>();
                this.addedModelInstance = new Dictionary<string, KeyValuePair<GameObject, string>>();
                this.lightPanes = new List<LightPane>();
                this.modelPanes = new List<ModelPane>();

                InitMainLight();

                if( GameMain.Instance.MainLight.GetComponent<Light>() != null )
                {
                    this.SetLightInstances();
                }

                this.SetModelInstances();
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
                if(!addedBackgrounds)
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
                    this.backgroundBox.SetItems(this.allBackgrounds.Keys.ToList());
                    addedBackgrounds = true;
                }

                this.UpdateChildControls();
                this.CheckForLightUpdates();
                this.CheckForModelUpdates();
                this.CheckForMiscUpdates();

                // FIXME: doesn't work.
                // this.CheckModelGizmoClick();
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
            this.backgroundBox.Width = this.Width - ControlBase.FixedMargin / 4;
            this.backgroundBox.Height = this.ControlHeight;
            this.backgroundBox.OnGUI();

            GUIUtil.AddGUICheckbox( this, this.bgButton, this.backgroundBox );

            ControlBase prev = this.bgButton;
            foreach( ModelPane pane in this.modelPanes )
            {
                GUIUtil.AddGUICheckbox( this, pane, prev );
                prev = pane;
            }

            GUIUtil.AddGUICheckbox( this, this.addLightButton, prev );
            // GUIUtil.AddGUICheckbox( this, this.modelBox, this.backgroundBox );
            // GUIUtil.AddGUICheckbox( this, this.addModelButton, this.modelBox );

            prev = this.addLightButton;
            foreach( LightPane pane in this.lightPanes )
            {
                GUIUtil.AddGUICheckbox( this, pane, prev );
                prev = pane;
            }

            this.Height = GUIUtil.GetHeightForParent(this);
        }

        private void ChangeBackground( object sender, EventArgs args )
        {
            string bg = this.backgroundBox.SelectedItem;
            if( String.IsNullOrEmpty( bg ) == false )
            {
                if( this.allBackgrounds.ContainsKey( bg ) )
                {
                    if( bg == "非表示") {
                        GameMain.Instance.BgMgr.DeleteBg();
                    }
                    else
                    {
                        GameMain.Instance.BgMgr.ChangeBg( this.allBackgrounds[ bg ] );
                    }
                    Instances.background = bg;
                }
            }
        }

        private void AddGizmo( GameObject target )
        {
            target.AddComponent<GizmoRenderTarget>().Visible = false;
            target.GetComponent<GizmoRenderTarget>().eRotate = false;
            target.GetComponent<GizmoRenderTarget>().eAxis = false;
            target.GetComponent<GizmoRenderTarget>().eScal = false;
        }

        private GameObject LoadModel(String modelFileName)
        {
            GameObject model = AssetLoader.LoadMesh(modelFileName);
            model.name = MODEL_TAG;
            Debug.Log("Load model " + modelFileName);

            this.AddGizmo( model );

            // model.AddComponent<Cloth>().enabled = true;
            // model.GetComponent<Cloth>().useGravity = true;
            // model.AddComponent<Rigidbody>().useGravity = true;
            // model.GetComponent<Rigidbody>().isKinematic = true;
            // this.Collidify(model);

            model.transform.localScale = new Vector3(1,1,1);
            model.transform.position = new Vector3(0, 0, 0);

            return model;
        }

        private void AddModel(String modelFileName)
        {
            GameObject model = this.LoadModel(modelFileName);
            this.AddModelPane(model, modelFileName);
        }

        private void AddModelPane( GameObject model, string modelFileName )
        {
            string modelName = "Model " + (this.modelsAdded + 1);
            while(this.addedModelInstance.ContainsKey(modelName)) {
                this.modelsAdded++;
                modelName = "Model " + (this.modelsAdded + 1);
            }

            this.addedModelInstance.Add(modelName, new KeyValuePair<GameObject, String>(model, modelFileName));
            ModelPane pane = new ModelPane(this.FontSize, modelName);
            this.modelPanes.Add( pane );
            this.ChildControls.Add( pane );
            this.SetModelInstances();
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

        private void CheckForModelUpdates()
        {
            if ( Instances.needModelReload )
            {
                List<ModelInfo> modelInfos = Instances.GetModels();

                this.ClearModels();
                this.modelPanes.Clear();
                for(int i = 0; i < modelInfos.Count; i++)
                {
                    GameObject model;
                    ModelInfo modelInfo = modelInfos[i];
                    AddModel(modelInfo.modelName);
                    string paneName = this.modelPanes[i].Name;

                    model = this.addedModelInstance[paneName].Key;
                    modelInfo.UpdateModel(model);
                }
                this.SetModelInstances();
                Instances.needModelReload = false;
            }
            else
            {
                bool changed = false;
                for (int i = this.modelPanes.Count - 1; i >= 0; i--)
                {
                    ModelPane pane = this.modelPanes[i];

                    GameObject model = this.addedModelInstance[ this.modelPanes[i].Name ].Key;
                    GizmoRenderTarget gizmo = model.GetComponent<GizmoRenderTarget>();
                    if(gizmo != null)
                        pane.UpdateCache( model.transform );

                    if( pane.IsDeleteRequested )
                    {
                        this.modelPanes.RemoveAt(i);
                        this.ChildControls.Remove( pane );
                        GameObject.Destroy( this.addedModelInstance[ pane.Name ].Key );
                        this.addedModelInstance.Remove( pane.Name );
                        changed = true;
                    }
                    else if( pane.wasChanged )
                    {
                        changed = true;
                        pane.wasChanged = false;

                        UpdateModelPane(ref pane);

                    }
                }

                if( changed )
                {
                    this.SetModelInstances();
                }
            }
        }

        private void UpdateModelPane( ref ModelPane pane )
        {
            GameObject model = this.addedModelInstance[pane.Name].Key;
            if( model == null )
                return;

            GizmoRenderTarget gizmo = model.GetComponent<GizmoRenderTarget>();
            if( gizmo == null )
            {
                Debug.Log("Gizmo null! " + pane.Name);
                return;
            }

            if( pane.WantsTogglePan != gizmo.eAxis ) {
                gizmo.eAxis = pane.WantsTogglePan;
                gizmo.eRotate = false;
                gizmo.eScal = false;
            }
            if( pane.WantsToggleRotate != gizmo.eRotate ) {
                gizmo.eAxis = false;
                gizmo.eRotate = pane.WantsToggleRotate;
                gizmo.eScal = false;
            }
            if( pane.WantsToggleScale != gizmo.eScal ) {
                gizmo.eAxis = false;
                gizmo.eRotate = false;
                gizmo.eScal = pane.WantsToggleScale;
            }
            if( pane.wantsResetPan ) {
                gizmo.transform.position = new Vector3(0, 0, 0);
                pane.wantsResetPan = false;
            }
            if( pane.wantsResetRotation ) {
                gizmo.transform.rotation = new Quaternion(0, 0, 0, 1);
                pane.wantsResetRotation = false;
            }
            if( pane.wantsResetScale ) {
                gizmo.transform.localScale = new Vector3(1, 1, 1);
                pane.wantsResetScale = false;
            }
            if( pane.wantsCopy )
            {
                this.CopyModel( pane );
                pane.wantsCopy = false;
            }

            if (!gizmo.eAxis && !gizmo.eRotate && !gizmo.eScal)
                gizmo.Visible = false;
            else
                gizmo.Visible = true;
        }

        private void CopyModel( ModelPane pane )
        {
            GameObject toCopy = this.addedModelInstance[ pane.Name ].Key;
            string modelName = this.addedModelInstance[ pane.Name ].Value;
            GameObject model = this.LoadModel(modelName);
            model.transform.position = toCopy.transform.position;
            model.transform.rotation = toCopy.transform.rotation;
            model.transform.localScale = toCopy.transform.localScale;

            this.AddModelPane( model, modelName );
            this.SetModelInstances();
        }

        public void SetModelInstances()
        {
            List<ModelInfo> models = new List<ModelInfo>();
            foreach(ModelPane pane in this.modelPanes)
            {
                Debug.Log("mod " + this.addedModelInstance[ pane.Name ].Value);
                models.Add(new ModelInfo(this.addedModelInstance[ pane.Name ].Key,
                                         this.addedModelInstance[ pane.Name ].Value));
            }
            Instances.SetModels(models);
        }

        public void ClearModels()
        {
            Debug.Log("Clear models");
            try
            {
                // 光源一覧クリア
                ModelPane[] panes = this.modelPanes.ToArray();
                foreach( ModelPane pane in panes )
                {
                    this.ChildControls.Remove( pane );
                    this.modelPanes.Remove( pane );
                }

                // 追加した光源を破棄
                foreach( var obj in this.addedModelInstance.Values )
                {
                    GameObject.Destroy( obj.Key );
                }

                // 追加光源オブジェクトクリア
                this.modelPanes.Clear();
                this.addedModelInstance.Clear();
                this.SetModelInstances();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        private void CheckModelGizmoClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Left down");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 31))
                {
                    GameObject target = hit.collider.gameObject;
                    Debug.Log("Hit " + target.name);
                    if (target.name == MODEL_TAG)
                    {
                        Debug.Log ("It's working!");
                        this.ReselectModels(target);
                    }
                }
            }
        }

        private void ReselectModels(GameObject target)
        {
            foreach(var kvp in this.addedModelInstance.Values)
            {
                GameObject model = kvp.Key;
                if( model == target )
                {
                    if(model.GetComponent<GizmoRenderTarget>() == null)
                    {
                        // this.AddGizmo( model );
                    }
                }
                else if(model.GetComponent<GizmoRenderTarget>() != null)
                {
                    UnityEngine.Object.Destroy(model.GetComponent<GizmoRenderTarget>());
                }
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
            if(!GlobalItemPicker.MenusAreSet())
            {
                string[] menuFiles = GameUty.FileSystem.GetList("menu", AFileSystemBase.ListType.AllFile)
                    .Where(f => f.EndsWith("menu")).ToArray();
                Dictionary<MPN, List<MenuInfo>> menus = new Dictionary<MPN, List<MenuInfo>>();
                // foreach(string menu in menuFiles)
                foreach(string menu in menuFiles)
                {
                    try {
                        MenuInfo mi = AssetLoader.LoadMenu(menu);

                        List<MenuInfo> existing;
                        if (!menus.TryGetValue(mi.partCategory, out existing)) {
                            existing = new List<MenuInfo>();
                            menus[mi.partCategory] = existing;
                        }
                        existing.Add(mi);
                    }
                    catch (Exception e)
                    {
                        Debug.Log( e );
                    }
                }
                GlobalItemPicker.SetMenus(menus);
            }

            GlobalItemPicker.Set(new Vector2(this.Left + this.ScreenPos.x, this.Top + this.ScreenPos.y),
                                 this.FontSize * 36,
                                 this.FontSize + 3,
                                 this.AddModel);

            // if (GameMain.Instance.CharacterMgr.GetMaidCount() > 0)
            // {
            //     foreach(TBodySkin skin in GameMain.Instance.CharacterMgr.GetMaid(0).body0.goSlot){
            //         Debug.Log(skin.m_strModelFileName);
            //         AFileBase file = GameUty.FileOpen(skin.m_strModelFileName);
            //         if(file.IsValid()) {
            //             Debug.Log(file.GetSize());
            //         }
            //     }
            // }
            // foreach(var a in )
            // {
            //     var open = GameUty.FileOpen(a);
            //     Debug.Log(a + " " + open.GetSize());
            // }

            // foreach (Transform child in GameMain.Instance.BgMgr.current_bg_object.transform) {
            //     GameObject o = child.gameObject;
            //     Debug.Log(o);
            //     MeshFilter mes = o.GetComponent<MeshFilter>();
            //     if(mes != null) {
            //         Debug.Log("GET");
            //         o.AddComponent<MeshCollider>().sharedMesh = mes.sharedMesh;
            //     }
            // }


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
                if( this.lightPanes.Count < ConstantValues.MaxLightCount )
                {
                    String lightName = ConstantValues.AddLightName + this.lightsAdded;
                    while(this.addedLightInstance.ContainsKey(lightName)) {
                        this.lightsAdded++;
                        lightName = ConstantValues.AddLightName + this.lightsAdded;
                    }

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

        private void CheckForMiscUpdates()
        {
            if( Instances.needMiscReload )
            {
                Instances.needMiscReload = false;
                this.backgroundBox.SelectedItem = Instances.background;
            }
        }

        #region Fields
        public const string MODEL_TAG = "CM3D2.SceneCapture.Model";
        private CustomButton addLightButton = null;
        private CustomButton bgButton = null;
        private CustomComboBox backgroundBox = null;
        private bool addedBackgrounds = false;

        private List<LightPane> lightPanes = null;
        private List<ModelPane> modelPanes = null;

        private int lightsAdded = 0;
        private int modelsAdded = 0;
        private Dictionary<String, GameObject> addedLightInstance = null;
        private Dictionary<string, KeyValuePair<GameObject, string>> addedModelInstance = null;

        private Dictionary<string, string> allBackgrounds = null;
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

    public class ModelInfo
    {
        public ModelInfo() { }

        public ModelInfo( GameObject obj, string modelName )
        {
            this.modelName = modelName;

            GizmoRenderTarget gizmo = obj.GetComponent<GizmoRenderTarget>();

            if(gizmo == null) {
                Debug.Log("null gizmo: " + modelName);
                this.position = Vector3.zero;
                this.rotation = Quaternion.identity;
                this.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                this.position = gizmo.transform.position;
                this.rotation = gizmo.transform.rotation;
                this.localScale = gizmo.transform.localScale;
            }
        }

        public void UpdateModel( GameObject obj )
        {
            if( obj.GetComponent<GizmoRenderTarget>() == null)
                obj.AddComponent<GizmoRenderTarget>();

            GizmoRenderTarget gizmo = obj.GetComponent<GizmoRenderTarget>();

            gizmo.transform.position = this.position;
            gizmo.transform.rotation = this.rotation;
            gizmo.transform.localScale = this.localScale;
        }

        public GameObject Load()
        {
            GameObject model = AssetLoader.LoadMesh(this.modelName);
            model.name = EnvWindow.MODEL_TAG;
            this.UpdateModel( model );
            return model;
        }

        public string modelName;

        public Vector3 position;

        public Quaternion rotation;

        public Vector3 localScale;
    }
    #endregion
}
