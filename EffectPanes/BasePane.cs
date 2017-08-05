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
    internal abstract class BasePane : ControlBase
    {
        public abstract void SetupPane();
        public abstract void ShowPane();
        public abstract void Reset();

        public BasePane( int fontSize, string title )
        {
            try
            {
                this.FontSize = fontSize;

                this.Text = title;

                this.Awake();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void Awake()
        {
            this.enableCheckbox = new CustomToggleButton( false, "toggle" );
            this.enableCheckbox.FontSize = this.FontSize;
            this.enableCheckbox.Text = this.Text;
            // this.enableCheckbox.CheckedChanged += this.EdgeDetectEnableCheckbox_CheckedChanged;
            this.ChildControls.Add( this.enableCheckbox );

            this.resetButton = new CustomButton();
            this.resetButton.FontSize = this.FontSize;
            this.resetButton.Text = "|";
            this.resetButton.Click += (o, e) => Reset();
            this.ChildControls.Add( this.resetButton );


            try
            {
                SetupPane();
            }
            catch(Exception e)
            {
                Debug.LogError("Error during SetupPane():\n" + e.ToString());
            }
        }

        override public void OnGUI()
        {
            try
            {
                this.SetAllVisible(false, 0);

                // 光源名ラベル
                this.enableCheckbox.Left = this.Left + ControlBase.FixedMargin;
                this.enableCheckbox.Top = this.Top + ControlBase.FixedMargin;
                this.enableCheckbox.Width = this.Width - ControlBase.FixedMargin * 2;
                this.enableCheckbox.Height = this.ControlHeight;

                // GUIUtil.AddResetButton(this, this.resetButton);

                this.Update();
                if( this.enableCheckbox.Value == true )
                {
                    ShowPane();
                }

                this.Height = GUIUtil.GetHeightForParent(this);

                // 子コントロールGUI処理
                this.OnGUIChildControls();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }
        public float ControlHeight
        {
            get
            {
                return this.FixedFontSize + ControlBase.FixedMargin;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return this.enableCheckbox.Value;
            }
            set
            {
                this.enableCheckbox.Value = value;
            }
        }

        private CustomToggleButton enableCheckbox = null;
        private CustomButton resetButton = null;
    }
}
