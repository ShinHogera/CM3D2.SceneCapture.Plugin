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
    #region PluginMain
    [PluginFilter( "CM3D2x64" ), PluginFilter( "CM3D2x86" ), PluginFilter( "CM3D2VRx64" ), PluginName( "CM3D2.SceneCapture.Plugin" ), PluginVersion( "0.3.1.0" )]
    public class SceneCapture : PluginBase
    {
        #region Methods
		///-------------------------------------------------------------------------
		/// <summary>起動処理</summary>
		///-------------------------------------------------------------------------
        public void Awake()
        {
            try
            {
                GameObject.DontDestroyOnLoad( this );

                // モーション情報初期化
                ReadPluginPreferences();
                ConstantValues.Initialize();
                Translation.Initialize(configLanguage);
                Util.LoadShaders();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>シーン変更</summary>
        ///-------------------------------------------------------------------------
        public void OnLevelWasLoaded( int level )
        {
            ConstantValues.Scene sceneLevel = ( ConstantValues.Scene )level;

            // メイドエディット画面でない、かつ夜伽画面でない場合に、メイドエディット画面または夜伽画面に遷移した場合
            // if( ( this.sceneNo != ConstantValues.Scene.SceneEdit && this.sceneNo != ConstantValues.Scene.SceneYotogi && this.sceneNo != ConstantValues.Scene.ScenePhoto ) &&
            // 	( sceneLevel == ConstantValues.Scene.SceneEdit || sceneLevel == ConstantValues.Scene.SceneYotogi || sceneLevel == ConstantValues.Scene.ScenePhoto ) )
            // {
            // 初期化
            // モーション情報初期化

            if(this.envView != null) {
                // // メイドエディット画面または夜伽画面から、メイドエディット画面、夜伽画面以外に遷移した場合
                if(( this.sceneNo == ConstantValues.Scene.SceneEdit || this.sceneNo == ConstantValues.Scene.SceneYotogi || this.sceneNo == ConstantValues.Scene.ScenePhoto  ) &&
                   ( sceneLevel != ConstantValues.Scene.SceneEdit && sceneLevel != ConstantValues.Scene.SceneYotogi && sceneLevel != ConstantValues.Scene.ScenePhoto ) )
                {
                    // 追加した光源を削除
                    this.envView.ClearLights(false);
                    this.envView.ClearModels();
                    initialized = false;
                    Translation.CurrentTranslation = configLanguage;

                }
                else if (( this.sceneNo != ConstantValues.Scene.SceneEdit && this.sceneNo != ConstantValues.Scene.SceneYotogi && this.sceneNo != ConstantValues.Scene.ScenePhoto  ) &&
                   ( sceneLevel == ConstantValues.Scene.SceneEdit || sceneLevel == ConstantValues.Scene.SceneYotogi || sceneLevel == ConstantValues.Scene.ScenePhoto ) )
                {
                    this.envView.ClearLights(false);
                    this.envView.ClearModels();
                    initialized = false;
                    Translation.CurrentTranslation = configLanguage;
                }
            }

            this.sceneNo = sceneLevel;
        }

        ///-------------------------------------------------------------------------
        /// <summary>更新</summary>
        ///-------------------------------------------------------------------------
        public void Update()
        {
            try
            {
                if(this.envView != null) {
                    this.envView.ShowGizmos(GizmoRender.UIVisible);
                }

                if(this.dataView != null && this.dataView.wantsLanguageChange)
                {
                    string lang = this.dataView.LanguageValue;
                    if(Translation.HasTranslation(lang)) {
                        Preferences["Config"]["Language"].Value = dataView.LanguageValue;
                        configLanguage = dataView.LanguageValue;
                        SaveConfig();

                        this.dataView.wantsLanguageChange = false;
                    }
                }
                // Bloom has to be loaded by the game first
                if (!initialized) {
                    {
                        this.Initialize();
                        this.effectView.Update();
                        this.initialized = true;
                    }
                }

                // 機能有効の場合
                // if( this.Enable )

                if(this.envView != null)
                {
                    // F10押下
                    if( Input.GetKeyDown( configEffectKey ) )
                    {
                        if( this.modeSelectView.SelectedMode == ConstantValues.EditMode.Effect )
                        {
                            this.modeSelectView.SelectedMode = ConstantValues.EditMode.Disable;
                        }
                        else
                        {
                            this.modeSelectView.SelectedMode = ConstantValues.EditMode.Effect;
                        }
                    }
                    // F11押下
                    else if( Input.GetKeyDown( configEnvironmentKey ) )
                    {
                        if( this.modeSelectView.SelectedMode == ConstantValues.EditMode.Environment )
                        {
                            this.modeSelectView.SelectedMode = ConstantValues.EditMode.Disable;
                        }
                        else
                        {
                            this.modeSelectView.SelectedMode = ConstantValues.EditMode.Environment;
                        }
                    }
                    // C押下
                    else if( Input.GetKeyDown( configDataKey ) )
                    {
                        if( this.modeSelectView.SelectedMode == ConstantValues.EditMode.Data )
                        {
                            this.modeSelectView.SelectedMode = ConstantValues.EditMode.Disable;
                        }
                        else
                        {
                            this.modeSelectView.SelectedMode = ConstantValues.EditMode.Data;
                        }
                    }

                    // エディットモードの場合
                    if( this.IsEditMode )
                    {
                        // 両方のボタン有効
                        this.modeSelectView.IsEnableEffectSettingButton = true;
                        this.modeSelectView.IsEnableEnvironmentSettingButton = true;
                        this.modeSelectView.IsEnableDataSettingButton = true;
                    }
                    // 夜伽モードの場合
                    else if( this.IsYotogiMode )
                    {
                        // 環境設定のみ有効
                        this.modeSelectView.IsEnableEffectSettingButton = true;
                        this.modeSelectView.IsEnableEnvironmentSettingButton = true;
                        this.modeSelectView.IsEnableDataSettingButton = true;
                    }
                    else
                    {
                        this.modeSelectView.IsEnableEffectSettingButton = true;
                        this.modeSelectView.IsEnableEnvironmentSettingButton = true;
                        this.modeSelectView.IsEnableDataSettingButton = true;
                    }

                    if(this.envView.ShouldUpdate)
                    {
                        if ( this.dataView.wasPresetLoaded )
                        {
                            Debug.Log(" === Scene Reload === ");
                            this.envView.Update();
                            this.effectView.Update();
                            this.dataView.Update();
                            this.dataView.wasPresetLoaded = false;
                        }
                        // 環境画面有効の場合
                        else if( this.modeSelectView.SelectedMode == ConstantValues.EditMode.Environment )
                        {
                            this.envView.Update();
                        }
                        // それ以外の場合
                        else if( this.modeSelectView.SelectedMode == ConstantValues.EditMode.Effect)
                        {
                            this.effectView.Update();
                        }
                        else if( this.modeSelectView.SelectedMode == ConstantValues.EditMode.Data)
                        {
                            this.dataView.Update();
                        }
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>GUI処理</summary>
        ///-------------------------------------------------------------------------
        public void OnGUI()
        {
            try
            {
                // 機能有効の場合
                // if( this.Enable )

                this.envView.ShowGizmos(GizmoRender.UIVisible);

                if( GizmoRender.UIVisible )
                {
                    // 補助キーの押下有無確認
                    bool isCtrl = Input.GetKey( KeyCode.LeftControl ) || Input.GetKey( KeyCode.RightControl );
                    bool isShift = Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift );
                    bool isAlt = Input.GetKey( KeyCode.LeftAlt ) || Input.GetKey( KeyCode.RightAlt );

                    // 表示中
                    if( this.modeSelectView.SelectedMode != ConstantValues.EditMode.Disable )
                    {
                        float windowWidth = Screen.width / 4 - ControlBase.FixedMargin * 2;

                        // Vector2 point = new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y );
                        Rect pluginPos = new Rect( Screen.width - windowWidth, Screen.height / 15 + ControlBase.FixedMargin, Screen.width / 5 - Screen.width / 65, Screen.height - Screen.height / 5 );

                        // プラグイン画面の外にある場合、かつ補助キーを押下していない場合
                        // bool isEnableControl = ( pluginPos.Contains( point ) == false && ( isCtrl == false && isShift == false && isAlt == false ) );
                        // GameMain.Instance.MainCamera.SetControl( isEnableControl );
                        // UICamera.InputEnable = isEnableControl;

                        // モード選択ウィンドウ
                        this.modeSelectView.OnGUI();

                        // メイド設定中
                        if( this.modeSelectView.SelectedMode == ConstantValues.EditMode.Effect )
                        {
                            // メイド設定ウィンドウ
                            this.effectView.rectGui.x = this.modeSelectView.Left;
                            this.effectView.rectGui.y = this.modeSelectView.Top + this.modeSelectView.Height + ControlBase.FixedMargin;
                            this.effectView.Left = 0;
                            this.effectView.Top = 0;
                            this.effectView.Width = this.modeSelectView.Width;
                            this.effectView.rectGui.width = this.effectView.Width;
                            this.effectView.OnGUI();
                        }
                        // 環境設定中
                        else if( this.modeSelectView.SelectedMode == ConstantValues.EditMode.Environment )
                        {
                            // 環境設定ウィンドウ
                            this.envView.rectGui.x = this.modeSelectView.Left;
                            this.envView.rectGui.y = this.modeSelectView.Top + this.modeSelectView.Height + ControlBase.FixedMargin;
                            this.envView.Left = 0;
                            this.envView.Top = 0;
                            this.envView.Width = this.modeSelectView.Width;
                            this.envView.rectGui.width = this.envView.Width;
                            this.envView.OnGUI();
                        }
                        else if( this.modeSelectView.SelectedMode == ConstantValues.EditMode.Data )
                        {
                            // 環境設定ウィンドウ
                            this.dataView.rectGui.x = this.modeSelectView.Left;
                            this.dataView.rectGui.y = this.modeSelectView.Top + this.modeSelectView.Height + ControlBase.FixedMargin;
                            this.dataView.Left = 0;
                            this.dataView.Top = 0;
                            this.dataView.Width = this.modeSelectView.Width;
                            this.dataView.rectGui.width = this.dataView.Width;
                            this.dataView.OnGUI();
                        }

                        // update external windows
                        // only one of these are ever needed at a time
                        GlobalComboBox.Update();
                        GlobalColorPicker.Update();
                        GlobalCurveWindow.Update();
                        GlobalItemPicker.Update();
                        GlobalTexturePicker.Update();

                        // Update
                        SunShaftsDef.Update();
                    }
                    else
                    {
                        // 補助キーを押下していない場合
                        bool isEnableControl = ( isCtrl == false && isShift == false && isAlt == false );
                        GameMain.Instance.MainCamera.SetControl( isEnableControl );
                        UICamera.InputEnable = isEnableControl;
                    }
                }
                // else
                // {
                //     GameMain.Instance.MainCamera.SetControl( true );
                //     UICamera.InputEnable = true;
                // }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>初期化</summary>
        ///-------------------------------------------------------------------------
        private void Initialize()
        {
            try
            {
                int fontSize;
                if(Screen.width < 1366)
                    fontSize = 10;
                else
                    fontSize = 11;

                float windowWidth = Screen.width / 4 - ControlBase.FixedMargin * 2;

                new Instances();

                // モード選択画面
                this.modeSelectView = new ModeSelectWindow( fontSize );
                this.modeSelectView.Text = String.Format( "{0} ver.{1}", SceneCapture.GetPluginName(), SceneCapture.GetPluginVersion() );
                this.modeSelectView.Left = Screen.width - windowWidth - ControlBase.FixedMargin;
                this.modeSelectView.Top = ControlBase.FixedMargin * 15;
                this.modeSelectView.Width = windowWidth;

                // メイド設定画面
                this.effectView = new EffectWindow( fontSize );

                // 環境設定画面
                this.envView = new EnvWindow( fontSize );

                this.dataView = new DataWindow( fontSize );
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>プラグイン名取得</summary>
        /// <returns>プラグイン名</returns>
        ///-------------------------------------------------------------------------
        public static String GetPluginName()
        {
            String name = String.Empty;
            try
            {
                // 属性クラスからプラグイン名取得
                PluginNameAttribute att = Attribute.GetCustomAttribute( typeof( SceneCapture ), typeof( PluginNameAttribute ) ) as PluginNameAttribute;
                if( att != null )
                {
                    name = att.Name;
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }

            return name;
        }

        ///-------------------------------------------------------------------------
        /// <summary>プラグインバージョン取得</summary>
        /// <returns>プラグインバージョン</returns>
        ///-------------------------------------------------------------------------
        public static String GetPluginVersion()
        {
            String version = String.Empty;
            try
            {
                // 属性クラスからバージョン番号取得
                PluginVersionAttribute att = Attribute.GetCustomAttribute( typeof( SceneCapture ), typeof( PluginVersionAttribute ) ) as PluginVersionAttribute;
                if( att != null )
                {
                    version = att.Version;
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }

            return version;
        }
        #endregion

        #region Properties
		///-------------------------------------------------------------------------
		/// <summary>プラグイン機能有効</summary>
		///-------------------------------------------------------------------------
        private bool Enable
        {
            get
            {
                return this.IsEditMode || this.IsYotogiMode || this.IsPhotoMode;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>エディットモード</summary>
        ///-------------------------------------------------------------------------
        private bool IsEditMode
        {
            get
            {
                return this.sceneNo == ConstantValues.Scene.SceneEdit && CharacterMgr.EditModeLookHaveItem;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>夜伽モード</summary>
        ///-------------------------------------------------------------------------
        private bool IsYotogiMode
        {
            get
            {
                if( this.yotogiManager == null )
                {
                    this.yotogiManager = FindObjectOfType<YotogiPlayManager>();
                }

                if( this.yotogiManager != null )
                {
                    return this.sceneNo == ConstantValues.Scene.SceneYotogi && yotogiManager.fade_status == WfScreenChildren.FadeStatus.Wait;
                }
                else
                {
                    return false;
                }
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>photoモード</summary>
        ///-------------------------------------------------------------------------
        private bool IsPhotoMode
        {
            get
            {
                return this.sceneNo == ConstantValues.Scene.ScenePhoto;
            }
        }

        #endregion

        #region .ini ファイルの読み込み関係
        /// <summary>.ini ファイルからプラグイン設定を読み込む</summary>
        private void ReadPluginPreferences()
        {
            configLanguage = GetPreferences("Config", "Language", "English");
            configEffectKey = GetPreferences("Config", "EffectWindowKey", "z");
            configEnvironmentKey = GetPreferences("Config", "EnvironmentWindowKey", "x");
            configDataKey = GetPreferences("Config", "DataWindowKey", "c");

            configEffectKey = configEffectKey.ToLower();
            configEnvironmentKey = configEnvironmentKey.ToLower();
            configDataKey = configDataKey.ToLower();
        }

        /// <summary>設定ファイルから string データを読む</summary>
        private string GetPreferences( string section, string key, string defaultValue )
        {
            if (!Preferences.HasSection(section) || !Preferences[section].HasKey(key) || string.IsNullOrEmpty(Preferences[section][key].Value))
            {
                Preferences[section][key].Value = defaultValue;
                SaveConfig();
            }
            return Preferences[section][key].Value;
        }

        /// <summary>設定ファイルから bool データを読む</summary>
        private bool GetPreferences( string section, string key, bool defaultValue )
        {
            if( !Preferences.HasSection( section ) || !Preferences[section].HasKey( key ) || string.IsNullOrEmpty( Preferences[section][key].Value ))
            {
                Preferences[section][key].Value = defaultValue.ToString();
                SaveConfig();
            }
            bool b = defaultValue;
            bool.TryParse( Preferences[section][key].Value, out b );
            return b;
        }

        /// <summary>設定ファイルから int データを読む</summary>
        private int GetPreferences( string section, string key, int defaultValue )
        {
            if( !Preferences.HasSection( section ) || !Preferences[section].HasKey( key ) || string.IsNullOrEmpty( Preferences[section][key].Value ))
            {
                Preferences[section][key].Value = defaultValue.ToString();
                SaveConfig();
            }
            int i = defaultValue;
            int.TryParse( Preferences[section][key].Value, out i );
            return i;
        }

        /// <summary>設定ファイルから float データを読む</summary>
        private float GetPreferences( string section, string key, float defaultValue )
        {
            if( !Preferences.HasSection( section ) || !Preferences[section].HasKey( key ) || string.IsNullOrEmpty( Preferences[section][key].Value ))
            {
                Preferences[section][key].Value = defaultValue.ToString();
                SaveConfig();
            }
            float f = defaultValue;
            float.TryParse( Preferences[section][key].Value, out f );
            return f;
        }
        #endregion

        #region Fields
                /// <summary>画面番号</summary>
        private ConstantValues.Scene sceneNo = ConstantValues.Scene.None;

        private bool initialized = false;

        string configLanguage = string.Empty;
        string configEffectKey = string.Empty;
        string configEnvironmentKey = string.Empty;
        string configDataKey = string.Empty;

        /// <summary>夜伽クラス</summary>
        YotogiPlayManager yotogiManager = null;

        /// <summary>モード選択画面</summary>
        private ModeSelectWindow modeSelectView = null;

        /// <summary>メイド設定画面</summary>
        private EffectWindow effectView = null;

        /// <summary>環境設定画面</summary>
        private EnvWindow envView = null;

        private DataWindow dataView = null;
        #endregion
    }
    #endregion
}
