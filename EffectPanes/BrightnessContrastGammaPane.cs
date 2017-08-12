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
    internal class BrightnessContrastGammaPane : BasePane
    {
        public BrightnessContrastGammaPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "BrightnessContrastGamma") ) {}

        override public void SetupPane()
        {
            this.brightnessSlider = new CustomSlider( BrightnessContrastGammaDef.brightnessContrastGammaEffect.brightness, -100f, 100f, 2 );
            this.brightnessSlider.Text = Translation.GetText("BrightnessContrastGamma", "brightness");
            this.ChildControls.Add( this.brightnessSlider );

            this.contrastSlider = new CustomSlider( BrightnessContrastGammaDef.brightnessContrastGammaEffect.contrast, -100f, 100f, 2 );
            this.contrastSlider.Text = Translation.GetText("BrightnessContrastGamma", "contrast");
            this.ChildControls.Add( this.contrastSlider );

            this.contrastCoeffXSlider = new CustomSlider( BrightnessContrastGammaDef.brightnessContrastGammaEffect.contrastCoeff.x, 0f, 2.5f, 4);
            this.contrastCoeffXSlider.Text = Translation.GetText("BrightnessContrastGamma", "contrastCoeffX");
            this.ChildControls.Add( this.contrastCoeffXSlider );

            this.contrastCoeffYSlider = new CustomSlider( BrightnessContrastGammaDef.brightnessContrastGammaEffect.contrastCoeff.y, 0f, 2.5f, 4);
            this.contrastCoeffYSlider.Text = Translation.GetText("BrightnessContrastGamma", "contrastCoeffY");
            this.ChildControls.Add( this.contrastCoeffYSlider );

            this.contrastCoeffZSlider = new CustomSlider( BrightnessContrastGammaDef.brightnessContrastGammaEffect.contrastCoeff.z, 0f, 2.5f, 4);
            this.contrastCoeffZSlider.Text = Translation.GetText("BrightnessContrastGamma", "contrastCoeffZ");
            this.ChildControls.Add( this.contrastCoeffZSlider );

            this.gammaSlider = new CustomSlider( BrightnessContrastGammaDef.brightnessContrastGammaEffect.gamma, 0.1f, 9.9f, 4 );
            this.gammaSlider.Text = Translation.GetText("BrightnessContrastGamma", "gamma");
            this.ChildControls.Add( this.gammaSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.brightnessSlider);
           GUIUtil.AddGUISlider(this, this.contrastSlider);
           GUIUtil.AddGUISlider(this, this.contrastCoeffXSlider);
           GUIUtil.AddGUISlider(this, this.contrastCoeffYSlider);
           GUIUtil.AddGUISlider(this, this.contrastCoeffZSlider);
           GUIUtil.AddGUISlider(this, this.gammaSlider);
        }

        override public void Reset()
        {
            BrightnessContrastGammaDef.Reset();
        }

        #region Properties
        public Vector3 ContrastCoeffValue
        {
            get
            {
                return new Vector3(this.contrastCoeffXSlider.Value, this.contrastCoeffYSlider.Value, this.contrastCoeffZSlider.Value);
            }
        }

        public float BrightnessValue
        {
            get
            {
                return this.brightnessSlider.Value;
            }
        }

        public float ContrastValue
        {
            get
            {
                return this.contrastSlider.Value;
            }
        }

        public float ContrastCoeffXValue
        {
            get
            {
                return this.contrastCoeffXSlider.Value;
            }
        }

        public float ContrastCoeffYValue
        {
            get
            {
                return this.contrastCoeffYSlider.Value;
            }
        }

        public float ContrastCoeffZValue
        {
            get
            {
                return this.contrastCoeffZSlider.Value;
            }
        }

        public float GammaValue
        {
            get
            {
                return this.gammaSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider brightnessSlider = null;
        private CustomSlider contrastSlider = null;
        private CustomSlider contrastCoeffXSlider = null;
        private CustomSlider contrastCoeffYSlider = null;
        private CustomSlider contrastCoeffZSlider = null;
        private CustomSlider gammaSlider = null;
        #endregion
    }
}
