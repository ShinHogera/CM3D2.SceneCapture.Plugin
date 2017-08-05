using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CM3D2.SceneCapture.Plugin
{
    internal class DepthOfFieldPane : BasePane {

        public DepthOfFieldPane( int fontSize ) : base( fontSize, "DepthOfField" ) {}

        override public void SetupPane()
        {
            this.visualizeFocusCheckbox = new CustomToggleButton( DepthOfFieldDef.depthOfFieldEffect.visualizeFocus, "toggle" );
            this.visualizeFocusCheckbox.Text = "Visualize Focus";
            this.ChildControls.Add( this.visualizeFocusCheckbox );

            this.focalLengthSlider = new CustomSlider( DepthOfFieldDef.depthOfFieldEffect.focalLength, 0f, 20f, 1 );
            this.focalLengthSlider.Text = "Focal Length";
            this.ChildControls.Add( this.focalLengthSlider );

            this.focalSizeSlider = new CustomSlider( DepthOfFieldDef.depthOfFieldEffect.focalSize, 0f, 2f, 1 );
            this.focalSizeSlider.Text = "Focal Size";
            this.ChildControls.Add( this.focalSizeSlider );

            this.apertureSlider = new CustomSlider( DepthOfFieldDef.depthOfFieldEffect.aperture, 0f, 60f, 1 );
            this.apertureSlider.Text = "Aperture";
            this.ChildControls.Add( this.apertureSlider );

            this.blurTypeCheckbox = new CustomToggleButton( false, "toggle" );
            this.blurTypeCheckbox.Text = "DX11 Blur";
            this.ChildControls.Add( this.blurTypeCheckbox );

            this.blurSampleCountBox = new CustomComboBox( DEPTH_BLURSAMPLECOUNTS );
            this.blurSampleCountBox.Text = "Blur Sample";
            this.blurSampleCountBox.SelectedIndex = (int)DepthOfFieldDef.depthOfFieldEffect.blurSampleCount;
            this.ChildControls.Add( this.blurSampleCountBox );

            this.maxBlurSizeSlider = new CustomSlider( DepthOfFieldDef.depthOfFieldEffect.maxBlurSize, 0f, 20f, 1 );
            this.maxBlurSizeSlider.Text = "Max Blur Size";
            this.ChildControls.Add( this.maxBlurSizeSlider );

            this.highResolutionCheckbox = new CustomToggleButton( DepthOfFieldDef.depthOfFieldEffect.highResolution, "toggle" );
            this.highResolutionCheckbox.Text = "High Resolution";
            this.ChildControls.Add( this.highResolutionCheckbox );

            this.nearBlurCheckbox = new CustomToggleButton( DepthOfFieldDef.depthOfFieldEffect.nearBlur, "toggle" );
            this.nearBlurCheckbox.Text = "Near Blur";
            this.ChildControls.Add( this.nearBlurCheckbox );

            this.foregroundOverlapSlider = new CustomSlider( DepthOfFieldDef.depthOfFieldEffect.foregroundOverlap, -20f, 20f, 1 );
            this.foregroundOverlapSlider.Text = "Foreground Overlap";
            this.ChildControls.Add( this.foregroundOverlapSlider );

            this.dx11BokehScaleSlider = new CustomSlider( DepthOfFieldDef.depthOfFieldEffect.dx11BokehScale, 0f, 10f, 1 );
            this.dx11BokehScaleSlider.Text = "Bokeh Scale";
            this.ChildControls.Add( this.dx11BokehScaleSlider );

            this.dx11BokehIntensitySlider = new CustomSlider( DepthOfFieldDef.depthOfFieldEffect.dx11BokehIntensity, 0f, 20f, 1 );
            this.dx11BokehIntensitySlider.Text = "Bokeh Intensity";
            this.ChildControls.Add( this.dx11BokehIntensitySlider );

            this.dx11BokehThreshholdSlider = new CustomSlider( DepthOfFieldDef.depthOfFieldEffect.dx11BokehThreshhold, 0f, 1f, 1 );
            this.dx11BokehThreshholdSlider.Text = "Bokeh Threshold";
            this.ChildControls.Add( this.dx11BokehThreshholdSlider );

            this.dx11SpawnHeuristicSlider = new CustomSlider( DepthOfFieldDef.depthOfFieldEffect.dx11SpawnHeuristic, 0f, 1f, 1 );
            this.dx11SpawnHeuristicSlider.Text = "Spawn Heuristic";
            this.ChildControls.Add( this.dx11SpawnHeuristicSlider );


            this.transformFromMaidCheckbox = new CustomToggleButton( DepthOfFieldDef.transformFromMaid, "toggle" );
            this.transformFromMaidCheckbox.Text = "Focal Transform";
            this.transformFromMaidCheckbox.CheckedChanged += (o, e) => {
                if( this.transformFromMaidCheckbox.Value == true )
                {
                    DepthOfFieldDef.SetTransform(this.maidManager);
                }
                else
                {
                    DepthOfFieldDef.SetTransform();
                }
            };
            this.ChildControls.Add( this.transformFromMaidCheckbox );

            this.prevMaidButton = new CustomButton();
            this.prevMaidButton.Text = "<";
            this.prevMaidButton.Click += (o, e) => {
                this.maidManager.Prev();
                DepthOfFieldDef.SetTransform(this.maidManager);
                this.maidManager.bUpdateRequest = true;
            };
            this.ChildControls.Add( this.prevMaidButton );

            this.nextMaidButton = new CustomButton();
            this.nextMaidButton.Text = ">";
            this.nextMaidButton.Click += (o, e) => {
                this.maidManager.Next();
                DepthOfFieldDef.SetTransform(this.maidManager);
                this.maidManager.bUpdateRequest = true;
            };
            this.ChildControls.Add( this.nextMaidButton );

            this.reloadMaidsButton = new CustomButton();
            this.reloadMaidsButton.Text = "Reload";
            this.reloadMaidsButton.Click += (o, e) => {
                this.maidManager.bUpdateRequest = true;
            };
            this.ChildControls.Add( this.reloadMaidsButton );

            this.maidManager = new MaidManager();
            this.maidManager.Find();

            this.focusMaidLabel = new CustomLabel();
            this.ChildControls.Add( this.focusMaidLabel );
        }

        override public void ShowPane()
        {
            this.SetAllVisible(false, 0);
            GUIUtil.AddGUICheckbox(this, this.visualizeFocusCheckbox);
            GUIUtil.AddGUICheckbox(this, this.blurTypeCheckbox);
            GUIUtil.AddGUICheckbox(this, this.transformFromMaidCheckbox);

            if( this.transformFromMaidCheckbox.Value == true )
            {
                // GUIUtil.AddGUICheckbox(this, this.reloadMaidsButton);
                this.reloadMaidsButton.Left = this.Left + this.FontSize * 11;
                this.reloadMaidsButton.Top = this.transformFromMaidCheckbox.Top + this.ControlHeight;
                this.reloadMaidsButton.Width = this.FontSize * 6;
                this.reloadMaidsButton.Height = this.ControlHeight;
                this.reloadMaidsButton.OnGUI();
                this.reloadMaidsButton.Visible = true;

                // GUIUtil.AddGUICheckbox(this, this.prevMaidButton);
                this.prevMaidButton.Left = this.Left + ControlBase.FixedMargin;
                this.prevMaidButton.Top = this.reloadMaidsButton.Top;
                this.prevMaidButton.Width = this.FontSize * 2;
                this.prevMaidButton.Height = this.ControlHeight;
                this.prevMaidButton.OnGUI();
                this.prevMaidButton.Visible = true;

                // GUIUtil.AddGUICheckbox(this, this.nextMaidButton);
                this.nextMaidButton.Left = this.prevMaidButton.Left + this.nextMaidButton.Width;
                this.nextMaidButton.Top = this.reloadMaidsButton.Top;
                this.nextMaidButton.Width = this.FontSize * 2;
                this.nextMaidButton.Height = this.ControlHeight;
                this.nextMaidButton.OnGUI();
                this.nextMaidButton.Visible = true;

                this.focusMaidLabel.Left = this.nextMaidButton.Left + this.nextMaidButton.Width + ControlBase.FixedMargin;
                this.focusMaidLabel.Top = this.nextMaidButton.Top;
                this.focusMaidLabel.Width = this.FontSize * 13;
                this.focusMaidLabel.Height = this.ControlHeight;
                this.focusMaidLabel.Text = this.maidManager.sCurrent;
                this.focusMaidLabel.OnGUI();
                this.focusMaidLabel.Visible = true;
            }
            else
            {
                GUIUtil.AddGUISlider(this, this.focalLengthSlider);
            }
            GUIUtil.AddGUISlider(this, this.focalSizeSlider);
            GUIUtil.AddGUISlider(this, this.apertureSlider);

            if( this.BlurTypeValue == DepthOfFieldScatter.BlurType.DiscBlur )
            {
                GUIUtil.AddGUICheckbox(this, this.blurSampleCountBox);
            }

            GUIUtil.AddGUISlider(this, this.maxBlurSizeSlider);

            GUIUtil.AddGUICheckbox(this, this.highResolutionCheckbox);
            GUIUtil.AddGUICheckbox(this, this.nearBlurCheckbox);

            GUIUtil.AddGUISlider(this, this.foregroundOverlapSlider);

            if( this.BlurTypeValue == DepthOfFieldScatter.BlurType.DX11 )
            {
                GUIUtil.AddGUISlider(this, this.dx11BokehScaleSlider);
                GUIUtil.AddGUISlider(this, this.dx11BokehIntensitySlider);
                GUIUtil.AddGUISlider(this, this.dx11BokehThreshholdSlider);
                GUIUtil.AddGUISlider(this, this.dx11SpawnHeuristicSlider);
            }

            this.focusMaidToggled = this.transformFromMaidCheckbox.Value;

            this.maidManager.Update();
        }

        override public void Reset()
        {

        }

        #region Properties
        public DepthOfFieldScatter.BlurType BlurTypeValue
        {
            get
            {
                return this.blurTypeCheckbox.Value ? DepthOfFieldScatter.BlurType.DX11 : DepthOfFieldScatter.BlurType.DiscBlur;
            }
        }

        public DepthOfFieldScatter.BlurSampleCount BlurSampleCountValue
        {
            get
            {
                return (DepthOfFieldScatter.BlurSampleCount)Enum.Parse( typeof( DepthOfFieldScatter.BlurSampleCount ), this.blurSampleCountBox.SelectedItem);
            }
        }

        public bool VisualizeFocusValue
        {
            get
            {
                return this.visualizeFocusCheckbox.Value;
            }
        }

        public bool HighResolutionValue
        {
            get
            {
                return this.highResolutionCheckbox.Value;
            }
        }

        public bool NearBlurValue
        {
            get
            {
                return this.nearBlurCheckbox.Value;
            }
        }

        public bool TransformFromMaidValue
        {
            get
            {
                return this.transformFromMaidCheckbox.Value;
            }
        }

        public float FocalLengthValue
        {
            get
            {
                return this.focalLengthSlider.Value;
            }
        }

        public float FocalSizeValue
        {
            get
            {
                return this.focalSizeSlider.Value;
            }
        }

        public float ApertureValue
        {
            get
            {
                return this.apertureSlider.Value;
            }
        }

        public float MaxBlurSizeValue
        {
            get
            {
                return this.maxBlurSizeSlider.Value;
            }
        }

        public float ForegroundOverlapValue
        {
            get
            {
                return this.foregroundOverlapSlider.Value;
            }
        }

        public float Dx11BokehScaleValue
        {
            get
            {
                return this.dx11BokehScaleSlider.Value;
            }
        }

        public float Dx11BokehIntensityValue
        {
            get
            {
                return this.dx11BokehIntensitySlider.Value;
            }
        }

        public float Dx11BokehThreshholdValue
        {
            get
            {
                return this.dx11BokehThreshholdSlider.Value;
            }
        }

        public float Dx11SpawnHeuristicValue
        {
            get
            {
                return this.dx11SpawnHeuristicSlider.Value;
            }
        }

        public MaidManager MaidManagerValue
        {
            get
            {
                return this.maidManager;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] DEPTH_BLURSAMPLECOUNTS = new string[] { "Low", "Medium", "High" };

        private CustomToggleButton visualizeFocusCheckbox = null;
        private CustomSlider focalLengthSlider = null;
        private CustomSlider focalSizeSlider = null;
        private CustomSlider apertureSlider = null;

        private CustomToggleButton blurTypeCheckbox = null;
        private CustomComboBox blurSampleCountBox = null;

        private CustomSlider maxBlurSizeSlider = null;
        private CustomToggleButton highResolutionCheckbox = null;

        private CustomToggleButton nearBlurCheckbox = null;
        private CustomSlider foregroundOverlapSlider = null;

        private CustomSlider dx11BokehScaleSlider = null;
        private CustomSlider dx11BokehIntensitySlider = null;
        private CustomSlider dx11BokehThreshholdSlider = null;
        private CustomSlider dx11SpawnHeuristicSlider = null;

        private CustomToggleButton transformFromMaidCheckbox = null;
        private MaidManager maidManager = null;
        private bool focusMaidToggled = false;

        private CustomButton prevMaidButton = null;
        private CustomButton nextMaidButton = null;
        private CustomButton reloadMaidsButton = null;
        private CustomLabel focusMaidLabel = null;
        #endregion
    }
}
