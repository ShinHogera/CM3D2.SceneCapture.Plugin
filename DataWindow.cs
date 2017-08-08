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

                this.saveButton = new CustomButton();
                this.saveButton.Text = Instances.isJapanese ? "セーブ" : "Save";
                this.saveButton.Click += SaveEnv;
                this.ChildControls.Add( this.saveButton );

                this.nameTextField = new CustomTextField();
                this.nameTextField.Text = Instances.isJapanese ? "名前" : "Name";
                this.ChildControls.Add( this.nameTextField );

                this.wasPresetLoaded = false;

                this.ReloadSaves();
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
            this.languageButton.Left = this.Left + ControlBase.FixedMargin;
            this.languageButton.Top = this.Top + ControlBase.FixedMargin;
            this.languageButton.Width = this.Width - ControlBase.FixedMargin / 4;
            this.languageButton.Height = this.ControlHeight;
            this.languageButton.OnGUI();

            GUIUtil.AddGUICheckbox( this, this.nameTextField, this.languageButton );
            GUIUtil.AddGUICheckbox( this, this.saveButton, this.nameTextField );

            ControlBase prev = this.saveButton;
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

        #region Fields
        private readonly static string PRESET_DIR = ConstantValues.ConfigDir + @"\Presets";
        private readonly static string PRESET_DIR_SYBARIS = ConstantValues.ConfigDirSybaris + @"\Presets";

        public bool wasPresetLoaded { get; set; }

        private CustomButton saveButton = null;
        private CustomButton languageButton = null;
        private CustomTextField nameTextField = null;

        private List<SavePane> savePanes = null;
        #endregion
    }
}
