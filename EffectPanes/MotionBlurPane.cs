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
    internal class MotionBlurPane : BasePane
    {
        public MotionBlurPane( int fontSize ) : base( fontSize, "MotionBlur" ) {}

        override public void SetupPane()
        {
            this.extraBlurCheckbox = new CustomToggleButton( MotionBlurDef.motionBlurEffect.extraBlur, "toggle" );
            this.extraBlurCheckbox.Text = "Extra Blur";
            this.ChildControls.Add( this.extraBlurCheckbox );

            this.blurAmountSlider = new CustomSlider( MotionBlurDef.motionBlurEffect.blurAmount, 0f, 1f, 1 );
            this.blurAmountSlider.Text = "Blur Amount";
            this.ChildControls.Add( this.blurAmountSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.extraBlurCheckbox);
            GUIUtil.AddGUISlider(this, this.blurAmountSlider);
        }

        override public void Reset()
        {

        }

        #region Properties
        public bool ExtraBlurValue
        {
            get
            {
                return this.extraBlurCheckbox.Value;
            }
        }

        public float BlurAmountValue
        {
            get
            {
                return this.blurAmountSlider.Value;
            }
        }
        #endregion

        #region Fields
        private CustomToggleButton extraBlurCheckbox = null;
        private CustomSlider blurAmountSlider = null;
        #endregion
    }
}
