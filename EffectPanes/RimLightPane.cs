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
    internal class RimLightPane : BasePane
    {
        public RimLightPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "RimLight") ) {}

        override public void SetupPane()
        {
            this.colorPicker = new CustomColorPicker( RimLightDef.rimLightEffect.color );
            this.colorPicker.Text = Translation.GetText("RimLight", "color");
            this.ChildControls.Add( this.colorPicker );

            this.intensitySlider = new CustomSlider( RimLightDef.rimLightEffect.intensity, 0f, 10f, 4 );
            this.intensitySlider.Text = Translation.GetText("RimLight", "intensity");
            this.ChildControls.Add( this.intensitySlider );

            this.fresnelBiasSlider = new CustomSlider( RimLightDef.rimLightEffect.fresnelBias, 0f, 10f, 4 );
            this.fresnelBiasSlider.Text = Translation.GetText("RimLight", "fresnelBias");
            this.ChildControls.Add( this.fresnelBiasSlider );

            this.fresnelScaleSlider = new CustomSlider( RimLightDef.rimLightEffect.fresnelScale, 0f, 10f, 4 );
            this.fresnelScaleSlider.Text = Translation.GetText("RimLight", "fresnelScale");
            this.ChildControls.Add( this.fresnelScaleSlider );

            this.fresnelPowSlider = new CustomSlider( RimLightDef.rimLightEffect.fresnelPow, 0f, 10f, 4 );
            this.fresnelPowSlider.Text = Translation.GetText("RimLight", "fresnelPow");
            this.ChildControls.Add( this.fresnelPowSlider );

            this.edgeHighlightingCheckbox = new CustomToggleButton( RimLightDef.rimLightEffect.edgeHighlighting, "toggle" );
            this.edgeHighlightingCheckbox.Text = Translation.GetText("RimLight", "edgeHighlighting");
            this.ChildControls.Add( this.edgeHighlightingCheckbox );

            this.edgeIntensitySlider = new CustomSlider( RimLightDef.rimLightEffect.edgeIntensity, 0f, 10f, 4 );
            this.edgeIntensitySlider.Text = Translation.GetText("RimLight", "edgeIntensity");
            this.ChildControls.Add( this.edgeIntensitySlider );

            this.edgeThresholdSlider = new CustomSlider( RimLightDef.rimLightEffect.edgeThreshold, 0.0f, 9.9f, 4 );
            this.edgeThresholdSlider.Text = Translation.GetText("RimLight", "edgeThreshold");
            this.ChildControls.Add( this.edgeThresholdSlider );

            this.edgeRadiusSlider = new CustomSlider( RimLightDef.rimLightEffect.edgeRadius, 0.0f, 10f, 4 );
            this.edgeRadiusSlider.Text = Translation.GetText("RimLight", "edgeRadius");
            this.ChildControls.Add( this.edgeRadiusSlider );

            this.mulSmoothnessCheckbox = new CustomToggleButton( RimLightDef.rimLightEffect.mulSmoothness, "toggle" );
            this.mulSmoothnessCheckbox.Text = Translation.GetText("RimLight", "mulSmoothness");
            this.ChildControls.Add( this.mulSmoothnessCheckbox );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.colorPicker);
           GUIUtil.AddGUISlider(this, this.intensitySlider);
           GUIUtil.AddGUISlider(this, this.fresnelBiasSlider);
           GUIUtil.AddGUISlider(this, this.fresnelScaleSlider);
           GUIUtil.AddGUISlider(this, this.fresnelPowSlider);
           GUIUtil.AddGUICheckbox(this, this.edgeHighlightingCheckbox);
           GUIUtil.AddGUISlider(this, this.edgeIntensitySlider);
           GUIUtil.AddGUISlider(this, this.edgeThresholdSlider);
           GUIUtil.AddGUISlider(this, this.edgeRadiusSlider);
           GUIUtil.AddGUICheckbox(this, this.mulSmoothnessCheckbox);
        }

        override public void Reset()
        {
            RimLightDef.Reset();
        }

        #region Properties
        public Color ColorValue
        {
            get
            {
                return this.colorPicker.Value;
            }
        }

        public float IntensityValue
        {
            get
            {
                return this.intensitySlider.Value;
            }
        }

        public float FresnelBiasValue
        {
            get
            {
                return this.fresnelBiasSlider.Value;
            }
        }

        public float FresnelScaleValue
        {
            get
            {
                return this.fresnelScaleSlider.Value;
            }
        }

        public float FresnelPowValue
        {
            get
            {
                return this.fresnelPowSlider.Value;
            }
        }

        public bool EdgeHighlightingValue
        {
            get
            {
                return this.edgeHighlightingCheckbox.Value;
            }
        }

        public float EdgeIntensityValue
        {
            get
            {
                return this.edgeIntensitySlider.Value;
            }
        }

        public float EdgeThresholdValue
        {
            get
            {
                return this.edgeThresholdSlider.Value;
            }
        }

        public float EdgeRadiusValue
        {
            get
            {
                return this.edgeRadiusSlider.Value;
            }
        }

        public bool MulSmoothnessValue
        {
            get
            {
                return this.mulSmoothnessCheckbox.Value;
            }
        }

        #endregion

        #region Fields
        private CustomColorPicker colorPicker = null;
        private CustomSlider intensitySlider = null;
        private CustomSlider fresnelBiasSlider = null;
        private CustomSlider fresnelScaleSlider = null;
        private CustomSlider fresnelPowSlider = null;
        private CustomToggleButton edgeHighlightingCheckbox = null;
        private CustomSlider edgeIntensitySlider = null;
        private CustomSlider edgeThresholdSlider = null;
        private CustomSlider edgeRadiusSlider = null;
        private CustomToggleButton mulSmoothnessCheckbox = null;
        #endregion
    }
}
