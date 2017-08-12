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
    internal class LookupFilterPane : BasePane
    {
        public LookupFilterPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "LookupFilter") ) {}

        override public void SetupPane()
        {
            this.amountSlider = new CustomSlider( LookupFilterDef.lookupFilterEffect.amount, 0f, 1f, 4 );
            this.amountSlider.Text = Translation.GetText("LookupFilter", "amount");
            this.ChildControls.Add( this.amountSlider );

            this.forceCompatibilityCheckbox = new CustomToggleButton( LookupFilterDef.lookupFilterEffect.forceCompatibility, "toggle" );
            this.forceCompatibilityCheckbox.Text = Translation.GetText("LookupFilter", "forceCompatibility");
            this.ChildControls.Add( this.forceCompatibilityCheckbox );

            this.lookupTexturePicker = new CustomImagePicker( LookupFilterDef.lookupFilterEffect.lookupTexture, LookupFilterDef.lookupTextureFile, ConstantValues.ImageDirsLUT );
            this.lookupTexturePicker.Text = Translation.GetText("LookupFilter", "lookupTexture");
            this.ChildControls.Add( this.lookupTexturePicker );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.lookupTexturePicker);
           GUIUtil.AddGUISlider(this, this.amountSlider);
           GUIUtil.AddGUICheckbox(this, this.forceCompatibilityCheckbox);
        }

        override public void Reset()
        {
            LookupFilterDef.Reset();
        }

        #region Properties
        public Texture2D LookupTextureValue
        {
            get
            {
                return this.lookupTexturePicker.Value;
            }
        }
        public string LookupTextureFileValue {
            get
            {
                return this.lookupTexturePicker.Filename;
            }
        }
        public float AmountValue
        {
            get
            {
                return this.amountSlider.Value;
            }
        }

        public bool ForceCompatibilityValue
        {
            get
            {
                return this.forceCompatibilityCheckbox.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider amountSlider = null;
        private CustomToggleButton forceCompatibilityCheckbox = null;
        private CustomImagePicker lookupTexturePicker = null;
        #endregion
    }
}
