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
    internal class WaveDistortionPane : BasePane
    {
        public WaveDistortionPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "WaveDistortion") ) {}

        override public void SetupPane()
        {
            this.amplitudeSlider = new CustomSlider( WaveDistortionDef.waveDistortionEffect.amplitude, 0f, 1f, 4 );
            this.amplitudeSlider.Text = Translation.GetText("WaveDistortion", "amplitude");
            this.ChildControls.Add( this.amplitudeSlider );

            this.wavesSlider = new CustomSlider( WaveDistortionDef.waveDistortionEffect.waves, 0f, 10f, 4 );
            this.wavesSlider.Text = Translation.GetText("WaveDistortion", "waves");
            this.ChildControls.Add( this.wavesSlider );

            this.colorGlitchSlider = new CustomSlider( WaveDistortionDef.waveDistortionEffect.colorGlitch, 0f, 5f, 4 );
            this.colorGlitchSlider.Text = Translation.GetText("WaveDistortion", "colorGlitch");
            this.ChildControls.Add( this.colorGlitchSlider );

            this.phaseSlider = new CustomSlider( WaveDistortionDef.waveDistortionEffect.phase, 0f, 2f * (float)Math.PI, 4 );
            this.phaseSlider.Text = Translation.GetText("WaveDistortion", "phase");
            this.ChildControls.Add( this.phaseSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.amplitudeSlider);
           GUIUtil.AddGUISlider(this, this.wavesSlider);
           GUIUtil.AddGUISlider(this, this.colorGlitchSlider);
           GUIUtil.AddGUISlider(this, this.phaseSlider);
        }

        override public void Reset()
        {
            WaveDistortionDef.Reset();
        }

        #region Properties
        public float AmplitudeValue
        {
            get
            {
                return this.amplitudeSlider.Value;
            }
        }

        public float WavesValue
        {
            get
            {
                return this.wavesSlider.Value;
            }
        }

        public float ColorGlitchValue
        {
            get
            {
                return this.colorGlitchSlider.Value;
            }
        }

        public float PhaseValue
        {
            get
            {
                return this.phaseSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider amplitudeSlider = null;
        private CustomSlider wavesSlider = null;
        private CustomSlider colorGlitchSlider = null;
        private CustomSlider phaseSlider = null;
        #endregion
    }
}
