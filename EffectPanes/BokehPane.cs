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
    internal class BokehPane : BasePane
    {
        public BokehPane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Bokeh") ) {}

        override public void SetupPane()
        {
            this.focusDistanceSlider = new CustomSlider( BokehDef.bokehEffect.focusDistance, 0f, 200f, 2 );
            this.focusDistanceSlider.Text = Translation.GetText("Bokeh", "focusDistance");
            this.ChildControls.Add( this.focusDistanceSlider  );

            this.fNumberSlider = new CustomSlider( BokehDef.bokehEffect.fNumber, 1f, 50f, 3 );
            this.fNumberSlider.Text = Translation.GetText("Bokeh", "fNumber");
            this.ChildControls.Add( this.fNumberSlider  );

            this.kernelSizeBox = new CustomComboBox( BOKEH_KERNEL_SIZES );
            this.kernelSizeBox.Text = Translation.GetText("Bokeh", "kernelSize");
            this.kernelSizeBox.SelectedIndex = (int)BokehDef.bokehEffect.kernelSize;
            this.ChildControls.Add( this.kernelSizeBox );

            this.useCameraFovCheckbox = new CustomToggleButton( true, "toggle" );
            this.useCameraFovCheckbox.Text = Translation.GetText("Bokeh", "useCameraFov");
            this.ChildControls.Add( this.useCameraFovCheckbox );

            this.focalLengthSlider = new CustomSlider( BokehDef.bokehEffect.focalLength, 0f, 10f, 4 );
            this.focalLengthSlider.Text = Translation.GetText("Bokeh", "focalLength");
            this.ChildControls.Add( this.focalLengthSlider  );

            this.visualizeCheckbox = new CustomToggleButton( false, "toggle" );
            this.visualizeCheckbox.Text = Translation.GetText("Bokeh", "visualize");
            this.ChildControls.Add( this.visualizeCheckbox );

            this.transformFromMaidCheckbox = new CustomToggleButton( BokehDef.transformFromMaid, "toggle" );
            this.transformFromMaidCheckbox.Text = Translation.GetText("UI", "transformFromMaid");
            this.transformFromMaidCheckbox.CheckedChanged += (o, e) => {
                if( this.transformFromMaidCheckbox.Value == true )
                {
                    BokehDef.SetTransform(this.maidManager);
                }
                else
                {
                    BokehDef.SetTransform();
                }
            };
            this.ChildControls.Add( this.transformFromMaidCheckbox );

            this.prevMaidButton = new CustomButton();
            this.prevMaidButton.Text = "<";
            this.prevMaidButton.Click += (o, e) => {
                this.maidManager.Prev();
                DepthOfFieldDef.SetTransform(this.maidManager);
                this.maidManager.bUpdateRequest = true;
            };
            this.ChildControls.Add( this.prevMaidButton );

            this.nextMaidButton = new CustomButton();
            this.nextMaidButton.Text = ">";
            this.nextMaidButton.Click += (o, e) => {
                this.maidManager.Next();
                DepthOfFieldDef.SetTransform(this.maidManager);
                this.maidManager.bUpdateRequest = true;
            };
            this.ChildControls.Add( this.nextMaidButton );

            this.reloadMaidsButton = new CustomButton();
            this.reloadMaidsButton.Text = Translation.GetText("UI", "reloadMaids");
            this.reloadMaidsButton.Click += (o, e) => {
                this.maidManager.bUpdateRequest = true;
            };
            this.ChildControls.Add( this.reloadMaidsButton );

            this.maidManager = new MaidManager();
            this.maidManager.Find();

            this.focusMaidLabel = new CustomLabel();
            this.ChildControls.Add( this.focusMaidLabel );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.transformFromMaidCheckbox);

            if( this.transformFromMaidCheckbox.Value == true )
            {
                // GUIUtil.AddGUICheckbox(this, this.reloadMaidsButton);
                this.reloadMaidsButton.Left = this.Left + this.FontSize * 11;
                this.reloadMaidsButton.Top = this.transformFromMaidCheckbox.Top + this.ControlHeight;
                this.reloadMaidsButton.Width = this.FontSize * 6;
                this.reloadMaidsButton.Height = this.ControlHeight;
                this.reloadMaidsButton.OnGUI();
                this.reloadMaidsButton.Visible = true;

                // GUIUtil.AddGUICheckbox(this, this.prevMaidButton);
                this.prevMaidButton.Left = this.Left + ControlBase.FixedMargin;
                this.prevMaidButton.Top = this.reloadMaidsButton.Top;
                this.prevMaidButton.Width = this.FontSize * 2;
                this.prevMaidButton.Height = this.ControlHeight;
                this.prevMaidButton.OnGUI();
                this.prevMaidButton.Visible = true;

                // GUIUtil.AddGUICheckbox(this, this.nextMaidButton);
                this.nextMaidButton.Left = this.prevMaidButton.Left + this.nextMaidButton.Width;
                this.nextMaidButton.Top = this.reloadMaidsButton.Top;
                this.nextMaidButton.Width = this.FontSize * 2;
                this.nextMaidButton.Height = this.ControlHeight;
                this.nextMaidButton.OnGUI();
                this.nextMaidButton.Visible = true;

                this.focusMaidLabel.Left = this.nextMaidButton.Left + this.nextMaidButton.Width + ControlBase.FixedMargin;
                this.focusMaidLabel.Top = this.nextMaidButton.Top;
                this.focusMaidLabel.Width = this.FontSize * 13;
                this.focusMaidLabel.Height = this.ControlHeight;
                this.focusMaidLabel.Text = this.maidManager.sCurrent;
                this.focusMaidLabel.OnGUI();
                this.focusMaidLabel.Visible = true;
            }
            else
            {
                GUIUtil.AddGUISlider(this, this.focusDistanceSlider);
            }

            GUIUtil.AddGUISlider(this, this.fNumberSlider);

            GUIUtil.AddGUICheckbox(this, this.kernelSizeBox);
            GUIUtil.AddGUICheckbox(this, this.useCameraFovCheckbox);

            if( this.useCameraFovCheckbox.Value == false )
            {
                GUIUtil.AddGUISlider(this, this.focalLengthSlider);
            }

            GUIUtil.AddGUICheckbox(this, this.visualizeCheckbox);

            this.focusMaidToggled = this.transformFromMaidCheckbox.Value;

            this.maidManager.Update();
        }

        override public void Reset()
        {

        }

        #region Properties
        public float FocusDistanceValue
        {
            get
            {
                return this.focusDistanceSlider.Value;
            }
        }

        public float FNumberValue
        {
            get
            {
                return this.fNumberSlider.Value;
            }
        }

        public bool UseCameraFovValue
        {
            get
            {
                return this.useCameraFovCheckbox.Value;
            }
        }

        public float FocalLengthValue
        {
            get
            {
                // TODO: adjust slider step
                return this.focalLengthSlider.Value / 10.0f;
            }
        }

        public Bokeh.KernelSize KernelSizeValue
        {
            get
            {
                return (Bokeh.KernelSize)Enum.Parse( typeof( Bokeh.KernelSize ), this.kernelSizeBox.SelectedItem );
            }
        }

        public bool VisualizeValue
        {
            get
            {
                return this.visualizeCheckbox.Value;
            }
        }

        #endregion

        #region Fields
        private static readonly string[] BOKEH_KERNEL_SIZES = new string[] { "Small", "Medium", "Large", "VeryLarge" };

        private CustomSlider focusDistanceSlider = null;
        private CustomSlider fNumberSlider = null;
        private CustomToggleButton useCameraFovCheckbox = null;
        private CustomSlider focalLengthSlider = null;
        private CustomComboBox kernelSizeBox = null;
        private CustomToggleButton visualizeCheckbox = null;

        private CustomToggleButton transformFromMaidCheckbox = null;
        private MaidManager maidManager = null;
        private bool focusMaidToggled = false;

        private CustomButton prevMaidButton = null;
        private CustomButton nextMaidButton = null;
        private CustomButton reloadMaidsButton = null;
        private CustomLabel focusMaidLabel = null;
        #endregion
    }
}
