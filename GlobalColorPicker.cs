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
    public static class GlobalColorPicker
    {
        static GlobalColorPicker() {
            color = new ColorWindow(300);
            gsWin = new GUIStyle("box");
            gsWin.fontSize = Util.GetPix(12);
            gsWin.alignment = TextAnchor.UpperRight;

        }

        public static void Update()
        {
            if(color.show)
            {
                color.rect = GUI.Window(color.WINDOW_ID, color.rect, color.GuiFunc, string.Empty, gsWin);
            }
        }

        public static bool Visible
        {
            get
            {
                return color.show;
            }
        }

        public static void Set(Vector2 p, float fWidth, int iFontSize, Color32 c, bool isRGBA, Action<Color32> f)
        {
            color.Set(p, fWidth, iFontSize, c, isRGBA, f);
        }

        private static GUIStyle gsWin;
        private static ColorWindow color;

        internal class ColorWindow
        {
            public readonly int WINDOW_ID;

            public Rect rect { get; set; }
            private float fMargin { get; set; }
            private float fRightPos { get; set; }
            private float fUpPos { get; set; }

            public bool show { get; private set; }

            public Action<Color32> func { get; private set; }

            private GUIStyle gsLabel { get; set; }
            private GUIStyle gsButton { get; set; }

            private Texture2D texture { get; set; }
            private byte r { get; set; }
            private byte g { get; set; }
            private byte b { get; set; }
            private byte a { get; set; }

            private bool isRGBA { get; set; }

            public ColorWindow(int iWIndowID)
            {
                WINDOW_ID = iWIndowID;
                r = g = b = a = 255;
                texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, new Color32(r, g, b, a));
                texture.Apply();
            }


            public void Set(Vector2 p, float fWidth, int iFontSize, Color32 c, bool isRGBA, Action<Color32> f)
            {
                rect = new Rect(p.x - fWidth, p.y, fWidth, 0f);
                fRightPos = p.x + fWidth;
                fUpPos = p.y;

                gsLabel = new GUIStyle("label");
                gsLabel.fontSize = iFontSize;
                gsLabel.alignment = TextAnchor.MiddleLeft;

                gsButton = new GUIStyle("button");
                gsButton.fontSize = iFontSize;
                gsButton.alignment = TextAnchor.MiddleCenter;

                fMargin = iFontSize * 0.3f;

                func = f;

                r = c.r;
                g = c.g;
                b = c.b;
                a = c.a;

                this.isRGBA = isRGBA;

                texture.SetPixel(0, 0, c);
                texture.Apply();

                show = true;
            }

            public void GuiFunc(int winId)
            {
                int iFontSize = gsLabel.fontSize;
                Rect rectItem = new Rect(iFontSize * 0.5f, iFontSize * 0.5f, iFontSize * 1.5f, iFontSize * 1.5f);
                GUI.DrawTexture(rectItem, texture);

                rectItem.width = rect.width - iFontSize;
                rectItem.y += rectItem.height + fMargin;
                GUI.Label(rectItem, "R: " + r.ToString(), gsLabel);

                rectItem.x = rect.width - iFontSize * 4.5f;
                rectItem.width = iFontSize * 2;
                if(GUI.Button(rectItem, "-1", gsButton))
                {
                    r = r == 0 ? r : (byte)(r - 1);
                }

                rectItem.x += rectItem.width;
                if (GUI.Button(rectItem, "+1", gsButton))
                {
                    r = r == 255 ? r : (byte)(r + 1);
                }

                rectItem.x = iFontSize * 0.5f;
                rectItem.width = rect.width - iFontSize;
                rectItem.y += rectItem.height;
                r = (byte)GUI.HorizontalSlider(rectItem, r, 0f, 255f);

                rectItem.y += rectItem.height + fMargin;
                GUI.Label(rectItem, "G: " + g.ToString(), gsLabel);

                rectItem.x = rect.width - iFontSize * 4.5f;
                rectItem.width = iFontSize * 2;
                if (GUI.Button(rectItem, "-1", gsButton))
                {
                    g = g == 0 ? g : (byte)(g - 1);
                }

                rectItem.x += rectItem.width;
                if (GUI.Button(rectItem, "+1", gsButton))
                {
                    g = g == 255 ? g : (byte)(g + 1);
                }

                rectItem.x = iFontSize * 0.5f;
                rectItem.width = rect.width - iFontSize;
                rectItem.y += rectItem.height;
                g = (byte)GUI.HorizontalSlider(rectItem, g, 0f, 255f);

                rectItem.y += rectItem.height + fMargin;
                GUI.Label(rectItem, "B: " + b.ToString(), gsLabel);

                rectItem.x = rect.width - iFontSize * 4.5f;
                rectItem.width = iFontSize * 2;
                if (GUI.Button(rectItem, "-1", gsButton))
                {
                    b = b == 0 ? b : (byte)(b - 1);
                }

                rectItem.x += rectItem.width;
                if (GUI.Button(rectItem, "+1", gsButton))
                {
                    b = b == 255 ? b : (byte)(b + 1);
                }

                rectItem.x = iFontSize * 0.5f;
                rectItem.width = rect.width - iFontSize;
                rectItem.y += rectItem.height;
                b = (byte)GUI.HorizontalSlider(rectItem, b, 0f, 255f);

                if( this.isRGBA )
                {
                    rectItem.y += rectItem.height + fMargin;
                    GUI.Label(rectItem, "A: " + a.ToString(), gsLabel);

                    rectItem.x = rect.width - iFontSize * 4.5f;
                    rectItem.width = iFontSize * 2;
                    if (GUI.Button(rectItem, "-1", gsButton))
                    {
                        a = a == 0 ? a : (byte)(a - 1);
                    }

                    rectItem.x += rectItem.width;
                    if (GUI.Button(rectItem, "+1", gsButton))
                    {
                        a = a == 255 ? a : (byte)(a + 1);
                    }

                    rectItem.x = iFontSize * 0.5f;
                    rectItem.width = rect.width - iFontSize;
                    rectItem.y += rectItem.height;
                    a = (byte)GUI.HorizontalSlider(rectItem, a, 0f, 255f);
                }

                float fHeight = rectItem.y + rectItem.height + fMargin;
                if (rect.height != fHeight)
                {
                    Rect rectTmp = new Rect(rect.x, rect.y - fHeight, rect.width, fHeight);
                    rect = rectTmp;
                }
                else if (rect.x < 0f)
                {
                    Rect rectTmp = new Rect(fRightPos, rect.y, rect.width, rect.height);
                    rect = rectTmp;
                }
                else if (rect.y < 0f)
                {
                    Rect rectTmp = new Rect(rect.x, fUpPos, rect.width, rect.height);
                    rect = rectTmp;
                }

                if (GUI.changed)
                {
                    texture.SetPixel(0, 0, new Color32(r, g, b, a));
                    texture.Apply();
                    func(new Color32(r, g, b, a));
                }

                GUI.DragWindow();

                if (GetAnyMouseButtonDown())
                {
                    Vector2 v2Tmp = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                    if (!rect.Contains(v2Tmp))
                    {
                        func(new Color32(r, g, b, a));
                        show = false;
                    }
                }
            }

            private bool GetAnyMouseButtonDown()
            {
                return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
            }

        }
    }
}
