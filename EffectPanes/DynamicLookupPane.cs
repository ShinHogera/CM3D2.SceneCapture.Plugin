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
    internal class DynamicLookupPane : BasePane
    {
        public DynamicLookupPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "DynamicLookup") ) {}

        override public void SetupPane()
        {
            this.whitePicker = new CustomColorPicker( DynamicLookupDef.dynamicLookupEffect.white );
            this.whitePicker.Text = Translation.GetText("DynamicLookup",  "white");
            this.whitePicker.IsRGBA = false;
            this.ChildControls.Add( this.whitePicker );

            this.blackPicker = new CustomColorPicker( DynamicLookupDef.dynamicLookupEffect.black );
            this.blackPicker.Text = Translation.GetText("DynamicLookup",  "black");
            this.blackPicker.IsRGBA = false;
            this.ChildControls.Add( this.blackPicker );

            this.redPicker = new CustomColorPicker( DynamicLookupDef.dynamicLookupEffect.red );
            this.redPicker.Text = Translation.GetText("DynamicLookup",  "red");
            this.redPicker.IsRGBA = false;
            this.ChildControls.Add( this.redPicker );

            this.greenPicker = new CustomColorPicker( DynamicLookupDef.dynamicLookupEffect.green );
            this.greenPicker.Text = Translation.GetText("DynamicLookup",  "green");
            this.greenPicker.IsRGBA = false;
            this.ChildControls.Add( this.greenPicker );

            this.bluePicker = new CustomColorPicker( DynamicLookupDef.dynamicLookupEffect.blue );
            this.bluePicker.Text = Translation.GetText("DynamicLookup",  "blue");
            this.bluePicker.IsRGBA = false;
            this.ChildControls.Add( this.bluePicker );

            this.yellowPicker = new CustomColorPicker( DynamicLookupDef.dynamicLookupEffect.yellow );
            this.yellowPicker.Text = Translation.GetText("DynamicLookup",  "yellow");
            this.yellowPicker.IsRGBA = false;
            this.ChildControls.Add( this.yellowPicker );

            this.magentaPicker = new CustomColorPicker( DynamicLookupDef.dynamicLookupEffect.magenta );
            this.magentaPicker.Text = Translation.GetText("DynamicLookup",  "magenta");
            this.magentaPicker.IsRGBA = false;
            this.ChildControls.Add( this.magentaPicker );

            this.cyanPicker = new CustomColorPicker( DynamicLookupDef.dynamicLookupEffect.cyan );
            this.cyanPicker.Text = Translation.GetText("DynamicLookup",  "cyan");
            this.cyanPicker.IsRGBA = false;
            this.ChildControls.Add( this.cyanPicker );

            this.amountSlider = new CustomSlider( DynamicLookupDef.dynamicLookupEffect.amount, 0f, 1f, 1 );
            this.amountSlider.Text = Translation.GetText("DynamicLookup",  "amount");
            this.ChildControls.Add( this.amountSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.whitePicker);
            GUIUtil.AddGUICheckbox(this, this.blackPicker);
            GUIUtil.AddGUICheckbox(this, this.redPicker);
            GUIUtil.AddGUICheckbox(this, this.greenPicker);
            GUIUtil.AddGUICheckbox(this, this.bluePicker);
            GUIUtil.AddGUICheckbox(this, this.yellowPicker);
            GUIUtil.AddGUICheckbox(this, this.magentaPicker);
            GUIUtil.AddGUICheckbox(this, this.cyanPicker);

            GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {

        }

        #region Properties
        public Color WhiteValue
        {
            get
            {
                return this.whitePicker.Value;
            }
        }

        public Color BlackValue
        {
            get
            {
                return this.blackPicker.Value;
            }
        }

        public Color RedValue
        {
            get
            {
                return this.redPicker.Value;
            }
        }

        public Color GreenValue
        {
            get
            {
                return this.greenPicker.Value;
            }
        }

        public Color BlueValue
        {
            get
            {
                return this.bluePicker.Value;
            }
        }

        public Color YellowValue
        {
            get
            {
                return this.yellowPicker.Value;
            }
        }

        public Color MagentaValue
        {
            get
            {
                return this.magentaPicker.Value;
            }
        }

        public Color CyanValue
        {
            get
            {
                return this.cyanPicker.Value;
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
        private CustomColorPicker whitePicker = null;
        private CustomColorPicker blackPicker = null;
        private CustomColorPicker redPicker = null;
        private CustomColorPicker greenPicker = null;
        private CustomColorPicker bluePicker = null;
        private CustomColorPicker yellowPicker = null;
        private CustomColorPicker magentaPicker = null;
        private CustomColorPicker cyanPicker = null;
        private CustomSlider amountSlider = null;
        #endregion
    }
}
