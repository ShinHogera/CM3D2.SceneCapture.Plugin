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
    internal class ScreenSpaceReflectionsPane : BasePane
    {
        public ScreenSpaceReflectionsPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "ScreenSpaceReflections") ) {}

        override public void SetupPane()
        {
            this.sampleCountComboBox = new CustomComboBox ( SSR_SAMPLE_COUNTS );
            this.sampleCountComboBox.Text = Translation.GetText("ScreenSpaceReflections", "sampleCount");
            this.sampleCountComboBox.SelectedIndex = (int)ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.sampleCount;
            this.ChildControls.Add( this.sampleCountComboBox );

            this.downsamplingSlider = new CustomSlider( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.downsampling, 1f, 8f, 0 );
            this.downsamplingSlider.Text = Translation.GetText("ScreenSpaceReflections", "downsampling");
            this.ChildControls.Add( this.downsamplingSlider );

            this.intensitySlider = new CustomSlider( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.intensity, 0.0f, 2.0f, 4 );
            this.intensitySlider.Text = Translation.GetText("ScreenSpaceReflections", "intensity");
            this.ChildControls.Add( this.intensitySlider );

            this.rayDiffusionSlider = new CustomSlider( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.rayDiffusion, 0.0f, 1.0f, 4 );
            this.rayDiffusionSlider.Text = Translation.GetText("ScreenSpaceReflections", "rayDiffusion");
            this.ChildControls.Add( this.rayDiffusionSlider );

            this.blurSizeSlider = new CustomSlider( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.blurSize, 0.0f, 8.0f, 4 );
            this.blurSizeSlider.Text = Translation.GetText("ScreenSpaceReflections", "blurSize");
            this.ChildControls.Add( this.blurSizeSlider );

            this.raymarchDistanceSlider = new CustomSlider( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.raymarchDistance, 0f, 10f, 4 );
            this.raymarchDistanceSlider.Text = Translation.GetText("ScreenSpaceReflections", "raymarchDistance");
            this.ChildControls.Add( this.raymarchDistanceSlider );

            this.falloffDistanceSlider = new CustomSlider( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.falloffDistance, 0f, 10f, 4 );
            this.falloffDistanceSlider.Text = Translation.GetText("ScreenSpaceReflections", "falloffDistance");
            this.ChildControls.Add( this.falloffDistanceSlider );

            this.rayHitDistanceSlider = new CustomSlider( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.rayHitDistance, 0f, 10f, 4 );
            this.rayHitDistanceSlider.Text = Translation.GetText("ScreenSpaceReflections", "rayHitDistance");
            this.ChildControls.Add( this.rayHitDistanceSlider );

            this.maxAccumulationSlider = new CustomSlider( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.maxAccumulation, 0f, 100f, 2 );
            this.maxAccumulationSlider.Text = Translation.GetText("ScreenSpaceReflections", "maxAccumulation");
            this.ChildControls.Add( this.maxAccumulationSlider );

            this.stepBoostSlider = new CustomSlider( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.stepBoost, 0f, 10f, 4 );
            this.stepBoostSlider.Text = Translation.GetText("ScreenSpaceReflections", "stepBoost");
            this.ChildControls.Add( this.stepBoostSlider );

            this.dangerousSamplesCheckbox = new CustomToggleButton( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.dangerousSamples, "toggle" );
            this.dangerousSamplesCheckbox.Text = Translation.GetText("ScreenSpaceReflections", "dangerousSamples");
            this.ChildControls.Add( this.dangerousSamplesCheckbox );

            this.preRaymarchPassCheckbox = new CustomToggleButton( ScreenSpaceReflectionsDef.screenSpaceReflectionsEffect.preRaymarchPass, "toggle" );
            this.preRaymarchPassCheckbox.Text = Translation.GetText("ScreenSpaceReflections", "preRaymarchPass");
            this.ChildControls.Add( this.preRaymarchPassCheckbox );

        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.sampleCountComboBox);
           GUIUtil.AddGUISlider(this, this.downsamplingSlider);
           GUIUtil.AddGUISlider(this, this.intensitySlider);
           GUIUtil.AddGUISlider(this, this.rayDiffusionSlider);
           GUIUtil.AddGUISlider(this, this.blurSizeSlider);
           GUIUtil.AddGUISlider(this, this.raymarchDistanceSlider);
           GUIUtil.AddGUISlider(this, this.falloffDistanceSlider);
           GUIUtil.AddGUISlider(this, this.rayHitDistanceSlider);
           GUIUtil.AddGUISlider(this, this.maxAccumulationSlider);
           GUIUtil.AddGUISlider(this, this.stepBoostSlider);
           GUIUtil.AddGUICheckbox(this, this.dangerousSamplesCheckbox);
           GUIUtil.AddGUICheckbox(this, this.preRaymarchPassCheckbox);
        }

        override public void Reset()
        {
            ScreenSpaceReflectionsDef.Reset();
        }

        #region Properties
        public ScreenSpaceReflections.SampleCount SampleCountValue
        {
            get
            {
                return (ScreenSpaceReflections.SampleCount)  Enum.Parse( typeof( ScreenSpaceReflections.SampleCount ), this.sampleCountComboBox.SelectedItem);
            }
        }

        public int DownsamplingValue
        {
            get
            {
                return (int)this.downsamplingSlider.Value;
            }
        }

        public float IntensityValue
        {
            get
            {
                return this.intensitySlider.Value;
            }
        }

        public float RayDiffusionValue
        {
            get
            {
                return this.rayDiffusionSlider.Value;
            }
        }

        public float BlurSizeValue
        {
            get
            {
                return this.blurSizeSlider.Value;
            }
        }

        public float RaymarchDistanceValue
        {
            get
            {
                return this.raymarchDistanceSlider.Value;
            }
        }

        public float FalloffDistanceValue
        {
            get
            {
                return this.falloffDistanceSlider.Value;
            }
        }

        public float RayHitDistanceValue
        {
            get
            {
                return this.rayHitDistanceSlider.Value;
            }
        }

        public float MaxAccumulationValue
        {
            get
            {
                return this.maxAccumulationSlider.Value;
            }
        }

        public float StepBoostValue
        {
            get
            {
                return this.stepBoostSlider.Value;
            }
        }

        public bool DangerousSamplesValue
        {
            get
            {
                return this.dangerousSamplesCheckbox.Value;
            }
        }

        public bool PreRaymarchPassValue
        {
            get
            {
                return this.preRaymarchPassCheckbox.Value;
            }
        }

        #endregion

        #region Fields
        private static readonly string[] SSR_SAMPLE_COUNTS = new string[] { "Low", "Medium", "High" };

        private CustomComboBox sampleCountComboBox = null;
        private CustomSlider downsamplingSlider = null;
        private CustomSlider intensitySlider = null;
        private CustomSlider rayDiffusionSlider = null;
        private CustomSlider blurSizeSlider = null;
        private CustomSlider raymarchDistanceSlider = null;
        private CustomSlider falloffDistanceSlider = null;
        private CustomSlider rayHitDistanceSlider = null;
        private CustomSlider maxAccumulationSlider = null;
        private CustomSlider stepBoostSlider = null;
        private CustomToggleButton dangerousSamplesCheckbox = null;
        private CustomToggleButton preRaymarchPassCheckbox = null;
        #endregion
    }
}
