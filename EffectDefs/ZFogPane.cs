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
    internal class ZFogPane : BasePane
    {
        public ZFogPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "ZFog") ) {}

        override public void SetupPane()
        {
            this.color1Picker = new CustomColorPicker( ZFogDef.zFogEffect.color1 );
            this.color1Picker.Text = Translation.GetText("ZFog", "color1");
            this.ChildControls.Add( this.color1Picker );

            this.hdr1Slider = new CustomSlider( ZFogDef.zFogEffect.hdr1, 0f, 50f, 2 );
            this.hdr1Slider.Text = Translation.GetText("ZFog", "hdr1");
            this.ChildControls.Add( this.hdr1Slider );

            this.near1Slider = new CustomSlider( ZFogDef.zFogEffect.near1, 0f, 50f, 2 );
            this.near1Slider.Text = Translation.GetText("ZFog", "near1");
            this.ChildControls.Add( this.near1Slider );

            this.far1Slider = new CustomSlider( ZFogDef.zFogEffect.far1, 0f, 50f, 2 );
            this.far1Slider.Text = Translation.GetText("ZFog", "far1");
            this.ChildControls.Add( this.far1Slider );

            this.pow1Slider = new CustomSlider( ZFogDef.zFogEffect.pow1, 0f, 50f, 2 );
            this.pow1Slider.Text = Translation.GetText("ZFog", "pow1");
            this.ChildControls.Add( this.pow1Slider );

            this.color2Picker = new CustomColorPicker( ZFogDef.zFogEffect.color2 );
            this.color2Picker.Text = Translation.GetText("ZFog", "color2");
            this.ChildControls.Add( this.color2Picker );

            this.hdr2Slider = new CustomSlider( ZFogDef.zFogEffect.hdr2, 0f, 50f, 2 );
            this.hdr2Slider.Text = Translation.GetText("ZFog", "hdr2");
            this.ChildControls.Add( this.hdr2Slider );

            this.near2Slider = new CustomSlider( ZFogDef.zFogEffect.near2, 0f, 50f, 2 );
            this.near2Slider.Text = Translation.GetText("ZFog", "near2");
            this.ChildControls.Add( this.near2Slider );

            this.far2Slider = new CustomSlider( ZFogDef.zFogEffect.far2, 0f, 50f, 2 );
            this.far2Slider.Text = Translation.GetText("ZFog", "far2");
            this.ChildControls.Add( this.far2Slider );

            this.pow2Slider = new CustomSlider( ZFogDef.zFogEffect.pow2, 0f, 50f, 2 );
            this.pow2Slider.Text = Translation.GetText("ZFog", "pow2");
            this.ChildControls.Add( this.pow2Slider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.color1Picker);
           GUIUtil.AddGUISlider(this, this.hdr1Slider);
           GUIUtil.AddGUISlider(this, this.near1Slider);
           GUIUtil.AddGUISlider(this, this.far1Slider);
           GUIUtil.AddGUISlider(this, this.pow1Slider);
           GUIUtil.AddGUICheckbox(this, this.color2Picker);
           GUIUtil.AddGUISlider(this, this.hdr2Slider);
           GUIUtil.AddGUISlider(this, this.near2Slider);
           GUIUtil.AddGUISlider(this, this.far2Slider);
           GUIUtil.AddGUISlider(this, this.pow2Slider);
        }

        override public void Reset()
        {
            ZFogDef.Reset();
        }

        #region Properties
        public Color Color1Value
        {
            get
            {
                return this.color1Picker.Value;
            }
        }

        public float Hdr1Value
        {
            get
            {
                return this.hdr1Slider.Value;
            }
        }

        public float Near1Value
        {
            get
            {
                return this.near1Slider.Value;
            }
        }

        public float Far1Value
        {
            get
            {
                return this.far1Slider.Value;
            }
        }

        public float Pow1Value
        {
            get
            {
                return this.pow1Slider.Value;
            }
        }

        public Color Color2Value
        {
            get
            {
                return this.color2Picker.Value;
            }
        }

        public float Hdr2Value
        {
            get
            {
                return this.hdr2Slider.Value;
            }
        }

        public float Near2Value
        {
            get
            {
                return this.near2Slider.Value;
            }
        }

        public float Far2Value
        {
            get
            {
                return this.far2Slider.Value;
            }
        }

        public float Pow2Value
        {
            get
            {
                return this.pow2Slider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomColorPicker color1Picker = null;
        private CustomSlider hdr1Slider = null;
        private CustomSlider near1Slider = null;
        private CustomSlider far1Slider = null;
        private CustomSlider pow1Slider = null;
        private CustomColorPicker color2Picker = null;
        private CustomSlider hdr2Slider = null;
        private CustomSlider near2Slider = null;
        private CustomSlider far2Slider = null;
        private CustomSlider pow2Slider = null;
        #endregion
    }
}
