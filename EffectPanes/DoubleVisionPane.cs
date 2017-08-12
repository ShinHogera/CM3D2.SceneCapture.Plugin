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
    internal class DoubleVisionPane : BasePane
    {
        public DoubleVisionPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "DoubleVision") ) {}

        override public void SetupPane()
        {
            this.displaceXSlider = new CustomSlider( DoubleVisionDef.doubleVisionEffect.displace.x, 0f, 1f, 4);
            this.displaceXSlider.Text = Translation.GetText("DoubleVision", "displaceX");
            this.ChildControls.Add( this.displaceXSlider );

            this.displaceYSlider = new CustomSlider( DoubleVisionDef.doubleVisionEffect.displace.y, 0f, 1f, 4);
            this.displaceYSlider.Text = Translation.GetText("DoubleVision", "displaceY");
            this.ChildControls.Add( this.displaceYSlider );

            this.amountSlider = new CustomSlider( DoubleVisionDef.doubleVisionEffect.amount, 0f, 1f, 4 );
            this.amountSlider.Text = Translation.GetText("DoubleVision", "amount");
            this.ChildControls.Add( this.amountSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.displaceXSlider);
           GUIUtil.AddGUISlider(this, this.displaceYSlider);
           GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {
            DoubleVisionDef.Reset();
        }

        #region Properties
        public Vector2 DisplaceValue
        {
            get
            {
                return new Vector2(this.displaceXSlider.Value, this.displaceYSlider.Value);
            }
        }

        public float DisplaceXValue
        {
            get
            {
                return this.displaceXSlider.Value;
            }
        }

        public float DisplaceYValue
        {
            get
            {
                return this.displaceYSlider.Value;
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
        private CustomSlider displaceXSlider = null;
        private CustomSlider displaceYSlider = null;
        private CustomSlider amountSlider = null;
        #endregion
    }
}
