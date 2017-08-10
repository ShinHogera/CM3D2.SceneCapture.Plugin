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
        public ModelPane( int fontSize, string name, string iconName )
        {
            this.FontSize = fontSize;
            this.name = name;
            this.iconName = iconName;

            this.Awake();
        }

        override public void Awake()
        {
            this.deleteRequested = false;

            this.modelNameLabel = new CustomLabel();
            this.modelNameLabel.Text = this.Name;
            this.ChildControls.Add( this.modelNameLabel );

            this.modelIconImage = new CustomImage( null );
            this.ChildControls.Add( this.modelIconImage );

            if( this.iconName != null && this.iconName != string.Empty )
            {
                try
                {
                    this.modelIconImage.Texture = ImportCM.CreateTexture(this.iconName);
                }
                catch( Exception e )
                {
                    Debug.LogError("Failed to load model icon: " + e);

                    Texture2D iconTexture = new Texture2D(1, 1);
                    iconTexture.SetPixel(0, 0, new Color32(1, 1, 1, 1));
                    iconTexture.Apply();
                    this.modelIconImage.Texture = iconTexture;
                }
            }
            else
            {
                Texture2D iconTexture = new Texture2D(1, 1);
                iconTexture.SetPixel(0, 0, new Color32(1, 1, 1, 1));
                iconTexture.Apply();
                this.modelIconImage.Texture = iconTexture;
            }

            this.gizmoPanToggle = new CustomToggleButton( false );
            this.gizmoPanToggle.FontSize = this.FontSize;
            this.gizmoPanToggle.Text = Translation.GetText("Model", "gizmoPan");
            this.gizmoPanToggle.CheckedChanged += this.TogglePan;
            this.ChildControls.Add( this.gizmoPanToggle );

            this.gizmoRotateToggle = new CustomToggleButton( false );
            this.gizmoRotateToggle.FontSize = this.FontSize;
            this.gizmoRotateToggle.Text = Translation.GetText("Model", "gizmoRotate");
            this.gizmoRotateToggle.CheckedChanged += this.ToggleRotate;
            this.ChildControls.Add( this.gizmoRotateToggle );

            this.gizmoScaleToggle = new CustomToggleButton( false );
            this.gizmoScaleToggle.FontSize = this.FontSize;
            this.gizmoScaleToggle.Text = Translation.GetText("Model", "gizmoScale");
            this.gizmoScaleToggle.CheckedChanged += this.ToggleScale;
            this.ChildControls.Add( this.gizmoScaleToggle );

            this.resetPanButton = new CustomButton();
            this.resetPanButton.FontSize = this.FontSize;
            this.resetPanButton.Text = Translation.GetText("UI", "reset");
            this.resetPanButton.Click += this.ResetPan;
            this.ChildControls.Add( this.resetPanButton );

            this.resetRotateButton = new CustomButton();
            this.resetRotateButton.FontSize = this.FontSize;
            this.resetRotateButton.Text = Translation.GetText("UI", "reset");
            this.resetRotateButton.Click += this.ResetRotation;
            this.ChildControls.Add( this.resetRotateButton );

            this.resetScaleButton = new CustomButton();
            this.resetScaleButton.FontSize = this.FontSize;
            this.resetScaleButton.Text = Translation.GetText("UI", "reset");
            this.resetScaleButton.Click += this.ResetScale;
            this.ChildControls.Add( this.resetScaleButton );

            this.modelDeleteButton = new CustomButton();
            this.modelDeleteButton.FontSize = this.FontSize;
            this.modelDeleteButton.Text = Translation.GetText("Model", "modelDelete");
            this.modelDeleteButton.Click += this.DeleteModel;
            this.ChildControls.Add( this.modelDeleteButton );

            this.modelCopyButton = new CustomButton();
            this.modelCopyButton.FontSize = this.FontSize;
            this.modelCopyButton.Text = Translation.GetText("Model", "modelCopy");
            this.modelCopyButton.Click += this.CopyModel;
            this.ChildControls.Add( this.modelCopyButton );
        }

        override public void OnGUI()
        {
            this.SetAllVisible(false, 0);

            this.modelIconImage.Left = this.Left + ControlBase.FixedMargin;
            this.modelIconImage.Top = this.Top + ControlBase.FixedMargin;
            this.modelIconImage.Width = (this.Width / 3 - ControlBase.FixedMargin / 4) / 4;
            this.modelIconImage.Height = this.modelIconImage.Width;
            this.modelIconImage.OnGUI();

            this.modelNameLabel.Left = this.modelIconImage.Left + this.modelIconImage.Width + ControlBase.FixedMargin;
            this.modelNameLabel.Top = this.Top + ControlBase.FixedMargin + this.ControlHeight / 2;
            this.modelNameLabel.Width = this.modelIconImage.Width * 2;
            this.modelNameLabel.Height = this.ControlHeight;
            this.modelNameLabel.OnGUI();

            GUIUtil.AddGUIButtonNoRender( this, this.gizmoPanToggle, this.modelNameLabel, 5 );
            this.gizmoPanToggle.Top -= this.ControlHeight / 2;
            this.gizmoPanToggle.OnGUI();

            GUIUtil.AddGUIButton( this, this.gizmoRotateToggle, this.gizmoPanToggle, 5 );
            GUIUtil.AddGUIButton( this, this.gizmoScaleToggle, this.gizmoRotateToggle, 5 );
            GUIUtil.AddGUIButton( this, this.modelCopyButton, this.gizmoScaleToggle, 8 );

            GUIUtil.AddGUIButtonNoRender( this, this.resetPanButton, this.modelNameLabel, 5 );
            this.resetPanButton.Top += this.ControlHeight / 2;
            this.resetPanButton.OnGUI();

            GUIUtil.AddGUIButton( this, this.resetRotateButton, this.resetPanButton, 5 );
            GUIUtil.AddGUIButton( this, this.resetScaleButton, this.resetRotateButton, 5 );
            GUIUtil.AddGUIButton( this, this.modelDeleteButton, this.resetScaleButton, 8 );

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
        private string iconName = null;
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
        private CustomImage modelIconImage = null;
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
