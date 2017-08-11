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
    internal class ObscurancePane : BasePane
    {
        public ObscurancePane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Obscurance") ) {}

        override public void SetupPane()
        {
            this.intensitySlider = new CustomSlider( ObscuranceDef.obscuranceEffect.intensity, 0f, 4f, 4 );
            this.intensitySlider.Text = Translation.GetText("Obscurance", "intensity");
            this.ChildControls.Add( this.intensitySlider );

            this.radiusSlider = new CustomSlider( ObscuranceDef.obscuranceEffect.radius, 0f, 2, 4 );
            this.radiusSlider.Text = Translation.GetText("Obscurance", "radius");
            this.ChildControls.Add( this.radiusSlider );

            this.sampleCountBox = new CustomComboBox( OBSCURANCE_SAMPLECOUNT );
            this.sampleCountBox.Text = Translation.GetText("Obscurance", "sampleCount");
            this.sampleCountBox.SelectedIndex = (int)ObscuranceDef.obscuranceEffect.sampleCount;
            this.ChildControls.Add( this.sampleCountBox );

            this.sampleCountValueSlider = new CustomSlider( ObscuranceDef.obscuranceEffect.sampleCountValue, 0f, 50f, 1 );
            this.sampleCountValueSlider.Text = Translation.GetText("Obscurance", "sampleCountValue");
            this.ChildControls.Add( this.sampleCountValueSlider );

            this.downsamplingCheckbox = new CustomToggleButton( ObscuranceDef.obscuranceEffect.downsampling, "toggle" );
            this.downsamplingCheckbox.Text = Translation.GetText("Obscurance", "downsampling");
            this.ChildControls.Add( this.downsamplingCheckbox );

            this.occlusionSourceBox = new CustomComboBox( OBSCURANCE_OCCLUSIONSOURCE );
            this.occlusionSourceBox.Text = Translation.GetText("Obscurance", "occlusionSource");
            this.occlusionSourceBox.SelectedIndex = (int)ObscuranceDef.obscuranceEffect.occlusionSource;
            this.ChildControls.Add( this.occlusionSourceBox );

            this.ambientOnlyCheckbox = new CustomToggleButton( ObscuranceDef.obscuranceEffect.ambientOnly, "toggle" );
            this.ambientOnlyCheckbox.Text = Translation.GetText("Obscurance", "ambientOnly");
            this.ChildControls.Add( this.ambientOnlyCheckbox );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.intensitySlider);
            GUIUtil.AddGUISlider(this, this.radiusSlider);
            GUIUtil.AddGUICheckbox(this, this.sampleCountBox);

            if( this.sampleCountBox.SelectedItem == "Variable" )
            {
                GUIUtil.AddGUISlider(this, this.sampleCountValueSlider);
            }

            GUIUtil.AddGUICheckbox(this, this.downsamplingCheckbox);
            GUIUtil.AddGUICheckbox(this, this.occlusionSourceBox);
            GUIUtil.AddGUICheckbox(this, this.ambientOnlyCheckbox);
        }

        override public void Reset()
        {

        }

        #region Properties
        public float IntensityValue
        {
            get
            {
                return this.intensitySlider.Value;
            }
        }

        public float RadiusValue
        {
            get
            {
                return this.radiusSlider.Value;
            }
        }

        public Obscurance.SampleCount SampleCountValue
        {
            get
            {
                return (Obscurance.SampleCount)Enum.Parse( typeof( Obscurance.SampleCount ), this.sampleCountBox.SelectedItem );
            }
        }

        public int SampleCountValueValue
        {
            get
            {
                return (int)this.sampleCountValueSlider.Value;
            }
        }

        public bool DownsamplingValue
        {
            get
            {
                return this.downsamplingCheckbox.Value;
            }
        }

        public Obscurance.OcclusionSource OcclusionSourceValue
        {
            get
            {
                return (Obscurance.OcclusionSource)Enum.Parse( typeof( Obscurance.OcclusionSource ), this.occlusionSourceBox.SelectedItem );
            }
        }

        public bool AmbientOnlyValue
        {
            get
            {
                return this.ambientOnlyCheckbox.Value;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] OBSCURANCE_SAMPLECOUNT = new string[] { "Lowest", "Low", "Medium", "High", "Variable" };
        private static readonly string[] OBSCURANCE_OCCLUSIONSOURCE = new string[] { "DepthTexture", "DepthNormalsTexture", "GBuffer" };

        private CustomSlider intensitySlider = null;
        private CustomSlider radiusSlider = null;
        private CustomComboBox sampleCountBox = null;
        private CustomSlider sampleCountValueSlider = null;
        private CustomToggleButton downsamplingCheckbox = null;
        private CustomComboBox occlusionSourceBox = null;
        private CustomToggleButton ambientOnlyCheckbox = null;
        #endregion
    }
}
