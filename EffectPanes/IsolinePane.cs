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
    internal class IsolinePane : BasePane
    {
        public IsolinePane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Isoline") ) {}

        override public void SetupPane()
        {
            this.lineColorPicker = new CustomColorPicker ( IsolineDef.isolineEffect.lineColor );
            this.lineColorPicker.Text = Translation.GetText("Isoline", "lineColor");
            this.ChildControls.Add( this.lineColorPicker );

            this.luminanceBlendingSlider = new CustomSlider ( IsolineDef.isolineEffect.luminanceBlending, 0f, 1f, 4 );
            this.luminanceBlendingSlider.Text = Translation.GetText("Isoline", "luminanceBlending");
            this.ChildControls.Add( this.luminanceBlendingSlider );

            this.fallOffDepthSlider = new CustomSlider ( IsolineDef.isolineEffect.fallOffDepth, 0f, 1f, 4 );
            this.fallOffDepthSlider.Text = Translation.GetText("Isoline", "fallOffDepth");
            this.ChildControls.Add( this.fallOffDepthSlider );

            this.backgroundColorPicker = new CustomColorPicker ( IsolineDef.isolineEffect.backgroundColor );
            this.backgroundColorPicker.Text = Translation.GetText("Isoline", "backgroundColor");
            this.ChildControls.Add( this.backgroundColorPicker );

            this.axisXSlider = new CustomSlider ( IsolineDef.isolineEffect.axis.x, 0f, 180f, 4);
            this.axisXSlider.Text = Translation.GetText("Isoline", "axisX");
            this.ChildControls.Add( this.axisXSlider );

            this.axisYSlider = new CustomSlider ( IsolineDef.isolineEffect.axis.y, 0f, 180f, 4);
            this.axisYSlider.Text = Translation.GetText("Isoline", "axisY");
            this.ChildControls.Add( this.axisYSlider );

            this.axisZSlider = new CustomSlider ( IsolineDef.isolineEffect.axis.z, 0f, 180f, 4);
            this.axisZSlider.Text = Translation.GetText("Isoline", "axisZ");
            this.ChildControls.Add( this.axisZSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.lineColorPicker);
           GUIUtil.AddGUISlider(this, this.luminanceBlendingSlider);
           GUIUtil.AddGUISlider(this, this.fallOffDepthSlider);
           GUIUtil.AddGUICheckbox(this, this.backgroundColorPicker);
           GUIUtil.AddGUISlider(this, this.axisXSlider);
           GUIUtil.AddGUISlider(this, this.axisYSlider);
           GUIUtil.AddGUISlider(this, this.axisZSlider);
        }

        override public void Reset()
        {
            IsolineDef.Reset();
        }

        #region Properties
        public Vector3 AxisValue {
            get
            {
                return new Vector3(this.axisXSlider.Value, this.axisYSlider.Value, this.axisZSlider.Value);
            }
        }

        public Color LineColorValue {
            get
            {
                return this.lineColorPicker.Value;
            }
        }

        public float LuminanceBlendingValue {
            get
            {
                return this.luminanceBlendingSlider.Value;
            }
        }

        public float FallOffDepthValue {
            get
            {
                return this.fallOffDepthSlider.Value;
            }
        }

        public Color BackgroundColorValue {
            get
            {
                return this.backgroundColorPicker.Value;
            }
        }

        public float AxisXValue {
            get
            {
                return this.axisXSlider.Value;
            }
        }

        public float AxisYValue {
            get
            {
                return this.axisYSlider.Value;
            }
        }

        public float AxisZValue {
            get
            {
                return this.axisZSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomColorPicker lineColorPicker = null;
        private CustomSlider luminanceBlendingSlider = null;
        private CustomSlider fallOffDepthSlider = null;
        private CustomColorPicker backgroundColorPicker = null;
        private CustomSlider axisXSlider = null;
        private CustomSlider axisYSlider = null;
        private CustomSlider axisZSlider = null;
        #endregion
    }
}
