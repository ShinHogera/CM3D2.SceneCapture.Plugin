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
    internal class CreasePane : BasePane
    {
        public CreasePane( int fontSize ) : base( fontSize, "Crease" ) {}

        override public void SetupPane()
        {
                this.intensitySlider = new CustomSlider( CreaseDef.creaseEffect.intensity, -40f, 40f, 1 );
                this.intensitySlider.Text = "Intensity";
                this.ChildControls.Add( this.intensitySlider );

                this.softnessSlider = new CustomSlider( CreaseDef.creaseEffect.softness, 0f, 10f, 1 );
                this.softnessSlider.Text = "Softness";
                this.ChildControls.Add( this.softnessSlider );

                this.spreadSlider = new CustomSlider( CreaseDef.creaseEffect.spread, 0f, 10f, 1 );
                this.spreadSlider.Text = "Spread";
                this.ChildControls.Add( this.spreadSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.intensitySlider);
            GUIUtil.AddGUISlider(this, this.softnessSlider);
            GUIUtil.AddGUISlider(this, this.spreadSlider);
        }

        override public void Reset()
        {
            CreaseDef.Reset();
        }

        #region Properties
        public float IntensityValue
        {
            get
            {
                return this.intensitySlider.Value;
            }
        }

        public int SoftnessValue
        {
            get
            {
                return (int)this.softnessSlider.Value;
            }
        }

        public float SpreadValue
        {
            get
            {
                return this.spreadSlider.Value;
            }
        }
        #endregion

        #region Fields
        private CustomSlider intensitySlider = null;
        private CustomSlider softnessSlider = null;
        private CustomSlider spreadSlider = null;
        #endregion
    }
}
