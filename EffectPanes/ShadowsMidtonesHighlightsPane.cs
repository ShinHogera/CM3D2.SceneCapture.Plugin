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
    internal class ShadowsMidtonesHighlightsPane : BasePane
    {
        public ShadowsMidtonesHighlightsPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "ShadowsMidtonesHighlights") ) {}

        override public void SetupPane()
        {
            this.modeComboBox = new CustomComboBox ( SMH_COLORMODES );
            this.modeComboBox.Text = Translation.GetText("ShadowsMidtonesHighlights", "mode");
            this.modeComboBox.SelectedIndex = (int)ShadowsMidtonesHighlightsDef.shadowsMidtonesHighlightsEffect.mode;
            this.ChildControls.Add( this.modeComboBox );

            this.shadowsPicker = new CustomColorPicker( ShadowsMidtonesHighlightsDef.shadowsMidtonesHighlightsEffect.shadows );
            this.shadowsPicker.Text = Translation.GetText("ShadowsMidtonesHighlights", "shadows");
            this.ChildControls.Add( this.shadowsPicker );

            this.midtonesPicker = new CustomColorPicker( ShadowsMidtonesHighlightsDef.shadowsMidtonesHighlightsEffect.midtones );
            this.midtonesPicker.Text = Translation.GetText("ShadowsMidtonesHighlights", "midtones");
            this.ChildControls.Add( this.midtonesPicker );

            this.highlightsPicker = new CustomColorPicker( ShadowsMidtonesHighlightsDef.shadowsMidtonesHighlightsEffect.highlights );
            this.highlightsPicker.Text = Translation.GetText("ShadowsMidtonesHighlights", "highlights");
            this.ChildControls.Add( this.highlightsPicker );

            this.amountSlider = new CustomSlider( ShadowsMidtonesHighlightsDef.shadowsMidtonesHighlightsEffect.amount, 0f, 1f, 4 );
            this.amountSlider.Text = Translation.GetText("ShadowsMidtonesHighlights", "amount");
            this.ChildControls.Add( this.amountSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.modeComboBox);
           GUIUtil.AddGUICheckbox(this, this.shadowsPicker);
           GUIUtil.AddGUICheckbox(this, this.midtonesPicker);
           GUIUtil.AddGUICheckbox(this, this.highlightsPicker);
           GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {
            ShadowsMidtonesHighlightsDef.Reset();
        }

        #region Properties
        public ShadowsMidtonesHighlights.ColorMode ModeValue
        {
            get
            {
                return (ShadowsMidtonesHighlights.ColorMode) Enum.Parse( typeof( ShadowsMidtonesHighlights.ColorMode ), this.modeComboBox.SelectedItem);
            }
        }

        public Color ShadowsValue
        {
            get
            {
                return this.shadowsPicker.Value;
            }
        }

        public Color MidtonesValue
        {
            get
            {
                return this.midtonesPicker.Value;
            }
        }

        public Color HighlightsValue
        {
            get
            {
                return this.highlightsPicker.Value;
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
        private static readonly string[] SMH_COLORMODES = new string[] { "LiftGammaGain" , "OffsetGammaSlope" };
        private CustomComboBox modeComboBox = null;
        private CustomColorPicker shadowsPicker = null;
        private CustomColorPicker midtonesPicker = null;
        private CustomColorPicker highlightsPicker = null;
        private CustomSlider amountSlider = null;
        #endregion
    }
}
