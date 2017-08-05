using System;
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

            ControlBase prev = this.addLightButton;
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
                    }
                    else
                    {
                        light = GameMain.Instance.MainLight.GetComponent<Light>();
                        lightInfo.UpdateLight(light);
                        LightPane pane = new LightPane( this.FontSize, light );
                        pane.Text = ConstantValues.MainLightName;
                        this.ChildControls.Add( pane );
                        this.lightPanes.Add( pane );
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
                    Debug.Log("CHAGNE");
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

        #region Fields
        private CustomButton addLightButton = null;
        private List<LightPane> lightPanes = null;
        private Dictionary<String, GameObject> addedLightInstance = null;
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
