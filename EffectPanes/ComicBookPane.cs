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
    internal class ComicBookPane : BasePane
    {
        public ComicBookPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "ComicBook") ) {}

        override public void SetupPane()
        {
            this.stripAngleSlider = new CustomSlider( ComicBookDef.comicBookEffect.stripAngle, 0f, 2f * (float)Math.PI, 4 );
            this.stripAngleSlider.Text = Translation.GetText("ComicBook", "stripAngle");
            this.ChildControls.Add( this.stripAngleSlider );

            this.stripDensitySlider = new CustomSlider( ComicBookDef.comicBookEffect.stripDensity, 0f, 500f, 2 );
            this.stripDensitySlider.Text = Translation.GetText("ComicBook", "stripDensity");
            this.ChildControls.Add( this.stripDensitySlider );

            this.stripThicknessSlider = new CustomSlider( ComicBookDef.comicBookEffect.stripThickness, 0f, 1f, 4 );
            this.stripThicknessSlider.Text = Translation.GetText("ComicBook", "stripThickness");
            this.ChildControls.Add( this.stripThicknessSlider );

            this.stripLimitsXSlider = new CustomSlider( ComicBookDef.comicBookEffect.stripLimits.x, 0f, 1f, 4);
            this.stripLimitsXSlider.Text = Translation.GetText("ComicBook", "stripLimitsX");
            this.ChildControls.Add( this.stripLimitsXSlider );

            this.stripLimitsYSlider = new CustomSlider( ComicBookDef.comicBookEffect.stripLimits.y, 0f, 1f, 4);
            this.stripLimitsYSlider.Text = Translation.GetText("ComicBook", "stripLimitsY");
            this.ChildControls.Add( this.stripLimitsYSlider );

            this.stripInnerColorPicker = new CustomColorPicker( ComicBookDef.comicBookEffect.stripInnerColor );
            this.stripInnerColorPicker.Text = Translation.GetText("ComicBook", "stripInnerColor");
            this.stripInnerColorPicker.IsRGBA = false;
            this.ChildControls.Add( this.stripInnerColorPicker );

            this.stripOuterColorPicker = new CustomColorPicker( ComicBookDef.comicBookEffect.stripOuterColor );
            this.stripOuterColorPicker.Text = Translation.GetText("ComicBook", "stripOuterColor");
            this.stripOuterColorPicker.IsRGBA = false;
            this.ChildControls.Add( this.stripOuterColorPicker );

            this.fillColorPicker = new CustomColorPicker( ComicBookDef.comicBookEffect.fillColor );
            this.fillColorPicker.Text = Translation.GetText("ComicBook", "fillColor");
            this.fillColorPicker.IsRGBA = false;
            this.ChildControls.Add( this.fillColorPicker );

            this.backgroundColorPicker = new CustomColorPicker( ComicBookDef.comicBookEffect.backgroundColor );
            this.backgroundColorPicker.Text = Translation.GetText("ComicBook", "backgroundColor");
            this.backgroundColorPicker.IsRGBA = false;
            this.ChildControls.Add( this.backgroundColorPicker );

            this.edgeDetectionCheckbox = new CustomToggleButton( ComicBookDef.comicBookEffect.edgeDetection, "toggle" );
            this.edgeDetectionCheckbox.Text = Translation.GetText("ComicBook", "edgeDetection");
            this.ChildControls.Add( this.edgeDetectionCheckbox );

            this.edgeThresholdSlider = new CustomSlider( ComicBookDef.comicBookEffect.edgeThreshold, 0.01f, 50f, 3 );
            this.edgeThresholdSlider.Text = Translation.GetText("ComicBook", "edgeThreshold");
            this.ChildControls.Add( this.edgeThresholdSlider );

            this.edgeColorPicker = new CustomColorPicker( ComicBookDef.comicBookEffect.edgeColor );
            this.edgeColorPicker.Text = Translation.GetText("ComicBook", "edgeColor");
            this.ChildControls.Add( this.edgeColorPicker );

            this.amountSlider = new CustomSlider( ComicBookDef.comicBookEffect.amount, 0f, 1f, 4 );
            this.amountSlider.Text = Translation.GetText("ComicBook", "amount");
            this.ChildControls.Add( this.amountSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.stripAngleSlider);
           GUIUtil.AddGUISlider(this, this.stripDensitySlider);
           GUIUtil.AddGUISlider(this, this.stripThicknessSlider);
           GUIUtil.AddGUISlider(this, this.stripLimitsXSlider);
           GUIUtil.AddGUISlider(this, this.stripLimitsYSlider);
           GUIUtil.AddGUICheckbox(this, this.stripInnerColorPicker);
           GUIUtil.AddGUICheckbox(this, this.stripOuterColorPicker);
           GUIUtil.AddGUICheckbox(this, this.fillColorPicker);
           GUIUtil.AddGUICheckbox(this, this.backgroundColorPicker);
           GUIUtil.AddGUICheckbox(this, this.edgeDetectionCheckbox);
           GUIUtil.AddGUISlider(this, this.edgeThresholdSlider);
           GUIUtil.AddGUICheckbox(this, this.edgeColorPicker);
           GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {
            ComicBookDef.Reset();
        }

        #region Properties
        public Vector2 StripLimitsValue
        {
            get
            {
                return new Vector2(this.stripLimitsXSlider.Value, this.stripLimitsYSlider.Value);
            }
        }

        public float StripAngleValue
        {
            get
            {
                return this.stripAngleSlider.Value;
            }
        }

        public float StripDensityValue
        {
            get
            {
                return this.stripDensitySlider.Value;
            }
        }

        public float StripThicknessValue
        {
            get
            {
                return this.stripThicknessSlider.Value;
            }
        }

        public float StripLimitsXValue
        {
            get
            {
                return this.stripLimitsXSlider.Value;
            }
        }

        public float StripLimitsYValue
        {
            get
            {
                return this.stripLimitsYSlider.Value;
            }
        }

        public Color StripInnerColorValue
        {
            get
            {
                return this.stripInnerColorPicker.Value;
            }
        }

        public Color StripOuterColorValue
        {
            get
            {
                return this.stripOuterColorPicker.Value;
            }
        }

        public Color FillColorValue
        {
            get
            {
                return this.fillColorPicker.Value;
            }
        }

        public Color BackgroundColorValue
        {
            get
            {
                return this.backgroundColorPicker.Value;
            }
        }

        public bool EdgeDetectionValue
        {
            get
            {
                return this.edgeDetectionCheckbox.Value;
            }
        }

        public float EdgeThresholdValue
        {
            get
            {
                return this.edgeThresholdSlider.Value;
            }
        }

        public Color EdgeColorValue
        {
            get
            {
                return this.edgeColorPicker.Value;
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
        private CustomSlider stripAngleSlider = null;
        private CustomSlider stripDensitySlider = null;
        private CustomSlider stripThicknessSlider = null;
        private CustomSlider stripLimitsXSlider = null;
        private CustomSlider stripLimitsYSlider = null;
        private CustomColorPicker stripInnerColorPicker = null;
        private CustomColorPicker stripOuterColorPicker = null;
        private CustomColorPicker fillColorPicker = null;
        private CustomColorPicker backgroundColorPicker = null;
        private CustomToggleButton edgeDetectionCheckbox = null;
        private CustomSlider edgeThresholdSlider = null;
        private CustomColorPicker edgeColorPicker = null;
        private CustomSlider amountSlider = null;
        #endregion
    }
}
