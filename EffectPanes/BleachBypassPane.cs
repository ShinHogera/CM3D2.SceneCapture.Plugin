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
    internal class BleachBypassPane : BasePane
    {
        public BleachBypassPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "BleachBypass") ) {}

        override public void SetupPane()
        {
            this.amountSlider = new CustomSlider ( BleachBypassDef.bleachBypassEffect.amount, 0f, 1f, 4 );
            this.amountSlider.Text = Translation.GetText("BleachBypass", "amount");
            this.ChildControls.Add( this.amountSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {
            BleachBypassDef.Reset();
        }

        #region Properties
        public float AmountValue
        {
            get
            {
                return this.amountSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider amountSlider = null;
        #endregion
    }
}
