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
    internal class PixelatePane : BasePane
    {
        public PixelatePane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Pixelate") ) {}

        override public void SetupPane()
        {
            this.scaleSlider = new CustomSlider( PixelateDef.pixelateEffect.scale, 1f, 1024f, 2 );
            this.scaleSlider.Text = Translation.GetText("Pixelate", "scale");
            this.ChildControls.Add( this.scaleSlider );

            this.automaticRatioCheckbox = new CustomToggleButton( PixelateDef.pixelateEffect.automaticRatio, "toggle" );
            this.automaticRatioCheckbox.Text = Translation.GetText("Pixelate", "automaticRatio");
            this.ChildControls.Add( this.automaticRatioCheckbox );

            this.ratioSlider = new CustomSlider( PixelateDef.pixelateEffect.ratio, 0f, 100f, 2 );
            this.ratioSlider.Text = Translation.GetText("Pixelate", "ratio");
            this.ChildControls.Add( this.ratioSlider );

            this.modeComboBox = new CustomComboBox ( PIXELATE_SIZEMODES );
            this.modeComboBox.Text = Translation.GetText("Pixelate", "mode");
            this.modeComboBox.SelectedIndex = (int)PixelateDef.pixelateEffect.mode;
            this.ChildControls.Add( this.modeComboBox );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.scaleSlider);
           GUIUtil.AddGUICheckbox(this, this.automaticRatioCheckbox);

           if( this.automaticRatioCheckbox.Value == false )
           {
               GUIUtil.AddGUISlider(this, this.ratioSlider);
           }

           GUIUtil.AddGUICheckbox(this, this.modeComboBox);
        }

        override public void Reset()
        {
            PixelateDef.Reset();
        }

        #region Properties
        public float ScaleValue
        {
            get
            {
                return this.scaleSlider.Value;
            }
        }

        public bool AutomaticRatioValue
        {
            get
            {
                return this.automaticRatioCheckbox.Value;
            }
        }

        public float RatioValue
        {
            get
            {
                return this.ratioSlider.Value;
            }
        }

        public Pixelate.SizeMode ModeValue
        {
            get
            {
                return (Pixelate.SizeMode) Enum.Parse( typeof( Pixelate.SizeMode ), this.modeComboBox.SelectedItem);
            }
        }

        #endregion

        #region Fields
        private static readonly string[] PIXELATE_SIZEMODES = new string[] { "ResolutionIndependent" , "PixelPerfect" };
        private CustomSlider scaleSlider = null;
        private CustomToggleButton automaticRatioCheckbox = null;
        private CustomSlider ratioSlider = null;
        private CustomComboBox modeComboBox = null;
        #endregion
    }
}
