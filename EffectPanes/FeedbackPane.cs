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
    internal class FeedbackPane : BasePane
    {
        public FeedbackPane( int fontSize ) : base( fontSize, "Feedback" ) {}

        override public void SetupPane()
        {
            this.colorPicker = new CustomColorPicker( FeedbackDef.feedbackEffect.color );
            this.colorPicker.Text = "Color";
            this.ChildControls.Add( this.colorPicker );

            this.offsetXSlider = new CustomSlider( FeedbackDef.feedbackEffect.offsetX, -1f, 1f, 1 );
            this.offsetXSlider.Text = "Offset X";
            this.ChildControls.Add( this.offsetXSlider );

            this.offsetYSlider = new CustomSlider( FeedbackDef.feedbackEffect.offsetY, -1f, 1f, 1 );
            this.offsetYSlider.Text = "Offset Y";
            this.ChildControls.Add( this.offsetYSlider );

            this.rotationSlider = new CustomSlider( FeedbackDef.feedbackEffect.rotation, -5f, 5f, 1 );
            this.rotationSlider.Text = "Rotation";
            this.ChildControls.Add( this.rotationSlider );

            this.scaleSlider = new CustomSlider( FeedbackDef.feedbackEffect.scale, 0.95f, 1.05f, 1 );
            this.scaleSlider.Text = "Scale";
            this.ChildControls.Add( this.scaleSlider );

            this.jaggiesCheckbox = new CustomToggleButton( false, "toggle" );
            this.jaggiesCheckbox.Text = "Disable bilinear filter";
            this.ChildControls.Add( this.jaggiesCheckbox );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.colorPicker);

            GUIUtil.AddGUISlider(this, this.offsetXSlider);
            GUIUtil.AddGUISlider(this, this.offsetYSlider);
            GUIUtil.AddGUISlider(this, this.rotationSlider);
            GUIUtil.AddGUISlider(this, this.scaleSlider);

            GUIUtil.AddGUICheckbox(this, this.jaggiesCheckbox);
        }

        override public void Reset()
        {

        }

        #region Properties
        public Color ColorValue
        {
            get
            {
                return this.colorPicker.Value;
            }
        }
        public float OffsetXValue
        {
            get
            {
                return this.offsetXSlider.Value;
            }
        }
        public float OffsetYValue
        {
            get
            {
                return this.offsetYSlider.Value;
            }
        }
        public float RotationValue
        {
            get
            {
                return this.rotationSlider.Value;
            }
        }
        public float ScaleValue
        {
            get
            {
                return this.scaleSlider.Value;
            }
        }
        public bool JaggiesValue
        {
            get
            {
                return this.jaggiesCheckbox.Value;
            }
        }
        #endregion

        #region Fields
        private CustomColorPicker colorPicker = null;
        private CustomSlider offsetXSlider = null;
        private CustomSlider offsetYSlider = null;
        private CustomSlider rotationSlider = null;
        private CustomSlider scaleSlider = null;
        private CustomToggleButton jaggiesCheckbox = null;
        #endregion
    }
}
