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
            this._lineColorPicker = new CustomColorPicker( IsolineDef.isolineEffect._lineColor );
            this._lineColorPicker.Text = Translation.GetText("Isoline", "_lineColor");
            this.ChildControls.Add( this._lineColorPicker );

            this._luminanceBlendingSlider = new CustomSlider( IsolineDef.isolineEffect._luminanceBlending, 0f, 1f, 4 );
            this._luminanceBlendingSlider.Text = Translation.GetText("Isoline", "_luminanceBlending");
            this.ChildControls.Add( this._luminanceBlendingSlider );

            this._fallOffDepthSlider = new CustomSlider( IsolineDef.isolineEffect._fallOffDepth, 0f, 200f, 4 );
            this._fallOffDepthSlider.Text = Translation.GetText("Isoline", "_fallOffDepth");
            this.ChildControls.Add( this._fallOffDepthSlider );

            this._backgroundColorPicker = new CustomColorPicker( IsolineDef.isolineEffect._backgroundColor );
            this._backgroundColorPicker.Text = Translation.GetText("Isoline", "_backgroundColor");
            this.ChildControls.Add( this._backgroundColorPicker );

            this._axisXSlider = new CustomSlider( IsolineDef.isolineEffect._axis.x, 0f, 100f, 4);
            this._axisXSlider.Text = Translation.GetText("Isoline", "_axisX");
            this.ChildControls.Add( this._axisXSlider );

            this._axisYSlider = new CustomSlider( IsolineDef.isolineEffect._axis.y, 0f, 100f, 4);
            this._axisYSlider.Text = Translation.GetText("Isoline", "_axisY");
            this.ChildControls.Add( this._axisYSlider );

            this._axisZSlider = new CustomSlider( IsolineDef.isolineEffect._axis.z, 0f, 100f, 4);
            this._axisZSlider.Text = Translation.GetText("Isoline", "_axisZ");
            this.ChildControls.Add( this._axisZSlider );

            this._intervalSlider = new CustomSlider( IsolineDef.isolineEffect._interval, 0f, 10f, 4 );
            this._intervalSlider.Text = Translation.GetText("Isoline", "_interval");
            this.ChildControls.Add( this._intervalSlider );

            this._offsetXSlider = new CustomSlider( IsolineDef.isolineEffect._offset.x, 0f, 500f, 4);
            this._offsetXSlider.Text = Translation.GetText("Isoline", "_offsetX");
            this.ChildControls.Add( this._offsetXSlider );

            this._offsetYSlider = new CustomSlider( IsolineDef.isolineEffect._offset.y, 0f, 500f, 4);
            this._offsetYSlider.Text = Translation.GetText("Isoline", "_offsetY");
            this.ChildControls.Add( this._offsetYSlider );

            this._offsetZSlider = new CustomSlider( IsolineDef.isolineEffect._offset.z, 0f, 500f, 4);
            this._offsetZSlider.Text = Translation.GetText("Isoline", "_offsetZ");
            this.ChildControls.Add( this._offsetZSlider );

            this._distortionFrequencySlider = new CustomSlider( IsolineDef.isolineEffect._distortionFrequency, 0f, 20f, 4 );
            this._distortionFrequencySlider.Text = Translation.GetText("Isoline", "_distortionFrequency");
            this.ChildControls.Add( this._distortionFrequencySlider );

            this._distortionAmountSlider = new CustomSlider( IsolineDef.isolineEffect._distortionAmount, 0f, 5f, 4 );
            this._distortionAmountSlider.Text = Translation.GetText("Isoline", "_distortionAmount");
            this.ChildControls.Add( this._distortionAmountSlider );

            this._modulationModeComboBox = new CustomComboBox ( ISOLINE_MODULATION_MODES );
            this._modulationModeComboBox.Text = Translation.GetText("Isoline", "_modulationMode");
            this._modulationModeComboBox.SelectedIndex = (int)IsolineDef.isolineEffect._modulationMode;
            this.ChildControls.Add( this._modulationModeComboBox );

            this._modulationAxisXSlider = new CustomSlider( IsolineDef.isolineEffect._modulationAxis.x, 0f, 100f, 4);
            this._modulationAxisXSlider.Text = Translation.GetText("Isoline", "_modulationAxisX");
            this.ChildControls.Add( this._modulationAxisXSlider );

            this._modulationAxisYSlider = new CustomSlider( IsolineDef.isolineEffect._modulationAxis.y, 0f, 100f, 4);
            this._modulationAxisYSlider.Text = Translation.GetText("Isoline", "_modulationAxisY");
            this.ChildControls.Add( this._modulationAxisYSlider );

            this._modulationAxisZSlider = new CustomSlider( IsolineDef.isolineEffect._modulationAxis.z, 0f, 100f, 4);
            this._modulationAxisZSlider.Text = Translation.GetText("Isoline", "_modulationAxisZ");
            this.ChildControls.Add( this._modulationAxisZSlider );

            this._modulationFrequencySlider = new CustomSlider( IsolineDef.isolineEffect._modulationFrequency, 0f, 10f, 4 );
            this._modulationFrequencySlider.Text = Translation.GetText("Isoline", "_modulationFrequency");
            this.ChildControls.Add( this._modulationFrequencySlider );

            this._modulationSpeedSlider = new CustomSlider( IsolineDef.isolineEffect._modulationSpeed, 0f, 25f, 4 );
            this._modulationSpeedSlider.Text = Translation.GetText("Isoline", "_modulationSpeed");
            this.ChildControls.Add( this._modulationSpeedSlider );

            this._modulationExponentSlider = new CustomSlider( IsolineDef.isolineEffect._modulationExponent, 0f, 50f, 4 );
            this._modulationExponentSlider.Text = Translation.GetText("Isoline", "_modulationExponent");
            this.ChildControls.Add( this._modulationExponentSlider );

            this._directionXSlider = new CustomSlider( IsolineDef.isolineEffect._direction.x, 0f, 100f, 4);
            this._directionXSlider.Text = Translation.GetText("Isoline", "_directionX");
            this.ChildControls.Add( this._directionXSlider );

            this._directionYSlider = new CustomSlider( IsolineDef.isolineEffect._direction.y, 0f, 100f, 4);
            this._directionYSlider.Text = Translation.GetText("Isoline", "_directionY");
            this.ChildControls.Add( this._directionYSlider );

            this._directionZSlider = new CustomSlider( IsolineDef.isolineEffect._direction.z, 0f, 100f, 4);
            this._directionZSlider.Text = Translation.GetText("Isoline", "_directionZ");
            this.ChildControls.Add( this._directionZSlider );

            this._speedSlider = new CustomSlider( IsolineDef.isolineEffect._speed, 0f, 20f, 4 );
            this._speedSlider.Text = Translation.GetText("Isoline", "_speed");
            this.ChildControls.Add( this._speedSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this._lineColorPicker);
           GUIUtil.AddGUISlider(this, this._luminanceBlendingSlider);
           GUIUtil.AddGUISlider(this, this._fallOffDepthSlider);
           GUIUtil.AddGUICheckbox(this, this._backgroundColorPicker);
           GUIUtil.AddGUISlider(this, this._axisXSlider);
           GUIUtil.AddGUISlider(this, this._axisYSlider);
           GUIUtil.AddGUISlider(this, this._axisZSlider);
           GUIUtil.AddGUISlider(this, this._intervalSlider);

           GUIUtil.AddGUISlider(this, this._directionXSlider);
           GUIUtil.AddGUISlider(this, this._directionYSlider);
           GUIUtil.AddGUISlider(this, this._directionZSlider);
           GUIUtil.AddGUISlider(this, this._speedSlider);

           if( this._speedSlider.Value == 0 )
           {
               GUIUtil.AddGUISlider(this, this._offsetXSlider);
               GUIUtil.AddGUISlider(this, this._offsetYSlider);
               GUIUtil.AddGUISlider(this, this._offsetZSlider);
           }

           GUIUtil.AddGUISlider(this, this._distortionFrequencySlider);
           GUIUtil.AddGUISlider(this, this._distortionAmountSlider);
           GUIUtil.AddGUICheckbox(this, this._modulationModeComboBox);
           GUIUtil.AddGUISlider(this, this._modulationAxisXSlider);
           GUIUtil.AddGUISlider(this, this._modulationAxisYSlider);
           GUIUtil.AddGUISlider(this, this._modulationAxisZSlider);
           GUIUtil.AddGUISlider(this, this._modulationFrequencySlider);
           GUIUtil.AddGUISlider(this, this._modulationSpeedSlider);
           GUIUtil.AddGUISlider(this, this._modulationExponentSlider);
        }

        override public void Reset()
        {
            IsolineDef.Reset();
        }

        #region Properties
        public Vector3 _axisValue
        {
            get
            {
                return new Vector3(this._axisXSlider.Value, this._axisYSlider.Value, this._axisZSlider.Value);
            }
        }

        public Vector3 _offsetValue
        {
            get
            {
                return new Vector3(this._offsetXSlider.Value, this._offsetYSlider.Value, this._offsetZSlider.Value);
            }
            set
            {
                this._offsetXSlider.Value = value.x;
                this._offsetYSlider.Value = value.y;
                this._offsetZSlider.Value = value.z;
            }
        }

        public Vector3 _modulationAxisValue
        {
            get
            {
                return new Vector3(this._modulationAxisXSlider.Value, this._modulationAxisYSlider.Value, this._modulationAxisZSlider.Value);
            }
        }

        public Vector3 _directionValue
        {
            get
            {
                return new Vector3(this._directionXSlider.Value, this._directionYSlider.Value, this._directionZSlider.Value);
            }
        }

        public Color _lineColorValue
        {
            get
            {
                return this._lineColorPicker.Value;
            }
        }

        public float _luminanceBlendingValue
        {
            get
            {
                return this._luminanceBlendingSlider.Value;
            }
        }

        public float _fallOffDepthValue
        {
            get
            {
                return this._fallOffDepthSlider.Value;
            }
        }

        public Color _backgroundColorValue
        {
            get
            {
                return this._backgroundColorPicker.Value;
            }
        }

        public float _axisXValue
        {
            get
            {
                return this._axisXSlider.Value;
            }
        }

        public float _axisYValue
        {
            get
            {
                return this._axisYSlider.Value;
            }
        }

        public float _axisZValue
        {
            get
            {
                return this._axisZSlider.Value;
            }
        }

        public float _intervalValue
        {
            get
            {
                return this._intervalSlider.Value;
            }
        }

        public float _offsetXValue
        {
            get
            {
                return this._offsetXSlider.Value;
            }
        }

        public float _offsetYValue
        {
            get
            {
                return this._offsetYSlider.Value;
            }
        }

        public float _offsetZValue
        {
            get
            {
                return this._offsetZSlider.Value;
            }
        }

        public float _distortionFrequencyValue
        {
            get
            {
                return this._distortionFrequencySlider.Value;
            }
        }

        public float _distortionAmountValue
        {
            get
            {
                return this._distortionAmountSlider.Value;
            }
        }

        public Isoline.ModulationMode _modulationModeValue
        {
            get
            {
                return (Isoline.ModulationMode) Enum.Parse( typeof( Isoline.ModulationMode ), this._modulationModeComboBox.SelectedItem);
            }
        }

        public float _modulationAxisXValue
        {
            get
            {
                return this._modulationAxisXSlider.Value;
            }
        }

        public float _modulationAxisYValue
        {
            get
            {
                return this._modulationAxisYSlider.Value;
            }
        }

        public float _modulationAxisZValue
        {
            get
            {
                return this._modulationAxisZSlider.Value;
            }
        }

        public float _modulationFrequencyValue
        {
            get
            {
                return this._modulationFrequencySlider.Value;
            }
        }

        public float _modulationSpeedValue
        {
            get
            {
                return this._modulationSpeedSlider.Value;
            }
        }

        public float _modulationExponentValue
        {
            get
            {
                return this._modulationExponentSlider.Value;
            }
        }

        public float _directionXValue
        {
            get
            {
                return this._directionXSlider.Value;
            }
        }

        public float _directionYValue
        {
            get
            {
                return this._directionYSlider.Value;
            }
        }

        public float _directionZValue
        {
            get
            {
                return this._directionZSlider.Value;
            }
        }

        public float _speedValue
        {
            get
            {
                return this._speedSlider.Value;
            }
        }

        #endregion

        #region Fields
        private static readonly string[] ISOLINE_MODULATION_MODES = new string[] { "None", "Frac", "Sin", "Noise" };
        private CustomColorPicker _lineColorPicker = null;
        private CustomSlider _luminanceBlendingSlider = null;
        private CustomSlider _fallOffDepthSlider = null;
        private CustomColorPicker _backgroundColorPicker = null;
        private CustomSlider _axisXSlider = null;
        private CustomSlider _axisYSlider = null;
        private CustomSlider _axisZSlider = null;
        private CustomSlider _intervalSlider = null;
        private CustomSlider _offsetXSlider = null;
        private CustomSlider _offsetYSlider = null;
        private CustomSlider _offsetZSlider = null;
        private CustomSlider _distortionFrequencySlider = null;
        private CustomSlider _distortionAmountSlider = null;
        private CustomComboBox _modulationModeComboBox = null;
        private CustomSlider _modulationAxisXSlider = null;
        private CustomSlider _modulationAxisYSlider = null;
        private CustomSlider _modulationAxisZSlider = null;
        private CustomSlider _modulationFrequencySlider = null;
        private CustomSlider _modulationSpeedSlider = null;
        private CustomSlider _modulationExponentSlider = null;
        private CustomSlider _directionXSlider = null;
        private CustomSlider _directionYSlider = null;
        private CustomSlider _directionZSlider = null;
        private CustomSlider _speedSlider = null;
        #endregion
    }
}
