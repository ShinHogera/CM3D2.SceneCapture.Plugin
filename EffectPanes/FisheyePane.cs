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
    internal class FisheyePane : BasePane
    {
        public FisheyePane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Fisheye") ) {}

        override public void SetupPane()
        {
            this.strengthXSlider = new CustomSlider( FisheyeDef.fisheyeEffect.strengthX, 0f, 10f, 1 );
            this.strengthXSlider.Text = Translation.GetText("Fisheye", "strengthX");
            this.ChildControls.Add( this.strengthXSlider );
            this.strengthYSlider = new CustomSlider( FisheyeDef.fisheyeEffect.strengthY, 0f, 10f, 1 );
            this.strengthYSlider.Text = Translation.GetText("Fisheye", "strengthY");
            this.ChildControls.Add( this.strengthYSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.strengthXSlider);
            GUIUtil.AddGUISlider(this, this.strengthYSlider);
        }

        override public void Reset()
        {

        }

        #region Properties
        public float StrengthXValue
        {
            get
            {
                return this.strengthXSlider.Value;
            }
        }

        public float StrengthYValue
        {
            get
            {
                return this.strengthYSlider.Value;
            }
        }
        #endregion

        #region Fields
        private CustomSlider strengthXSlider = null;
        private CustomSlider strengthYSlider = null;
        #endregion
    }
}
