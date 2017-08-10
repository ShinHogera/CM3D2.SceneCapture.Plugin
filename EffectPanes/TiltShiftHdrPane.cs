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
    internal class TiltShiftHdrPane : BasePane
    {
        public TiltShiftHdrPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "TiltShiftHdr") ) {}

        override public void SetupPane()
        {
            this.blurAreaSlider = new CustomSlider( TiltShiftHdrDef.tiltShiftHdrEffect.blurArea, 0f, 15f, 2 );
            this.blurAreaSlider.Text = Translation.GetText("TiltShiftHdr", "blurArea");
            this.ChildControls.Add( this.blurAreaSlider );
            this.maxBlurSizeSlider = new CustomSlider( TiltShiftHdrDef.tiltShiftHdrEffect.maxBlurSize, 0f, 25f, 2 );
            this.maxBlurSizeSlider.Text = Translation.GetText("TiltShiftHdr", "maxBlurSize");
            this.ChildControls.Add( this.maxBlurSizeSlider );
            this.downsampleSlider = new CustomSlider( TiltShiftHdrDef.tiltShiftHdrEffect.downsample, 0f, 1f, 3 );
            this.downsampleSlider.Text = Translation.GetText("TiltShiftHdr", "downsample");
            this.ChildControls.Add( this.downsampleSlider );

            this.modeBox = new CustomComboBox( TILT_MODE );
            this.modeBox.Text = Translation.GetText("TiltShiftHdr", "mode");
            this.modeBox.SelectedIndex = (int)TiltShiftHdrDef.tiltShiftHdrEffect.mode;
            this.ChildControls.Add( this.modeBox );
            this.qualityBox = new CustomComboBox( TILT_QUALITY );
            this.qualityBox.Text = Translation.GetText("TiltShiftHdr", "quality");
            this.qualityBox.SelectedIndex = (int)TiltShiftHdrDef.tiltShiftHdrEffect.quality;
            this.ChildControls.Add( this.qualityBox );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.modeBox);
            GUIUtil.AddGUICheckbox(this, this.qualityBox);
            GUIUtil.AddGUISlider(this, this.blurAreaSlider);
            GUIUtil.AddGUISlider(this, this.maxBlurSizeSlider);
            GUIUtil.AddGUISlider(this, this.downsampleSlider);
        }

        override public void Reset()
        {

        }

        #region Properties
        public TiltShiftHdr.TiltShiftMode ModeValue
        {
            get
            {
                return (TiltShiftHdr.TiltShiftMode)Enum.Parse( typeof( TiltShiftHdr.TiltShiftMode ), this.modeBox.SelectedItem);
            }
        }

        public TiltShiftHdr.TiltShiftQuality QualityValue
        {
            get
            {
                return (TiltShiftHdr.TiltShiftQuality)Enum.Parse( typeof( TiltShiftHdr.TiltShiftQuality ), this.qualityBox.SelectedItem);
            }
        }

        public float BlurAreaValue
        {
            get
            {
                return this.blurAreaSlider.Value;
            }
        }

        public float MaxBlurSizeValue
        {
            get
            {
                return this.maxBlurSizeSlider.Value;
            }
        }

        public int DownsampleValue
        {
            get
            {
                return (int)this.downsampleSlider.Value;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] TILT_MODE = new string[] { "TiltShiftMode", "IrisMode" };
        private static readonly string[] TILT_QUALITY = new string[] { "Normal", "High", "Preview" };

        private CustomSlider blurAreaSlider = null;
        private CustomSlider maxBlurSizeSlider = null;
        private CustomSlider downsampleSlider = null;

        private CustomComboBox modeBox = null;
        private CustomComboBox qualityBox = null;
        #endregion
    }
}
