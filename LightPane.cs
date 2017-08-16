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
    internal class LightPane : ControlBase
    {
        public LightPane( int fontSize, Light light )
        {
            this.FontSize = fontSize;
            this.light = light;

            if(this.light == null) {
                Debug.LogError("Light is null!");
            }

            this.Awake();
        }

        override public void Awake()
        {
            if(this.light == null) {
                Debug.LogError("Light is null!");
                return;
            }

            this.dragManager = new DragManager();
            this.isDrag = false;
            this.deleteRequested = false;
            this.resetRequested = false;
            this.changingDrag = false;

            this.lightNameLabel = new CustomLabel();
            this.lightNameLabel.Text = this.Text;
            this.ChildControls.Add( this.lightNameLabel );

            this.lightTypeComboBox = new CustomComboBox( ConstantValues.Light );
            this.lightTypeComboBox.FontSize = this.FontSize;
            this.lightTypeComboBox.Text = Translation.GetText("Light", "lightType");
            this.lightTypeComboBox.SelectedIndex = (int)this.light.type;
            this.lightTypeComboBox.SelectedIndexChanged += this.SwitchLightType;
            this.ChildControls.Add( this.lightTypeComboBox );

            this.lightEnableToggle = new CustomToggleButton( this.light.enabled );
            this.lightEnableToggle.FontSize = this.FontSize;
            this.lightEnableToggle.Text = Translation.GetText("Light", "lightEnable");
            this.lightEnableToggle.CheckedChanged += this.ToggleLight;
            this.ChildControls.Add( this.lightEnableToggle );

            this.lightDragToggle = new CustomToggleButton( false );
            this.lightDragToggle.FontSize = this.FontSize;
            this.lightDragToggle.Text = Translation.GetText("UI", "dragSource");
            this.lightDragToggle.CheckedChanged += this.DragLight;
            this.ChildControls.Add( this.lightDragToggle );

            this.lightRotationXSlider = new CustomSlider( this.light.transform.eulerAngles.x, 0f, 360f, 4 );
            this.lightRotationXSlider.FontSize = this.FontSize;
            this.lightRotationXSlider.Text = Translation.GetText("Light", "lightRotationX");
            this.lightRotationXSlider.ValueChanged += this.RotateLight;
            this.ChildControls.Add( this.lightRotationXSlider );

            this.lightRotationYSlider = new CustomSlider( this.light.transform.eulerAngles.y, 0f, 360f, 4 );
            this.lightRotationYSlider.FontSize = this.FontSize;
            this.lightRotationYSlider.ValueChanged += this.RotateLight;
            this.lightRotationYSlider.Text = Translation.GetText("Light", "lightRotationY");
            this.ChildControls.Add( this.lightRotationYSlider );

            this.lightRotationZSlider = new CustomSlider( this.light.transform.eulerAngles.z, 0f, 360f, 4 );
            this.lightRotationZSlider.ValueChanged += this.RotateLight;
            this.lightRotationZSlider.FontSize = this.FontSize;
            this.lightRotationZSlider.Text = Translation.GetText("Light", "lightRotationZ");
            this.ChildControls.Add( this.lightRotationZSlider );

            this.lightResetButton = new CustomButton();
            this.lightResetButton.FontSize = this.FontSize;
            this.lightResetButton.Text = Translation.GetText("UI", "reset");
            this.lightResetButton.Click += this.ResetLight;
            this.ChildControls.Add( this.lightResetButton );

            this.lightResetPosButton = new CustomButton();
            this.lightResetPosButton.FontSize = this.FontSize;
            this.lightResetPosButton.Text = Translation.GetText("Light", "lightResetPos");
            this.lightResetPosButton.Click += this.ResetLightPos;
            this.ChildControls.Add( this.lightResetPosButton );

            this.lightDeleteButton = new CustomButton();
            this.lightDeleteButton.FontSize = this.FontSize;
            this.lightDeleteButton.Text = Translation.GetText("UI", "delete");
            this.lightDeleteButton.Click += this.DeleteLight;
            this.ChildControls.Add( this.lightDeleteButton );

            this.lightIntensitySlider = new CustomSlider( this.light.intensity, 0, 3f, 3 );
            this.lightIntensitySlider.FontSize = this.FontSize;
            this.lightIntensitySlider.Text = Translation.GetText("Light", "lightIntensity");
            this.lightIntensitySlider.ValueChanged += this.ChangeIntensity;
            this.ChildControls.Add( this.lightIntensitySlider );

            this.lightBounceIntensitySlider = new CustomSlider( this.light.bounceIntensity, 0, 8f, 2 );
            this.lightBounceIntensitySlider.FontSize = this.FontSize;
            this.lightBounceIntensitySlider.Text = Translation.GetText("Light", "lightBounceIntensity");
            this.lightBounceIntensitySlider.ValueChanged += this.ChangeIntensity;
            this.ChildControls.Add( this.lightBounceIntensitySlider );

            this.lightRangeSlider = new CustomSlider( this.light.range, 0, 200f, 4 );
            this.lightRangeSlider.FontSize = this.FontSize;
            this.lightRangeSlider.Text = Translation.GetText("Light", "lightRange");
            this.lightRangeSlider.ValueChanged += this.ChangeRange;
            this.ChildControls.Add( this.lightRangeSlider );

            this.spotLightAngleSlider = new CustomSlider( this.light.spotAngle, 0, 360f, 4 );
            this.spotLightAngleSlider.FontSize = this.FontSize;
            this.spotLightAngleSlider.Text = Translation.GetText("Light", "spotLightAngle");
            this.spotLightAngleSlider.ValueChanged += this.ChangeSpotAngle;
            this.ChildControls.Add( this.spotLightAngleSlider );

            this.areaLightWidthSlider = new CustomSlider( 0, 0, 50f, 1 );
            this.areaLightWidthSlider.FontSize = this.FontSize;
            this.areaLightWidthSlider.Text = Translation.GetText("Light", "areaLightWidth");
            this.areaLightWidthSlider.ValueChanged += this.ChangeAreaSize;
            this.ChildControls.Add( this.areaLightWidthSlider );

            this.areaLightHeightSlider = new CustomSlider( 0, 0, 50f, 1 );
            this.areaLightHeightSlider.FontSize = this.FontSize;
            this.areaLightHeightSlider.Text = Translation.GetText("Light", "areaLightHeight");
            this.areaLightHeightSlider.ValueChanged += this.ChangeAreaSize;
            this.ChildControls.Add( this.areaLightHeightSlider );

            this.lightColorPicker = new CustomColorPicker( this.light.color );
            this.lightColorPicker.FontSize = this.FontSize;
            this.lightColorPicker.Text = Translation.GetText("Light", "lightColor");
            this.lightColorPicker.ColorChanged = this.ChangeColor;
            this.lightColorPicker.IsRGBA = false;
            this.ChildControls.Add( this.lightColorPicker );

            this.shadowStrengthSlider = new CustomSlider( this.light.shadowStrength, 0, 1f, 4 );
            this.shadowStrengthSlider.FontSize = this.FontSize;
            this.shadowStrengthSlider.Text = Translation.GetText("Light", "shadowStrength");
            this.shadowStrengthSlider.ValueChanged += this.ChangeShadowStrength;
            this.ChildControls.Add( this.shadowStrengthSlider );

            this.shadowsBox = new CustomComboBox( LIGHT_SHADOWS );
            this.shadowsBox.Text = Translation.GetText("Light", "shadows");
            this.shadowsBox.SelectedIndex = (int)this.light.shadows;
            this.shadowsBox.SelectedIndexChanged += this.ChangeShadowType;
            this.ChildControls.Add( this.shadowsBox );

            this.shadowBiasSlider = new CustomSlider( this.light.shadowBias, 0, 2f, 4 );
            this.shadowBiasSlider.FontSize = this.FontSize;
            this.shadowBiasSlider.Text = Translation.GetText("Light", "shadowBias");
            this.shadowBiasSlider.ValueChanged += this.ChangeShadowBias;
            this.ChildControls.Add( this.shadowBiasSlider );

            this.shadowNormalBiasSlider = new CustomSlider( this.light.shadowNormalBias, 0, 3f, 4 );
            this.shadowNormalBiasSlider.FontSize = this.FontSize;
            this.shadowNormalBiasSlider.Text = Translation.GetText("Light", "shadowNormalBias");
            this.shadowNormalBiasSlider.ValueChanged += this.ChangeShadowNormalBias;
            this.ChildControls.Add( this.shadowNormalBiasSlider );
        }

        override public void Update()
        {
            if( !this.isDrag ) {
                this.dragManager.SetTransform(this.light.transform);
            }

            Vector3 newPos = new Vector3(this.dragManager.goDrag.transform.position.x,
                                         this.dragManager.goDrag.transform.position.y,
                                         this.dragManager.goDrag.transform.position.z);
            this.light.transform.position = newPos;
            this.wasChanged = true;
        }

        override public void OnGUI()
        {
            this.SetAllVisible(false, 0);

            this.lightNameLabel.Left = this.Left + ControlBase.FixedMargin;
            this.lightNameLabel.Top = this.Top + ControlBase.FixedMargin;
            this.lightNameLabel.Width = this.Width / 2 - ControlBase.FixedMargin / 4;
            this.lightNameLabel.Height = this.ControlHeight;
            this.lightNameLabel.Text = this.Text;
            this.lightNameLabel.OnGUI();

            GUIUtil.AddGUICheckbox(this, this.lightTypeComboBox );

            bool isMainLight = this.Text == Translation.GetText("UI", "mainLight");
            int buttonCount = isMainLight ? 3 : 5;

            GUIUtil.AddGUIButtonAfter(this, this.lightResetButton, this.lightTypeComboBox, buttonCount );
            this.lightResetButton.Height = this.ControlHeight;

            GUIUtil.AddGUIButton(this, this.lightResetPosButton, this.lightResetButton, buttonCount );
            GUIUtil.AddGUIButton(this, this.lightDragToggle, this.lightResetPosButton, buttonCount );

            if( !isMainLight )
            {
                GUIUtil.AddGUIButton(this, this.lightEnableToggle, this.lightDragToggle, buttonCount );
                GUIUtil.AddGUIButton(this, this.lightDeleteButton, this.lightEnableToggle, buttonCount );
            }

            GUIUtil.AddGUISlider(this, this.lightRotationXSlider );
            GUIUtil.AddGUISlider(this, this.lightRotationYSlider );
            // GUIUtil.AddGUISlider(this, this.lightRotationZSlider );
            GUIUtil.AddGUISlider(this, this.lightIntensitySlider );
            // GUIUtil.AddGUISlider(this, this.lightBounceIntensitySlider );

            if( this.lightTypeComboBox.SelectedItem != "Directional" )
            {
                GUIUtil.AddGUISlider(this, this.lightRangeSlider );
            }

            if( this.lightTypeComboBox.SelectedItem == "Spot" )
            {
                GUIUtil.AddGUISlider(this, this.spotLightAngleSlider );
            }

            GUIUtil.AddGUICheckbox(this, this.lightColorPicker );

            if( this.lightTypeComboBox.SelectedItem == "Directional" )
            {
                GUIUtil.AddGUICheckbox(this, this.shadowsBox );
                if( this.shadowsBox.SelectedItem != "None" )
                {
                    GUIUtil.AddGUISlider(this, this.shadowStrengthSlider );
                    GUIUtil.AddGUISlider(this, this.shadowBiasSlider );
                    GUIUtil.AddGUISlider(this, this.shadowNormalBiasSlider );
                }
            }

            foreach(ControlBase control in this.ChildControls)
            {
                // TODO: make this always happen
                control.ScreenPos = this.ScreenPos;
            }

            this.Height = GUIUtil.GetHeightForParent(this);
        }

        public void UpdateFromLight()
        {
            // prevent rotation slider callback from firing
            if(!this.updating) {
                this.updating = true;

                this.lightTypeComboBox.SelectedIndex = (int)light.type;
                this.lightIntensitySlider.Value = light.intensity;
                this.lightEnableToggle.Value = light.enabled;
                this.lightDragToggle.Value = false;
                this.lightRotationXSlider.Value = light.transform.eulerAngles.x;
                this.lightRotationYSlider.Value = light.transform.eulerAngles.y;
                this.lightRotationZSlider.Value = light.transform.eulerAngles.z;
                this.lightRangeSlider.Value = light.range;
                this.spotLightAngleSlider.Value = light.spotAngle;
                this.lightColorPicker.Value = light.color;
                this.shadowsBox.SelectedIndex = (int)light.shadows;
                this.shadowStrengthSlider.Value = light.shadowStrength;
                this.shadowBiasSlider.Value = light.shadowBias;
                this.shadowNormalBiasSlider.Value = light.shadowNormalBias;

                this.updating = false;
            }
        }

        private void SwitchLightType( object sender, EventArgs args)
        {
            LightType type = ( LightType )Enum.Parse( typeof( LightType ), this.lightTypeComboBox.SelectedItem );
            this.light.type = type;
            if( this.light.type == LightType.Directional && this.isDrag == true )
            {
                StopDrag();
            }
            this.wasChanged = true;
        }

        private void ToggleLight( object sender, EventArgs args )
        {
            this.light.enabled = this.lightEnableToggle.Value;
            this.wasChanged = true;
        }

        private void DragLight( object sender, EventArgs args )
        {
            if(this.changingDrag == false) {
                this.changingDrag = true;
                if( this.lightDragToggle.Value == false )
                {
                    StopDrag();
                }
                else
                {
                    StartDrag();
                }
                this.changingDrag = false;
                this.wasChanged = true;
            }
        }

        private void RotateLight( object sender, EventArgs args )
        {
            if( !this.updating ) {
                this.light.transform.eulerAngles = new Vector3( this.lightRotationXSlider.Value,
                                                                this.lightRotationYSlider.Value,
                                                                this.lightRotationZSlider.Value );
                this.wasChanged = true;
            }
        }

        private void ResetLight( object sender, EventArgs args )
        {
            this.resetRequested = true;
        }

        private void ResetLightPos( object sender, EventArgs args )
        {
            this.light.transform.position = new Vector3(0, 2, 0);
            this.dragManager.SetTransform(this.light.transform);

            this.wasChanged = true;
        }

        private void DeleteLight( object sender, EventArgs args )
        {
            StopDrag();

            this.deleteRequested = true;
        }

        private void ChangeIntensity( object sender, EventArgs args )
        {
            this.light.intensity = this.lightIntensitySlider.Value;
            this.wasChanged = true;
        }

        private void ChangeBounceIntensity( object sender, EventArgs args )
        {
            this.light.bounceIntensity = this.lightBounceIntensitySlider.Value;
            this.wasChanged = true;
        }

        private void ChangeRange( object sender, EventArgs args )
        {
            this.light.range = this.lightRangeSlider.Value;
            this.wasChanged = true;
        }

        private void ChangeSpotAngle( object sender, EventArgs args )
        {
            this.light.spotAngle = this.spotLightAngleSlider.Value;
            this.wasChanged = true;
        }

        private void ChangeAreaSize( object sender, EventArgs args )
        {
            // this.light.areaSize = new Vector2( this.areaLightWidthSlider.Value, this.areaLightHeightSlider.Value );
            this.wasChanged = true;
        }

        private void ChangeColor( object sender, EventArgs args )
        {
            this.light.color = this.lightColorPicker.Value;
            this.wasChanged = true;
        }

        private void ChangeShadowType( object sender, EventArgs args )
        {
            this.light.shadows = (LightShadows)Enum.Parse( typeof( LightShadows ), this.shadowsBox.SelectedItem);
            this.wasChanged = true;
        }

        private void ChangeShadowStrength( object sender, EventArgs args )
        {
            this.light.shadowStrength = this.shadowStrengthSlider.Value;
            this.wasChanged = true;
        }

        private void ChangeShadowBias( object sender, EventArgs args )
        {
            this.light.shadowBias = this.shadowBiasSlider.Value;
            this.wasChanged = true;
        }

        private void ChangeShadowNormalBias( object sender, EventArgs args )
        {
            this.light.shadowNormalBias = this.shadowNormalBiasSlider.Value;
            this.wasChanged = true;
        }

        public void StartDrag()
        {
            this.isDrag = true;
            this.lightDragToggle.Value = true;
            this.dragManager.StartDrag();
        }

        public void StopDrag()
        {
            this.isDrag = false;
            this.lightDragToggle.Value = false;
            this.dragManager.StopDrag();
        }

        #region Properties
        public float ControlHeight
        {
            get
            {
                return this.FixedFontSize + ControlBase.FixedMargin * 1;
            }
        }

        // public LightInfo LightInfoValue
        // {

        // }

        public Light LightValue
        {
            get
            {
                return this.light;
            }
            set
            {
                this.light = value;
            }
        }

        public bool IsDeleteRequested
        {
            get
            {
                return this.deleteRequested;
            }
        }

        public bool WasChanged
        {
            get
            {
                return this.wasChanged;
            }
            set
            {
                this.wasChanged = value;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] LIGHT_SHADOWS = new string[] { "None", "Hard", "Soft" };

        private Light light = null;
        private DragManager dragManager = null;
        private bool isDrag;
        private bool deleteRequested;
        public bool resetRequested { get; set; }
        private bool wasChanged;
        private bool changingDrag;
        private bool updating = false;

        private CustomLabel lightNameLabel = null;
        private CustomComboBox lightTypeComboBox = null;
        private CustomToggleButton lightEnableToggle = null;
        private CustomButton lightResetButton = null;
        private CustomButton lightResetPosButton = null;
        private CustomButton lightDeleteButton = null;
        private CustomToggleButton lightDragToggle = null;

        private CustomSlider lightRotationXSlider = null;
        private CustomSlider lightRotationYSlider = null;
        private CustomSlider lightRotationZSlider = null;

        private CustomComboBox shadowsBox = null;
        private CustomSlider shadowStrengthSlider = null;
        private CustomSlider shadowBiasSlider = null;
        private CustomSlider shadowNormalBiasSlider = null;

        private CustomSlider lightIntensitySlider = null;
        private CustomSlider lightBounceIntensitySlider = null;
        private CustomSlider lightRangeSlider = null;
        private CustomSlider spotLightAngleSlider = null;
        private CustomSlider areaLightWidthSlider = null;
        private CustomSlider areaLightHeightSlider = null;
        private CustomColorPicker lightColorPicker = null;
        #endregion
    }
}
