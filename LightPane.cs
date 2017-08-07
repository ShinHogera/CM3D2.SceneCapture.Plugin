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
            this.changingDrag = false;

            this.lightNameLabel = new CustomLabel();
            this.lightNameLabel.Text = this.Text;
            this.ChildControls.Add( this.lightNameLabel );

            this.lightTypeComboBox = new CustomComboBox( ConstantValues.Light );
            this.lightTypeComboBox.FontSize = this.FontSize;
            this.lightTypeComboBox.Text = "Type";
            this.lightTypeComboBox.SelectedIndex = (int)this.light.type;
            this.lightTypeComboBox.SelectedIndexChanged += this.SwitchLightType;
            this.ChildControls.Add( this.lightTypeComboBox );

            this.lightEnableToggle = new CustomToggleButton( this.light.enabled );
            this.lightEnableToggle.FontSize = this.FontSize;
            this.lightEnableToggle.Text = "Enable";
            this.lightEnableToggle.CheckedChanged += this.ToggleLight;
            this.ChildControls.Add( this.lightEnableToggle );

            this.lightDragToggle = new CustomToggleButton( false );
            this.lightDragToggle.FontSize = this.FontSize;
            this.lightDragToggle.Text = "DragSource";
            this.lightDragToggle.CheckedChanged += this.DragLight;
            this.ChildControls.Add( this.lightDragToggle );

            this.lightRotationXSlider = new CustomSlider( this.light.transform.eulerAngles.x, 0f, 360f, 4 );
            this.lightRotationXSlider.FontSize = this.FontSize;
            this.lightRotationXSlider.Text = "Rotation X";
            this.lightRotationXSlider.ValueChanged += this.RotateLight;
            this.ChildControls.Add( this.lightRotationXSlider );

            this.lightRotationYSlider = new CustomSlider( this.light.transform.eulerAngles.y, 0f, 360f, 4 );
            this.lightRotationYSlider.FontSize = this.FontSize;
            this.lightRotationYSlider.ValueChanged += this.RotateLight;
            this.lightRotationYSlider.Text = "Rotation Y";
            this.ChildControls.Add( this.lightRotationYSlider );

            this.lightRotationZSlider = new CustomSlider( this.light.transform.eulerAngles.z, 0f, 360f, 4 );
            this.lightRotationZSlider.ValueChanged += this.RotateLight;
            this.lightRotationZSlider.FontSize = this.FontSize;
            this.lightRotationZSlider.Text = "Rotation Z";
            this.ChildControls.Add( this.lightRotationZSlider );

            this.lightDeleteButton = new CustomButton();
            this.lightDeleteButton.FontSize = this.FontSize;
            this.lightDeleteButton.Text = "Delete";
            this.lightDeleteButton.Click += this.DeleteLight;
            this.ChildControls.Add( this.lightDeleteButton );

            this.lightIntensitySlider = new CustomSlider( this.light.intensity, 0, 3f, 2 );
            this.lightIntensitySlider.FontSize = this.FontSize;
            this.lightIntensitySlider.Text = "Intensity";
            this.lightIntensitySlider.ValueChanged += this.ChangeIntensity;
            this.ChildControls.Add( this.lightIntensitySlider );

            this.lightBounceIntensitySlider = new CustomSlider( this.light.bounceIntensity, 0, 8f, 2 );
            this.lightBounceIntensitySlider.FontSize = this.FontSize;
            this.lightBounceIntensitySlider.Text = "Bounce Intensity";
            this.lightBounceIntensitySlider.ValueChanged += this.ChangeIntensity;
            this.ChildControls.Add( this.lightBounceIntensitySlider );

            this.lightRangeSlider = new CustomSlider( this.light.range, 0, 200f, 1 );
            this.lightRangeSlider.FontSize = this.FontSize;
            this.lightRangeSlider.Text = "Range";
            this.lightRangeSlider.ValueChanged += this.ChangeRange;
            this.ChildControls.Add( this.lightRangeSlider );

            this.spotLightAngleSlider = new CustomSlider( this.light.spotAngle, 0, 360f, 1 );
            this.spotLightAngleSlider.FontSize = this.FontSize;
            this.spotLightAngleSlider.Text = "Spotlight Angle";
            this.spotLightAngleSlider.ValueChanged += this.ChangeSpotAngle;
            this.ChildControls.Add( this.spotLightAngleSlider );

            this.areaLightWidthSlider = new CustomSlider( 0, 0, 50f, 1 );
            this.areaLightWidthSlider.FontSize = this.FontSize;
            this.areaLightWidthSlider.Text = "Area Width";
            this.areaLightWidthSlider.ValueChanged += this.ChangeAreaSize;
            this.ChildControls.Add( this.areaLightWidthSlider );

            this.areaLightHeightSlider = new CustomSlider( 0, 0, 50f, 1 );
            this.areaLightHeightSlider.FontSize = this.FontSize;
            this.areaLightHeightSlider.Text = "Area Height";
            this.areaLightHeightSlider.ValueChanged += this.ChangeAreaSize;
            this.ChildControls.Add( this.areaLightHeightSlider );

            this.lightColorPicker = new CustomColorPicker( this.light.color );
            this.lightColorPicker.FontSize = this.FontSize;
            this.lightColorPicker.Text = "Color";
            this.lightColorPicker.ColorChanged = this.ChangeColor;
            this.ChildControls.Add( this.lightColorPicker );

            this.shadowStrengthSlider = new CustomSlider( this.light.shadowStrength, 0, 1f, 1 );
            this.shadowStrengthSlider.FontSize = this.FontSize;
            this.shadowStrengthSlider.Text = "Shadow Strength";
            this.shadowStrengthSlider.ValueChanged += this.ChangeShadowStrength;
            this.ChildControls.Add( this.shadowStrengthSlider );

            this.shadowsBox = new CustomComboBox( LIGHT_SHADOWS );
            this.shadowsBox.Text = "Shadow Type";
            this.shadowsBox.SelectedIndex = (int)this.light.shadows;
            this.shadowsBox.SelectedIndexChanged += this.ChangeShadowType;
            this.ChildControls.Add( this.shadowsBox );

            this.shadowBiasSlider = new CustomSlider( this.light.shadowBias, 0, 2f, 1 );
            this.shadowBiasSlider.FontSize = this.FontSize;
            this.shadowBiasSlider.Text = "Shadow Bias";
            this.shadowBiasSlider.ValueChanged += this.ChangeShadowBias;
            this.ChildControls.Add( this.shadowBiasSlider );

            this.shadowNormalBiasSlider = new CustomSlider( this.light.shadowNormalBias, 0, 3f, 1 );
            this.shadowNormalBiasSlider.FontSize = this.FontSize;
            this.shadowNormalBiasSlider.Text = "Shadow NormalBias";
            this.shadowNormalBiasSlider.ValueChanged += this.ChangeShadowNormalBias;
            this.ChildControls.Add( this.shadowNormalBiasSlider );
        }

        override public void Update()
        {
            if( !this.isDrag )
                return;

            this.dragManager.Drag();
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
            this.lightNameLabel.OnGUI();

            GUIUtil.AddGUICheckbox(this, this.lightTypeComboBox );

            if( this.lightTypeComboBox.SelectedItem != "Directional" )
            {
                GUIUtil.AddGUICheckbox(this, this.lightEnableToggle );
            }

            if( this.Text != ConstantValues.MainLightName )
            {
                GUIUtil.AddGUICheckbox(this, this.lightDeleteButton );
            }

            GUIUtil.AddGUICheckbox(this, this.lightDragToggle );
            GUIUtil.AddGUISlider(this, this.lightRotationXSlider );
            GUIUtil.AddGUISlider(this, this.lightRotationYSlider );
            // GUIUtil.AddGUISlider(this, this.lightRotationZSlider );
            GUIUtil.AddGUISlider(this, this.lightIntensitySlider );
            // GUIUtil.AddGUISlider(this, this.lightBounceIntensitySlider );
            GUIUtil.AddGUISlider(this, this.lightRangeSlider );

            if( this.lightTypeComboBox.SelectedItem == "Spot" )
            {
                GUIUtil.AddGUISlider(this, this.spotLightAngleSlider );
            }

            GUIUtil.AddGUICheckbox(this, this.lightColorPicker );

            if( this.lightTypeComboBox.SelectedItem == "Directional" )
            {
                GUIUtil.AddGUICheckbox(this, this.shadowsBox );
            }

            GUIUtil.AddGUISlider(this, this.shadowStrengthSlider );
            GUIUtil.AddGUISlider(this, this.shadowBiasSlider );
            GUIUtil.AddGUISlider(this, this.shadowNormalBiasSlider );

            foreach(ControlBase control in this.ChildControls)
            {
                // TODO: make this always happen
                control.ScreenPos = this.ScreenPos;
            }

            this.Height = GUIUtil.GetHeightForParent(this);
        }

        public void UpdateFromLight()
        {
            this.lightTypeComboBox.SelectedIndex = (int)light.type;
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
            this.light.transform.eulerAngles = new Vector3( this.lightRotationXSlider.Value,
                                                            this.lightRotationYSlider.Value,
                                                            this.lightRotationZSlider.Value );
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
        private LightInfo lightSetting = null;
        private DragManager dragManager = null;
        private bool isDrag;
        private bool deleteRequested;
        private bool wasChanged;
        private bool changingDrag;

        private CustomLabel lightNameLabel = null;
        private CustomComboBox lightTypeComboBox = null;
        private CustomToggleButton lightEnableToggle = null;
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
        private CustomButton lightResetButton = null;
        #endregion
    }
}
