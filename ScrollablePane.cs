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
    internal abstract class ScrollablePane : ControlBase
    {
        public abstract void ShowPane();

        public ScrollablePane() : base() {}

        public ScrollablePane( int fontSize )
        {
            try
            {
                this.FontSize = fontSize;

                this.guiScroll = Vector2.zero;

                screenSize = new Vector2(Screen.width, Screen.height);

                this.Awake();
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
                GUIStyle gsWin = new GUIStyle("box");
                gsWin.fontSize = Util.GetPix(12);
                gsWin.alignment = TextAnchor.UpperRight;

                float fWidth = gsWin.fontSize * (isGuiScroll ? 19 : 18);
                if (rectGui.width > fWidth)
                {
                    fWidth = rectGui.width;
                }

                if (rectGui.width < 1)
                {
                    rectGui.Set(Screen.width - fWidth - ControlBase.FixedMargin * 4, ControlBase.FixedMargin * 4, fWidth, guiHeight);
                }
                if (guiHeight != rectGui.height || fWidth != rectGui.width)
                {
                    rectGui.Set(rectGui.x, rectGui.y, fWidth, guiHeight);
                }

                if (screenSize != new Vector2(Screen.width, Screen.height))
                {
                    rectGui.Set(rectGui.x, rectGui.y, fWidth, guiHeight);
                    screenSize = new Vector2(Screen.width, Screen.height);
                }
                if (rectGui.x < 0 - rectGui.width * 0.9f)
                {
                    rectGui.x = 0;
                }
                else if (rectGui.x > screenSize.x - rectGui.width * 0.1f)
                {
                    rectGui.x = screenSize.x - rectGui.width;
                }
                else if (rectGui.y < 0 - rectGui.height * 0.9f)
                {
                    rectGui.y = 0;
                }
                else if (rectGui.y > screenSize.y - rectGui.height * 0.1f)
                {
                    rectGui.y = screenSize.y - rectGui.height;
                }

                rectGui = GUI.Window(252, rectGui, GuiFunc, "", gsWin);
                this.ScreenPos = new Rect(rectGui.x + guiScroll.x, rectGui.y - guiScroll.y, rectGui.width, rectGui.height);

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
                        enableGameGui = !rectGui.Contains(mousePos);
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

        private void GuiFunc(int winId)
        {
            float itemHeight = this.FontSize * 1.5f;
            float margin = this.FontSize * 0.3f;

            Rect rectInner = new Rect(this.FontSize / 2, this.FontSize, this.FontSize * ITEM_SIZE - this.FontSize, this.rectGui.height - this.FontSize * 2);
            Rect rectItem = new Rect(0f, 0f, Util.GetPix(8) * 2, Util.GetPix(8) * 2);


            if (guiScrollHeight > screenSize.y * 0.7f)
            {
                guiHeight = screenSize.y * 0.7f;
                isGuiScroll = true;
            }
            else
            {
                guiHeight = guiScrollHeight;
                isGuiScroll = false;
            }

            float windowWidth = this.FontSize * (isGuiScroll ? 19 : 18);

            // this.Height = GUIUtil.GetHeightForParent(this);

            Rect rectScroll = new Rect(0f, rectInner.y + margin * 2, this.rectGui.width, this.rectGui.height);
            Rect rectScrollView = new Rect(0f, 0, this.rectGui.width, guiScrollHeight);

            guiScroll = GUI.BeginScrollView(rectScroll, guiScroll, rectScrollView);

            rectItem.Set(rectInner.x, 0f, this.FontSize * 15, itemHeight);

            rectItem.width = (this.FontSize * 1.2f) + this.FontSize * 18 * 0.6f;

            ShowPane();

            GUIStyle gsButton = new GUIStyle("button");
            gsButton.fontSize = this.FontSize;
            gsButton.alignment = TextAnchor.MiddleCenter;

            this.OnGUIChildControls();
            GUI.EndScrollView();

            guiScrollHeight = this.LastElementSize + rectInner.y + margin * 3;

            GUI.DragWindow();
        }

        public float ControlHeight
        {
            get
            {
                return this.FixedFontSize + ControlBase.FixedMargin * 2;
            }
        }

        public static readonly float ITEM_SIZE = 18;

        private float guiHeight;
        private float guiScrollHeight;
        private Vector2 screenSize;
        private Vector2 guiScroll;
        private bool isGuiScroll;
        public Rect rectGui;
    }
}
