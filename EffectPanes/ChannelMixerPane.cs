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
    internal class ChannelMixerPane : BasePane
    {
        public ChannelMixerPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "ChannelMixer") ) {}

        override public void SetupPane()
        {
            this.redXSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.red.x, 0f, 100f, 4);
            this.redXSlider.Text = Translation.GetText("ChannelMixer", "redX");
            this.ChildControls.Add( this.redXSlider );

            this.redYSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.red.y, 0f, 100f, 4);
            this.redYSlider.Text = Translation.GetText("ChannelMixer", "redY");
            this.ChildControls.Add( this.redYSlider );

            this.redZSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.red.z, 0f, 100f, 4);
            this.redZSlider.Text = Translation.GetText("ChannelMixer", "redZ");
            this.ChildControls.Add( this.redZSlider );

            this.greenXSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.green.x, 0f, 100f, 4);
            this.greenXSlider.Text = Translation.GetText("ChannelMixer", "greenX");
            this.ChildControls.Add( this.greenXSlider );

            this.greenYSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.green.y, 0f, 100f, 4);
            this.greenYSlider.Text = Translation.GetText("ChannelMixer", "greenY");
            this.ChildControls.Add( this.greenYSlider );

            this.greenZSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.green.z, 0f, 100f, 4);
            this.greenZSlider.Text = Translation.GetText("ChannelMixer", "greenZ");
            this.ChildControls.Add( this.greenZSlider );

            this.blueXSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.blue.x, 0f, 100f, 4);
            this.blueXSlider.Text = Translation.GetText("ChannelMixer", "blueX");
            this.ChildControls.Add( this.blueXSlider );

            this.blueYSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.blue.y, 0f, 100f, 4);
            this.blueYSlider.Text = Translation.GetText("ChannelMixer", "blueY");
            this.ChildControls.Add( this.blueYSlider );

            this.blueZSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.blue.z, 0f, 100f, 4);
            this.blueZSlider.Text = Translation.GetText("ChannelMixer", "blueZ");
            this.ChildControls.Add( this.blueZSlider );

            this.constantXSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.constant.x, 0f, 100f, 4);
            this.constantXSlider.Text = Translation.GetText("ChannelMixer", "constantX");
            this.ChildControls.Add( this.constantXSlider );

            this.constantYSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.constant.y, 0f, 100f, 4);
            this.constantYSlider.Text = Translation.GetText("ChannelMixer", "constantY");
            this.ChildControls.Add( this.constantYSlider );

            this.constantZSlider = new CustomSlider( ChannelMixerDef.channelMixerEffect.constant.z, 0f, 100f, 4);
            this.constantZSlider.Text = Translation.GetText("ChannelMixer", "constantZ");
            this.ChildControls.Add( this.constantZSlider );
        }

        override public void ShowPane()
        {
           GUIUtil.AddGUISlider(this, this.redXSlider);
           GUIUtil.AddGUISlider(this, this.redYSlider);
           GUIUtil.AddGUISlider(this, this.redZSlider);
           GUIUtil.AddGUISlider(this, this.greenXSlider);
           GUIUtil.AddGUISlider(this, this.greenYSlider);
           GUIUtil.AddGUISlider(this, this.greenZSlider);
           GUIUtil.AddGUISlider(this, this.blueXSlider);
           GUIUtil.AddGUISlider(this, this.blueYSlider);
           GUIUtil.AddGUISlider(this, this.blueZSlider);
           GUIUtil.AddGUISlider(this, this.constantXSlider);
           GUIUtil.AddGUISlider(this, this.constantYSlider);
           GUIUtil.AddGUISlider(this, this.constantZSlider);
        }

        override public void Reset()
        {
            ChannelMixerDef.Reset();
        }

        #region Properties
        public Vector3 RedValue
        {
            get
            {
                return new Vector3(this.redXSlider.Value, this.redYSlider.Value, this.redZSlider.Value);
            }
        }

        public Vector3 GreenValue
        {
            get
            {
                return new Vector3(this.greenXSlider.Value, this.greenYSlider.Value, this.greenZSlider.Value);
            }
        }

        public Vector3 BlueValue
        {
            get
            {
                return new Vector3(this.blueXSlider.Value, this.blueYSlider.Value, this.blueZSlider.Value);
            }
        }

        public Vector3 ConstantValue
        {
            get
            {
                return new Vector3(this.constantXSlider.Value, this.constantYSlider.Value, this.constantZSlider.Value);
            }
        }

        public float RedXValue
        {
            get
            {
                return this.redXSlider.Value;
            }
        }

        public float RedYValue
        {
            get
            {
                return this.redYSlider.Value;
            }
        }

        public float RedZValue
        {
            get
            {
                return this.redZSlider.Value;
            }
        }

        public float GreenXValue
        {
            get
            {
                return this.greenXSlider.Value;
            }
        }

        public float GreenYValue
        {
            get
            {
                return this.greenYSlider.Value;
            }
        }

        public float GreenZValue
        {
            get
            {
                return this.greenZSlider.Value;
            }
        }

        public float BlueXValue
        {
            get
            {
                return this.blueXSlider.Value;
            }
        }

        public float BlueYValue
        {
            get
            {
                return this.blueYSlider.Value;
            }
        }

        public float BlueZValue
        {
            get
            {
                return this.blueZSlider.Value;
            }
        }

        public float ConstantXValue
        {
            get
            {
                return this.constantXSlider.Value;
            }
        }

        public float ConstantYValue
        {
            get
            {
                return this.constantYSlider.Value;
            }
        }

        public float ConstantZValue
        {
            get
            {
                return this.constantZSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomSlider redXSlider = null;
        private CustomSlider redYSlider = null;
        private CustomSlider redZSlider = null;
        private CustomSlider greenXSlider = null;
        private CustomSlider greenYSlider = null;
        private CustomSlider greenZSlider = null;
        private CustomSlider blueXSlider = null;
        private CustomSlider blueYSlider = null;
        private CustomSlider blueZSlider = null;
        private CustomSlider constantXSlider = null;
        private CustomSlider constantYSlider = null;
        private CustomSlider constantZSlider = null;
        #endregion
    }
}
