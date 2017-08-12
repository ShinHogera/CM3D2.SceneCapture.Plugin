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
    internal class ExamplePane : BasePane
    {
        public ExamplePane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Example") ) {}

        override public void SetupPane()
        {
            this.doodSlider = new CustomSlider ( ExampleDef.exampleEffect.dood, 1f, 100f,  );
            this.doodSlider.Text = Translation.GetText("Example", "dood");
            this.ChildControls.Add( this.doodSlider );
            this.lightTypeComboBox = new CustomComboBox ( LIGHT_TYPES );
            this.lightTypeComboBox.Text = Translation.GetText("Example", "lightType");
            this.lightTypeComboBox.SelectedIndex = (int)ExampleDef.exampleEffect.lightType;
            this.ChildControls.Add( this.lightTypeComboBox );
            this.isWorkingCheckbox = new CustomToggleButton ( ExampleDef.exampleEffect.isWorking, "toggle" );
            this.isWorkingCheckbox.Text = Translation.GetText("Example", "isWorking");
            this.ChildControls.Add( this.isWorkingCheckbox );
            this.backgroundColorPicker = new CustomColorPicker ( ExampleDef.exampleEffect.backgroundColor );
            this.backgroundColorPicker.Text = Translation.GetText("Example", "backgroundColor");
            this.ChildControls.Add( this.backgroundColorPicker );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.doodSlider);
           GUIUtil.AddGUICheckbox(this, this.lightTypeBox);
           GUIUtil.AddGUICheckbox(this, this.isWorkingCheckbox);
           GUIUtil.AddGUICheckbox(this, this.backgroundColorPicker);
        }

        override public void Reset()
        {
            ExampleDef.Reset();
        }

        #region Properties
        public float DoodValue
        {
            get
            {
                return this.doodSlider.Value;
            }
        }

        public Light.LightType LightTypeValue
        {
            get
            {
                return (Light.LightType) Enum.Parse( typeof( Light.LightType ), this.lightTypeComboBox.SelectedItem);
            }
        }

        public bool IsWorkingValue
        {
            get
            {
                return this.isWorkingCheckbox.Value;
            }
        }

        public Color BackgroundColorValue
        {
            get
            {
                return this.backgroundColorPicker.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider doodSlider = null;
        private CustomComboBox lightTypeBox = null;
        private CustomToggleButton isWorkingCheckbox = null;
        private CustomColorPicker backgroundColorPicker = null;
        #endregion
    }
}
