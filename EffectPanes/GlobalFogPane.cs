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
    internal class GlobalFogPane : BasePane
    {
        public GlobalFogPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "GlobalFog") ) {}

        override public void SetupPane()
        {
            if(GlobalFogDef.globalFogEffect == null)
            {
                GlobalFogDef.Setup();
            }

            this.fogModeBox = new CustomComboBox( FOG_MODES );
            this.fogModeBox.Text = Translation.GetText("GlobalFog", "fogMode");
            this.fogModeBox.SelectedIndex = (int)GlobalFogDef.globalFogEffect.fogMode;
            this.ChildControls.Add( this.fogModeBox );

            this.globalDensitySlider = new CustomSlider( GlobalFogDef.globalFogEffect.globalDensity, 0f, 5f, 1 );
            this.globalDensitySlider.Text = Translation.GetText("GlobalFog", "globalDensity");
            this.ChildControls.Add( this.globalDensitySlider );

            this.heightSlider = new CustomSlider( GlobalFogDef.globalFogEffect.height, 0f, 50f, 1 );
            this.heightSlider.Text = Translation.GetText("GlobalFog", "height");
            this.ChildControls.Add( this.heightSlider );

            this.heightScaleSlider = new CustomSlider( GlobalFogDef.globalFogEffect.heightScale, 1f, 10f, 1 );
            this.heightScaleSlider.Text = Translation.GetText("GlobalFog", "heightScale");
            this.ChildControls.Add( this.heightScaleSlider );

            this.startDistanceSlider = new CustomSlider( GlobalFogDef.globalFogEffect.startDistance, 0f, 50f, 1 );
            this.startDistanceSlider.Text = Translation.GetText("GlobalFog", "startDistance");
            this.ChildControls.Add( this.startDistanceSlider );

            this.globalFogColorPicker = new CustomColorPicker( GlobalFogDef.globalFogEffect.globalFogColor );
            this.globalFogColorPicker.Text = Translation.GetText("GlobalFog", "globalFogColor");
            this.globalFogColorPicker.IsRGBA = false;
            this.ChildControls.Add( this.globalFogColorPicker );

            this.adjustHeightScaleToggle = new CustomToggleButton( GlobalFogDef.AdjustHeightScale, "button" );
            this.adjustHeightScaleToggle.Text =Translation.GetText("GlobalFog", "adjustHeightScale");
            this.ChildControls.Add( this.adjustHeightScaleToggle );

            this.adjustStartDistanceToggle = new CustomToggleButton( GlobalFogDef.AdjustStartDistance, "button" );
            this.adjustStartDistanceToggle.Text = Translation.GetText("GlobalFog", "adjustStartDistance");
            this.ChildControls.Add( this.adjustStartDistanceToggle );

            setup = true;
        }

        override public void ShowPane()
        {
            if(!setup)
            {
                SetupPane();
                return;
            }

            GUIUtil.AddGUICheckbox(this, this.fogModeBox);
            GUIUtil.AddGUICheckbox(this, this.adjustStartDistanceToggle);
            GUIUtil.AddGUISlider(this, this.startDistanceSlider);
            GUIUtil.AddGUISlider(this, this.globalDensitySlider);
            GUIUtil.AddGUICheckbox(this, this.adjustHeightScaleToggle);
            GUIUtil.AddGUISlider(this, this.heightScaleSlider);
            GUIUtil.AddGUISlider(this, this.heightSlider);
            GUIUtil.AddGUICheckbox(this, this.globalFogColorPicker);
        }

        override public void Reset()
        {

        }

        #region Properties
        public GlobalFog.FogMode FogModeValue
        {
            get
            {
                return (GlobalFog.FogMode)Enum.Parse( typeof( GlobalFog.FogMode ), this.fogModeBox.SelectedItem);
            }
        }

        public bool AdjustHeightScaleValue
        {
            get
            {
                return this.adjustHeightScaleToggle.Value;
            }
        }

        public bool AdjustStartDistanceValue
        {
            get
            {
                return this.adjustStartDistanceToggle.Value;
            }
        }

        public float GlobalDensityValue
        {
            get
            {
                return this.globalDensitySlider.Value;
            }
        }

        public float HeightValue
        {
            get
            {
                return this.heightSlider.Value;
            }
        }

        public float HeightScaleValue
        {
            get
            {
                return this.heightScaleSlider.Value;
            }
        }

        public float StartDistanceValue
        {
            get
            {
                return this.startDistanceSlider.Value;
            }
        }

        public Color GlobalFogColorValue
        {
            get
            {
                return this.globalFogColorPicker.Value;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] FOG_MODES = new string[] { "AbsoluteYAndDistance", "AbsoluteY", "Distance", "RelativeYAndDistance" };

        private bool setup = false;
        private CustomComboBox fogModeBox = null;
        private CustomSlider globalDensitySlider = null;
        private CustomSlider heightSlider = null;
        private CustomSlider heightScaleSlider = null;
        private CustomSlider startDistanceSlider = null;
        private CustomColorPicker globalFogColorPicker = null;
        private CustomToggleButton adjustHeightScaleToggle = null;
        private CustomToggleButton adjustStartDistanceToggle = null;
        #endregion
    }
}
