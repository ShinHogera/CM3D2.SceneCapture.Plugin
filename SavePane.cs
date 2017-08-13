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
    internal class SavePane : ControlBase
    {
        public SavePane( int fontSize, string name )
        {
            this.FontSize = fontSize;
            this.name = name;
            this.wantsLoad = false;

            this.Awake();
        }

        override public void Awake()
        {
            this.nameLabel = new CustomLabel();
            this.nameLabel.FontSize = this.FontSize;
            this.nameLabel.Text = this.name;
            this.ChildControls.Add( this.nameLabel );

            this.loadButton = new CustomButton();
            this.loadButton.FontSize = this.FontSize;
            this.loadButton.Text = Translation.GetText("UI", "load");
            this.loadButton.Click += this.LoadPressed;
            this.ChildControls.Add( this.loadButton );

            this.deleteButton = new CustomButton();
            this.deleteButton.FontSize = this.FontSize;
            this.deleteButton.Text = Translation.GetText("UI", "delete");
            this.deleteButton.Click += this.DeletePressed;
            this.ChildControls.Add( this.deleteButton );
        }

        override public void OnGUI()
        {
            this.SetAllVisible(false, 0);

            this.nameLabel.Left = this.Left + ControlBase.FixedMargin;
            this.nameLabel.Top = this.Top + ControlBase.FixedMargin;
            this.nameLabel.Width = this.Width / 2 - ControlBase.FixedMargin / 4;
            this.nameLabel.Height = this.ControlHeight;
            this.nameLabel.OnGUI();

            this.loadButton.Left = this.Left + this.nameLabel.Width + ControlBase.FixedMargin;
            this.loadButton.Top = this.nameLabel.Top;
            this.loadButton.Width = this.Width / 4 - ControlBase.FixedMargin / 4;
            this.loadButton.Height = this.ControlHeight;
            this.loadButton.OnGUI();

            this.deleteButton.Left = this.Left + this.nameLabel.Width + this.loadButton.Width + ControlBase.FixedMargin;
            this.deleteButton.Top = this.nameLabel.Top;
            this.deleteButton.Width = this.Width / 4 - ControlBase.FixedMargin / 4;
            this.deleteButton.Height = this.ControlHeight;
            this.deleteButton.OnGUI();

            foreach(ControlBase control in this.ChildControls)
            {
                // TODO: make this always happen
                control.ScreenPos = this.ScreenPos;
            }

            this.Height = GUIUtil.GetHeightForParent(this);
        }

        private void LoadPressed( object sender, EventArgs args )
        {
            this.wantsLoad = true;
        }

        private void DeletePressed( object sender, EventArgs args )
        {
            this.wantsDelete = true;
        }

        public float ControlHeight
        {
            get
            {
                return this.FixedFontSize + ControlBase.FixedMargin * 1;
            }
        }

        #region Fields
        public bool wantsLoad { get; set; }
        public bool wantsDelete { get; set; }

        private string name = "";
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        private CustomLabel nameLabel = null;
        private CustomButton loadButton = null;
        private CustomButton deleteButton = null;
        #endregion
    }
}
