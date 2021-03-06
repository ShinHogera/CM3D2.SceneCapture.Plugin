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
            this.savedCameraInfo = new CameraInfo();
        }

        override public void Awake()
        {
            try
            {
                this.cameraPositionXField = new CustomTextField();
                this.cameraPositionXField.Text = Translation.GetText("Camera", "cameraPositionX");
                this.cameraPositionXField.ValueChanged += this.ChangeCameraPosition;
                this.ChildControls.Add( this.cameraPositionXField );

                this.cameraPositionYField = new CustomTextField();
                this.cameraPositionYField.Text = Translation.GetText("Camera", "cameraPositionY");
                this.cameraPositionYField.ValueChanged += this.ChangeCameraPosition;
                this.ChildControls.Add( this.cameraPositionYField );

                this.cameraPositionZField = new CustomTextField();
                this.cameraPositionZField.Text = Translation.GetText("Camera", "cameraPositionZ");
                this.cameraPositionZField.ValueChanged += this.ChangeCameraPosition;
                this.ChildControls.Add( this.cameraPositionZField );

                this.cameraRotationXSlider = new CustomSlider( 0, -90f, 90f, 2 );
                this.cameraRotationXSlider.FontSize = this.FontSize;
                this.cameraRotationXSlider.Text = Translation.GetText("Camera", "cameraRotationX");
                this.cameraRotationXSlider.ValueChanged += this.ChangeCameraRotation;
                this.ChildControls.Add( this.cameraRotationXSlider );

                this.cameraRotationYSlider = new CustomSlider( 0, 0f, 359.9999f, 2 );
                this.cameraRotationYSlider.FontSize = this.FontSize;
                this.cameraRotationYSlider.Text = Translation.GetText("Camera", "cameraRotationY");
                this.cameraRotationYSlider.ValueChanged += this.ChangeCameraRotation;
                this.ChildControls.Add( this.cameraRotationYSlider );

                this.cameraRotationZSlider = new CustomSlider( 0, 0f, 359.9999f, 2 );
                this.cameraRotationZSlider.FontSize = this.FontSize;
                this.cameraRotationZSlider.Text = Translation.GetText("Camera", "cameraRotationZ");
                this.cameraRotationZSlider.ValueChanged += this.ChangeCameraRotation;
                this.ChildControls.Add( this.cameraRotationZSlider );

                this.cameraFovSlider = new CustomSlider( 0, 0f, 180f, 2 );
                this.cameraFovSlider.FontSize = this.FontSize;
                this.cameraFovSlider.Text = Translation.GetText("Camera", "cameraFov");
                this.cameraFovSlider.ValueChanged += this.ChangeCameraFov;
                this.ChildControls.Add( this.cameraFovSlider );

                this.cameraDistanceSlider = new CustomSlider( 0.1f, 0f, 25f, 2 );
                this.cameraDistanceSlider.FontSize = this.FontSize;
                this.cameraDistanceSlider.Text = Translation.GetText("Camera", "cameraDistance");
                this.cameraDistanceSlider.ValueChanged += this.ChangeCameraDistance;
                this.ChildControls.Add( this.cameraDistanceSlider );

                this.cameraSavePositionButton = new CustomButton();
                this.cameraSavePositionButton.Text = Translation.GetText("Camera", "cameraSavePosition");
                this.cameraSavePositionButton.Click += CameraSavePositionButtonPressed;
                this.ChildControls.Add( this.cameraSavePositionButton );

                this.cameraRestorePositionButton = new CustomButton();
                this.cameraRestorePositionButton.Text = Translation.GetText("Camera", "cameraRestorePosition");
                this.cameraRestorePositionButton.Click += CameraRestorePositionButtonPressed;
                this.ChildControls.Add( this.cameraRestorePositionButton );

                this.cameraResetPositionButton = new CustomButton();
                this.cameraResetPositionButton.Text = Translation.GetText("Camera", "cameraResetPosition");
                this.cameraResetPositionButton.Click += CameraResetPositionButtonPressed;
                this.ChildControls.Add( this.cameraResetPositionButton );

                this.cameraAllSettingsCheckbox = new CustomToggleButton( false, "toggle" );
                this.cameraAllSettingsCheckbox.Text = Translation.GetText("Camera", "cameraAllSettings");
                this.ChildControls.Add( this.cameraAllSettingsCheckbox );

                this.updateCheckbox = new CustomToggleButton( true, "toggle" );
                this.updateCheckbox.Text = "Update";//Translation.GetText("Camera", "update");
                this.ChildControls.Add( this.updateCheckbox );

                this.bgButton = new CustomButton();
                this.bgButton.Text = Translation.GetText("UI", "addModel");
                this.bgButton.Click += BGButtonPressed;
                this.ChildControls.Add( this.bgButton );

                this.addLightButton = new CustomButton();
                this.addLightButton.Text = Translation.GetText("UI", "addLight");
                this.addLightButton.Click += AddLightButtonPressed;
                this.ChildControls.Add( this.addLightButton );

                this.backgroundBox = new CustomComboBox( ConstantValues.Background.Keys.ToArray() );
                this.backgroundBox.FontSize = this.FontSize;
                this.backgroundBox.Text = Translation.GetText("UI", "background");
                this.backgroundBox.SelectedIndex = 0;
                this.backgroundBox.SelectedIndexChanged += this.ChangeBackground;
                this.ChildControls.Add( this.backgroundBox );

                this.addedLightInstance = new Dictionary<String, GameObject>();
                this.addedModelInstance = new Dictionary<string, ModelInstanceInfo>();
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
                pane.Text = Translation.GetText("UI", "mainLight");
                this.ChildControls.Add( pane );
                this.lightPanes.Add( pane );
            }
        }

        override public void Update()
        {
            try
            {
                if( this.needCameraUpdate )
                {
                    this.CameraSavePositionButtonPressed( this, new EventArgs() );
                    this.needCameraUpdate = false;
                }
                this.UpdateChildControls();
                this.CheckForLightUpdates();
                this.CheckForModelUpdates();
                this.CheckForMiscUpdates();
                this.UpdateCameraValues();

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
            if( this.cameraAllSettingsCheckbox.Value == true )
            {
                this.cameraPositionXField.Left = this.Left + ControlBase.FixedMargin;
                this.cameraPositionXField.Top = this.Top + ControlBase.FixedMargin;
                this.cameraPositionXField.Width = (this.Width / 3) - ControlBase.FixedMargin * 4;
                this.cameraPositionXField.Height = this.FixedFontSize + ControlBase.FixedMargin;
                this.cameraPositionXField.FontSize = this.FontSize;
                this.cameraPositionXField.OnGUI();
                this.cameraPositionXField.Visible = true;

                GUIUtil.AddGUIButton(this, this.cameraPositionYField, this.cameraPositionXField, 3);
                GUIUtil.AddGUIButton(this, this.cameraPositionZField, this.cameraPositionYField, 3);

                this.cameraRotationXSlider.Left = this.Left + ControlBase.FixedMargin;
                this.cameraRotationXSlider.Top = this.cameraPositionZField.Top + this.cameraPositionZField.Height + ControlBase.FixedMargin;
                this.cameraRotationXSlider.Width = this.Width - ControlBase.FixedMargin * 7;
                this.cameraRotationXSlider.Height = this.ControlHeight * 2;
                this.cameraRotationXSlider.OnGUI();
                this.cameraRotationXSlider.Visible = true;

                GUIUtil.AddGUISliderNoRender(this, this.cameraRotationYSlider, this.cameraRotationXSlider);
                this.cameraRotationYSlider.Width = this.Width - ControlBase.FixedMargin * 7;
                this.cameraRotationYSlider.OnGUI();
                GUIUtil.AddGUISliderNoRender(this, this.cameraRotationZSlider, this.cameraRotationYSlider);
                this.cameraRotationZSlider.Width = this.Width - ControlBase.FixedMargin * 7;
                this.cameraRotationZSlider.OnGUI();
                GUIUtil.AddGUISliderNoRender(this, this.cameraDistanceSlider, this.cameraRotationZSlider);
                this.cameraDistanceSlider.Width = this.Width - ControlBase.FixedMargin * 7;
                this.cameraDistanceSlider.OnGUI();
                GUIUtil.AddGUISliderNoRender(this, this.cameraFovSlider, this.cameraDistanceSlider);
                this.cameraFovSlider.Width = this.Width - ControlBase.FixedMargin * 7;
                this.cameraFovSlider.OnGUI();
                GUIUtil.AddGUICheckbox(this, this.cameraAllSettingsCheckbox, this.cameraFovSlider);
            }
            else
            {
                this.cameraFovSlider.Left = this.Left + ControlBase.FixedMargin;
                this.cameraFovSlider.Top = this.Top + ControlBase.FixedMargin;
                this.cameraFovSlider.Width = this.Width - ControlBase.FixedMargin * 7;
                this.cameraFovSlider.Height = this.ControlHeight * 2;
                this.cameraFovSlider.FontSize = this.FontSize;
                this.cameraFovSlider.OnGUI();
                this.cameraFovSlider.Visible = true;

                GUIUtil.AddGUISliderNoRender(this, this.cameraRotationZSlider, this.cameraFovSlider);
                this.cameraRotationZSlider.Width = this.Width - ControlBase.FixedMargin * 7;
                this.cameraRotationZSlider.OnGUI();
                GUIUtil.AddGUICheckbox(this, this.cameraAllSettingsCheckbox, this.cameraRotationZSlider);

                this.cameraPositionXField.Visible = false;
                this.cameraPositionYField.Visible = false;
                this.cameraPositionZField.Visible = false;
                this.cameraRotationXSlider.Visible = false;
                this.cameraRotationYSlider.Visible = false;
                this.cameraRotationZSlider.Visible = false;
                this.cameraDistanceSlider.Visible = false;
            }

            GUIUtil.AddGUIButtonAfter(this, this.cameraSavePositionButton, this.cameraAllSettingsCheckbox, 3);
            GUIUtil.AddGUIButton(this, this.cameraRestorePositionButton, this.cameraSavePositionButton, 3);
            GUIUtil.AddGUIButton(this, this.cameraResetPositionButton, this.cameraRestorePositionButton, 3);

            GUIUtil.AddGUICheckbox( this, this.backgroundBox, this.cameraSavePositionButton );
            GUIUtil.AddGUICheckbox( this, this.updateCheckbox, this.backgroundBox );
            GUIUtil.AddGUICheckbox( this, this.bgButton, this.updateCheckbox );

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

        private void ChangeCameraPosition( object sender, EventArgs args )
        {
            if(this.updatingCamera)
                return;

            Vector3 cameraPos = GameMain.Instance.MainCamera.GetTargetPos();

            float fTmpX;
            float fTmpY;
            float fTmpZ;
            if (!float.TryParse(this.cameraPositionXField.Value, out fTmpX))
            {
                fTmpX = cameraPos.x;
            }
            if (!float.TryParse(this.cameraPositionYField.Value, out fTmpY))
            {
                fTmpX = cameraPos.y;
            }
            if (!float.TryParse(this.cameraPositionZField.Value, out fTmpZ))
            {
                fTmpX = cameraPos.z;
            }
            GameMain.Instance.MainCamera.SetTargetPos(new Vector3(fTmpX, fTmpY, fTmpZ), true);
        }

        private void ChangeCameraRotation( object sender, EventArgs args )
        {
            if(this.updatingCamera)
                return;

            Camera camera = GameMain.Instance.MainCamera.gameObject.GetComponent<Camera>();
            camera.transform.eulerAngles = new Vector3(this.cameraRotationXSlider.Value,
                                                       this.cameraRotationYSlider.Value,
                                                       this.cameraRotationZSlider.Value);
        }

        private void ChangeCameraFov( object sender, EventArgs args )
        {
            if(this.updatingCamera)
                return;

            // Changing the camera FOV bugs out the gizmos
            this.DisableAllGizmos();

            Camera camera = GameMain.Instance.MainCamera.gameObject.GetComponent<Camera>();
            camera.fieldOfView = this.cameraFovSlider.Value;
        }

        private void ChangeCameraDistance( object sender, EventArgs args )
        {
            if(this.updatingCamera)
                return;

            CameraMain camera = GameMain.Instance.MainCamera;
            camera.SetDistance(this.cameraDistanceSlider.Value, true);
        }

        private void CameraSavePositionButtonPressed( object sender, EventArgs args )
        {
            GameObject cameraObj = GameMain.Instance.MainCamera.gameObject;
            this.savedCameraInfo = new CameraInfo()
                {
                    position = GameMain.Instance.MainCamera.GetTargetPos(),
                    rotation = cameraObj.transform.eulerAngles,
                    distance = GameMain.Instance.MainCamera.GetDistance(),
                    fieldOfView = cameraObj.GetComponent<Camera>().fieldOfView,
                };
        }

        private void CameraRestorePositionButtonPressed( object sender, EventArgs args )
        {
            GameObject cameraObj = GameMain.Instance.MainCamera.gameObject;
            GameMain.Instance.MainCamera.SetTargetPos(this.savedCameraInfo.position, true);
            cameraObj.transform.eulerAngles = this.savedCameraInfo.rotation;
            GameMain.Instance.MainCamera.SetDistance(this.savedCameraInfo.distance, true);
            cameraObj.GetComponent<Camera>().fieldOfView = this.savedCameraInfo.fieldOfView;
        }

        private void CameraResetPositionButtonPressed( object sender, EventArgs args )
        {
            Instances.ResetCamera();
        }

        private void UpdateCameraValues()
        {
            GameObject cameraObj = GameMain.Instance.MainCamera.gameObject;
            Camera camera = cameraObj.GetComponent<Camera>();
            Vector3 cameraPos = GameMain.Instance.MainCamera.GetTargetPos();
            this.updatingCamera = true;
            this.cameraPositionXField.Value = cameraPos.x.ToString();
            this.cameraPositionYField.Value = cameraPos.y.ToString();
            this.cameraPositionZField.Value = cameraPos.z.ToString();
            this.cameraRotationXSlider.Value = cameraObj.transform.eulerAngles.x;
            this.cameraRotationYSlider.Value = cameraObj.transform.eulerAngles.y;
            this.cameraRotationZSlider.Value = cameraObj.transform.eulerAngles.z;
            this.cameraFovSlider.Value = camera.fieldOfView;
            this.cameraDistanceSlider.Value = GameMain.Instance.MainCamera.GetDistance();
            this.updatingCamera = false;
        }

        private void ChangeBackground( object sender, EventArgs args )
        {
            string bg = this.backgroundBox.SelectedItem;
            if( String.IsNullOrEmpty( bg ) == false )
            {
                if( ConstantValues.Background.ContainsKey( bg ) )
                {
                    if( bg == "非表示") {
                        GameMain.Instance.BgMgr.DeleteBg();
                    }
                    else
                    {
                        GameMain.Instance.BgMgr.ChangeBg( ConstantValues.Background[ bg ] );
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

        public void ShowGizmos(bool show)
        {
            this.showGizmos = show;
        }

        private void DisableAllGizmos()
        {
            foreach( ModelPane pane in this.modelPanes )
            {
                pane.WantsTogglePan = false;
                pane.WantsToggleRotate = false;
                pane.WantsToggleScale = false;
            }
            this.CheckForModelUpdates();
        }

        private GameObject LoadModel(MenuInfo menuInfo)
        {
            if(menuInfo.modelType == ModelType.BGObject)
            {
                GameObject obj = AssetLoader.LoadBackgroundObject(menuInfo.modelName);
                this.AddGizmo( obj );

                obj.transform.localScale = new Vector3(1,1,1);
                obj.transform.position = new Vector3(0, 0, 0);
                return obj;
            }
            else if(menuInfo.modelType == ModelType.Background)
            {
                UnityEngine.Object prefab = GameMain.Instance.BgMgr.CreateAssetBundle(menuInfo.modelName);
                if(prefab == null)
                {
                    prefab = Resources.Load("BG/" + menuInfo.modelName);
                }
                GameObject obj = UnityEngine.Object.Instantiate(prefab) as GameObject;
                this.AddGizmo( obj );

                obj.transform.localScale = new Vector3(1,1,1);
                obj.transform.position = new Vector3(0, 0, 0);
                return obj;
            }
            else
            {
                GameObject model = AssetLoader.LoadMesh(menuInfo.modelName);
                model.name = MODEL_TAG;

                this.AddGizmo( model );

                model.transform.localScale = new Vector3(1,1,1);
                model.transform.position = new Vector3(0, 0, 0);

                foreach(SkinnedMeshRenderer renderer in model.transform.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    if(renderer != null)
                    {
                        Material[] materialArray = new Material[renderer.materials.Length];
                        for(int i = 0; i < materialArray.Length; i++)
                        {
                            materialArray[i] = new Material(renderer.materials[i]);
                        }

                        foreach(MaterialChangeInfo mat in menuInfo.materialChanges)
                        {
                            materialArray[mat.materialNo] = AssetLoader.LoadMaterial(mat.filename);
                        }

                        foreach(TextureChangeInfo tex in menuInfo.textureChanges)
                        {
                            materialArray[tex.materialNo].SetTexture(tex.propName, AssetLoader.LoadTexture(tex.filename));
                        }

                        renderer.materials = materialArray;
                    }
                }
                return model;
            }
        }

        private void AddModel( MenuInfo menuInfo )
        {
            GameObject model = this.LoadModel(menuInfo);

            // Spawn in front of camera
            GizmoRenderTarget gizmo = model.GetComponent<GizmoRenderTarget>();
            if(gizmo != null)
            {
                gizmo.transform.position = GameMain.Instance.MainCamera.camera.transform.forward * 2 + GameMain.Instance.MainCamera.camera.transform.position;
            }

            this.AddModelPane(model, menuInfo);
        }

        private void AddModelPane( GameObject model, MenuInfo menu )
        {
            string modelString = Translation.GetText("UI", "model");
            string modelName = modelString + " " + (this.modelsAdded + 1);
            while(this.addedModelInstance.ContainsKey(modelName)) {
                this.modelsAdded++;
                modelName = modelString + " " + (this.modelsAdded + 1);
            }

            var modelInstanceInfo = new ModelInstanceInfo(model, menu);
            this.addedModelInstance.Add(modelName, modelInstanceInfo);

            ModelPane pane = new ModelPane(this.FontSize, menu, modelName);
            this.modelPanes.Add( pane );
            this.ChildControls.Add( pane );
            this.SetModelInstances();
        }

        private void AddLightButtonPressed( object sender, EventArgs args )
        {
            AddLight();
            this.SetLightInstances();
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

                    MenuInfo menu = null;
                    if(modelInfo.modelType == ModelType.MaidEquip)
                        menu = AssetLoader.LoadMenu(modelInfo.menuFileName);
                    else if(modelInfo.modelType == ModelType.BGObject)
                        menu = MenuInfo.MakeBGObjectMenu(modelInfo.modelName);
                    else if(modelInfo.modelType == ModelType.Background)
                        menu = MenuInfo.MakeBackgroundMenu(modelInfo.modelName);

                    AddModel(menu);

                    string paneName = this.modelPanes[i].Name;

                    model = this.addedModelInstance[paneName].model;
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

                    ModelInstanceInfo info = this.addedModelInstance[ this.modelPanes[i].Name ];
                    GameObject model = info.model;
                    GizmoRenderTarget gizmo = model.GetComponent<GizmoRenderTarget>();
                    if( pane.GizmoScaleAllAxesValue == true )
                    {
                        if(gizmo.transform.localScale.y == gizmo.transform.localScale.z)
                        {
                            gizmo.transform.localScale = new Vector3(gizmo.transform.localScale.x,
                                                                     gizmo.transform.localScale.x,
                                                                     gizmo.transform.localScale.x);
                        }
                        else if(gizmo.transform.localScale.x == gizmo.transform.localScale.z)
                        {
                            gizmo.transform.localScale = new Vector3(gizmo.transform.localScale.y,
                                                                     gizmo.transform.localScale.y,
                                                                     gizmo.transform.localScale.y);
                        }
                        else
                        {
                            gizmo.transform.localScale = new Vector3(gizmo.transform.localScale.z,
                                                                     gizmo.transform.localScale.z,
                                                                     gizmo.transform.localScale.z);
                        }
                    }

                    if( pane.IsDeleteRequested )
                    {
                        this.modelPanes.RemoveAt(i);
                        this.ChildControls.Remove( pane );
                        GameObject.Destroy( this.addedModelInstance[ pane.Name ].model );
                        this.addedModelInstance.Remove( pane.Name );
                        changed = true;
                    }
                    else if( pane.wasChanged )
                    {
                        changed = true;
                        pane.wasChanged = false;

                        UpdateModelPane(ref pane);
                    }

                    if (!this.showGizmos || (!gizmo.eAxis && !gizmo.eRotate && !gizmo.eScal))
                        gizmo.Visible = false;
                    else
                        gizmo.Visible = true;

                    if(gizmo.Visible)
                        changed = true;
                }

                if( changed )
                {
                    this.SetModelInstances();
                }
            }
        }

        private void UpdateModelPane( ref ModelPane pane )
        {
            GameObject model = this.addedModelInstance[pane.Name].model;
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
        }

        private void CopyModel( ModelPane pane )
        {
            ModelInstanceInfo instance = this.addedModelInstance[ pane.Name ];
            GameObject toCopy = instance.model;

            MenuInfo menu = null;
            if(instance.modelType == ModelType.MaidEquip)
                menu = AssetLoader.LoadMenu(instance.menuFileName);
            else if(instance.modelType == ModelType.BGObject)
                menu = MenuInfo.MakeBGObjectMenu(instance.modelName);
            else if(instance.modelType == ModelType.Background)
                menu = MenuInfo.MakeBackgroundMenu(instance.modelName);

            GameObject model = this.LoadModel(menu);
            model.transform.position = toCopy.transform.position;
            model.transform.rotation = toCopy.transform.rotation;
            model.transform.localScale = toCopy.transform.localScale;

            this.AddModelPane( model, menu );
            this.SetModelInstances();
        }

        public void SetModelInstances()
        {
            List<ModelInfo> models = new List<ModelInfo>();
            foreach(ModelPane pane in this.modelPanes)
            {
                models.Add(new ModelInfo(this.addedModelInstance[ pane.Name ]));
            }
            Instances.SetModels(models);
        }

        public void ClearModels()
        {
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
                    GameObject.Destroy( obj.model );
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
            foreach(ModelInstanceInfo info in this.addedModelInstance.Values)
            {
                GameObject model = info.model;
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
                        Debug.LogError( e );
                    }
                }
                GlobalItemPicker.SetMenus(menus);
            }

            GlobalItemPicker.Set(new Vector2(this.Left + this.ScreenPos.x, this.Top + this.ScreenPos.y),
                                 this.FontSize * 36,
                                 this.FontSize + 3,
                                 this.AddModel);
        }

        private void AddLight()
        {
            try
            {
                if( this.lightPanes.Count < ConstantValues.MaxLightCount )
                {
                    String lightString = Translation.GetText("UI", "light");
                    String lightName = lightString + " " + this.lightsAdded;
                    while(this.addedLightInstance.ContainsKey(lightName)) {
                        this.lightsAdded++;
                        lightName = lightString + " " + this.lightsAdded;
                    }

                    // 光源追加し、選択中の設定をコピー
                    GameObject newObject = new GameObject( "Light" );
                    if( newObject != null )
                    {
                        newObject.AddComponent<Light>();
                        this.addedLightInstance[ lightName ] = newObject;

                        Light currentLight = this.lightPanes.Last().LightValue;
                        Light newLight = newObject.GetComponent<Light>();

                        if( newLight != null )
                        {
                            newLight.type = currentLight.type;
                            newLight.intensity = currentLight.intensity;
                            newLight.range = currentLight.range;
                            newLight.color = new Color( currentLight.color.r,
                                                        currentLight.color.g,
                                                        currentLight.color.b,
                                                        currentLight.color.a );
                            newLight.spotAngle = currentLight.spotAngle;
                            newLight.transform.rotation = new Quaternion( currentLight.transform.rotation.x,
                                                                          currentLight.transform.rotation.y,
                                                                          currentLight.transform.rotation.z,
                                                                          currentLight.transform.rotation.w );
                            newLight.transform.position = new Vector3( currentLight.transform.position.x,
                                                                       currentLight.transform.position.y,
                                                                       currentLight.transform.position.z );
                            newLight.shadows = currentLight.shadows;
                            newLight.shadowStrength = currentLight.shadowStrength;
                            newLight.shadowBias = currentLight.shadowBias;
                            newLight.shadowNormalBias = currentLight.shadowNormalBias;
                            newLight.enabled = true;

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

        private void ResetLightPane( ref LightPane pane )
        {
            Light light;
            if(pane.Text == Translation.GetText("UI", "mainLight"))
                light = GameMain.Instance.MainLight.GetComponent<Light>();
            else
                light = this.addedLightInstance[ pane.Text ].GetComponent<Light>();

            // Taken from GameMain.Instace.MainLight default values
            light.intensity = 0.95f;
            light.range = 10;
            light.color = new Color(1, 1, 1, 1);
            light.transform.eulerAngles = new Vector3(40, 180, 18);
            light.spotAngle = 30;
            light.shadows = LightShadows.Soft;
            light.shadowStrength = 0.098f;
            light.shadowBias = 0.01f;
            light.shadowNormalBias = 0.4f;
            pane.LightValue = light;

            pane.UpdateFromLight();
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
                        pane.Text = Translation.GetText("UI", "mainLight");
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
                    else {
                        if( pane.resetRequested )
                        {
                            this.ResetLightPane( ref pane );
                            changed = true;
                            pane.resetRequested = false;
                        }
                        if( pane.WasChanged )
                        {
                            changed = true;
                            pane.WasChanged = false;
                        }
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
                    if( clearMain || pane.Text != Translation.GetText("UI", "mainLight") )
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

        private CustomSlider cameraFovSlider = null;
        private CustomSlider cameraDistanceSlider = null;
        private CustomSlider cameraRotationXSlider = null;
        private CustomSlider cameraRotationYSlider = null;
        private CustomSlider cameraRotationZSlider = null;
        private CustomTextField cameraPositionXField = null;
        private CustomTextField cameraPositionYField = null;
        private CustomTextField cameraPositionZField = null;
        private CustomButton cameraSavePositionButton = null;
        private CustomButton cameraRestorePositionButton = null;
        private CustomButton cameraResetPositionButton = null;
        private CustomToggleButton cameraAllSettingsCheckbox = null;
        private CustomToggleButton updateCheckbox = null;

        private List<LightPane> lightPanes = null;
        private List<ModelPane> modelPanes = null;

        private int lightsAdded = 0;
        private int modelsAdded = 0;
        private bool showGizmos = true;
        private bool needCameraUpdate = true;
        private bool updatingCamera = false;
        private CameraInfo savedCameraInfo;
        private Dictionary<String, GameObject> addedLightInstance = null;

        private Dictionary<string, ModelInstanceInfo> addedModelInstance = null;

        public bool ShouldUpdate
        {
            get
            {
                return this.updateCheckbox.Value;
            }
        }
        #endregion
    }

    #region InsideClasses
    public class CameraInfo
    {
        public CameraInfo()
        {
            this.position = Vector3.zero;
            this.rotation = Vector3.zero;
            this.distance = 5f;
            this.fieldOfView = 1.0f;
        }

        public Vector3 position;
        public Vector3 rotation;
        public float distance;
        public float fieldOfView;
    }

    public class ModelInstanceInfo
    {
        public GameObject model;
        public string modelName;
        public string modelIconName;
        public string menuFileName;
        private MaidParts.PartsColor[] partsColors = new MaidParts.PartsColor[7];
        private Texture2D[] partsColorTextures = new Texture2D[7];

        public Texture baseTexture;
        public RenderTexture modifiedTexture;
        public ModelType modelType;

        public ModelInstanceInfo( GameObject model, MenuInfo menu )
        {
            this.modelType = menu.modelType;

            if(menu.modelType == ModelType.MaidEquip)
            {
                this.model = model;
                this.modelName = menu.modelName;
                this.modelIconName = menu.iconTextureName;
                this.menuFileName = menu.menuFileName;
                this.baseTexture = (Texture)null;
                this.modifiedTexture = (RenderTexture)null;
                for (int index = 0; index < 7; ++index) {
                    this.partsColorTextures[index] = new Texture2D(256, 1, TextureFormat.RGBA32, false);
                }
            }
            else
            {
                this.model = model;
                this.modelName = menu.modelName;
            }
        }

        public void SetPartsColor(MaidParts.PARTS_COLOR colorType, MaidParts.PartsColor color)
        {
            this.partsColors[(int)colorType] = color;
            UTY.UpdateColorTableTexture(color, ref this.partsColorTextures[(int) colorType]);
        }

        public MaidParts.PartsColor GetPartsColor(MaidParts.PARTS_COLOR f_eColorType)
        {
            return this.partsColors[(int) f_eColorType];
        }

        public Texture2D GetPartsColorTableTex(MaidParts.PARTS_COLOR f_eColorType)
        {
            return this.partsColorTextures[(int) f_eColorType];
        }
    }

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

    public enum ModelType
    {
        MaidEquip,
        BGObject,
        Background
    }

    public class ModelInfo
    {
        public ModelInfo() {}

        public ModelInfo( ModelInstanceInfo info ) {
            this.modelName = info.modelName;
            this.modelIconName = info.modelIconName;
            this.menuFileName = info.menuFileName;
            this.modelType = info.modelType;

            GizmoRenderTarget gizmo = info.model.GetComponent<GizmoRenderTarget>();

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

        public ModelType modelType;

        public string menuFileName;

        public string modelName;

        public string modelIconName;

        public Vector3 position;

        public Quaternion rotation;

        public Vector3 localScale;
    }

    public static class Vector3Ext {
        public static Vector3 reciprocal(this Vector3 input){
            return new Vector3(1f/input.x,1f/input.y,1f/input.z);
        }
    }
    #endregion
}
