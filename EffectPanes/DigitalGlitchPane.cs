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
    internal class DigitalGlitchPane : BasePane
    {
        public DigitalGlitchPane( int fontSize ) : base( fontSize, "DigitalGlitch" ) {}

        override public void SetupPane()
        {
            this.intensitySlider = new CustomSlider( DigitalGlitchDef.digitalGlitchEffect.intensity, 0f, 10f, 1 );
            this.intensitySlider.Text = Translation.GetText("DigitalGlitch", "intensity");
            this.ChildControls.Add( this.intensitySlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.intensitySlider);
        }

        override public void Reset()
        {

        }

        #region Properties
        public float IntensityValue
        {
            get
            {
                return this.intensitySlider.Value / 10.0f;
            }
        }
        #endregion

        #region Fields
        private CustomSlider intensitySlider = null;
        #endregion
    }
}
