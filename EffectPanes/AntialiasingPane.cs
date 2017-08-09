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
    internal class AntialiasingPane : BasePane
    {

        public AntialiasingPane( int fontSize ) : base( fontSize, "Antialiasing" ) {}

        override public void SetupPane()
        {
            this.modeBox = new CustomComboBox( AA_MODES );
            this.modeBox.Text = Translation.GetText("Antialiasing", "mode");
            this.modeBox.SelectedIndex = (int)AntialiasingDef.antialiasingEffect.mode;
            this.ChildControls.Add( this.modeBox );

            this.dlaaSharpCheckbox = new CustomToggleButton( false, "toggle" );
            this.dlaaSharpCheckbox.Text = Translation.GetText("Antialiasing", "dlaaSharp");
            this.ChildControls.Add( this.dlaaSharpCheckbox );

            this.showGeneratedNormalsCheckbox = new CustomToggleButton( false, "toggle" );
            this.showGeneratedNormalsCheckbox.Text = Translation.GetText("Antialiasing", "showGeneratedNormals");
            this.ChildControls.Add( this.showGeneratedNormalsCheckbox );

            this.blurRadiusSlider = new CustomSlider( AntialiasingDef.antialiasingEffect.blurRadius, 0f, 50f, 1 );
            this.blurRadiusSlider.Text = Translation.GetText("Antialiasing", "blurRadius");
            this.ChildControls.Add( this.blurRadiusSlider );

            this.edgeSharpnessSlider = new CustomSlider( AntialiasingDef.antialiasingEffect.edgeSharpness, 0f, 10f, 1 );
            this.edgeSharpnessSlider.Text = Translation.GetText("Antialiasing", "edgeSharpness");
            this.ChildControls.Add( this.edgeSharpnessSlider );

            this.edgeThresholdSlider = new CustomSlider( AntialiasingDef.antialiasingEffect.edgeThreshold, 0f, 10f, 1 );
            this.edgeThresholdSlider.Text = Translation.GetText("Antialiasing", "edgeThreshold");
            this.ChildControls.Add( this.edgeThresholdSlider );

            this.edgeThresholdMinSlider = new CustomSlider( AntialiasingDef.antialiasingEffect.edgeThresholdMin, 0f, 10f, 1 );
            this.edgeThresholdMinSlider.Text = Translation.GetText("Antialiasing", "edgeThresholdMin");
            this.ChildControls.Add( this.edgeThresholdMinSlider );

            this.offsetScaleSlider = new CustomSlider( AntialiasingDef.antialiasingEffect.offsetScale, 0f, 10f, 1 );
            this.offsetScaleSlider.Text = Translation.GetText("Antialiasing", "offsetScale");
            this.ChildControls.Add( this.offsetScaleSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.modeBox);

            switch(this.ModeValue)
            {
                case AAMode.FXAA3Console:
                    GUIUtil.AddGUISlider(this, this.edgeThresholdMinSlider);
                    GUIUtil.AddGUISlider(this, this.edgeThresholdSlider);
                    break;

                case AAMode.NFAA:
                    GUIUtil.AddGUICheckbox(this, this.showGeneratedNormalsCheckbox);
                    GUIUtil.AddGUISlider(this, this.blurRadiusSlider);
                    GUIUtil.AddGUISlider(this, this.offsetScaleSlider);
                    break;

                case AAMode.DLAA:
                    GUIUtil.AddGUICheckbox(this, this.dlaaSharpCheckbox, this.modeBox);
                    break;

                case AAMode.FXAA2:
                case AAMode.FXAA1PresetA:
                case AAMode.FXAA1PresetB:
                case AAMode.SSAA:
                default:
                    break;
            }
        }

        override public void Reset()
        {

        }

        #region Properties
        public AAMode ModeValue
        {
            get
            {
                return (AAMode)Enum.Parse( typeof( AAMode ), this.modeBox.SelectedItem );
            }
        }

        public bool DlaaSharpValue
        {
            get
            {
                return this.dlaaSharpCheckbox.Value;
            }
        }

        public bool ShowGeneratedNormalsValue
        {
            get
            {
                return this.showGeneratedNormalsCheckbox.Value;
            }
        }

        public float BlurRadiusValue
        {
            get
            {
                return this.blurRadiusSlider.Value;
            }
        }

        public float EdgeSharpnessValue
        {
            get
            {
                return this.edgeSharpnessSlider.Value;
            }
        }

        public float EdgeThresholdValue
        {
            get
            {
                return this.edgeThresholdSlider.Value;
            }
        }

        public float EdgeThresholdMinValue
        {
            get
            {
                return this.edgeThresholdMinSlider.Value;
            }
        }

        public float OffsetScaleValue
        {
            get
            {
                return this.offsetScaleSlider.Value;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] AA_MODES = new string[] { "FXAA2", "FXAA3Console", "FXAA1PresetA", "FXAA1PresetB", "NFAA", "SSAA", "DLAA" };

        private CustomComboBox modeBox = null;
        private CustomToggleButton dlaaSharpCheckbox = null;
        private CustomToggleButton showGeneratedNormalsCheckbox = null;
        private CustomSlider blurRadiusSlider = null;
        private CustomSlider edgeSharpnessSlider = null;
        private CustomSlider edgeThresholdSlider = null;
        private CustomSlider edgeThresholdMinSlider = null;
        private CustomSlider offsetScaleSlider = null;
        #endregion
    }
}
