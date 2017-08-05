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
    internal class ChannelSwapPane : BasePane
    {
        public ChannelSwapPane( int fontSize ) : base( fontSize, "ChannelSwap" ) {}

        override public void SetupPane()
        {
                this.redSourceBox = new CustomComboBox( CHANNELS );
                this.redSourceBox.Text = "Red Source";
                this.redSourceBox.SelectedIndex = 0;
                this.ChildControls.Add( this.redSourceBox );

                this.greenSourceBox = new CustomComboBox( CHANNELS );
                this.greenSourceBox.Text = "Green Source";
                this.greenSourceBox.SelectedIndex = 1;
                this.ChildControls.Add( this.greenSourceBox );

                this.blueSourceBox = new CustomComboBox( CHANNELS );
                this.blueSourceBox.Text = "Blue Source";
                this.blueSourceBox.SelectedIndex = 2;
                this.ChildControls.Add( this.blueSourceBox );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.redSourceBox);
            GUIUtil.AddGUICheckbox(this, this.greenSourceBox);
            GUIUtil.AddGUICheckbox(this, this.blueSourceBox);
        }

        override public void Reset()
        {
            ChannelSwapDef.Reset();
        }

        #region Properties
        public ChannelSwapper.Channel RedSourceValue
        {
            get
            {
                return (ChannelSwapper.Channel) Enum.Parse( typeof( ChannelSwapper.Channel ), this.redSourceBox.SelectedItem);
            }
        }

        public ChannelSwapper.Channel GreenSourceValue
        {
            get
            {
                return (ChannelSwapper.Channel) Enum.Parse( typeof( ChannelSwapper.Channel ), this.greenSourceBox.SelectedItem);
            }
        }

        public ChannelSwapper.Channel BlueSourceValue
        {
            get
            {
                return (ChannelSwapper.Channel) Enum.Parse( typeof( ChannelSwapper.Channel ), this.blueSourceBox.SelectedItem);
            }
        }
        #endregion

        #region Fields
        private static readonly string[] CHANNELS = new string[] { "Red", "Green", "Blue" };

        private CustomComboBox redSourceBox = null;
        private CustomComboBox greenSourceBox = null;
        private CustomComboBox blueSourceBox = null;
        #endregion
    }
}
