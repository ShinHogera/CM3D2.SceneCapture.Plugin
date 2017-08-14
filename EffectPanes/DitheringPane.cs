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
    internal class DitheringPane : BasePane
    {
        public DitheringPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Dithering") ) {}

        override public void SetupPane()
        {
            this.showOriginalCheckbox = new CustomToggleButton( DitheringDef.ditheringEffect.showOriginal, "toggle" );
            this.showOriginalCheckbox.Text = Translation.GetText("Dithering", "showOriginal");
            this.ChildControls.Add( this.showOriginalCheckbox );

            this.convertToGrayscaleCheckbox = new CustomToggleButton( DitheringDef.ditheringEffect.convertToGrayscale, "toggle" );
            this.convertToGrayscaleCheckbox.Text = Translation.GetText("Dithering", "convertToGrayscale");
            this.ChildControls.Add( this.convertToGrayscaleCheckbox );

            this.redLuminanceSlider = new CustomSlider( DitheringDef.ditheringEffect.redLuminance, 0f, 1f, 4 );
            this.redLuminanceSlider.Text = Translation.GetText("Dithering", "redLuminance");
            this.ChildControls.Add( this.redLuminanceSlider );

            this.greenLuminanceSlider = new CustomSlider( DitheringDef.ditheringEffect.greenLuminance, 0f, 1f, 4 );
            this.greenLuminanceSlider.Text = Translation.GetText("Dithering", "greenLuminance");
            this.ChildControls.Add( this.greenLuminanceSlider );

            this.blueLuminanceSlider = new CustomSlider( DitheringDef.ditheringEffect.blueLuminance, 0f, 1f, 4 );
            this.blueLuminanceSlider.Text = Translation.GetText("Dithering", "blueLuminance");
            this.ChildControls.Add( this.blueLuminanceSlider );

            this.amountSlider = new CustomSlider( DitheringDef.ditheringEffect.amount, 0f, 1f, 4 );
            this.amountSlider.Text = Translation.GetText("Dithering", "amount");
            this.ChildControls.Add( this.amountSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.showOriginalCheckbox);
           GUIUtil.AddGUICheckbox(this, this.convertToGrayscaleCheckbox);
           GUIUtil.AddGUISlider(this, this.redLuminanceSlider);
           GUIUtil.AddGUISlider(this, this.greenLuminanceSlider);
           GUIUtil.AddGUISlider(this, this.blueLuminanceSlider);
           GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {
            DitheringDef.Reset();
        }

        #region Properties
        public bool ShowOriginalValue
        {
            get
            {
                return this.showOriginalCheckbox.Value;
            }
        }

        public bool ConvertToGrayscaleValue
        {
            get
            {
                return this.convertToGrayscaleCheckbox.Value;
            }
        }

        public float RedLuminanceValue
        {
            get
            {
                return this.redLuminanceSlider.Value;
            }
        }

        public float GreenLuminanceValue
        {
            get
            {
                return this.greenLuminanceSlider.Value;
            }
        }

        public float BlueLuminanceValue
        {
            get
            {
                return this.blueLuminanceSlider.Value;
            }
        }

        public float AmountValue
        {
            get
            {
                return this.amountSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomToggleButton showOriginalCheckbox = null;
        private CustomToggleButton convertToGrayscaleCheckbox = null;
        private CustomSlider redLuminanceSlider = null;
        private CustomSlider greenLuminanceSlider = null;
        private CustomSlider blueLuminanceSlider = null;
        private CustomSlider amountSlider = null;
        #endregion
    }
}
