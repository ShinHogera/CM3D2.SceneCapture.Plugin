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
    internal class TemporalSSAOPane : BasePane
    {
        public TemporalSSAOPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "TemporalSSAO") ) {}

        override public void SetupPane()
        {
            this.sampleCountComboBox = new CustomComboBox ( TSSAO_SAMPLE_COUNTS );
            this.sampleCountComboBox.Text = Translation.GetText("TemporalSSAO", "sampleCount");
            this.sampleCountComboBox.SelectedIndex = (int)TemporalSSAODef.temporalSSAOEffect.sampleCount;
            this.ChildControls.Add( this.sampleCountComboBox );

            this.downsamplingSlider = new CustomSlider( TemporalSSAODef.temporalSSAOEffect.downsampling, 1f, 8f, 0 );
            this.downsamplingSlider.Text = Translation.GetText("TemporalSSAO", "downsampling");
            this.ChildControls.Add( this.downsamplingSlider );

            this.radiusSlider = new CustomSlider( TemporalSSAODef.temporalSSAOEffect.radius, 0.0f, 5.0f, 4 );
            this.radiusSlider.Text = Translation.GetText("TemporalSSAO", "radius");
            this.ChildControls.Add( this.radiusSlider );

            this.intensitySlider = new CustomSlider( TemporalSSAODef.temporalSSAOEffect.intensity, 0.0f, 8.0f, 4 );
            this.intensitySlider.Text = Translation.GetText("TemporalSSAO", "intensity");
            this.ChildControls.Add( this.intensitySlider );

            this.blurSizeSlider = new CustomSlider( TemporalSSAODef.temporalSSAOEffect.blurSize, 0.0f, 8.0f, 4 );
            this.blurSizeSlider.Text = Translation.GetText("TemporalSSAO", "blurSize");
            this.ChildControls.Add( this.blurSizeSlider );

            this.dangerousSamplesCheckbox = new CustomToggleButton( TemporalSSAODef.temporalSSAOEffect.dangerousSamples, "toggle" );
            this.dangerousSamplesCheckbox.Text = Translation.GetText("TemporalSSAO", "dangerousSamples");
            this.ChildControls.Add( this.dangerousSamplesCheckbox );

            this.maxAccumulationSlider = new CustomSlider( TemporalSSAODef.temporalSSAOEffect.maxAccumulation, 0f, 100f, 2 );
            this.maxAccumulationSlider.Text = Translation.GetText("TemporalSSAO", "maxAccumulation");
            this.ChildControls.Add( this.maxAccumulationSlider );

        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.sampleCountComboBox);
           GUIUtil.AddGUISlider(this, this.downsamplingSlider);
           GUIUtil.AddGUISlider(this, this.radiusSlider);
           GUIUtil.AddGUISlider(this, this.intensitySlider);
           GUIUtil.AddGUISlider(this, this.blurSizeSlider);
           GUIUtil.AddGUICheckbox(this, this.dangerousSamplesCheckbox);
           GUIUtil.AddGUISlider(this, this.maxAccumulationSlider);
        }

        override public void Reset()
        {
            TemporalSSAODef.Reset();
        }

        #region Properties
        public TemporalSSAO.SampleCount  SampleCountValue
        {
            get
            {
                return (TemporalSSAO.SampleCount  ) Enum.Parse( typeof( TemporalSSAO.SampleCount   ), this.sampleCountComboBox.SelectedItem);
            }
        }

        public int DownsamplingValue
        {
            get
            {
                return (int)this.downsamplingSlider.Value;
            }
        }

        public float RadiusValue
        {
            get
            {
                return this.radiusSlider.Value;
            }
        }

        public float IntensityValue
        {
            get
            {
                return this.intensitySlider.Value;
            }
        }

        public float BlurSizeValue
        {
            get
            {
                return this.blurSizeSlider.Value;
            }
        }

        public bool DangerousSamplesValue
        {
            get
            {
                return this.dangerousSamplesCheckbox.Value;
            }
        }

        public float MaxAccumulationValue
        {
            get
            {
                return this.maxAccumulationSlider.Value;
            }
        }

        #endregion

        #region Fields
        private static readonly string[] TSSAO_SAMPLE_COUNTS = new string[] { "Low", "Medium", "High" };

        private CustomComboBox sampleCountComboBox = null;
        private CustomSlider blurSizeSlider = null;
        private CustomSlider downsamplingSlider = null;
        private CustomSlider radiusSlider = null;
        private CustomSlider intensitySlider = null;
        private CustomToggleButton dangerousSamplesCheckbox = null;
        private CustomSlider maxAccumulationSlider = null;
        #endregion
    }
}
