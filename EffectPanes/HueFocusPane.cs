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
    internal class HueFocusPane : BasePane
    {
        public HueFocusPane( int fontSize ) : base( fontSize, "Hue Focus" ) {}

        override public void SetupPane()
        {
            this.hueSlider = new CustomSlider( HueFocusDef.hueFocusEffect.hue, 0f, 360f, 1 );
            this.hueSlider.Text = "Hue";
            this.ChildControls.Add( this.hueSlider );

            this.rangeSlider = new CustomSlider( HueFocusDef.hueFocusEffect.range, 1f, 180f, 1 );
            this.rangeSlider.Text = "Range";
            this.ChildControls.Add( this.rangeSlider );

            this.boostSlider = new CustomSlider( HueFocusDef.hueFocusEffect.boost, 0f, 1f, 1 );
            this.boostSlider.Text = "Boost";
            this.ChildControls.Add( this.boostSlider );

            this.amountSlider = new CustomSlider( HueFocusDef.hueFocusEffect.amount, 0f, 1f, 1 );
            this.amountSlider.Text = "Amount";
            this.ChildControls.Add( this.amountSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.hueSlider);
            GUIUtil.AddGUISlider(this, this.rangeSlider);
            GUIUtil.AddGUISlider(this, this.boostSlider);
            GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {
            HueFocusDef.Reset();
        }

        #region Properties
        public float HueValue
        {
            get
            {
                return this.hueSlider.Value;
            }
        }

        public float RangeValue
        {
            get
            {
                return this.rangeSlider.Value;
            }
        }

        public float BoostValue
        {
            get
            {
                return this.boostSlider.Value;
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
        private CustomSlider hueSlider = null;
        private CustomSlider rangeSlider = null;
        private CustomSlider boostSlider = null;
        private CustomSlider amountSlider = null;
        #endregion
    }
}
