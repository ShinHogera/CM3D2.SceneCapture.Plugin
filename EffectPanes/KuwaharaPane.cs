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
    internal class KuwaharaPane : BasePane
    {
        public KuwaharaPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Kuwahara") ) {}

        override public void SetupPane()
        {
            this.radiusSlider = new CustomSlider( KuwaharaDef.kuwaharaEffect.radius, 1f, 6f, 0 );
            this.radiusSlider.Text = Translation.GetText("Kuwahara", "radius");
            this.ChildControls.Add( this.radiusSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.radiusSlider);
        }

        override public void Reset()
        {
            KuwaharaDef.Reset();
        }

        #region Properties
        public int RadiusValue
        {
            get
            {
                return (int)this.radiusSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider radiusSlider = null;
        #endregion
    }
}
