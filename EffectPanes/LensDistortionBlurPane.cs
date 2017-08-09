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
    internal class LensDistortionBlurPane : BasePane
    {
        public LensDistortionBlurPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "LensDistortionBlur") ) {}

        override public void SetupPane()
        {
            this.qualityBox = new CustomComboBox( QUALITY_PRESETS );
            this.qualityBox.Text = Translation.GetText("LensDistortionBlur", "quality");
            this.qualityBox.SelectedIndex = 3;
            this.ChildControls.Add( this.qualityBox );

            this.samplesSlider = new CustomSlider( LensDistortionBlurDef.lensDistortionBlurEffect.samples, 2f, 32f, 1 );
            this.samplesSlider.Text = Translation.GetText("LensDistortionBlur", "samples");
            this.ChildControls.Add( this.samplesSlider );

            this.distortionSlider = new CustomSlider( LensDistortionBlurDef.lensDistortionBlurEffect.distortion, -2f, 2f, 1 );
            this.distortionSlider.Text = Translation.GetText("LensDistortionBlur", "distortion");
            this.ChildControls.Add( this.distortionSlider );

            this.cubicDistortionSlider = new CustomSlider( LensDistortionBlurDef.lensDistortionBlurEffect.cubicDistortion, -2f, 2f, 1 );
            this.cubicDistortionSlider.Text = Translation.GetText("LensDistortionBlur", "cubicDistortion");
            this.ChildControls.Add( this.cubicDistortionSlider );

            this.scaleSlider = new CustomSlider( LensDistortionBlurDef.lensDistortionBlurEffect.scale, 0.01f, 2f, 1 );
            this.scaleSlider.Text = Translation.GetText("LensDistortionBlur", "scale");
            this.ChildControls.Add( this.scaleSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.qualityBox);
            if( this.QualityValue == LensDistortionBlur.QualityPreset.Custom )
            {
                GUIUtil.AddGUISlider(this, this.samplesSlider);
                GUIUtil.AddGUISlider(this, this.distortionSlider);
                GUIUtil.AddGUISlider(this, this.cubicDistortionSlider);
                GUIUtil.AddGUISlider(this, this.scaleSlider);
            }
        }

        override public void Reset()
        {

        }

        #region Properties
        public LensDistortionBlur.QualityPreset QualityValue
        {
            get
            {
                return (LensDistortionBlur.QualityPreset)Enum.Parse( typeof( LensDistortionBlur.QualityPreset ), this.qualityBox.SelectedItem);
            }
        }

        public int SamplesValue
        {
            get
            {
                return (int)this.samplesSlider.Value;
            }
        }

        public float DistortionValue
        {
            get
            {
                return this.distortionSlider.Value;
            }
        }

        public float CubicDistortionValue
        {
            get
            {
                return this.cubicDistortionSlider.Value;
            }
        }

        public float ScaleValue
        {
            get
            {
                return this.scaleSlider.Value;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] QUALITY_PRESETS = new string[] { "Low", "Medium", "High", "Custom" };

        private CustomComboBox qualityBox = null;
        private CustomSlider samplesSlider = null;
        private CustomSlider distortionSlider = null;
        private CustomSlider cubicDistortionSlider = null;
        private CustomSlider scaleSlider = null;
        #endregion
    }
}
