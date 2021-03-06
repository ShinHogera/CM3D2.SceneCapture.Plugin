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
    internal class LetterboxPane : BasePane
    {
        public LetterboxPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Letterbox") ) {}

        override public void SetupPane()
        {
            this.aspectWidthSlider = new CustomSlider( LetterboxDef.letterboxEffect.aspectWidth, 0f, 50f, 2 );
            this.aspectWidthSlider.Text = Translation.GetText("Letterbox", "aspectWidth");
            this.ChildControls.Add( this.aspectWidthSlider );
            this.aspectHeightSlider = new CustomSlider( LetterboxDef.letterboxEffect.aspectHeight, 0f, 50, 2 );
            this.aspectHeightSlider.Text = Translation.GetText("Letterbox", "aspectHeight");
            this.ChildControls.Add( this.aspectHeightSlider );
            this.fillColorPicker = new CustomColorPicker( LetterboxDef.letterboxEffect.fillColor );
            this.fillColorPicker.Text = Translation.GetText("Letterbox", "fillColor");
            this.ChildControls.Add( this.fillColorPicker );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.aspectWidthSlider);
            GUIUtil.AddGUISlider(this, this.aspectHeightSlider);
            GUIUtil.AddGUICheckbox(this, this.fillColorPicker);
        }

        override public void Reset()
        {

        }

        #region Properties
        public float AspectWidthValue
        {
            get
            {
                return this.aspectWidthSlider.Value;
            }
        }

        public float AspectHeightValue
        {
            get
            {
                return this.aspectHeightSlider.Value;
            }
        }

        public Color FillColorValue
        {
            get
            {
                return this.fillColorPicker.Value;
            }
        }
        #endregion

        #region Fields
        private CustomSlider aspectWidthSlider = null;
        private CustomSlider aspectHeightSlider = null;
        private CustomColorPicker fillColorPicker = null;
        #endregion
    }
}
