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
    #region ModeSelectWindow
    ///=========================================================================
    /// <summary>モード選択画面</summary>
    ///=========================================================================
    internal class ModeSelectWindow : ControlBase
    {
        #region Methods
		///-------------------------------------------------------------------------
		/// <summary>コンストラクタ</summary>
		///-------------------------------------------------------------------------
        public ModeSelectWindow( int fontSize )
        {
            try
            {
                this.FontSize = fontSize;

                // 初期化
                this.Awake();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>起動処理</summary>
        ///-------------------------------------------------------------------------
        override public void Awake()
        {
            try
            {
                // ウィンドウスタイル
                this.windowStyle = new GUIStyle( "box" );
                this.windowStyle.alignment = TextAnchor.UpperLeft;
                this.windowStyle.normal.textColor = this.windowStyle.onNormal.textColor =
                    this.windowStyle.hover.textColor = this.windowStyle.onHover.textColor =
                    this.windowStyle.active.textColor = this.windowStyle.onActive.textColor =
                    this.windowStyle.focused.textColor = this.windowStyle.onFocused.textColor = Color.white;

                // エフェクト設定トグルボタン
                this.effectSettingButton = new CustomToggleButton( false );
                this.effectSettingButton.FontSize = this.FontSize;
                this.effectSettingButton.Text = Translation.GetText("UI", "effectSetting");
                this.effectSettingButton.CheckedChanged += this.EffectSettingButton_CheckedChanged;

                // 環境設定トグルボタン
                this.envSettingButton = new CustomToggleButton( false );
                this.envSettingButton.FontSize = this.FontSize;
                this.envSettingButton.Text = Translation.GetText("UI", "envSetting");
                this.envSettingButton.CheckedChanged += this.EnvSettingButton_CheckedChanged;

                // データ設定トグルボタン
                this.dataSettingButton = new CustomToggleButton( false );
                this.dataSettingButton.FontSize = this.FontSize;
                this.dataSettingButton.Text = Translation.GetText("UI", "dataSetting");
                this.dataSettingButton.CheckedChanged += this.DataSettingButton_CheckedChanged;

                CustomToggleButton.SetPairButton( this.effectSettingButton, this.envSettingButton );
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>GUI処理</summary>
        ///-------------------------------------------------------------------------
        override public void OnGUI()
        {
            try
            {
                // ウィンドウ高さ調整
                this.Height = this.ControlHeight * 2 + ControlBase.FixedMargin * 3;

                // ウィンドウ
                Rect windowRect = new Rect( this.Left, this.Top, this.Width, this.Height );
                this.windowStyle.fontSize = this.FixedFontSize;
                GUI.Box( windowRect, this.Text, this.windowStyle );

                // メイド設定トグルボタン
                this.effectSettingButton.Left = this.Left + ControlBase.FixedMargin;
                this.effectSettingButton.Top = this.Top + this.ControlHeight + ControlBase.FixedMargin * 2;
                this.effectSettingButton.Width = this.Width / 3 - ControlBase.FixedMargin * 2;
                this.effectSettingButton.Height = this.ControlHeight;

                if( this.IsEnableEffectSettingButton )
                {
                    this.effectSettingButton.OnGUI();
                }

                // 環境設定トグルボタン
                this.envSettingButton.Left = this.effectSettingButton.Left + this.effectSettingButton.Width + ControlBase.FixedMargin * 2;
                this.envSettingButton.Top = this.Top + this.ControlHeight + ControlBase.FixedMargin * 2;
                this.envSettingButton.Width = this.Width / 3 - ControlBase.FixedMargin * 2;
                this.envSettingButton.Height = this.ControlHeight;

                if( this.IsEnableEnvironmentSettingButton )
                {
                    this.envSettingButton.OnGUI();
                }

                // データ設定トグルボタン
                this.dataSettingButton.Left = this.envSettingButton.Left + this.envSettingButton.Width + ControlBase.FixedMargin * 2;
                this.dataSettingButton.Top = this.Top + this.ControlHeight + ControlBase.FixedMargin * 2;
                this.dataSettingButton.Width = this.Width / 3 - ControlBase.FixedMargin * 2;
                this.dataSettingButton.Height = this.ControlHeight;

                if( this.IsEnableDataSettingButton )
                {
                    this.dataSettingButton.OnGUI();
                }


                {
                    Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

                    bool enableGameGui = true;
                    bool m = Input.GetAxis("Mouse ScrollWheel") != 0;
                    for (int i = 0; i < 3; i++)
                    {
                        m |= Input.GetMouseButtonDown(i);
                    }
                    if (m)
                    {
                        enableGameGui = !this.WindowRect.Contains(mousePos);
                    }
                    if(this.WindowRect.Contains(mousePos) && Input.GetMouseButtonDown(0))
                    {
                        this.dragging = true;
                        this.dragStartPos = mousePos;
                        this.dragStartWindowPos = new Vector2( this.Left, this.Top );
                    }
                    if(Input.GetMouseButtonUp(0))
                    {
                        this.dragging = false;
                    }
                    if(this.dragging)
                    {
                        Vector2 diff = this.dragStartWindowPos - (this.dragStartPos - mousePos);
                        this.Left = diff.x;
                        this.Top = diff.y;
                    }
                    GameMain.Instance.MainCamera.SetControl(enableGameGui);
                    UICamera.InputEnable = enableGameGui;
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }
        #endregion

        #region EventHandler
		///-------------------------------------------------------------------------
		/// <summary>エフェクト設定トグルチェック変更イベント</summary>
		/// <param name="sender">イベント送信者</param>
		/// <param name="args">イベント引数(未使用)</param>
		///-------------------------------------------------------------------------
        private void EffectSettingButton_CheckedChanged( object sender, EventArgs args )
        {
            try
            {
                // コンボボックスを閉じる
                CustomComboBox.CloseAllDropDownList();

                // メイド設定ボタン有効の場合のみ
                if( this.IsEnableEffectSettingButton )
                {
                    if( this.effectSettingButton.Value )
                    {
                        this.SelectedMode = ConstantValues.EditMode.Effect;
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>環境設定トグルチェック変更イベント</summary>
        /// <param name="sender">イベント送信者</param>
        /// <param name="args">イベント引数(未使用)</param>
        ///-------------------------------------------------------------------------
        private void EnvSettingButton_CheckedChanged( object sender, EventArgs args )
        {
            try
            {
                // コンボボックスを閉じる
                CustomComboBox.CloseAllDropDownList();

                // 環境設定ボタン有効の場合のみ
                if( this.IsEnableEnvironmentSettingButton )
                {
                    if( this.envSettingButton.Value )
                    {
                        this.SelectedMode = ConstantValues.EditMode.Environment;
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>データ設定トグルチェック変更イベント</summary>
        /// <param name="sender">イベント送信者</param>
        /// <param name="args">イベント引数(未使用)</param>
        ///-------------------------------------------------------------------------
        private void DataSettingButton_CheckedChanged( object sender, EventArgs args )
        {
            try
            {
                // コンボボックスを閉じる
                CustomComboBox.CloseAllDropDownList();

                // 環境設定ボタン有効の場合のみ
                if( this.IsEnableDataSettingButton )
                {
                    if( this.dataSettingButton.Value )
                    {
                        this.SelectedMode = ConstantValues.EditMode.Data;
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }



        #region Properties
		///-------------------------------------------------------------------------
		/// <summary>現在のモード</summary>
		///-------------------------------------------------------------------------
        private ConstantValues.EditMode _selectedMode = ConstantValues.EditMode.Disable;
        public ConstantValues.EditMode SelectedMode
        {
            get
            {
                return this._selectedMode;
            }

            set
            {
                if( this._selectedMode != value )
                {
                    this._selectedMode = value;

                    switch( this._selectedMode )
                    {
                        case ConstantValues.EditMode.Effect:
                            this.effectSettingButton.Value = true;
                            this.envSettingButton.Value = false;
                            this.dataSettingButton.Value = false;
                            break;

                        case ConstantValues.EditMode.Environment:
                            this.effectSettingButton.Value = false;
                            this.envSettingButton.Value = true;
                            this.dataSettingButton.Value = false;
                            break;

                        case ConstantValues.EditMode.Data:
                            this.effectSettingButton.Value = false;
                            this.envSettingButton.Value = false;
                            this.dataSettingButton.Value = true;
                            break;

                        case ConstantValues.EditMode.Disable:
                        default:
                            this.effectSettingButton.Value = false;
                            this.envSettingButton.Value = false;
                            this.dataSettingButton.Value = false;
                            break;
                    }
                }
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>エフェクト設定ボタン有効無効</summary>
        ///-------------------------------------------------------------------------
        private bool _isEnableEffectSettingButton = true;
        public bool IsEnableEffectSettingButton
        {
            get
            {
                return this._isEnableEffectSettingButton;
            }

            set
            {
                this._isEnableEffectSettingButton = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>環境設定ボタン有効無効</summary>
        ///-------------------------------------------------------------------------
        private bool _isEnableEnvironmentSettingButton = true;
        public bool IsEnableEnvironmentSettingButton
        {
            get
            {
                return this._isEnableEnvironmentSettingButton;
            }

            set
            {
                this._isEnableEnvironmentSettingButton = value;
            }
        }

        /// <summary>データ設定ボタン有効無効</summary>
        ///-------------------------------------------------------------------------
        private bool _isEnableDataSettingButton = true;
        public bool IsEnableDataSettingButton
        {
            get
            {
                return this._isEnableDataSettingButton;
            }

            set
            {
                this._isEnableDataSettingButton = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>コントロール高さ</summary>
        ///-------------------------------------------------------------------------
        public float ControlHeight
        {
            get
            {
                return this.FixedFontSize + ControlBase.FixedMargin * 2;
            }
        }
        #endregion

        #region Fields
		/// <summary>ウィンドウスタイル</summary>
        private GUIStyle windowStyle = null;

        /// <summary>エフェクト設定トグルボタン</summary>
        private CustomToggleButton effectSettingButton = null;

        /// <summary>環境設定トグルボタン</summary>
        private CustomToggleButton envSettingButton = null;

        /// <summary>データ設定トグルボタン</summary>
        private CustomToggleButton dataSettingButton = null;

        private bool dragging = false;
        private Vector2 dragStartPos = Vector2.zero;
        private Vector2 dragStartWindowPos = Vector2.zero;
        #endregion
    }
    #endregion
    #endregion
}
