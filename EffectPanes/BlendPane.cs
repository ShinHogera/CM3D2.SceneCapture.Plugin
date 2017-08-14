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
    internal class BlendPane : BasePane
    {
        public BlendPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Blend") ) {}

        override public void SetupPane()
        {
            this.amountSlider = new CustomSlider( BlendDef.blendEffect.amount, 0f, 1f, 4 );
            this.amountSlider.Text = Translation.GetText("Blend", "amount");
            this.ChildControls.Add( this.amountSlider );

            this.modeComboBox = new CustomComboBox ( BLEND_MODES );
            this.modeComboBox.Text = Translation.GetText("Blend", "mode");
            this.modeComboBox.SelectedIndex = (int)BlendDef.blendEffect.mode;
            this.ChildControls.Add( this.modeComboBox );

            this.texturePicker = new CustomImagePicker( BlendDef.blendEffect.texture, BlendDef.textureFile, ConstantValues.ImageDirsAll );
            this.texturePicker.Text = Translation.GetText("Blend", "texture");
            this.ChildControls.Add( this.texturePicker );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.texturePicker);
            GUIUtil.AddGUICheckbox(this, this.modeComboBox);
            GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {
            BlendDef.Reset();
        }

        #region Properties
        public Texture2D TextureValue
        {
            get
            {
                return this.texturePicker.Value;
            }
        }
        public string TextureFileValue {
            get
            {
                return this.texturePicker.Filename;
            }
        }
        public float AmountValue
        {
            get
            {
                return this.amountSlider.Value;
            }
        }

        public Blend.BlendingMode ModeValue
        {
            get
            {
                return (Blend.BlendingMode) Enum.Parse( typeof( Blend.BlendingMode ), this.modeComboBox.SelectedItem);
            }
        }
        #endregion

        #region Fields
        private static readonly string[] BLEND_MODES = new string[] { "Darken" ,
                                                                      "Multiply" ,
                                                                      "ColorBurn" ,
                                                                      "LinearBurn" ,
                                                                      "DarkerColor" ,
                                                                      "Lighten" ,
                                                                      "Screen" ,
                                                                      "ColorDodge" ,
                                                                      "LinearDodge" ,
                                                                      "LighterColor" ,
                                                                      "Overlay" ,
                                                                      "SoftLight" ,
                                                                      "HardLight" ,
                                                                      "VividLight" ,
                                                                      "LinearLight" ,
                                                                      "PinLight" ,
                                                                      "HardMix" ,
                                                                      "Difference" ,
                                                                      "Exclusion" ,
                                                                      "Subtract" ,
                                                                      "Divide"  };

        private CustomSlider amountSlider = null;
        private CustomComboBox modeComboBox = null;
        private CustomImagePicker texturePicker = null;
        #endregion
    }
}
