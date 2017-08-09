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
    internal class ModelPane : ControlBase
    {
        public ModelPane( int fontSize, string name )
        {
            this.FontSize = fontSize;
            this.name = name;

            this.Awake();
        }

        override public void Awake()
        {
            this.deleteRequested = false;

            this.modelNameLabel = new CustomLabel();
            this.modelNameLabel.Text = this.Name;
            this.ChildControls.Add( this.modelNameLabel );

            this.gizmoPanToggle = new CustomToggleButton( false );
            this.gizmoPanToggle.FontSize = this.FontSize;
            this.gizmoPanToggle.Text = "Pan";
            this.gizmoPanToggle.CheckedChanged += this.TogglePan;
            this.ChildControls.Add( this.gizmoPanToggle );

            this.gizmoRotateToggle = new CustomToggleButton( false );
            this.gizmoRotateToggle.FontSize = this.FontSize;
            this.gizmoRotateToggle.Text = "Rotation";
            this.gizmoRotateToggle.CheckedChanged += this.ToggleRotate;
            this.ChildControls.Add( this.gizmoRotateToggle );

            this.gizmoScaleToggle = new CustomToggleButton( false );
            this.gizmoScaleToggle.FontSize = this.FontSize;
            this.gizmoScaleToggle.Text = "Scale";
            this.gizmoScaleToggle.CheckedChanged += this.ToggleScale;
            this.ChildControls.Add( this.gizmoScaleToggle );

            this.resetPanButton = new CustomButton();
            this.resetPanButton.FontSize = this.FontSize;
            this.resetPanButton.Text = "Reset Pan";
            this.resetPanButton.Click += this.ResetPan;
            this.ChildControls.Add( this.resetPanButton );

            this.resetRotateButton = new CustomButton();
            this.resetRotateButton.FontSize = this.FontSize;
            this.resetRotateButton.Text = "Reset Rot";
            this.resetRotateButton.Click += this.ResetRotation;
            this.ChildControls.Add( this.resetRotateButton );

            this.resetScaleButton = new CustomButton();
            this.resetScaleButton.FontSize = this.FontSize;
            this.resetScaleButton.Text = "Reset Scl";
            this.resetScaleButton.Click += this.ResetScale;
            this.ChildControls.Add( this.resetScaleButton );

            this.modelDeleteButton = new CustomButton();
            this.modelDeleteButton.FontSize = this.FontSize;
            this.modelDeleteButton.Text = "Delete";
            this.modelDeleteButton.Click += this.DeleteModel;
            this.ChildControls.Add( this.modelDeleteButton );

            this.modelCopyButton = new CustomButton();
            this.modelCopyButton.FontSize = this.FontSize;
            this.modelCopyButton.Text = "Copy";
            this.modelCopyButton.Click += this.CopyModel;
            this.ChildControls.Add( this.modelCopyButton );
        }

        override public void OnGUI()
        {
            this.SetAllVisible(false, 0);

            this.modelNameLabel.Left = this.Left + ControlBase.FixedMargin;
            this.modelNameLabel.Top = this.Top + ControlBase.FixedMargin + this.ControlHeight / 2;
            this.modelNameLabel.Width = this.Width / 3 - ControlBase.FixedMargin / 4;
            this.modelNameLabel.Height = this.ControlHeight;
            this.modelNameLabel.OnGUI();

            GUIUtil.AddGUIButton( this, this.gizmoPanToggle, this.modelNameLabel, 5 );
            this.gizmoPanToggle.Top -= this.ControlHeight / 2;

            GUIUtil.AddGUIButton( this, this.gizmoRotateToggle, this.gizmoPanToggle, 5 );
            GUIUtil.AddGUIButton( this, this.gizmoScaleToggle, this.gizmoRotateToggle, 5 );
            GUIUtil.AddGUIButton( this, this.modelCopyButton, this.gizmoScaleToggle, 8 );

            GUIUtil.AddGUIButton( this, this.resetPanButton, this.modelNameLabel, 5 );
            this.resetPanButton.Top += this.ControlHeight / 2;

            GUIUtil.AddGUIButton( this, this.resetRotateButton, this.resetPanButton, 5 );
            GUIUtil.AddGUIButton( this, this.resetScaleButton, this.resetRotateButton, 5 );
            GUIUtil.AddGUIButton( this, this.modelDeleteButton, this.gizmoScaleToggle, 8 );

            this.Height = GUIUtil.GetHeightForParent(this);
        }

        private void TogglePan( object sender, EventArgs args )
        {
            if(!this.toggling)
            {
                this.toggling = true;
                this.gizmoRotateToggle.Value = false;
                this.gizmoScaleToggle.Value = false;
                this.wasChanged = true;
                this.toggling = false;
            }
        }

        private void ToggleRotate( object sender, EventArgs args )
        {
            if(!this.toggling)
            {
                this.toggling = true;
                this.gizmoPanToggle.Value = false;
                this.gizmoScaleToggle.Value = false;
                this.wasChanged = true;
                this.toggling = false;
            }
        }

        private void ToggleScale( object sender, EventArgs args )
        {
            if(!this.toggling)
            {
                this.toggling = true;
                this.gizmoPanToggle.Value = false;
                this.gizmoRotateToggle.Value = false;
                this.wasChanged = true;
                this.toggling = false;
            }
        }

        private void ResetPan( object sender, EventArgs args )
        {
            this.wantsResetPan = true;
            this.wasChanged = true;
        }

        private void ResetRotation( object sender, EventArgs args )
        {
            this.wantsResetRotation = true;
            this.wasChanged = true;
        }

        private void ResetScale( object sender, EventArgs args )
        {
            this.wantsResetScale = true;
            this.wasChanged = true;
        }

        private void CopyModel( object sender, EventArgs args )
        {
            this.wantsCopy = true;
            this.wasChanged = true;
        }

        private void DeleteModel( object sender, EventArgs args )
        {
            this.deleteRequested = true;
            this.wasChanged = true;
        }

        public void UpdateCache( Transform transform )
        {
            if( transform.position != this.cachedPosition || transform.rotation != this.cachedRotation || transform.localScale != this.cachedLocalScale ) {
                this.wasChanged = true;
                this.cachedPosition = transform.position;
                this.cachedRotation = transform.rotation;
                this.cachedLocalScale = transform.localScale;
            }
        }

        #region Properties
        public float ControlHeight
        {
            get
            {
                return this.FixedFontSize + ControlBase.FixedMargin * 1;
            }
        }

        // public ModelInfo ModelInfoValue
        // {

        // }

        public bool WantsTogglePan
        {
            get
            {
                return this.gizmoPanToggle.Value;
            }
            set
            {
                this.gizmoPanToggle.Value = value;
            }
        }

        public bool WantsToggleRotate
        {
            get
            {
                return this.gizmoRotateToggle.Value;
            }
            set
            {
                this.gizmoRotateToggle.Value = value;
            }
        }

        public bool WantsToggleScale
        {
            get
            {
                return this.gizmoScaleToggle.Value;
            }
            set
            {
                this.gizmoScaleToggle.Value = value;
            }
        }

        public bool IsDeleteRequested
        {
            get
            {
                return this.deleteRequested;
            }
        }

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
        #endregion

        #region Fields
        private static readonly string[] LIGHT_SHADOWS = new string[] { "None", "Hard", "Soft" };

        private string name = null;
        private bool deleteRequested;
        private bool toggling = false;
        public bool wasChanged { get; set; }

        private Vector3 cachedPosition = Vector3.zero;
        private Quaternion cachedRotation = Quaternion.identity;
        private Vector3 cachedLocalScale = Vector3.zero;

        public bool wantsTogglePan { get; set; }
        public bool wantsToggleRotation { get; set; }
        public bool wantsToggleScale { get; set; }
        public bool wantsResetPan { get; set; }
        public bool wantsResetRotation { get; set; }
        public bool wantsResetScale { get; set; }
        public bool wantsCopy { get; set; }

        private CustomLabel modelNameLabel = null;
        private CustomButton modelDeleteButton = null;
        private CustomButton modelCopyButton = null;
        private CustomToggleButton gizmoPanToggle = null;
        private CustomToggleButton gizmoRotateToggle = null;
        private CustomToggleButton gizmoScaleToggle = null;
        private CustomButton resetPanButton = null;
        private CustomButton resetRotateButton = null;
        private CustomButton resetScaleButton = null;
        #endregion
    }
}
