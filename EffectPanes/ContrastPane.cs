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
    internal class ContrastPane : BasePane
    {
        public ContrastPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Contrast") ) {}

        override public void SetupPane()
        {
            this.intensitySlider = new CustomSlider( ContrastDef.contrastEffect.intensity, -10.0f, 10.0f, 1 );
            this.intensitySlider.Text = Translation.GetText("Contrast", "intensity");
            this.ChildControls.Add( this.intensitySlider );

            this.threshholdSlider = new CustomSlider( ContrastDef.contrastEffect.threshhold, 0.0f, 1.0f, 1 );
            this.threshholdSlider.Text = Translation.GetText("Contrast", "threshold");
            this.ChildControls.Add( this.threshholdSlider );

            this.blurSpreadSlider = new CustomSlider( ContrastDef.contrastEffect.blurSpread, 0.0f, 10.0f, 1 );
            this.blurSpreadSlider.Text = Translation.GetText("Contrast", "blurSpread");
            this.ChildControls.Add( this.blurSpreadSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.intensitySlider);
            GUIUtil.AddGUISlider(this, this.threshholdSlider);
            GUIUtil.AddGUISlider(this, this.blurSpreadSlider);
        }

        override public void Reset()
        {
            ContrastDef.Reset();
        }

        #region Properties
        public float IntensityValue
        {
            get
            {
                return this.intensitySlider.Value;
            }
        }

        public float ThreshholdValue
        {
            get
            {
                return this.threshholdSlider.Value;
            }
        }

        public float BlurSpreadValue
        {
            get
            {
                return this.blurSpreadSlider.Value;
            }
        }
        #endregion

        #region Fields
        private CustomSlider intensitySlider = null;
        private CustomSlider threshholdSlider = null;
        private CustomSlider blurSpreadSlider = null;
        #endregion
    }
}
