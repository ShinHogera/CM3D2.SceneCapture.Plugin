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
    internal class BlurPane : BasePane
    {
        public BlurPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Blur") ) {}

        override public void SetupPane()
        {
            this.blurIterationsSlider = new CustomSlider( BlurDef.blurEffect.blurIterations, 0f, 2f, 1 );
            this.blurIterationsSlider.Text = Translation.GetText("Blur", "blurIterations");
            this.ChildControls.Add( this.blurIterationsSlider );

            this.blurSizeSlider = new CustomSlider( BlurDef.blurEffect.blurSize, 0f, 10f, 1 );
            this.blurSizeSlider.Text = Translation.GetText("Blur", "blurSize");
            this.ChildControls.Add( this.blurSizeSlider );

            this.downsampleSlider = new CustomSlider( BlurDef.blurEffect.downsample, 1f, 4f, 1 );
            this.downsampleSlider.Text = Translation.GetText("Blur", "downsample");
            this.ChildControls.Add( this.downsampleSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.blurIterationsSlider);
            GUIUtil.AddGUISlider(this, this.blurSizeSlider);
            GUIUtil.AddGUISlider(this, this.downsampleSlider);
        }

        override public void Reset()
        {

        }

        #region Properties
        public int BlurIterationsValue
        {
            get
            {
                return (int)this.blurIterationsSlider.Value;
            }
        }

        public float BlurSizeValue
        {
            get
            {
                return this.blurSizeSlider.Value;
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
        private CustomSlider blurIterationsSlider = null;
        private CustomSlider blurSizeSlider = null;
        private CustomSlider downsampleSlider = null;
        #endregion
    }
}
