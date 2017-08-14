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
    internal class RaymarcherPane : BasePane
    {
        public RaymarcherPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Raymarcher") ) {}

        override public void SetupPane()
        {
            this.screenSpaceCheckbox = new CustomToggleButton( RaymarcherDef.raymarcherEffect.screenSpace, "toggle" );
            this.screenSpaceCheckbox.Text = Translation.GetText("Raymarcher", "screenSpace");
            this.ChildControls.Add( this.screenSpaceCheckbox );

            this.enableAdaptiveCheckbox = new CustomToggleButton( RaymarcherDef.raymarcherEffect.enableAdaptive, "toggle" );
            this.enableAdaptiveCheckbox.Text = Translation.GetText("Raymarcher", "enableAdaptive");
            this.ChildControls.Add( this.enableAdaptiveCheckbox );

            this.enableTemporalCheckbox = new CustomToggleButton( RaymarcherDef.raymarcherEffect.enableTemporal, "toggle" );
            this.enableTemporalCheckbox.Text = Translation.GetText("Raymarcher", "enableTemporal");
            this.ChildControls.Add( this.enableTemporalCheckbox );

            this.enableGlowlineCheckbox = new CustomToggleButton( RaymarcherDef.raymarcherEffect.enableGlowline, "toggle" );
            this.enableGlowlineCheckbox.Text = Translation.GetText("Raymarcher", "enableGlowline");
            this.ChildControls.Add( this.enableGlowlineCheckbox );

            this.dbgShowStepsCheckbox = new CustomToggleButton( RaymarcherDef.raymarcherEffect.dbgShowSteps, "toggle" );
            this.dbgShowStepsCheckbox.Text = Translation.GetText("Raymarcher", "dbgShowSteps");
            this.ChildControls.Add( this.dbgShowStepsCheckbox );

            this.sceneSlider = new CustomSlider( RaymarcherDef.raymarcherEffect.scene, 0f, 5f, 0 );
            this.sceneSlider.Text = Translation.GetText("Raymarcher", "scene");
            this.ChildControls.Add( this.sceneSlider );

            this.fogColorPicker = new CustomColorPicker( RaymarcherDef.raymarcherEffect.fogColor );
            this.fogColorPicker.Text = Translation.GetText("Raymarcher", "fogColor");
            this.ChildControls.Add( this.fogColorPicker );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.screenSpaceCheckbox);
           GUIUtil.AddGUICheckbox(this, this.enableAdaptiveCheckbox);
           GUIUtil.AddGUICheckbox(this, this.enableTemporalCheckbox);
           GUIUtil.AddGUICheckbox(this, this.enableGlowlineCheckbox);
           GUIUtil.AddGUICheckbox(this, this.dbgShowStepsCheckbox);
           GUIUtil.AddGUISlider(this, this.sceneSlider);
           GUIUtil.AddGUICheckbox(this, this.fogColorPicker);
        }

        override public void Reset()
        {
            RaymarcherDef.Reset();
        }

        #region Properties
        public bool ScreenSpaceValue
        {
            get
            {
                return this.screenSpaceCheckbox.Value;
            }
        }

        public bool EnableAdaptiveValue
        {
            get
            {
                return this.enableAdaptiveCheckbox.Value;
            }
        }

        public bool EnableTemporalValue
        {
            get
            {
                return this.enableTemporalCheckbox.Value;
            }
        }

        public bool EnableGlowlineValue
        {
            get
            {
                return this.enableGlowlineCheckbox.Value;
            }
        }

        public bool DbgShowStepsValue
        {
            get
            {
                return this.dbgShowStepsCheckbox.Value;
            }
        }

        public int SceneValue
        {
            get
            {
                return (int)this.sceneSlider.Value;
            }
        }

        public Color FogColorValue
        {
            get
            {
                return this.fogColorPicker.Value;
            }
        }

        #endregion

        #region Fields
        private CustomToggleButton screenSpaceCheckbox = null;
        private CustomToggleButton enableAdaptiveCheckbox = null;
        private CustomToggleButton enableTemporalCheckbox = null;
        private CustomToggleButton enableGlowlineCheckbox = null;
        private CustomToggleButton dbgShowStepsCheckbox = null;
        private CustomSlider sceneSlider = null;
        private CustomColorPicker fogColorPicker = null;
        #endregion
    }
}
