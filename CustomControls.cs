using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CM3D2.SceneCapture.Plugin
{
    #region ControlBase
    ///=========================================================================
    /// <summary>コントロール基本機能</summary>
    ///=========================================================================
    internal class ControlBase
    {
        #region Methods
                ///-------------------------------------------------------------------------
                /// <summary>画面サイズに応じてサイズ調整</summary>
                /// <param name="px">調整対象のサイズ</param>
                /// <returns>調整後のサイズ</returns>
                ///-------------------------------------------------------------------------
        private static int FixPx( int px )
        {
            return ( int )( ( 1.0f + ( Screen.width / 1280.0f - 1.0f ) * 0.3f ) * px );
        }

        ///-------------------------------------------------------------------------
        /// <summary>子コントロール起動処理</summary>
        ///-------------------------------------------------------------------------
        virtual protected void AwakeChildControls()
        {
            try
            {
                foreach( ControlBase control in this._childControls )
                {
                    control.Awake();
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>子コントロールシーン変更</summary>
        ///-------------------------------------------------------------------------
        virtual protected void OnLevelWasLoadedChildControls( int level )
        {
            try
            {
                foreach( ControlBase control in this._childControls )
                {
                    control.OnLevelWasLoaded( level );
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>子コントロール更新処理</summary>
        ///-------------------------------------------------------------------------
        virtual protected void UpdateChildControls()
        {
            try
            {
                foreach( ControlBase control in this._childControls )
                {
                    control.Update();
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>子コントロールGUI処理</summary>
        ///-------------------------------------------------------------------------
        virtual protected void OnGUIChildControls()
        {
            try
            {
                foreach( ControlBase control in this._childControls )
                {
                    if(control.Visible) {
                        control.ScreenPos = this.ScreenPos;
                        control.OnGUI();
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        virtual protected void OnGUIChildControls( Action<ControlBase, ControlBase> action )
        {
            try
            {
                foreach( ControlBase control in this._childControls )
                {
                    if(control.Visible) {
                        control.ScreenPos = this.ScreenPos;
                        control.OnGUI();
                        action(this, control);
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        public void SetAllVisible( bool value, int ignoreId )
        {
            try
            {
                for(int i = 0; i < this.ChildControls.Count; i++)
                {
                    if( ignoreId != i )
                    {
                        ControlBase control = this.ChildControls[i];
                        control.Visible = value;
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        public void SetFromRect(Rect rect)
        {
            this.Left = rect.x;
            this.Top = rect.y;
            this.Width = rect.width;
            this.Height = rect.height;
        }


        /// <summary>起動処理</summary>
        virtual public void Awake() { }

        /// <summary>シーン変更</summary>
        virtual public void OnLevelWasLoaded( int level ) { }

        /// <summary>更新処理</summary>
        virtual public void Update() { }

        /// <summary>GUI処理</summary>
        virtual public void OnGUI() { }
        #endregion

        #region Properties
                ///-------------------------------------------------------------------------
                /// <summary>子コントロール一覧</summary>
                ///-------------------------------------------------------------------------
        private List<ControlBase> _childControls = new List<ControlBase>();
        virtual protected List<ControlBase> ChildControls
        {
            get
            {
                return this._childControls;
            }

            set
            {
                this._childControls = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>有効/無効</summary>
        ///-------------------------------------------------------------------------
        private bool _enabled = true;
        virtual public bool Enabled
        {
            get
            {
                return this._enabled;
            }

            set
            {
                this._enabled = value;
            }
        }

        private bool _visible = true;
        virtual public bool Visible
        {
            get
            {
                return this._visible;
            }

            set
            {
                this._visible = value;
            }
        }

        virtual public float LastElementSize
        {
            get
            {
                List<ControlBase> controls = this._childControls.Where(x => x.Visible).ToList();

                if(controls.Count == 0)
                {
                    return this.Top + this.Height;
                }

                ControlBase control = controls.OrderByDescending(c => c.Top + c.Height).First();

                return control.Top + control.Height;
            }
        }

        virtual public void OrderChildControls()
        {
            this._childControls = this._childControls.OrderBy(c=>c.Top).ToList();
        }

        ///-------------------------------------------------------------------------
        /// <summary>Window位置/サイズ</summary>
        ///-------------------------------------------------------------------------
        virtual public Rect WindowRect
        {
            get
            {
                return new Rect( this._left, this._top, this._width, this._height );
            }
        }

        public Rect ScreenPos { get; set; }

        ///-------------------------------------------------------------------------
        /// <summary>テキスト</summary>
        ///-------------------------------------------------------------------------
        private String _text = String.Empty;
        virtual public String Text
        {
            get
            {
                return this._text;
            }

            set
            {
                this._text = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>テキスト色</summary>
        ///-------------------------------------------------------------------------
        private Color _textColor = Color.white;
        virtual public Color TextColor
        {
            get
            {
                return this._textColor;
            }

            set
            {
                this._textColor = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>背景色</summary>
        ///-------------------------------------------------------------------------
        private Color _backgroundColor = Color.black;
        virtual public Color BackgroundColor
        {
            get
            {
                return this._backgroundColor;
            }

            set
            {
                this._backgroundColor = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>コントロール左位置</summary>
        ///-------------------------------------------------------------------------
        private float _left = 0.0f;
        virtual public float Left
        {
            get
            {
                return this._left;
            }

            set
            {
                this._left = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>コントロール上位置</summary>
        ///-------------------------------------------------------------------------
        private float _top = 0.0f;
        virtual public float Top
        {
            get
            {
                return this._top;
            }

            set
            {
                this._top = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>コントロール幅</summary>
        ///-------------------------------------------------------------------------
        private float _width = 0.0f;
        virtual public float Width
        {
            get
            {
                return this._width;
            }

            set
            {
                this._width = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>コントロール高さ</summary>
        ///-------------------------------------------------------------------------
        private float _height = 0.0f;
        virtual public float Height
        {
            get
            {
                return this._height;
            }

            set
            {
                this._height = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>フォントサイズ</summary>
        ///-------------------------------------------------------------------------
        private int _fontSize = 0;
        virtual public int FontSize
        {
            get
            {
                return this._fontSize;
            }

            set
            {
                this._fontSize = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>画面サイズに応じたフォントサイズ</summary>
        ///-------------------------------------------------------------------------
        virtual public int FixedFontSize
        {
            get
            {
                return FixPx( this._fontSize );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>画面サイズに応じたコントロール間のマージン</summary>
        ///-------------------------------------------------------------------------
        public static int FixedMargin
        {
            get
            {
                return FixPx( Margin );
            }
        }
        #endregion

        #region ConstantValues
                /// <summary>コントロール間のマージン</summary>
        public const int Margin = 5;
        #endregion
    }
    #endregion

    #region GUIColor
    ///=========================================================================
    /// <summary>GUI色設定</summary>
    ///=========================================================================
    internal class GUIColor : IDisposable
    {
        #region Methods
                ///-------------------------------------------------------------------------
                /// <summary>コンストラクタ</summary>
                ///-------------------------------------------------------------------------
        public GUIColor( Color backgroundColor, Color contentColor )
        {
            try
            {
                // 元の色退避
                this.oldBackgroundColor = GUI.backgroundColor;
                this.oldcontentColor = GUI.contentColor;

                // 色設定
                GUI.backgroundColor = backgroundColor;
                GUI.contentColor = contentColor;
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>アンマネージリソース開放</summary>
        ///-------------------------------------------------------------------------
        public void Dispose()
        {
            try
            {
                // 色を戻す
                GUI.backgroundColor = this.oldBackgroundColor;
                GUI.contentColor = this.oldcontentColor;
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }
        #endregion

        #region Fileds
                /// <summary>元の色</summary>
        private Color oldBackgroundColor;

        /// <summary>元の色</summary>
        private Color oldcontentColor;
        #endregion
    }
    #endregion

    #region CustomComboBox
    ///=========================================================================
    /// <summary>コンボボックス</summary>
    ///=========================================================================
    internal class CustomComboBox : ControlBase
    {
        #region Methods
                ///-------------------------------------------------------------------------
                /// <summary>コンストラクタ</summary>
                /// <param name="items">コンボボックス項目</param>
                ///-------------------------------------------------------------------------
        public CustomComboBox()
        {
            try
            {
                // ID登録
                this._id = NewWindowID;
                NewWindowID++;

                // コンボボックス一覧に追加
                allComboBoxes[ this._id ] = this;

                this._items = new List<GUIContent>();
                this.Awake();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>コンストラクタ</summary>
        /// <param name="rect">コンボボックス位置/サイズ</param>
        /// <param name="dropDownListHeight">ドロップダウンリスト最大高さ</param>
        /// <param name="items">コンボボックス項目</param>
        /// <param name="buttonStyle">ボタンスタイル</param>
        /// <param name="boxStyle">ボックススタイル</param>
        /// <param name="listStyle">リストスタイル</param>
        ///-------------------------------------------------------------------------
        public CustomComboBox( List<GUIContent> items )
        {
            try
            {
                // ID登録
                this._id = NewWindowID;
                NewWindowID++;

                // コンボボックス一覧に追加
                allComboBoxes[ this._id ] = this;

                this._items = items;
                this.Awake();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        public CustomComboBox( string[] items )
        {
            try
            {
                // ID登録
                this._id = NewWindowID;
                NewWindowID++;

                // コンボボックス一覧に追加
                allComboBoxes[ this._id ] = this;

                List<string> itemList = new List<string>(items);
                this._items = itemList.Select( x => new GUIContent( x ) ).ToList();
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
                // コンボボタンスタイル
                this.comboButtonStyle = new GUIStyle( "button" );
                this.comboButtonStyle.alignment = TextAnchor.MiddleLeft;
                this.comboButtonStyle.normal.textColor = this.comboButtonStyle.onNormal.textColor =
                    this.comboButtonStyle.hover.textColor = this.comboButtonStyle.onHover.textColor =
                    this.comboButtonStyle.active.textColor = this.comboButtonStyle.onActive.textColor =
                    this.comboButtonStyle.focused.textColor = this.comboButtonStyle.onFocused.textColor = Color.white;

                // コンボボックススタイル
                this.comboBoxStyle = new GUIStyle( "box" );
                this.comboBoxStyle.normal.textColor = this.comboBoxStyle.onNormal.textColor =
                    this.comboBoxStyle.hover.textColor = this.comboBoxStyle.onHover.textColor =
                    this.comboBoxStyle.active.textColor = this.comboBoxStyle.onActive.textColor =
                    this.comboBoxStyle.focused.textColor = this.comboBoxStyle.onFocused.textColor = Color.white;

                // コンボリストスタイル
                this.comboListStyle = new GUIStyle( "box" );
                this.comboListStyle.onHover.background = comboListStyle.hover.background = new Texture2D( 2, 2 );
                this.comboListStyle.padding.left = comboListStyle.padding.right = comboListStyle.padding.top = comboListStyle.padding.bottom = 4;
                this.comboListStyle.normal.textColor = this.comboListStyle.onNormal.textColor =
                    this.comboListStyle.hover.textColor = this.comboListStyle.onHover.textColor =
                    this.comboListStyle.active.textColor = this.comboListStyle.onActive.textColor =
                    this.comboListStyle.focused.textColor = this.comboListStyle.onFocused.textColor = Color.white;

                this.labelStyle = new GUIStyle( "label" );
                labelStyle.alignment = TextAnchor.MiddleLeft;
                labelStyle.fontSize = this.FixedFontSize;
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
                if( this._isShowDropDownList && !GlobalComboBox.Visible )
                {
                    this._isShowDropDownList = false;
                }

                this.comboButtonStyle.fontSize = this.FixedFontSize;
                this.comboBoxStyle.fontSize = this.FixedFontSize;
                this.comboListStyle.fontSize = this.FixedFontSize;

                // 現在選択中の項目をボタンに表示
                this.comboBoxButton = null;
                if( this._items != null && 0 <= this._selectedIndex && this._selectedIndex < this.Count )
                {
                    this.comboBoxButton = this._items[ this._selectedIndex ];
                }
                else
                {
                    this.comboBoxButton = new GUIContent( String.Empty );
                }

                Rect labelRect = new Rect( this.Left, this.Top, this.Width / 3, this.Height );
                GUI.Label( labelRect, this.Text, this.labelStyle );

                // ボタン
                Rect comboBoxRect = new Rect( this.Left + this.Width / 3, this.Top, (this.Width / 3) * 2, this.Height );
                if( GUI.Button( comboBoxRect, this.comboBoxButton, this.comboButtonStyle ) )
                {
                    if( this._items != null )
                    {
                        // ボタン押下でドロップダウンリスト表示/非表示切り替え
                        this._isShowDropDownList = !this._isShowDropDownList;

                        // 自分以外のドロップダウンリストを全て閉じる
                        CustomComboBox.CloseAllDropDownList( this._id );
                    }
                }

                // ドロップダウンリスト表示の場合
                if( this._items != null && this._isShowDropDownList )
                {
                    float maxDropDownListHeight = Screen.height - Screen.height / 5 - this.ScreenPos.y;
                    float itemHeight = comboListStyle.CalcHeight( this.comboBoxButton, 1.0f ) * ( this._items.Count );
                    float windowHeight = maxDropDownListHeight < itemHeight ? maxDropDownListHeight : itemHeight;

                    this.dropDownListRect = new Rect( comboBoxRect.x + this.ScreenPos.x, this.Top + this.ScreenPos.y + this.ScreenPos.height, comboBoxRect.width, windowHeight );
                    // this.dropDownListRect = new Rect( this.Left, this.Top + this.Height, this.Width, windowHeight );

                    // ウィンドウ表示
                    GUIStyle style = new GUIStyle( "box" );
                    // GUI.Window( this._id, this.dropDownListRect, GuiFunc, String.Empty, style );

                    // グリッド表示
                    // The '36' is from the scrollable pane's item size times 2
                    // Need to replace later.
                    Rect listRect = new Rect( this.dropDownListRect.x, this.Top + this.ScreenPos.y + 36, this.dropDownListRect.width, this.dropDownListRect.height );

                    GUIContent[] contents = new GUIContent[ this._items.Count ];
                    this._items.CopyTo( contents );

                    GlobalComboBox.Set(listRect, contents, this.FontSize, pick);
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        private void pick(int newSelectedItemIndex)
        {
            if( newSelectedItemIndex != this._selectedIndex )
            {
                // グリッド内の項目が選択されたので、選択Indexを更新し、ドロップダウンリストを閉じる
                this.SelectedIndex = newSelectedItemIndex;
                this._isShowDropDownList = false;
            }
        }

        public void SetItems(List<String> itemList)
        {
            try
            {
                this._items = itemList.Select( x => new GUIContent( x ) ).ToList();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>ドロップダウンリストのGUI処理</summary>
        /// <param name="windowID">ウィンドウID</param>
        ///-------------------------------------------------------------------------
        private void OnGUI_DropDownList( int windowID )
        {
            try
            {
                float itemHeight = this.comboListStyle.CalcHeight( comboBoxButton, 1.0f ) * ( this._items.Count );

                // スクロールビュー開始
                // Rect posRect = new Rect( 0, 0, this.dropDownListRect.width, this.dropDownListRect.height );
                // Rect viewRect = new Rect( 0, 0, this.dropDownListRect.width - 20, itemHeight );
                // scrollViewVector = GUI.BeginScrollView( posRect, scrollViewVector, viewRect );

                // グリッド表示
                Rect listRect = new Rect( this.ScreenPos.x, this.ScreenPos.y, this.dropDownListRect.width, this.dropDownListRect.height );

                GUIContent[] contents = new GUIContent[ this._items.Count ];
                this._items.CopyTo( contents );
                int newSelectedItemIndex = GUI.SelectionGrid( listRect, _selectedIndex, contents, 1, this.comboListStyle );

                // 選択Indexが変更
                if( newSelectedItemIndex != this._selectedIndex )
                {
                    // グリッド内の項目が選択されたので、選択Indexを更新し、ドロップダウンリストを閉じる
                    this.SelectedIndex = newSelectedItemIndex;
                    this._isShowDropDownList = false;
                }

                // スクロールビュー終了
                // GUI.EndScrollView();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        private bool GetAnyMouseButtonDown()
        {
            return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
        }

        ///-------------------------------------------------------------------------
        /// <summary>選択位置を次の項目に移動。末尾の場合は先頭へ移動</summary>
        /// <returns>移動後のインデックス</returns>
        ///-------------------------------------------------------------------------
        public int Next()
        {
            try
            {
                if( this._items != null && 0 < this._items.Count )
                {
                    if( this._selectedIndex + 1 < this._items.Count )
                    {
                        this.SelectedIndex++;
                    }
                    else
                    {
                        this.SelectedIndex = 0;
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }

            return this._selectedIndex;
        }

        ///-------------------------------------------------------------------------
        /// <summary>選択位置を前の項目に移動。先頭の場合は末尾へ移動</summary>
        /// <returns>移動後のインデックス</returns>
        ///-------------------------------------------------------------------------
        public int Prev()
        {
            try
            {
                if( this._items != null && 0 < this._items.Count )
                {
                    if( 0 <= this._selectedIndex - 1 )
                    {
                        this.SelectedIndex--;
                    }
                    else
                    {
                        this.SelectedIndex = this._items.Count - 1;
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }

            return this._selectedIndex;
        }

        ///-------------------------------------------------------------------------
        /// <summary>全てのコンボボックスのドロップダウンリストを閉じる</summary>
        ///-------------------------------------------------------------------------
        public static void CloseAllDropDownList()
        {
            try
            {
                CloseAllDropDownList( -1 );
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>全てのコンボボックスのドロップダウンリストを閉じる</summary>
        ///-------------------------------------------------------------------------
        public static void CloseAllDropDownList( int ignoreID )
        {
            try
            {
                foreach( CustomComboBox combo in allComboBoxes.Values )
                {
                    if( combo.ID != ignoreID )
                    {
                        combo.IsShowDropDownList = false;
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }
        #endregion

        #region Properties
                ///-------------------------------------------------------------------------
                /// <summary>コンボボックスID</summary>
                ///-------------------------------------------------------------------------
        private int _id = 0;
        public int ID
        {
            get
            {
                return this._id;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>コンボボックス項目</summary>
        ///-------------------------------------------------------------------------
        private List<GUIContent> _items = null;
        public List<GUIContent> Items
        {
            get
            {
                return this._items;
            }

            set
            {
                this._items = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>選択中の項目インデックス</summary>
        ///-------------------------------------------------------------------------
        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get
            {
                return this._selectedIndex;
            }

            set
            {
                this._selectedIndex = value;

                if( this.Enabled )
                {
                    // 選択項目変更イベント送信
                    this.SelectedIndexChanged( this, new EventArgs() );
                }
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>選択中の項目</summary>
        ///-------------------------------------------------------------------------
        public String SelectedItem
        {
            get
            {
                String text = String.Empty;
                if( 0 <= this._selectedIndex && this._selectedIndex < this._items.Count )
                {
                    text = this._items[ this._selectedIndex ].text;
                }

                return text;
            }

            set
            {
                // 指定された項目を検索
                int index = this._items.FindIndex( item => item.text == value );
                if( 0 <= index )
                {
                    // 選択位置切り替え
                    this.SelectedIndex = index;
                }
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>コンボボックス項目数</summary>
        ///-------------------------------------------------------------------------
        public int Count
        {
            get
            {
                if( this._items != null )
                {
                    return this._items.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>ドロップダウンリスト表示有無</summary>
        ///-------------------------------------------------------------------------
        private bool _isShowDropDownList = false;
        public bool IsShowDropDownList
        {
            get
            {
                return this._isShowDropDownList;
            }

            set
            {
                this._isShowDropDownList = value;
            }
        }
        #endregion

        #region Fields
                /// <summary>全てのコンボボックス</summary>
        private static Dictionary<int, CustomComboBox> allComboBoxes = new Dictionary<int, CustomComboBox>();

        /// <summary>次に作成するウィンドウID</summary>
        private static int NewWindowID = 0;

        // ドロップダウンリストサイズ
        private Rect dropDownListRect;

        // コンボボタンスタイル
        private GUIContent comboBoxButton = null;

        // コンボボタンスタイル
        private GUIStyle comboButtonStyle = null;

        // コンボボックススタイル
        private GUIStyle comboBoxStyle = null;

        // コンボリストスタイル
        private GUIStyle comboListStyle = null;

        private GUIStyle labelStyle = null;

        /// <summary>スクロール位置</summary>
        private Vector2 scrollViewVector = Vector2.zero;
        #endregion

        #region Events
                ///-------------------------------------------------------------------------
                /// <summary>選択位置変更イベント</summary>
                /// <param name="sender">イベント送信者</param>
                /// <param name="args">イベント引数(未使用)</param>
                ///-------------------------------------------------------------------------
        public EventHandler SelectedIndexChanged = delegate { };
        #endregion
    }
    #endregion

    #region CustomButton
    ///=========================================================================
    /// <summary>ボタン</summary>
    ///=========================================================================
    internal class CustomButton : ControlBase
    {
        #region Methods
                ///-------------------------------------------------------------------------
                /// <summary>コンストラクタ</summary>
                ///-------------------------------------------------------------------------
        public CustomButton()
        {
            this.TextColor = Color.white;
        }

        ///-------------------------------------------------------------------------
        /// <summary>GUI処理</summary>
        ///-------------------------------------------------------------------------
        override public void OnGUI()
        {
            try
            {
                Rect buttonRect = new Rect( this.Left, this.Top, this.Width, this.Height );

                // スタイル
                GUIStyle buttonStyle = new GUIStyle( "button" );
                buttonStyle.alignment = TextAnchor.MiddleCenter;
                buttonStyle.fontSize = this.FixedFontSize;
                if( this.Enabled == false )
                {
                    buttonStyle.normal.background = buttonStyle.onNormal.background =
                        buttonStyle.hover.background = buttonStyle.onHover.background =
                        buttonStyle.active.background = buttonStyle.onActive.background =
                        buttonStyle.focused.background = buttonStyle.onFocused.background = new Texture2D( 2, 2 );

                    buttonStyle.normal.textColor = buttonStyle.onNormal.textColor =
                        buttonStyle.hover.textColor = buttonStyle.onHover.textColor =
                        buttonStyle.active.textColor = buttonStyle.onActive.textColor =
                        buttonStyle.focused.textColor = buttonStyle.onFocused.textColor = Color.gray;
                }

                // ボタン表示
                if( GUI.Button( buttonRect, this.Text, buttonStyle ) )
                {
                    if( this.Enabled )
                    {
                        // クリックイベント送信
                        this.Click( this, new EventArgs() );
                    }
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }
        #endregion

        #region Events
                ///-------------------------------------------------------------------------
                /// <summary>クリックイベント</summary>
                /// <param name="sender">イベント送信者</param>
                /// <param name="args">イベント引数(未使用)</param>
                ///-------------------------------------------------------------------------
        public EventHandler Click = delegate { };
        #endregion
    }
    #endregion

    #region CustomToggleButton
    ///=========================================================================
    /// <summary>トグルボタン</summary>
    ///=========================================================================
    internal class CustomToggleButton : ControlBase
    {
        #region Methods
                ///-------------------------------------------------------------------------
                /// <summary>コンストラクタ</summary>
                /// <param name="value">トグル初期状態</param>
                ///-------------------------------------------------------------------------
        public CustomToggleButton( bool value )
        {
            this._value = value;
            this._style = "button";

            this.BackgroundColor = Color.white;
            this.TextColor = Color.grey;
        }

        public CustomToggleButton( bool value, string style )
        {
            this._value = value;
            this._style = style;

            this.BackgroundColor = Color.white;
            this.TextColor = Color.grey;
        }

        ///-------------------------------------------------------------------------
        /// <summary>GUI処理</summary>
        ///-------------------------------------------------------------------------
        override public void OnGUI()
        {
            try
            {
                Rect toggleRect = new Rect( this.Left, this.Top, this.Width, this.Height );

                // スタイル
                GUIStyle labelStyle = new GUIStyle( "label" );
                labelStyle.alignment = TextAnchor.MiddleLeft;
                labelStyle.fontSize = this.FixedFontSize;

                // トグル表示
                bool retVal = false;
                using( GUIColor color = new GUIColor( this.CurrentBackgroundColor, GUI.contentColor ) )
                {
                    retVal = GUI.Toggle( toggleRect, this._value, "", new GUIStyle( this._style ) );
                }

                if( this._style == "toggle" )
                {
                    toggleRect.x += 10;
                }

                // ラベル表示
                using( GUIColor color = new GUIColor( GUI.backgroundColor, this.CurrentTextColor ) )
                {
                    GUI.Label( toggleRect, this.Text, labelStyle );
                }

                if( retVal != this._value )
                {
                    if( pair != null )
                    {
                        if( retVal == false && pair.Value == false )
                        {
                            retVal = true;
                        }
                        else if( retVal && pair.Value )
                        {
                            pair.Value = false;
                        }
                    }

                    this.Value = retVal;
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>ボタングループ化</summary>
        ///-------------------------------------------------------------------------
        public static void SetPairButton( CustomToggleButton button1, CustomToggleButton button2 )
        {
            button1.pair = button2;
            button2.pair = button1;
        }
        #endregion

        #region Properties
                ///-------------------------------------------------------------------------
                /// <summary>現在の状態</summary>
                ///-------------------------------------------------------------------------
        private string _style = "button";
        private bool _value = false;
        public bool Value
        {
            get
            {
                return this._value;
            }

            set
            {
                this._value = value;

                if( this.Enabled )
                {
                    // チェック変更送信
                    this.CheckedChanged( this, new EventArgs() );
                }
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>現在の背景色</summary>
        ///-------------------------------------------------------------------------
        public Color CurrentBackgroundColor
        {
            get
            {
                return this._value ? this._selectBackgroundColor : this.BackgroundColor;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>現在のテキスト色</summary>
        ///-------------------------------------------------------------------------
        public Color CurrentTextColor
        {
            get
            {
                return this._value ? this._selectTextColor : this.TextColor;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>選択色</summary>
        ///-------------------------------------------------------------------------
        private Color _selectBackgroundColor = Color.green;
        public Color SelectBackgroundColor
        {
            get
            {
                return this._selectBackgroundColor;
            }

            set
            {
                this._selectBackgroundColor = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>選択テキスト色</summary>
        ///-------------------------------------------------------------------------
        private Color _selectTextColor = Color.white;
        public Color SelectTextColor
        {
            get
            {
                return this._selectTextColor;
            }

            set
            {
                this._selectTextColor = value;
            }
        }
        #endregion

        #region Fields
        private CustomToggleButton pair = null;
        #endregion

        #region Events
                ///-------------------------------------------------------------------------
                /// <summary>チェック変更イベント</summary>
                /// <param name="sender">イベント送信者</param>
                /// <param name="args">イベント引数(未使用)</param>
                ///-------------------------------------------------------------------------
        public EventHandler CheckedChanged = delegate { };
        #endregion
    }
    #endregion

    #region CustomSlider
    ///=========================================================================
    /// <summary>スライダー</summary>
    ///=========================================================================
    internal class CustomSlider : ControlBase
    {
        #region Methods
                ///-------------------------------------------------------------------------
                /// <summary>コンストラクタ</summary>
                ///-------------------------------------------------------------------------
        public CustomSlider()
        {
            this.BackgroundColor = Color.white;
        }

        ///-------------------------------------------------------------------------
        /// <summary>コンストラクタ</summary>
        /// <param name="value">初期値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <param name="dec">小数点以下桁数</param>
        ///-------------------------------------------------------------------------
        public CustomSlider( float value, float min, float max, int dec )
        {
            this._value = value;
            this._min = min;
            this._max = max;
            this._decimal = dec;
            this.BackgroundColor = Color.white;
        }

        ///-------------------------------------------------------------------------
        /// <summary>GUI処理</summary>
        ///-------------------------------------------------------------------------
        override public void OnGUI()
        {
            try
            {
                Rect sliderRect;
                if( this.Text != string.Empty )
                {
                    Rect labelRect = new Rect( this.Left, this.Top, this.Width, this.Height - this.FontSize - ControlBase.FixedMargin );
                    sliderRect = new Rect( this.Left, this.Top + this.Height - this.FontSize - ControlBase.FixedMargin, this.Width, this.FontSize + ControlBase.FixedMargin );

                    GUIStyle labelStyle = new GUIStyle( "label" );
                    labelStyle.alignment = TextAnchor.MiddleLeft;
                    labelStyle.normal.textColor = this.TextColor;
                    labelStyle.fontSize = this.FixedFontSize;

                    GUI.Label(labelRect, this.Text, labelStyle);
                }
                else
                {
                    sliderRect = new Rect( this.Left, this.Top, this.Width, this.Height );
                }

                // スライダー表示
                float retVal = 0.0f;
                using( GUIColor color = new GUIColor( this.BackgroundColor, this.TextColor ) )
                {
                    retVal = GUI.HorizontalSlider( sliderRect, this._value, this._min, this._max );
                }

                // 値が変更された場合
                if( Convert.ToInt32( retVal * Math.Pow( 10, this._decimal ) ) != Convert.ToInt32( this._value * Math.Pow( 10, this._decimal ) ) )
                {
                    String format = "{0:f" + this._decimal.ToString() + "}";
                    this.Value = ( float )Convert.ToDouble( String.Format( format, retVal ) );
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }
        #endregion

        #region Properties
                ///-------------------------------------------------------------------------
                /// <summary>現在の位置</summary>
                ///-------------------------------------------------------------------------
        private float _value = 0.0f;
        public float Value
        {
            get
            {
                return this._value;
            }

            set
            {
                // 入力範囲内でかつ設定値変更された場合
                if( this._min <= value && value <= this._max )
                {
                    this._value = value;

                    if( this.Enabled )
                    {
                        // 値変更イベント送信
                        this.ValueChanged( this, new EventArgs() );
                    }
                }
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>最小値</summary>
        ///-------------------------------------------------------------------------
        private float _min = 0.0f;
        public float Min
        {
            get
            {
                return this._min;
            }

            set
            {
                this._min = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>最大値</summary>
        ///-------------------------------------------------------------------------
        private float _max = 0.0f;
        public float Max
        {
            get
            {
                return this._max;
            }

            set
            {
                this._max = value;
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>小数点以下桁数</summary>
        ///-------------------------------------------------------------------------
        private int _decimal = 0;
        public int Decimal
        {
            get
            {
                return this._decimal;
            }

            set
            {
                this._decimal = value;
            }
        }
        #endregion

        #region Events
                ///-------------------------------------------------------------------------
                /// <summary>値変更イベント</summary>
                /// <param name="sender">イベント送信者</param>
                /// <param name="args">イベント引数(未使用)</param>
                ///-------------------------------------------------------------------------
        public EventHandler ValueChanged = delegate { };
        #endregion
    }
    #endregion

    #region CustomLabel
    ///=========================================================================
    /// <summary>ラベル</summary>
    ///=========================================================================
    internal class CustomLabel : ControlBase
    {
        #region Methods
                ///-------------------------------------------------------------------------
                /// <summary>コンストラクタ</summary>
                ///-------------------------------------------------------------------------
        public CustomLabel()
        {
            try
            {
                this.BackgroundColor = Color.clear;
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
                Rect labelRect = new Rect( this.Left, this.Top, this.Width, this.Height );

                // スタイル
                GUIStyle labelStyle = new GUIStyle( "label" );
                labelStyle.alignment = TextAnchor.MiddleLeft;
                labelStyle.normal.textColor = this.TextColor;
                labelStyle.fontSize = this.FixedFontSize;

                GUI.Label( labelRect, this.Text, labelStyle );
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }
        #endregion
    }
    #endregion

    internal class CustomColorPicker : ControlBase
    {
        public CustomColorPicker( Color color )
        {
            try
            {
                this.BackgroundColor = Color.clear;
                this._value = color;

                this._colorTex = new Texture2D(1, 1);
                this._colorTex.SetPixel(0, 0, color);
                this._colorTex.Apply();
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void OnGUI()
        {
            try
            {
                Rect colorRect = new Rect( this.Left, this.Top, this.Height, this.Height );
                Rect labelRect = new Rect( this.Left + this.Height, this.Top, this.Width - this.Height, this.Height );

                // スタイル
                GUIStyle labelStyle = new GUIStyle( "label" );
                labelStyle.alignment = TextAnchor.MiddleLeft;
                labelStyle.normal.textColor = this.TextColor;
                labelStyle.fontSize = this.FixedFontSize;

                GUI.DrawTexture( colorRect, this._colorTex );
                GUI.Label( labelRect, this.Text, labelStyle );

                if( GUI.Button( colorRect, string.Empty, labelStyle ) )
                {
                    GlobalColorPicker.Set(new Vector2(this.Left + this.ScreenPos.x, this.Top + this.ScreenPos.y), this.FontSize * 15, this.FontSize, this._value,
                                          (x) =>
                            {
                                this.Value = x;
                                this.ColorChanged(this, new EventArgs());
                            });
                }
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        private Color _value;
        virtual public Color Value
        {
            get
            {
                return this._value;
            }

            set

            {
                this._value = value;
                this._colorTex.SetPixel(0, 0, value);
                this._colorTex.Apply();
            }
        }

        private Texture2D _colorTex;

        public EventHandler ColorChanged = delegate { };
    }

    internal class CustomCurve : ControlBase
    {
        public CustomCurve( AnimationCurve curve )
        {
            try
            {
                this.BackgroundColor = Color.clear;
                this._value = curve;
                this._curveTex = GlobalCurveWindow.CreateCurveTexture(curve, true);
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle( "label" );
            labelStyle.alignment = TextAnchor.MiddleLeft;
            labelStyle.normal.textColor = this.TextColor;
            labelStyle.fontSize = this.FixedFontSize;

            Rect curveRect;

            if(this.Text != string.Empty)
            {
                curveRect = new Rect( this.Left, this.Top, this.Width, this.Height / 2 );
                Rect labelRect = new Rect( this.Left + this.Height, this.Top, this.Width - this.Height, this.Height );

                GUI.Label( labelRect, this.Text, labelStyle );
            }
            else
            {
                curveRect = new Rect( this.Left, this.Top, this.Width, this.Height );
            }

            GUI.DrawTexture( curveRect, this._curveTex );

            if( GUI.Button( curveRect, string.Empty, labelStyle ) )
            {
                GlobalCurveWindow.Set(new Vector2(this.Left + this.ScreenPos.x, this.Top + this.ScreenPos.y), this.FontSize * 15, this.FontSize, this._value,
                                      (x) =>
                        {
                            this._value = x;
                            this._curveTex = GlobalCurveWindow.CreateCurveTexture(x, true);
                            this.Changed(this, new EventArgs());
                        });
            }
        }

        private AnimationCurve _value;
        virtual public AnimationCurve Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        private Texture2D _curveTex;
        public EventHandler Changed = delegate { };
    }

    internal class CustomPositionSlider : ControlBase
    {
        public CustomPositionSlider()
        {
            try
            {
                this.BackgroundColor = Color.clear;
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void OnGUI() {}

        // private CustomSlider _xPosSlider;
        // private CustomSlider _yPosSlider;
        // private CustomSlider _zPosSlider;
    }

    internal class CustomTextField : ControlBase
    {
        public CustomTextField()
        {
            try
            {
                this.BackgroundColor = Color.clear;
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void OnGUI() {
                Rect labelRect = new Rect( this.Left, this.Top, this.Width / 5, this.Height );
                Rect rectItem = new Rect( labelRect.x + this.Width / 5, this.Top, (this.Width / 5) * 4, this.Height );

                // スタイル
                GUIStyle labelStyle = new GUIStyle( "label" );
                labelStyle.alignment = TextAnchor.MiddleLeft;
                labelStyle.normal.textColor = this.TextColor;
                labelStyle.fontSize = this.FixedFontSize;

                GUIStyle textFieldStyle = new GUIStyle("textarea");
                textFieldStyle.alignment = TextAnchor.UpperLeft;

                GUI.Label( labelRect, this.Text, labelStyle );
                string temp = GUI.TextField(rectItem, this.Value, textFieldStyle);

                if (temp != this.Value) {
                    this.ValueChanged( this, new EventArgs() );
                    this.Value = temp;
                }
        }

        private string _value = "";
        public string Value
        {
            get
            {
                return this._value;
            }

            set
            {
                this._value = value;

                if( this.Enabled )
                {
                    // チェック変更送信
                    this.ValueChanged( this, new EventArgs() );
                }
            }
        }

        public EventHandler ValueChanged = delegate { };
    }

    internal class CustomTextureButton : ControlBase
    {
        public CustomTextureButton( Texture2D tex )
        {
            try
            {
                this.Texture = tex;
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void OnGUI() {
                Rect buttonRect = new Rect( this.Left, this.Top, this.Width, this.Height );

                // スタイル
                GUIStyle buttonStyle = new GUIStyle( "button" );
                buttonStyle.alignment = TextAnchor.MiddleCenter;
                buttonStyle.fontSize = this.FixedFontSize;

                if( this.Enabled == false )
                {
                    buttonStyle.normal.background = buttonStyle.onNormal.background =
                        buttonStyle.hover.background = buttonStyle.onHover.background =
                        buttonStyle.active.background = buttonStyle.onActive.background =
                        buttonStyle.focused.background = buttonStyle.onFocused.background = new Texture2D( 2, 2 );

                    buttonStyle.normal.textColor = buttonStyle.onNormal.textColor =
                        buttonStyle.hover.textColor = buttonStyle.onHover.textColor =
                        buttonStyle.active.textColor = buttonStyle.onActive.textColor =
                        buttonStyle.focused.textColor = buttonStyle.onFocused.textColor = Color.gray;
                }

                // ボタン表示
                if( GUI.Button( buttonRect, this.Text, buttonStyle ) )
                {
                    if( this.Enabled )
                    {
                        // クリックイベント送信
                        this.Click( this, new EventArgs() );
                    }
                }

                GUI.DrawTexture( buttonRect, this._texture );
        }

        private Texture2D _texture = null;
        public Texture2D Texture
        {
            get
            {
                return this.Texture;
            }

            set
            {
                this._texture = value;
            }
        }

        public EventHandler Click = delegate { };
    }
}
