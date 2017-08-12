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
    internal class RGBSplitPane : BasePane
    {
        public RGBSplitPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "RGBSplit") ) {}

        override public void SetupPane()
        {
            this.amountSlider = new CustomSlider( RGBSplitDef.rGBSplitEffect.amount, 0f, 5f, 4 );
            this.amountSlider.Text = Translation.GetText("RGBSplit", "amount");
            this.ChildControls.Add( this.amountSlider );

            this.angleSlider = new CustomSlider( RGBSplitDef.rGBSplitEffect.angle, 0f, 180f, 2 );
            this.angleSlider.Text = Translation.GetText("RGBSplit", "angle");
            this.ChildControls.Add( this.angleSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.amountSlider);
           GUIUtil.AddGUISlider(this, this.angleSlider);
        }

        override public void Reset()
        {
            RGBSplitDef.Reset();
        }

        #region Properties
        public float AmountValue
        {
            get
            {
                return this.amountSlider.Value;
            }
        }

        public float AngleValue
        {
            get
            {
                return this.angleSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider amountSlider = null;
        private CustomSlider angleSlider = null;
        #endregion
    }
}
