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
    internal class ContrastVignettePane : BasePane
    {
        public ContrastVignettePane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "ContrastVignette") ) {}

        override public void SetupPane()
        {
            this.centerXSlider = new CustomSlider( ContrastVignetteDef.contrastVignetteEffect.center.x, 0f, 1f, 4);
            this.centerXSlider.Text = Translation.GetText("ContrastVignette", "centerX");
            this.ChildControls.Add( this.centerXSlider );

            this.centerYSlider = new CustomSlider( ContrastVignetteDef.contrastVignetteEffect.center.y, 0f, 1f, 4);
            this.centerYSlider.Text = Translation.GetText("ContrastVignette", "centerY");
            this.ChildControls.Add( this.centerYSlider );

            this.sharpnessSlider = new CustomSlider( ContrastVignetteDef.contrastVignetteEffect.sharpness, -100f, 100f, 2 );
            this.sharpnessSlider.Text = Translation.GetText("ContrastVignette", "sharpness");
            this.ChildControls.Add( this.sharpnessSlider );

            this.darknessSlider = new CustomSlider( ContrastVignetteDef.contrastVignetteEffect.darkness, 0f, 100f, 2 );
            this.darknessSlider.Text = Translation.GetText("ContrastVignette", "darkness");
            this.ChildControls.Add( this.darknessSlider );

            this.contrastSlider = new CustomSlider( ContrastVignetteDef.contrastVignetteEffect.contrast, 0f, 200f, 2 );
            this.contrastSlider.Text = Translation.GetText("ContrastVignette", "contrast");
            this.ChildControls.Add( this.contrastSlider );

            this.contrastCoeffXSlider = new CustomSlider( ContrastVignetteDef.contrastVignetteEffect.contrastCoeff.x, 0f, 1f, 4);
            this.contrastCoeffXSlider.Text = Translation.GetText("ContrastVignette", "contrastCoeffX");
            this.ChildControls.Add( this.contrastCoeffXSlider );

            this.contrastCoeffYSlider = new CustomSlider( ContrastVignetteDef.contrastVignetteEffect.contrastCoeff.y, 0f, 1f, 4);
            this.contrastCoeffYSlider.Text = Translation.GetText("ContrastVignette", "contrastCoeffY");
            this.ChildControls.Add( this.contrastCoeffYSlider );

            this.contrastCoeffZSlider = new CustomSlider( ContrastVignetteDef.contrastVignetteEffect.contrastCoeff.z, 0f, 1f, 4);
            this.contrastCoeffZSlider.Text = Translation.GetText("ContrastVignette", "contrastCoeffZ");
            this.ChildControls.Add( this.contrastCoeffZSlider );

            this.edgeBlendingSlider = new CustomSlider( ContrastVignetteDef.contrastVignetteEffect.edgeBlending, 0f, 200f, 2 );
            this.edgeBlendingSlider.Text = Translation.GetText("ContrastVignette", "edgeBlending");
            this.ChildControls.Add( this.edgeBlendingSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.centerXSlider);
           GUIUtil.AddGUISlider(this, this.centerYSlider);
           GUIUtil.AddGUISlider(this, this.sharpnessSlider);
           GUIUtil.AddGUISlider(this, this.darknessSlider);
           GUIUtil.AddGUISlider(this, this.contrastSlider);
           GUIUtil.AddGUISlider(this, this.contrastCoeffXSlider);
           GUIUtil.AddGUISlider(this, this.contrastCoeffYSlider);
           GUIUtil.AddGUISlider(this, this.contrastCoeffZSlider);
           GUIUtil.AddGUISlider(this, this.edgeBlendingSlider);
        }

        override public void Reset()
        {
            ContrastVignetteDef.Reset();
        }

        #region Properties
        public Vector2 CenterValue
        {
            get
            {
                return new Vector2(this.centerXSlider.Value, this.centerYSlider.Value);
            }
        }

        public Vector3 ContrastCoeffValue
        {
            get
            {
                return new Vector3(this.contrastCoeffXSlider.Value, this.contrastCoeffYSlider.Value, this.contrastCoeffZSlider.Value);
            }
        }

        public float CenterXValue
        {
            get
            {
                return this.centerXSlider.Value;
            }
        }

        public float CenterYValue
        {
            get
            {
                return this.centerYSlider.Value;
            }
        }

        public float SharpnessValue
        {
            get
            {
                return this.sharpnessSlider.Value;
            }
        }

        public float DarknessValue
        {
            get
            {
                return this.darknessSlider.Value;
            }
        }

        public float ContrastValue
        {
            get
            {
                return this.contrastSlider.Value;
            }
        }

        public float ContrastCoeffXValue
        {
            get
            {
                return this.contrastCoeffXSlider.Value;
            }
        }

        public float ContrastCoeffYValue
        {
            get
            {
                return this.contrastCoeffYSlider.Value;
            }
        }

        public float ContrastCoeffZValue
        {
            get
            {
                return this.contrastCoeffZSlider.Value;
            }
        }

        public float EdgeBlendingValue
        {
            get
            {
                return this.edgeBlendingSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider centerXSlider = null;
        private CustomSlider centerYSlider = null;
        private CustomSlider sharpnessSlider = null;
        private CustomSlider darknessSlider = null;
        private CustomSlider contrastSlider = null;
        private CustomSlider contrastCoeffXSlider = null;
        private CustomSlider contrastCoeffYSlider = null;
        private CustomSlider contrastCoeffZSlider = null;
        private CustomSlider edgeBlendingSlider = null;
        #endregion
    }
}
