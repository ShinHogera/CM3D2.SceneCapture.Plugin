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
    internal class TechnicolorPane : BasePane
    {
        public TechnicolorPane( int fontSize ) : base( fontSize, "Technicolor" ) {}

        override public void SetupPane()
        {
            this.exposureSlider = new CustomSlider( TechnicolorDef.technicolorEffect.exposure, 0f, 8f, 1 );
            this.exposureSlider.Text = "Exposure";
            this.ChildControls.Add( this.exposureSlider );
            this.balanceRedSlider = new CustomSlider( TechnicolorDef.technicolorEffect.balance.x, 0f, 1f, 1 );
            this.balanceRedSlider.Text = "Red Balance";
            this.ChildControls.Add( this.balanceRedSlider );
            this.balanceGreenSlider = new CustomSlider( TechnicolorDef.technicolorEffect.balance.y, 0f, 1f, 1 );
            this.balanceGreenSlider.Text = "Green Balance";
            this.ChildControls.Add( this.balanceGreenSlider );
            this.balanceBlueSlider = new CustomSlider( TechnicolorDef.technicolorEffect.balance.z, 0f, 1f, 1 );
            this.balanceBlueSlider.Text = "Blue Balance";
            this.ChildControls.Add( this.balanceBlueSlider );
            this.amountSlider = new CustomSlider( TechnicolorDef.technicolorEffect.amount, 0f, 1f, 1 );
            this.amountSlider.Text = "Amount";
            this.ChildControls.Add( this.amountSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.exposureSlider);
            GUIUtil.AddGUISlider(this, this.balanceRedSlider);
            GUIUtil.AddGUISlider(this, this.balanceGreenSlider);
            GUIUtil.AddGUISlider(this, this.balanceBlueSlider);
            GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {

        }

        #region Properties
        public float ExposureValue
        {
            get
            {
                return this.exposureSlider.Value;
            }
        }

        public Vector3 BalanceValue
        {
            get
            {
                return new Vector3(this.balanceRedSlider.Value, this.balanceGreenSlider.Value, this.balanceBlueSlider.Value);
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
        private CustomSlider exposureSlider = null;
        private CustomSlider balanceRedSlider = null;
        private CustomSlider balanceGreenSlider = null;
        private CustomSlider balanceBlueSlider = null;
        private CustomSlider amountSlider = null;
        #endregion
    }
}
