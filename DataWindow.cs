using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.SceneCapture.Plugin
{
    internal class DataWindow : ScrollablePane
    {
        public DataWindow( int fontSize ) : base ( fontSize ) {}

        override public void Awake()
        {
            try
            {
                Directory.CreateDirectory(PRESET_DIR);

                this.savePanes = new List<SavePane>();

                this.languageBox = new CustomComboBox( Translation.GetTranslations() );
                this.languageBox.Text = Translation.GetText("UI", "language");
                this.languageBox.SelectedItem = Translation.CurrentTranslation;
                this.languageBox.SelectedIndexChanged += this.ChangeLanguage;
                this.ChildControls.Add( this.languageBox );

                this.saveButton = new CustomButton();
                this.saveButton.Text = Translation.GetText("UI", "save");
                this.saveButton.Click += SaveEnv;
                this.ChildControls.Add( this.saveButton );

                this.nameTextField = new CustomTextField();
                this.nameTextField.Text = Translation.GetText("UI", "name");
                this.ChildControls.Add( this.nameTextField );

                this.loadTargetLabel = new CustomLabel();
                this.loadTargetLabel.FontSize = this.FontSize;
                this.loadTargetLabel.Text = Translation.GetText("UI", "loadTarget");
                this.ChildControls.Add( this.loadTargetLabel );

                this.loadEffectsCheckbox = new CustomToggleButton( true, "toggle" );
                this.loadEffectsCheckbox.Text = Translation.GetText("UI", "loadEffects");
                this.loadEffectsCheckbox.CheckedChanged += ChangeLoadTargets;
                this.ChildControls.Add( this.loadEffectsCheckbox );

                this.loadLightsCheckbox = new CustomToggleButton( true, "toggle" );
                this.loadLightsCheckbox.Text = Translation.GetText("UI", "loadLights");
                this.loadLightsCheckbox.CheckedChanged += ChangeLoadTargets;
                this.ChildControls.Add( this.loadLightsCheckbox );

                this.loadModelsCheckbox = new CustomToggleButton( true, "toggle" );
                this.loadModelsCheckbox.Text = Translation.GetText("UI", "loadModels");
                this.loadModelsCheckbox.CheckedChanged += ChangeLoadTargets;
                this.ChildControls.Add( this.loadModelsCheckbox );

                this.loadCameraCheckbox = new CustomToggleButton( true, "toggle" );
                this.loadCameraCheckbox.Text = Translation.GetText("UI", "loadCamera");
                this.loadCameraCheckbox.CheckedChanged += ChangeLoadTargets;
                this.ChildControls.Add( this.loadCameraCheckbox );

                this.loadMiscCheckbox = new CustomToggleButton( true, "toggle" );
                this.loadMiscCheckbox.Text = Translation.GetText("UI", "loadMisc");
                this.loadMiscCheckbox.CheckedChanged += ChangeLoadTargets;
                this.ChildControls.Add( this.loadMiscCheckbox );

                this.wasPresetLoaded = false;

                this.ReloadSaves();
                this.ChangeLoadTargets( this, new EventArgs() );
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void Update()
        {
            try
            {
                this.UpdateChildControls();
                this.CheckForSaveChanges();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        private void ChangeLoadTargets( object sender, EventArgs args )
        {
            Instances.loadEffects = this.loadEffectsCheckbox.Value;
            Instances.loadLights = this.loadLightsCheckbox.Value;
            Instances.loadModels = this.loadModelsCheckbox.Value;
            Instances.loadCamera = this.loadCameraCheckbox.Value;
            Instances.loadMisc = this.loadMiscCheckbox.Value;
        }

        private void CheckForSaveChanges()
        {
            string toLoad = null;
            string toDelete = null;
            for (int i = this.savePanes.Count - 1; i >= 0; i--)
            {
                SavePane pane = this.savePanes[i];
                if( pane.wantsLoad )
                {
                    toLoad = pane.Name;
                    pane.wantsLoad = false;
                }
                if( pane.wantsDelete )
                {
                    toDelete = pane.Name;
                    pane.wantsDelete = false;
                }
            }

            if( toLoad != null )
            {
                LoadEnv( toLoad );
            }
            else if( toDelete != null )
            {
                DeleteEnv( toDelete );
            }
        }

        override public void ShowPane()
        {
            this.languageBox.Left = this.Left + ControlBase.FixedMargin;
            this.languageBox.Top = this.Top + ControlBase.FixedMargin;
            this.languageBox.Width = this.Width - ControlBase.FixedMargin * 2;
            this.languageBox.Height = this.ControlHeight;
            this.languageBox.OnGUI();

            GUIUtil.AddGUICheckbox( this, this.nameTextField, this.languageBox );
            GUIUtil.AddGUICheckbox( this, this.saveButton, this.nameTextField );
            GUIUtil.AddGUICheckbox( this, this.loadTargetLabel, this.saveButton );

            GUIUtil.AddGUIButtonAfter( this, this.loadEffectsCheckbox, this.loadTargetLabel, 5 );
            GUIUtil.AddGUIButton( this, this.loadLightsCheckbox, this.loadEffectsCheckbox, 5 );
            GUIUtil.AddGUIButton( this, this.loadModelsCheckbox, this.loadLightsCheckbox, 5 );
            GUIUtil.AddGUIButton( this, this.loadCameraCheckbox, this.loadModelsCheckbox, 5 );
            GUIUtil.AddGUIButton( this, this.loadMiscCheckbox, this.loadCameraCheckbox, 5 );

            ControlBase prev = this.loadLightsCheckbox;
            foreach( SavePane pane in this.savePanes )
            {
                GUIUtil.AddGUICheckbox( this, pane, prev );
                prev = pane;
            }

            this.Height = GUIUtil.GetHeightForParent(this);
        }

        private string GetPresetPath( string name)
        {
            return PRESET_DIR + @"\" + name + ".xml";
        }

        private string GetPresetPathSybaris( string name)
        {
            return PRESET_DIR_SYBARIS + @"\" + name + ".xml";
        }

        private void ClearSavePanes()
        {
            SavePane[] panes = this.savePanes.ToArray();
            foreach( SavePane pane in panes )
            {
                this.ChildControls.Remove( pane );
                this.savePanes.Remove( pane );
            }
        }

        private void ReloadSaves()
        {
            this.ClearSavePanes();

            DirectoryInfo info = new DirectoryInfo(PRESET_DIR);
            FileInfo[] files = info.GetFiles("*.xml").OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo f in files)
            {
                SavePane pane = new SavePane( this.FontSize, Path.GetFileNameWithoutExtension(f.Name) );
                this.savePanes.Add( pane );
                this.ChildControls.Add( pane );
            }
        }

        private void SaveEnv( object sender, EventArgs args )
        {
            try {
                string name = nameTextField.Value;
                if (name == string.Empty)
                    return;

                XDocument doc = Instances.Save();
                string savePath = GetPresetPath(name);
                doc.Save(savePath);
                ReloadSaves();
            }
            catch( Exception e )
            {
                Debug.LogError( e );
            }
        }

        private void LoadEnv( string name )
        {
            string loadPath = GetPresetPath(name);
            Instances.Load(loadPath);
            this.nameTextField.Value = name;
            this.wasPresetLoaded = true;
        }

        private void DeleteEnv( string name )
        {
            try
            {
                string deletePath = GetPresetPath(name);
                File.Delete(deletePath);
                this.ReloadSaves();
            }
            catch
            {
                // Directory.GetCurrentDirectory() doesn't include the
                // Sybaris path, but it still saves and loads the XML
                // data at the correct location (?!). Only File.Delete
                // fails.
                try
                {
                    string deletePath = GetPresetPathSybaris(name);
                    File.Delete(deletePath);
                    this.ReloadSaves();
                }
                catch( Exception e )
                {
                    Debug.LogError( e );
                }
            }
        }

        private void ChangeLanguage( object sender, EventArgs args )
        {
            this.wantsLanguageChange = true;
        }

        public String LanguageValue
        {
            get
            {
                return this.languageBox.SelectedItem;
            }
        }

        #region Fields
        private readonly static string PRESET_DIR = ConstantValues.ConfigDir + @"\Presets";
        private readonly static string PRESET_DIR_SYBARIS = ConstantValues.ConfigDirSybaris + @"\Presets";

        public bool wasPresetLoaded { get; set; }
        public bool wantsLanguageChange { get; set; }
        public string changeLanguage = "";

        private CustomButton saveButton = null;
        private CustomTextField nameTextField = null;
        private CustomComboBox languageBox = null;
        private CustomLabel loadTargetLabel = null;
        private CustomToggleButton loadEffectsCheckbox = null;
        private CustomToggleButton loadLightsCheckbox = null;
        private CustomToggleButton loadModelsCheckbox = null;
        private CustomToggleButton loadCameraCheckbox = null;
        private CustomToggleButton loadMiscCheckbox = null;

        private List<SavePane> savePanes = null;
        #endregion
    }
}
