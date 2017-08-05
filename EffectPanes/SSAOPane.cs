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
    internal class SSAOPane : BasePane
    {
        public SSAOPane( int fontSize ) : base( fontSize, "SSAO" ) {}

        override public void SetupPane()
        {
            this.intensitySlider = new CustomSlider( SSAODef.ssaoEffect.intensity, 0.0f, 3.0f, 1 );
            this.intensitySlider.Text = "Intensity";
            this.ChildControls.Add( this.intensitySlider );

            this.radiusSlider = new CustomSlider( SSAODef.ssaoEffect.radius, 0.1f, 3.0f, 1 );
            this.radiusSlider.Text = "Radius";
            this.ChildControls.Add( this.radiusSlider );

            this.blurIterationsSlider = new CustomSlider( SSAODef.ssaoEffect.blurIterations, 0.0f, 3.0f, 1 );
            this.blurIterationsSlider.Text = "Blur Iterations";
            this.ChildControls.Add( this.blurIterationsSlider );

            this.blurFilterDistanceSlider = new CustomSlider( SSAODef.ssaoEffect.blurFilterDistance, 0.0f, 5.0f, 1 );
            this.blurFilterDistanceSlider.Text = "Blur Filter Distance";
            this.ChildControls.Add( this.blurFilterDistanceSlider );

            this.downsampleSlider = new CustomSlider( SSAODef.ssaoEffect.downsample, 0.0f, 1.0f, 1 );
            this.downsampleSlider.Text = "Downsample";
            this.ChildControls.Add( this.downsampleSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUISlider(this, this.intensitySlider);
            GUIUtil.AddGUISlider(this, this.radiusSlider);
            GUIUtil.AddGUISlider(this, this.blurIterationsSlider);
            GUIUtil.AddGUISlider(this, this.blurFilterDistanceSlider);
            GUIUtil.AddGUISlider(this, this.downsampleSlider);
        }

        override public void Reset()
        {
            SSAODef.Reset();
        }

        #region Properties
        public float IntensityValue
        {
            get
            {
                return this.intensitySlider.Value;
            }
        }

        public float RadiusValue
        {
            get
            {
                return this.radiusSlider.Value;
            }
        }

        public int BlurIterationsValue
        {
            get
            {
                return (int)this.blurIterationsSlider.Value;
            }
        }

        public float BlurFilterDistanceValue
        {
            get
            {
                return this.blurFilterDistanceSlider.Value;
            }
        }

        public int DownsampleValue
        {
            get
            {
                return (int)this.downsampleSlider.Value;
            }
        }
        #endregion

        #region Fields
        private CustomSlider intensitySlider = null;
        private CustomSlider radiusSlider = null;
        private CustomSlider blurIterationsSlider = null;
        private CustomSlider blurFilterDistanceSlider = null;
        private CustomSlider downsampleSlider = null;
        #endregion
    }
}
