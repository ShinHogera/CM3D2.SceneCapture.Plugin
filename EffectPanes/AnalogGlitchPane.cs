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
    internal class AnalogGlitchPane : BasePane
    {
        public AnalogGlitchPane( int fontSize ) : base( fontSize, "AnalogGlitch" ) {}

        override public void SetupPane()
        {
            this.scanLineJitterSlider = new CustomSlider( AnalogGlitchDef.analogGlitchEffect.scanLineJitter, 0f, 1f, 1 );
            this.scanLineJitterSlider.Text = Instances.isJapanese ? "走査線の強さ" "Scan Line Jitter";
            this.ChildControls.Add( this.scanLineJitterSlider );
            this.verticalJumpSlider = new CustomSlider( AnalogGlitchDef.analogGlitchEffect.verticalJump, 0f, 1f, 1 );
            this.verticalJumpSlider.Text = Instances.isJapanese ? "垂直跳び度" : "Vertical Jump";
            this.ChildControls.Add( this.verticalJumpSlider );
            this.horizontalShakeSlider = new CustomSlider( AnalogGlitchDef.analogGlitchEffect.horizontalShake, 0f, 1f, 1 );
            this.horizontalShakeSlider.Text = Instances.isJapanese ? "横振動" : "Horizontal Shake";
            this.ChildControls.Add( this.horizontalShakeSlider );
            this.colorDriftSlider = new CustomSlider( AnalogGlitchDef.analogGlitchEffect.colorDrift, 0f, 1f, 1 );
            this.colorDriftSlider.Text = Instances.isJapanese ? "色ずれ" : "Color Drift";
            this.ChildControls.Add( this.colorDriftSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.scanLineJitterSlider);
            GUIUtil.AddGUISlider(this, this.verticalJumpSlider);
            GUIUtil.AddGUISlider(this, this.horizontalShakeSlider);
            GUIUtil.AddGUISlider(this, this.colorDriftSlider);
        }

        override public void Reset()
        {

        }

        #region Properties
        public float ScanLineJitterValue
        {
            get
            {
                return this.scanLineJitterSlider.Value;
            }
        }

        public float VerticalJumpValue
        {
            get
            {
                return this.verticalJumpSlider.Value;
            }
        }

        public float HorizontalShakeValue
        {
            get
            {
                return this.horizontalShakeSlider.Value;
            }
        }

        public float ColorDriftValue
        {
            get
            {
                return this.colorDriftSlider.Value;
            }
        }
        #endregion

        #region Fields
        private CustomSlider scanLineJitterSlider = null;
        private CustomSlider verticalJumpSlider = null;
        private CustomSlider horizontalShakeSlider = null;
        private CustomSlider colorDriftSlider = null;
        #endregion
    }
}
