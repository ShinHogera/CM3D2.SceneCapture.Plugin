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
    internal class VignettingPane : BasePane
    {
        public VignettingPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Vignetting") ) {}

        override public void SetupPane()
        {
            this.intensitySlider = new CustomSlider( VignettingDef.vignettingEffect.intensity, -20f, 20f, 1 );
            this.intensitySlider.Text = Translation.GetText("Vignetting", "intensity");
            this.ChildControls.Add( this.intensitySlider );
            this.blurSlider = new CustomSlider( VignettingDef.vignettingEffect.blur, -20f, 20f, 1 );
            this.blurSlider.Text = Translation.GetText("Vignetting", "blur");
            this.ChildControls.Add( this.blurSlider );
            this.blurSpreadSlider = new CustomSlider( VignettingDef.vignettingEffect.blurSpread, -20f, 20f, 1 );
            this.blurSpreadSlider.Text = Translation.GetText("Vignetting", "blurSpread");
            this.ChildControls.Add( this.blurSpreadSlider );

            this.modeCheckbox = new CustomToggleButton( false, "toggle" );
            this.modeCheckbox.Text = Translation.GetText("Vignetting", "mode");
            this.ChildControls.Add( this.modeCheckbox );

            this.chromaticAberrationSlider = new CustomSlider( VignettingDef.vignettingEffect.chromaticAberration, -20f, 20f, 1 );
            this.chromaticAberrationSlider.Text = Translation.GetText("Vignetting", "chromaticAberration");
            this.ChildControls.Add( this.chromaticAberrationSlider );
            this.luminanceDependencySlider = new CustomSlider( VignettingDef.vignettingEffect.luminanceDependency, 0f, 2f, 1 );
            this.luminanceDependencySlider.Text = Translation.GetText("Vignetting", "luminanceDependency");
            this.ChildControls.Add( this.luminanceDependencySlider );
            this.axialAberrationSlider = new CustomSlider( VignettingDef.vignettingEffect.axialAberration, 0f, 20f, 1 );
            this.axialAberrationSlider.Text = Translation.GetText("Vignetting", "axialAberration");
            this.ChildControls.Add( this.axialAberrationSlider );
            this.blurDistanceSlider = new CustomSlider( VignettingDef.vignettingEffect.blurDistance, -20f, 20f, 1 );
            this.blurDistanceSlider.Text = Translation.GetText("Vignetting", "blurDistance");
            this.ChildControls.Add( this.blurDistanceSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.intensitySlider);
            GUIUtil.AddGUISlider(this, this.blurSlider);
            GUIUtil.AddGUISlider(this, this.blurSpreadSlider);
            GUIUtil.AddGUICheckbox(this, this.modeCheckbox);

            if( this.ModeValue == Vignetting.AberrationMode.Simple )
            {
                GUIUtil.AddGUISlider(this, this.chromaticAberrationSlider);
            }
            else
            {
                GUIUtil.AddGUISlider(this, this.luminanceDependencySlider);
                GUIUtil.AddGUISlider(this, this.axialAberrationSlider);
                GUIUtil.AddGUISlider(this, this.blurDistanceSlider);
            }
        }

        override public void Reset()
        {

        }

        #region Properties
        public Vignetting.AberrationMode ModeValue
        {
            get
            {
                return this.modeCheckbox.Value ? Vignetting.AberrationMode.Advanced : Vignetting.AberrationMode.Simple;
            }
        }

        public float IntensityValue
        {
            get
            {
                return this.intensitySlider.Value;
            }
        }

        public float BlurValue
        {
            get
            {
                return this.blurSlider.Value;
            }
        }

        public float BlurSpreadValue
        {
            get
            {
                return this.blurSpreadSlider.Value;
            }
        }

        public float LuminanceDependencyValue
        {
            get
            {
                return this.luminanceDependencySlider.Value;
            }
        }

        public float ChromaticAberrationValue
        {
            get
            {
                return this.chromaticAberrationSlider.Value;
            }
        }

        public float AxialAberrationValue
        {
            get
            {
                return this.axialAberrationSlider.Value;
            }
        }

        public float BlurDistanceValue
        {
            get
            {
                return this.blurDistanceSlider.Value;
            }
        }
        #endregion

        #region Fields
        private CustomSlider intensitySlider = null;
        private CustomSlider blurSlider = null;
        private CustomSlider blurSpreadSlider = null;

        private CustomSlider luminanceDependencySlider = null;
        private CustomToggleButton modeCheckbox = null;
        private CustomSlider chromaticAberrationSlider = null;
        private CustomSlider axialAberrationSlider = null;
        private CustomSlider blurDistanceSlider = null;
        #endregion
    }
}
