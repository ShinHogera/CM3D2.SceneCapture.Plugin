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
    internal class ColorCorrectionCurvesPane : BasePane
    {
        public ColorCorrectionCurvesPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "ColorCorrectionCurves") ) {}

        override public void SetupPane()
        {
            this.modeCheckbox = new CustomToggleButton( false, "toggle" );
            this.modeCheckbox.Text = Translation.GetText("ColorCorrectionCurves", "mode");
            this.ChildControls.Add( this.modeCheckbox );

            this.saturationSlider = new CustomSlider( ColorCorrectionCurvesDef.colorCurvesEffect.saturation, 0f, 10f, 1 );
            this.saturationSlider.Text = Translation.GetText("ColorCorrectionCurves", "saturation");
            this.ChildControls.Add( this.saturationSlider );
            this.redChannelCurve = new CustomCurve( ColorCorrectionCurvesDef.colorCurvesEffect.redChannel );
            this.redChannelCurve.Text = Translation.GetText("ColorCorrectionCurves", "redChannel");
            this.ChildControls.Add( this.redChannelCurve );
            this.greenChannelCurve = new CustomCurve( ColorCorrectionCurvesDef.colorCurvesEffect.greenChannel );
            this.greenChannelCurve.Text = Translation.GetText("ColorCorrectionCurves", "greenChannel");
            this.ChildControls.Add( this.greenChannelCurve );
            this.blueChannelCurve = new CustomCurve( ColorCorrectionCurvesDef.colorCurvesEffect.blueChannel );
            this.blueChannelCurve.Text = Translation.GetText("ColorCorrectionCurves", "blueChannel");
            this.ChildControls.Add( this.blueChannelCurve );

            this.depthRedChannelCurve = new CustomCurve( ColorCorrectionCurvesDef.colorCurvesEffect.depthRedChannel );
            this.depthRedChannelCurve.Text = Translation.GetText("ColorCorrectionCurves", "depthRedChannel");
            this.ChildControls.Add( this.depthRedChannelCurve );
            this.depthGreenChannelCurve = new CustomCurve( ColorCorrectionCurvesDef.colorCurvesEffect.depthGreenChannel );
            this.depthGreenChannelCurve.Text = Translation.GetText("ColorCorrectionCurves", "depthGreenChannel");
            this.ChildControls.Add( this.depthGreenChannelCurve );
            this.depthBlueChannelCurve = new CustomCurve( ColorCorrectionCurvesDef.colorCurvesEffect.depthBlueChannel );
            this.depthBlueChannelCurve.Text = Translation.GetText("ColorCorrectionCurves", "depthBlueChannel");
            this.ChildControls.Add( this.depthBlueChannelCurve );
            this.zCurveCurve = new CustomCurve( ColorCorrectionCurvesDef.colorCurvesEffect.zCurve );
            this.zCurveCurve.Text = Translation.GetText("ColorCorrectionCurves", "zCurve");
            this.ChildControls.Add( this.zCurveCurve );

            this.selectiveCcCheckbox = new CustomToggleButton( ColorCorrectionCurvesDef.colorCurvesEffect.selectiveCc, "toggle" );
            this.selectiveCcCheckbox.Text = Translation.GetText("ColorCorrectionCurves", "selectiveCc");
            this.ChildControls.Add( this.selectiveCcCheckbox );

            this.selectiveFromColorPicker = new CustomColorPicker( ColorCorrectionCurvesDef.selectiveFromColor );
            this.ChildControls.Add( this.selectiveFromColorPicker );
            this.selectiveToColorPicker = new CustomColorPicker( ColorCorrectionCurvesDef.selectiveToColor );
            this.ChildControls.Add( this.selectiveToColorPicker );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.modeCheckbox);
            GUIUtil.AddGUISlider(this, this.saturationSlider);

            GUIUtil.AddGUICheckbox(this, this.redChannelCurve);
            GUIUtil.AddGUICheckbox(this, this.greenChannelCurve);
            GUIUtil.AddGUICheckbox(this, this.blueChannelCurve);
            if( this.ModeValue == ColorCorrectionMode.Advanced )
            {
                GUIUtil.AddGUICheckbox(this, this.depthRedChannelCurve);
                GUIUtil.AddGUICheckbox(this, this.depthGreenChannelCurve);
                GUIUtil.AddGUICheckbox(this, this.depthBlueChannelCurve);
                GUIUtil.AddGUICheckbox(this, this.zCurveCurve);
            }

            GUIUtil.AddGUICheckbox(this, this.selectiveCcCheckbox);

            if( this.selectiveCcCheckbox.Value == true )
            {
                GUIUtil.AddGUICheckbox(this, this.selectiveFromColorPicker);
                GUIUtil.AddGUICheckbox(this, this.selectiveToColorPicker);
            }
        }

        override public void Reset()
        {

        }

        #region Properties
        public ColorCorrectionMode ModeValue
        {
            get
            {
                return this.modeCheckbox.Value ? ColorCorrectionMode.Advanced : ColorCorrectionMode.Simple;
            }
        }

        public bool SelectiveCcValue
        {
            get
            {
                return this.selectiveCcCheckbox.Value;
            }
        }

        public bool UseDepthCorrectionValue
        {
            get
            {
                return this.modeCheckbox.Value ? true : false;
            }
        }

        public float SaturationValue
        {
            get
            {
                return this.saturationSlider.Value;
            }
        }

        public AnimationCurve RedChannelValue
        {
            get
            {
                return this.redChannelCurve.Value;
            }
        }

        public AnimationCurve GreenChannelValue
        {
            get
            {
                return this.greenChannelCurve.Value;
            }
        }

        public AnimationCurve BlueChannelValue
        {
            get
            {
                return this.blueChannelCurve.Value;
            }
        }

        public AnimationCurve DepthRedChannelValue
        {
            get
            {
                return this.depthRedChannelCurve.Value;
            }
        }

        public AnimationCurve DepthGreenChannelValue
        {
            get
            {
                return this.depthGreenChannelCurve.Value;
            }
        }

        public AnimationCurve DepthBlueChannelValue
        {
            get
            {
                return this.depthBlueChannelCurve.Value;
            }
        }

        public AnimationCurve ZCurveValue
        {
            get
            {
                return this.zCurveCurve.Value;
            }
        }

        public Color SelectiveFromColorValue
        {
            get
            {
                return this.selectiveFromColorPicker.Value;
            }
        }

        public Color SelectiveToColorValue
        {
            get
            {
                return this.selectiveToColorPicker.Value;
            }
        }
        #endregion

        #region Fields
        private CustomToggleButton modeCheckbox = null;
        private CustomToggleButton selectiveCcCheckbox = null;
        private CustomSlider saturationSlider = null;
        private CustomCurve redChannelCurve = null;
        private CustomCurve greenChannelCurve = null;
        private CustomCurve blueChannelCurve = null;

        private CustomCurve depthRedChannelCurve = null;
        private CustomCurve depthGreenChannelCurve = null;
        private CustomCurve depthBlueChannelCurve = null;
        private CustomCurve zCurveCurve = null;

        private CustomColorPicker selectiveFromColorPicker = null;
        private CustomColorPicker selectiveToColorPicker = null;
        #endregion
    }
}
