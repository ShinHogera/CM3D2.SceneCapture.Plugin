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
    internal class HalftonePane : BasePane
    {
        public HalftonePane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Halftone") ) {}

        override public void SetupPane()
        {
            this.scaleSlider = new CustomSlider( HalftoneDef.halftoneEffect.scale, 0f, 100f, 2 );
            this.scaleSlider.Text = Translation.GetText("Halftone", "scale");
            this.ChildControls.Add( this.scaleSlider );

            this.dotSizeSlider = new CustomSlider( HalftoneDef.halftoneEffect.dotSize, 0f, 100f, 2 );
            this.dotSizeSlider.Text = Translation.GetText("Halftone", "dotSize");
            this.ChildControls.Add( this.dotSizeSlider );

            this.angleSlider = new CustomSlider( HalftoneDef.halftoneEffect.angle, 0f, 180f, 2 );
            this.angleSlider.Text = Translation.GetText("Halftone", "angle");
            this.ChildControls.Add( this.angleSlider );

            this.smoothnessSlider = new CustomSlider( HalftoneDef.halftoneEffect.smoothness, 0f, 1, 4 );
            this.smoothnessSlider.Text = Translation.GetText("Halftone", "smoothness");
            this.ChildControls.Add( this.smoothnessSlider );

            this.centerXSlider = new CustomSlider( HalftoneDef.halftoneEffect.center.x, 0f, 1f, 4);
            this.centerXSlider.Text = Translation.GetText("Halftone", "centerX");
            this.ChildControls.Add( this.centerXSlider );

            this.centerYSlider = new CustomSlider( HalftoneDef.halftoneEffect.center.y, 0f, 1f, 4);
            this.centerYSlider.Text = Translation.GetText("Halftone", "centerY");
            this.ChildControls.Add( this.centerYSlider );

            this.desaturateCheckbox = new CustomToggleButton( HalftoneDef.halftoneEffect.desaturate, "toggle" );
            this.desaturateCheckbox.Text = Translation.GetText("Halftone", "desaturate");
            this.ChildControls.Add( this.desaturateCheckbox );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.scaleSlider);
           GUIUtil.AddGUISlider(this, this.dotSizeSlider);
           GUIUtil.AddGUISlider(this, this.angleSlider);
           GUIUtil.AddGUISlider(this, this.smoothnessSlider);
           GUIUtil.AddGUISlider(this, this.centerXSlider);
           GUIUtil.AddGUISlider(this, this.centerYSlider);
           GUIUtil.AddGUICheckbox(this, this.desaturateCheckbox);
        }

        override public void Reset()
        {
            HalftoneDef.Reset();
        }

        #region Properties
        public Vector2 CenterValue
        {
            get
            {
                return new Vector2(this.centerXSlider.Value, this.centerYSlider.Value);
            }
        }

        public float ScaleValue
        {
            get
            {
                return this.scaleSlider.Value;
            }
        }

        public float DotSizeValue
        {
            get
            {
                return this.dotSizeSlider.Value;
            }
        }

        public float AngleValue
        {
            get
            {
                return this.angleSlider.Value;
            }
        }

        public float SmoothnessValue
        {
            get
            {
                return this.smoothnessSlider.Value;
            }
        }

        public float CenterXValue
        {
            get
            {
                return this.centerXSlider.Value;
            }
        }

        public float CenterYValue
        {
            get
            {
                return this.centerYSlider.Value;
            }
        }

        public bool DesaturateValue
        {
            get
            {
                return this.desaturateCheckbox.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider scaleSlider = null;
        private CustomSlider dotSizeSlider = null;
        private CustomSlider angleSlider = null;
        private CustomSlider smoothnessSlider = null;
        private CustomSlider centerXSlider = null;
        private CustomSlider centerYSlider = null;
        private CustomToggleButton desaturateCheckbox = null;
        #endregion
    }
}
