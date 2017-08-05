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
    internal class BloomPane : BasePane
    {
        public BloomPane( int fontSize ) : base( fontSize, "Bloom" ) {}

        override public void SetupPane()
        {
            if( BloomDef.bloomEffect == null )
            {
                BloomDef.Setup();
            }

            this.hdrBox = new CustomComboBox( BLOOM_HDRMODE );
            this.hdrBox.Text = "HDR";
            this.hdrBox.SelectedIndex = (int)BloomDef.bloomEffect.hdr;
            this.ChildControls.Add( this.hdrBox );

            this.screenBlendModeBox = new CustomComboBox( BLOOM_SCREENBLENDMODE );
            this.screenBlendModeBox.Text = "Blend";
            this.screenBlendModeBox.SelectedIndex = (int)BloomDef.bloomEffect.screenBlendMode;
            this.ChildControls.Add( this.screenBlendModeBox );

            this.tweakModeBox = new CustomComboBox( BLOOM_TWEAKMODE );
            this.tweakModeBox.Text = "Mode";
            this.tweakModeBox.SelectedIndex = (int)BloomDef.bloomEffect.tweakMode;
            this.ChildControls.Add( this.tweakModeBox );

            this.qualityCheckbox = new CustomToggleButton( false, "toggle" );
            this.qualityCheckbox.Text = "Complex";
            this.ChildControls.Add( this.qualityCheckbox );

            this.bloomIntensitySlider = new CustomSlider( GameMain.Instance.CMSystem.BloomValue, 0f, 100f, 1 );
            this.bloomIntensitySlider.Text = "Intensity";
            this.ChildControls.Add( this.bloomIntensitySlider  );
            this.bloomThreshholdSlider = new CustomSlider( BloomDef.bloomEffect.bloomThreshhold, 0f, 1f, 1 );
            this.bloomThreshholdSlider.Text = "Threshold";
            this.ChildControls.Add( this.bloomThreshholdSlider  );
            this.bloomBlurIterationsSlider = new CustomSlider( BloomDef.bloomEffect.bloomBlurIterations, 1f, 10f, 1 );
            this.bloomBlurIterationsSlider.Text = "Blur Iterations";
            this.ChildControls.Add( this.bloomBlurIterationsSlider  );

            this.sepBlurSpreadSlider = new CustomSlider( BloomDef.bloomEffect.sepBlurSpread, 0f, 10f, 1 );
            this.sepBlurSpreadSlider.Text = "Spread";
            this.ChildControls.Add( this.sepBlurSpreadSlider  );

            this.bloomThreshholdColorPicker = new CustomColorPicker( BloomDef.bloomEffect.bloomThreshholdColor );
            this.bloomThreshholdColorPicker.Text = "Threshold Color";
            this.ChildControls.Add( this.bloomThreshholdColorPicker  );

            this.lensflareModeBox = new CustomComboBox( BLOOM_LENSFLARESTYLE );
            this.lensflareModeBox.Text = "Lensflare Mode";
            this.lensflareModeBox.SelectedIndex = (int)BloomDef.bloomEffect.lensflareMode;
            this.ChildControls.Add( this.lensflareModeBox );

            this.lensflareIntensitySlider = new CustomSlider( BloomDef.bloomEffect.lensflareIntensity, 0f, 10f, 1 );
            this.lensflareIntensitySlider.Text = "Lensflare Intensity";
            this.ChildControls.Add( this.lensflareIntensitySlider  );
            this.lensFlareSaturationSlider = new CustomSlider( BloomDef.bloomEffect.lensFlareSaturation, 0f, 1f, 1 );
            this.lensFlareSaturationSlider.Text = "Lensflare Saturation";
            this.ChildControls.Add( this.lensFlareSaturationSlider  );
            this.lensflareThreshholdSlider = new CustomSlider( BloomDef.bloomEffect.lensflareThreshhold, 0f, 1f, 1 );
            this.lensflareThreshholdSlider.Text = "Lensflare Threshold";
            this.ChildControls.Add( this.lensflareThreshholdSlider  );

            this.flareRotationSlider = new CustomSlider( BloomDef.bloomEffect.flareRotation, 0f, 50f, 1 );
            this.flareRotationSlider.Text = "Lensflare Rotation";
            this.ChildControls.Add( this.flareRotationSlider  );

            this.blurWidthSlider = new CustomSlider( BloomDef.bloomEffect.blurWidth, 0f, 50f, 1 );
            this.blurWidthSlider.Text = "Blur Width";
            this.ChildControls.Add( this.blurWidthSlider  );

            this.hollyStretchWidthSlider = new CustomSlider( BloomDef.bloomEffect.hollyStretchWidth, 0f, 10f, 1 );
            this.hollyStretchWidthSlider.Text = "Hollywood Stretch Width";
            this.ChildControls.Add( this.hollyStretchWidthSlider  );
            this.hollywoodFlareBlurIterationsSlider = new CustomSlider( BloomDef.bloomEffect.hollywoodFlareBlurIterations, 0f, 10f, 1 );
            this.hollywoodFlareBlurIterationsSlider.Text = "Hollywood Blur Iterations";
            this.ChildControls.Add( this.hollywoodFlareBlurIterationsSlider  );

            this.flareColorAPicker = new CustomColorPicker( BloomDef.bloomEffect.flareColorA );
            this.ChildControls.Add( this.flareColorAPicker  );
            this.flareColorBPicker = new CustomColorPicker( BloomDef.bloomEffect.flareColorB );
            this.ChildControls.Add( this.flareColorBPicker  );
            this.flareColorCPicker = new CustomColorPicker( BloomDef.bloomEffect.flareColorC );
            this.ChildControls.Add( this.flareColorCPicker  );
            this.flareColorDPicker = new CustomColorPicker( BloomDef.bloomEffect.flareColorD );
            this.ChildControls.Add( this.flareColorDPicker  );
        }

        override public void Update()
        {
            try
            {
                if( !Instances.needEffectWindowReload )
                {
                    if( this.IsEnabled != BloomDef.enable )
                    {
                        Debug.Log("Bloom " + (this.IsEnabled) + " " + (BloomDef.enable));
                        if (this.IsEnabled)
                        {
                            // BloomDef.BackUp();
                            BloomDef.Reset();
                        }
                        else
                        {
                            BloomDef.Restore();
                        }
                        BloomDef.enable = this.IsEnabled;
                        BloomDef.enabledInPane = this.IsEnabled;
                    }
                }
                else
                {
                    BloomDef.Restore();
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.tweakModeBox);
            GUIUtil.AddGUICheckbox(this, this.screenBlendModeBox);
            GUIUtil.AddGUICheckbox(this, this.hdrBox);
            GUIUtil.AddGUICheckbox(this, this.qualityCheckbox);
            GUIUtil.AddGUISlider(this, this.bloomIntensitySlider);
            GUIUtil.AddGUISlider(this, this.bloomThreshholdSlider);
            GUIUtil.AddGUISlider(this, this.bloomBlurIterationsSlider);
            GUIUtil.AddGUISlider(this, this.sepBlurSpreadSlider);

            GUIUtil.AddGUICheckbox(this, this.bloomThreshholdColorPicker);
            GUIUtil.AddGUICheckbox(this, this.lensflareModeBox);
            GUIUtil.AddGUISlider(this, this.lensflareIntensitySlider);
            GUIUtil.AddGUISlider(this, this.lensFlareSaturationSlider);
            GUIUtil.AddGUISlider(this, this.lensflareThreshholdSlider);
            GUIUtil.AddGUISlider(this, this.flareRotationSlider);
            GUIUtil.AddGUISlider(this, this.hollyStretchWidthSlider);
            GUIUtil.AddGUISlider(this, this.hollywoodFlareBlurIterationsSlider);

            GUIUtil.AddGUICheckbox(this, this.flareColorAPicker);
            GUIUtil.AddGUICheckbox(this, this.flareColorBPicker);
            GUIUtil.AddGUICheckbox(this, this.flareColorCPicker);
            GUIUtil.AddGUICheckbox(this, this.flareColorDPicker);
        }

        override public void Reset()
        {

        }

        #region Properties
        public Bloom.HDRBloomMode HdrValue
        {
            get
            {
                return (Bloom.HDRBloomMode)Enum.Parse( typeof( Bloom.HDRBloomMode ), this.hdrBox.SelectedItem);
            }
        }

        public Bloom.BloomScreenBlendMode ScreenBlendModeValue
        {
            get
            {
                return (Bloom.BloomScreenBlendMode)Enum.Parse( typeof( Bloom.BloomScreenBlendMode ), this.screenBlendModeBox.SelectedItem);
            }
        }

        public Bloom.TweakMode TweakModeValue
        {
            get
            {
                return (Bloom.TweakMode)Enum.Parse( typeof( Bloom.TweakMode ), this.tweakModeBox.SelectedItem);
            }
        }

        public Bloom.BloomQuality QualityValue
        {
            get
            {
                return this.qualityCheckbox.Value ? Bloom.BloomQuality.High : Bloom.BloomQuality.Cheap;
            }
        }

        public int BloomBlurIterationsValue
        {
            get
            {
                return (int)this.bloomBlurIterationsSlider.Value;
            }
        }

        public float BloomIntensityValue
        {
            get
            {
                return this.bloomIntensitySlider.Value;
            }
            set
            {
                this.bloomIntensitySlider.Value = value;
            }
        }

        public float BloomThreshholdValue
        {
            get
            {
                return this.bloomThreshholdSlider.Value;
            }
        }

        public float BlurWidthValue
        {
            get
            {
                return this.blurWidthSlider.Value;
            }
        }

        public float FlareRotationValue
        {
            get
            {
                return this.flareRotationSlider.Value;
            }
        }

        public float HollyStretchWidthValue
        {
            get
            {
                return this.hollyStretchWidthSlider.Value;
            }
        }

        public int HollywoodFlareBlurIterationsValue
        {
            get
            {
                return (int)this.hollywoodFlareBlurIterationsSlider.Value;
            }
        }

        public float LensflareIntensityValue
        {
            get
            {
                return this.lensflareIntensitySlider.Value;
            }
        }

        public Bloom.LensFlareStyle LensflareModeValue
        {
            get
            {
                return (Bloom.LensFlareStyle)Enum.Parse( typeof( Bloom.LensFlareStyle ), this.lensflareModeBox.SelectedItem);
            }
        }

        public float LensFlareSaturationValue
        {
            get
            {
                return this.lensFlareSaturationSlider.Value;
            }
        }

        public float LensflareThreshholdValue
        {
            get
            {
                return this.lensflareThreshholdSlider.Value;
            }
        }

        public float SepBlurSpreadValue
        {
            get
            {
                return this.sepBlurSpreadSlider.Value;
            }
        }

        public Color BloomThreshholdColorValue
        {
            get
            {
                return Color.white - this.bloomThreshholdColorPicker.Value;
            }
        }

        public Color FlareColorAValue
        {
            get
            {
                return this.flareColorAPicker.Value;
            }
        }

        public Color FlareColorBValue
        {
            get
            {
                return this.flareColorBPicker.Value;
            }
        }

        public Color FlareColorCValue
        {
            get
            {
                return this.flareColorCPicker.Value;
            }
        }

        public Color FlareColorDValue
        {
            get
            {
                return this.flareColorDPicker.Value;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] BLOOM_SCREENBLENDMODE = new string[] { "Screen", "Add" };
        private static readonly string[] BLOOM_HDRMODE = new string[] { "Auto", "On", "Off" };
        private static readonly string[] BLOOM_LENSFLARESTYLE = new string[] { "Ghosting", "Anamorphic", "Combined" };
        private static readonly string[] BLOOM_TWEAKMODE = new string[] { "Basic", "Complex" };

        private CustomComboBox hdrBox = null;
        private CustomComboBox screenBlendModeBox = null;
        private CustomComboBox tweakModeBox = null;
        private CustomToggleButton qualityCheckbox = null;

        private CustomSlider bloomBlurIterationsSlider = null;
        private CustomSlider bloomIntensitySlider = null;
        private CustomSlider bloomThreshholdSlider = null;

        private CustomSlider blurWidthSlider = null;

        private CustomSlider flareRotationSlider = null;

        private CustomSlider hollyStretchWidthSlider = null;
        private CustomSlider hollywoodFlareBlurIterationsSlider = null;

        private CustomComboBox lensflareModeBox = null;
        private CustomSlider lensflareIntensitySlider = null;
        private CustomSlider lensFlareSaturationSlider = null;
        private CustomSlider lensflareThreshholdSlider = null;
        private CustomColorPicker flareColorAPicker = null;
        private CustomColorPicker flareColorBPicker = null;
        private CustomColorPicker flareColorCPicker = null;
        private CustomColorPicker flareColorDPicker = null;
        private CustomSlider sepBlurSpreadSlider = null;

        private CustomColorPicker bloomThreshholdColorPicker = null;
        #endregion
    }
}
