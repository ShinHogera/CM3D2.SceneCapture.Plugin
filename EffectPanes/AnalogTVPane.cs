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
    internal class AnalogTVPane : BasePane
    {
        public AnalogTVPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "AnalogTV") ) {}

        override public void SetupPane()
        {
            this.automaticPhaseCheckbox = new CustomToggleButton( AnalogTVDef.analogTVEffect.automaticPhase, "toggle" );
            this.automaticPhaseCheckbox.Text = Translation.GetText("AnalogTV", "automaticPhase");
            this.ChildControls.Add( this.automaticPhaseCheckbox );

            this.phaseSlider = new CustomSlider( AnalogTVDef.analogTVEffect.phase, 0f, 180f, 2 );
            this.phaseSlider.Text = Translation.GetText("AnalogTV", "phase");
            this.ChildControls.Add( this.phaseSlider );

            this.convertToGrayscaleCheckbox = new CustomToggleButton( AnalogTVDef.analogTVEffect.convertToGrayscale, "toggle" );
            this.convertToGrayscaleCheckbox.Text = Translation.GetText("AnalogTV", "convertToGrayscale");
            this.ChildControls.Add( this.convertToGrayscaleCheckbox );

            this.noiseIntensitySlider = new CustomSlider( AnalogTVDef.analogTVEffect.noiseIntensity, 0f, 1f, 4 );
            this.noiseIntensitySlider.Text = Translation.GetText("AnalogTV", "noiseIntensity");
            this.ChildControls.Add( this.noiseIntensitySlider );

            this.scanlinesIntensitySlider = new CustomSlider( AnalogTVDef.analogTVEffect.scanlinesIntensity, 0f, 2f, 4 );
            this.scanlinesIntensitySlider.Text = Translation.GetText("AnalogTV", "scanlinesIntensity");
            this.ChildControls.Add( this.scanlinesIntensitySlider );

            this.scanlinesCountSlider = new CustomSlider( AnalogTVDef.analogTVEffect.scanlinesCount, 0f, 4096f, 0 );
            this.scanlinesCountSlider.Text = Translation.GetText("AnalogTV", "scanlinesCount");
            this.ChildControls.Add( this.scanlinesCountSlider );

            this.scanlinesOffsetSlider = new CustomSlider( AnalogTVDef.analogTVEffect.scanlinesOffset, 0f, 300f, 2 );
            this.scanlinesOffsetSlider.Text = Translation.GetText("AnalogTV", "scanlinesOffset");
            this.ChildControls.Add( this.scanlinesOffsetSlider );

            this.verticalScanlinesCheckbox = new CustomToggleButton( AnalogTVDef.analogTVEffect.verticalScanlines, "toggle" );
            this.verticalScanlinesCheckbox.Text = Translation.GetText("AnalogTV", "verticalScanlines");
            this.ChildControls.Add( this.verticalScanlinesCheckbox );

            this.distortionSlider = new CustomSlider( AnalogTVDef.analogTVEffect.distortion, -2f, 2f, 4 );
            this.distortionSlider.Text = Translation.GetText("AnalogTV", "distortion");
            this.ChildControls.Add( this.distortionSlider );

            this.cubicDistortionSlider = new CustomSlider( AnalogTVDef.analogTVEffect.cubicDistortion, -2f, 2f, 4 );
            this.cubicDistortionSlider.Text = Translation.GetText("AnalogTV", "cubicDistortion");
            this.ChildControls.Add( this.cubicDistortionSlider );

            this.scaleSlider = new CustomSlider( AnalogTVDef.analogTVEffect.scale, 0.01f, 2f, 4 );
            this.scaleSlider.Text = Translation.GetText("AnalogTV", "scale");
            this.ChildControls.Add( this.scaleSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.automaticPhaseCheckbox);
           GUIUtil.AddGUISlider(this, this.phaseSlider);
           GUIUtil.AddGUICheckbox(this, this.convertToGrayscaleCheckbox);
           GUIUtil.AddGUISlider(this, this.noiseIntensitySlider);
           GUIUtil.AddGUISlider(this, this.scanlinesIntensitySlider);
           GUIUtil.AddGUISlider(this, this.scanlinesCountSlider);
           GUIUtil.AddGUISlider(this, this.scanlinesOffsetSlider);
           GUIUtil.AddGUICheckbox(this, this.verticalScanlinesCheckbox);
           GUIUtil.AddGUISlider(this, this.distortionSlider);
           GUIUtil.AddGUISlider(this, this.cubicDistortionSlider);
           GUIUtil.AddGUISlider(this, this.scaleSlider);
        }

        override public void Reset()
        {
            AnalogTVDef.Reset();
        }

        #region Properties
        public bool AutomaticPhaseValue
        {
            get
            {
                return this.automaticPhaseCheckbox.Value;
            }
        }

        public float PhaseValue
        {
            get
            {
                return this.phaseSlider.Value;
            }
        }

        public bool ConvertToGrayscaleValue
        {
            get
            {
                return this.convertToGrayscaleCheckbox.Value;
            }
        }

        public float NoiseIntensityValue
        {
            get
            {
                return this.noiseIntensitySlider.Value;
            }
        }

        public float ScanlinesIntensityValue
        {
            get
            {
                return this.scanlinesIntensitySlider.Value;
            }
        }

        public int ScanlinesCountValue
        {
            get
            {
                return (int)this.scanlinesCountSlider.Value;
            }
        }

        public float ScanlinesOffsetValue
        {
            get
            {
                return this.scanlinesOffsetSlider.Value;
            }
        }

        public bool VerticalScanlinesValue
        {
            get
            {
                return this.verticalScanlinesCheckbox.Value;
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
        private CustomToggleButton automaticPhaseCheckbox = null;
        private CustomSlider phaseSlider = null;
        private CustomToggleButton convertToGrayscaleCheckbox = null;
        private CustomSlider noiseIntensitySlider = null;
        private CustomSlider scanlinesIntensitySlider = null;
        private CustomSlider scanlinesCountSlider = null;
        private CustomSlider scanlinesOffsetSlider = null;
        private CustomToggleButton verticalScanlinesCheckbox = null;
        private CustomSlider distortionSlider = null;
        private CustomSlider cubicDistortionSlider = null;
        private CustomSlider scaleSlider = null;
        #endregion
    }
}
