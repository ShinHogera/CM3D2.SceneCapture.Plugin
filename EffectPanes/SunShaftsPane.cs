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
    internal class SunShaftsPane : BasePane
    {
        public SunShaftsPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "SunShafts") ) {}

        override public void SetupPane()
        {
            this.resolutionBox = new CustomComboBox( SUNSHAFTS_RESOLUTIONS );
            this.resolutionBox.Text = Translation.GetText("SunShafts", "resolution");
            this.resolutionBox.SelectedIndex = (int)SunShaftsDef.sunShaftsEffect.resolution;
            this.ChildControls.Add( this.resolutionBox );
            this.screenBlendModeBox = new CustomComboBox( SUNSHAFTS_BLENDMODES );
            this.screenBlendModeBox.Text = Translation.GetText("SunShafts", "screenBlendMode");
            this.screenBlendModeBox.SelectedIndex = (int)SunShaftsDef.sunShaftsEffect.screenBlendMode;
            this.ChildControls.Add( this.screenBlendModeBox );

            this.useDepthTextureCheckbox = new CustomToggleButton( false, "toggle" );
            this.useDepthTextureCheckbox.Text = Translation.GetText("SunShafts", "useDepthTexture");
            this.ChildControls.Add( this.useDepthTextureCheckbox );

            this.dragSourceButton = new CustomButton();
            this.dragSourceButton.Text = Translation.GetText("UI", "dragSource");
            this.dragSourceButton.Click += (o, e) => {
                if( SunShaftsDef.isDrag )
                {
                    SunShaftsDef.StoptDrag();
                }
                else
                {
                    SunShaftsDef.StartDrag();
                }
            };
            this.ChildControls.Add( this.dragSourceButton );

            this.sunColorPicker = new CustomColorPicker( SunShaftsDef.sunShaftsEffect.sunColor );
            this.sunColorPicker.Text = Translation.GetText("SunShafts", "sunColor");
            this.sunColorPicker.IsRGBA = false;
            this.ChildControls.Add( this.sunColorPicker );

            this.maxRadiusSlider = new CustomSlider( SunShaftsDef.sunShaftsEffect.maxRadius, 0f, 1f, 1 );
            this.maxRadiusSlider.Text = Translation.GetText("SunShafts", "maxRadius");
            this.ChildControls.Add( this.maxRadiusSlider  );

            this.sunShaftBlurRadiusSlider = new CustomSlider( SunShaftsDef.sunShaftsEffect.sunShaftBlurRadius, -40f, 40, 1 );
            this.sunShaftBlurRadiusSlider.Text = Translation.GetText("SunShafts", "sunShaftBlurRadius");
            this.ChildControls.Add( this.sunShaftBlurRadiusSlider  );
            this.radialBlurIterationsSlider = new CustomSlider( SunShaftsDef.sunShaftsEffect.radialBlurIterations, 1f, 3f, 1 );
            this.radialBlurIterationsSlider.Text = Translation.GetText("SunShafts", "radialBlurIterations");
            this.ChildControls.Add( this.radialBlurIterationsSlider  );
            this.sunShaftIntensitySlider = new CustomSlider( SunShaftsDef.sunShaftsEffect.sunShaftIntensity, 0f, 20f, 1 );
            this.sunShaftIntensitySlider.Text = Translation.GetText("SunShafts", "sunShaftIntensity");
            this.ChildControls.Add( this.sunShaftIntensitySlider  );

            this.useSkyBoxAlphaSlider = new CustomSlider( SunShaftsDef.sunShaftsEffect.useSkyBoxAlpha, 0f, 50f, 1 );
            this.useSkyBoxAlphaSlider.Text = Translation.GetText("SunShafts", "useSkyBoxAlpha");
            this.ChildControls.Add( this.useSkyBoxAlphaSlider  );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.resolutionBox);
            GUIUtil.AddGUICheckbox(this, this.screenBlendModeBox);
            GUIUtil.AddGUICheckbox(this, this.useDepthTextureCheckbox);

            GUIUtil.AddGUICheckbox(this, this.dragSourceButton);
            GUIUtil.AddGUICheckbox(this, this.sunColorPicker);

            GUIUtil.AddGUISlider(this, this.maxRadiusSlider);
            GUIUtil.AddGUISlider(this, this.sunShaftBlurRadiusSlider);
            GUIUtil.AddGUISlider(this, this.radialBlurIterationsSlider);
            GUIUtil.AddGUISlider(this, this.sunShaftIntensitySlider);
            GUIUtil.AddGUISlider(this, this.useSkyBoxAlphaSlider);
        }

        override public void Reset()
        {

        }

        #region Properties
        public SunShaftsResolution ResolutionValue
        {
            get
            {
                return (SunShaftsResolution)Enum.Parse( typeof( SunShaftsResolution ), this.resolutionBox.SelectedItem);
            }
        }

        public ShaftsScreenBlendMode ScreenBlendModeValue
        {
            get
            {
                return (ShaftsScreenBlendMode)Enum.Parse( typeof( ShaftsScreenBlendMode ), this.screenBlendModeBox.SelectedItem);
            }
        }

        public bool UseDepthTextureValue
        {
            get
            {
                return this.useDepthTextureCheckbox.Value;
            }
        }

        public float MaxRadiusValue
        {
            get
            {
                return this.maxRadiusSlider.Value;
            }
        }

        public int RadialBlurIterationsValue
        {
            get
            {
                return (int)this.radialBlurIterationsSlider.Value;
            }
        }

        public float SunShaftBlurRadiusValue
        {
            get
            {
                return this.sunShaftBlurRadiusSlider.Value;
            }
        }

        public float SunShaftIntensityValue
        {
            get
            {
                return this.sunShaftIntensitySlider.Value;
            }
        }

        public float UseSkyBoxAlphaValue
        {
            get
            {
                return this.useSkyBoxAlphaSlider.Value;
            }
        }

        public Color SunColorValue
        {
            get
            {
                return this.sunColorPicker.Value;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] SUNSHAFTS_RESOLUTIONS = new string[] { "Low", "Normal", "High" };
        private static readonly string[] SUNSHAFTS_BLENDMODES = new string[] { "Screen", "Add" };

        private CustomComboBox resolutionBox = null;
        private CustomComboBox screenBlendModeBox = null;
        private CustomToggleButton useDepthTextureCheckbox = null;

        private CustomButton dragSourceButton = null;

        private CustomColorPicker sunColorPicker = null;

        private CustomSlider maxRadiusSlider = null;
        private CustomSlider radialBlurIterationsSlider = null;

        private CustomSlider sunShaftBlurRadiusSlider = null;
        private CustomSlider sunShaftIntensitySlider = null;

        private CustomSlider useSkyBoxAlphaSlider = null;
        #endregion
    }
}
