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
    internal class WigglePane : BasePane
    {
        public WigglePane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Wiggle") ) {}

        override public void SetupPane()
        {
            this.modeComboBox = new CustomComboBox ( WIGGLE_ALGORITHMS );
            this.modeComboBox.Text = Translation.GetText("Wiggle", "mode");
            this.modeComboBox.SelectedIndex = (int)WiggleDef.wiggleEffect.mode;
            this.ChildControls.Add( this.modeComboBox );

            this.timerSlider = new CustomSlider( WiggleDef.wiggleEffect.timer, 0f, 100f, 2 );
            this.timerSlider.Text = Translation.GetText("Wiggle", "timer");
            this.ChildControls.Add( this.timerSlider );

            this.speedSlider = new CustomSlider( WiggleDef.wiggleEffect.speed, 0f, 100f, 2 );
            this.speedSlider.Text = Translation.GetText("Wiggle", "speed");
            this.ChildControls.Add( this.speedSlider );

            this.frequencySlider = new CustomSlider( WiggleDef.wiggleEffect.frequency, 0f, 300f, 2 );
            this.frequencySlider.Text = Translation.GetText("Wiggle", "frequency");
            this.ChildControls.Add( this.frequencySlider );

            this.amplitudeSlider = new CustomSlider( WiggleDef.wiggleEffect.amplitude, 0f, 2f * (float)Math.PI, 4 );
            this.amplitudeSlider.Text = Translation.GetText("Wiggle", "amplitude");
            this.ChildControls.Add( this.amplitudeSlider );

            this.automaticTimerCheckbox = new CustomToggleButton( WiggleDef.wiggleEffect.automaticTimer, "toggle" );
            this.automaticTimerCheckbox.Text = Translation.GetText("Wiggle", "automaticTimer");
            this.ChildControls.Add( this.automaticTimerCheckbox );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUICheckbox(this, this.modeComboBox);
           GUIUtil.AddGUISlider(this, this.timerSlider);
           GUIUtil.AddGUISlider(this, this.speedSlider);
           GUIUtil.AddGUISlider(this, this.frequencySlider);
           GUIUtil.AddGUISlider(this, this.amplitudeSlider);
           GUIUtil.AddGUICheckbox(this, this.automaticTimerCheckbox);
        }

        override public void Reset()
        {
            WiggleDef.Reset();
        }

        #region Properties
        public Wiggle.Algorithm ModeValue
        {
            get
            {
                return (Wiggle.Algorithm) Enum.Parse( typeof( Wiggle.Algorithm ), this.modeComboBox.SelectedItem);
            }
        }

        public float TimerValue
        {
            get
            {
                return this.timerSlider.Value;
            }
            set
            {
                this.timerSlider.Value = value;
            }
        }

        public float SpeedValue
        {
            get
            {
                return this.speedSlider.Value;
            }
        }

        public float FrequencyValue
        {
            get
            {
                return this.frequencySlider.Value;
            }
        }

        public float AmplitudeValue
        {
            get
            {
                return this.amplitudeSlider.Value;
            }
        }

        public bool AutomaticTimerValue
        {
            get
            {
                return this.automaticTimerCheckbox.Value;
            }
        }

        #endregion

        #region Fields
        private static readonly string[] WIGGLE_ALGORITHMS = new string[] { "Simple" , "Complex" };
        private CustomComboBox modeComboBox = null;
        private CustomSlider timerSlider = null;
        private CustomSlider speedSlider = null;
        private CustomSlider frequencySlider = null;
        private CustomSlider amplitudeSlider = null;
        private CustomToggleButton automaticTimerCheckbox = null;
        #endregion
    }
}
