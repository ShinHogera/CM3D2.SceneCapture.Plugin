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
    internal class WhiteBalancePane : BasePane
    {
        public WhiteBalancePane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "WhiteBalance") ) {}

        override public void SetupPane()
        {
            this.whitePicker = new CustomColorPicker( WhiteBalanceDef.whiteBalanceEffect.white );
            this.whitePicker.Text = Translation.GetText("WhiteBalance", "white");
            this.whitePicker.IsRGBA = false;
            this.ChildControls.Add( this.whitePicker );

            this.modeComboBox = new CustomComboBox ( WHITEBALANCE_BALANCEMODES );
            this.modeComboBox.Text = Translation.GetText("WhiteBalance", "mode");
            this.modeComboBox.SelectedIndex = (int)WhiteBalanceDef.whiteBalanceEffect.mode;
            this.ChildControls.Add( this.modeComboBox );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.whitePicker);
           GUIUtil.AddGUICheckbox(this, this.modeComboBox);
        }

        override public void Reset()
        {
            WhiteBalanceDef.Reset();
        }

        #region Properties
        public Color WhiteValue
        {
            get
            {
                return this.whitePicker.Value;
            }
        }

        public WhiteBalance.BalanceMode ModeValue
        {
            get
            {
                return (WhiteBalance.BalanceMode) Enum.Parse( typeof( WhiteBalance.BalanceMode ), this.modeComboBox.SelectedItem);
            }
        }

        #endregion

        #region Fields
        private static readonly string[] WHITEBALANCE_BALANCEMODES = new string[] { "Simple" , "Complex" };
        private CustomColorPicker whitePicker = null;
        private CustomComboBox modeComboBox = null;
        #endregion
    }
}
