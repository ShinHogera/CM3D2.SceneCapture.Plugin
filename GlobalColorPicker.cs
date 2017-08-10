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
    public enum ColorPickerType
    {
        RGB,
        RGBA,
        MaidProp
    }

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

        public static void Set(Vector2 p, float fWidth, int iFontSize, Color32 c, ColorPickerType type, Action<Color32, MaidParts.PartsColor> f)
        {
            color.Set(p, fWidth, iFontSize, c, type, f);
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

            public Action<Color32, MaidParts.PartsColor> func { get; private set; }

            private GUIStyle gsLabel { get; set; }
            private GUIStyle gsButton { get; set; }

            private Texture2D texture { get; set; }
            private byte r { get; set; }
            private byte g { get; set; }
            private byte b { get; set; }
            private byte a { get; set; }
            private MaidParts.PartsColor partsColor;

            private ColorPickerType type { get; set; }

            public ColorWindow(int iWIndowID)
            {
                WINDOW_ID = iWIndowID;
                r = g = b = a = 255;
                texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, new Color32(r, g, b, a));
                texture.Apply();
            }

            public void Set(Vector2 p, float fWidth, int iFontSize, Color32 c, ColorPickerType type, Action<Color32, MaidParts.PartsColor> f)
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

                partsColor = new MaidParts.PartsColor();

                this.type = type;

                texture.SetPixel(0, 0, c);
                texture.Apply();

                show = true;
            }

            private float slider(float val, float min, float max, string name, ref Rect rectItem)
            {
                rectItem.y += rectItem.height + fMargin;
                GUI.Label(rectItem, name + ": " + r.ToString(), gsLabel);

                rectItem.x = rect.width - gsLabel.fontSize * 4.5f;
                rectItem.width = gsLabel.fontSize * 2;
                if(GUI.Button(rectItem, "-1", gsButton))
                {
                    val = val == min ? r : (r - 1);
                }

                rectItem.x += rectItem.width;
                if (GUI.Button(rectItem, "+1", gsButton))
                {
                    val = val == max ? r : (r + 1);
                }

                rectItem.x = gsLabel.fontSize * 0.5f;
                rectItem.width = rect.width - gsLabel.fontSize;
                rectItem.y += rectItem.height;

                return GUI.HorizontalSlider(rectItem, val, 0f, 255f);
            }

            public void GuiFunc(int winId)
            {
                int iFontSize = gsLabel.fontSize;
                Rect rectItem = new Rect(iFontSize * 0.5f, iFontSize * 0.5f, iFontSize * 1.5f, iFontSize * 1.5f);
                GUI.DrawTexture(rectItem, texture);

                if( this.type == ColorPickerType.RGB || this.type == ColorPickerType.RGBA )
                {
                    r = (byte)slider((float)r, 0, 255, "R", ref rectItem);
                    g = (byte)slider((float)g, 0, 255, "G", ref rectItem);
                    b = (byte)slider((float)b, 0, 255, "B", ref rectItem);
                    if( this.type == ColorPickerType.RGBA )
                    {
                        a = (byte)slider(a, 0, 255, "A", ref rectItem);
                    }
                }

                else if( this.type == ColorPickerType.MaidProp )
                {
                    partsColor.m_nMainHue = (int)slider((float)partsColor.m_nMainHue, 0, 300, Translation.GetText("ColorPicker", "hue"), ref rectItem);
                    partsColor.m_nMainChroma = (int)slider((float)partsColor.m_nMainChroma, 0, 300, Translation.GetText("ColorPicker", "chroma"), ref rectItem);
                    partsColor.m_nMainBrightness = (int)slider((float)partsColor.m_nMainBrightness, 0, 300, Translation.GetText("ColorPicker", "brightness"), ref rectItem);
                    partsColor.m_nMainContrast = (int)slider((float)partsColor.m_nMainContrast, 0, 300, Translation.GetText("ColorPicker", "contrast"), ref rectItem);
                    partsColor.m_nShadowRate = (int)slider((float)partsColor.m_nShadowRate, 0, 300, Translation.GetText("ColorPicker", "shadowRate"), ref rectItem);
                    partsColor.m_nShadowHue = (int)slider((float)partsColor.m_nShadowHue, 0, 300, Translation.GetText("ColorPicker", "shadowHue"), ref rectItem);
                    partsColor.m_nShadowChroma = (int)slider((float)partsColor.m_nShadowChroma, 0, 300, Translation.GetText("ColorPicker", "shadowChroma"), ref rectItem);
                    partsColor.m_nShadowBrightness = (int)slider((float)partsColor.m_nShadowBrightness, 0, 300, Translation.GetText("ColorPicker", "shadowBrightness"), ref rectItem);
                    partsColor.m_nShadowContrast = (int)slider((float)partsColor.m_nShadowContrast, 0, 300, Translation.GetText("ColorPicker", "shadowContrast"), ref rectItem);
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
                    func(new Color32(r, g, b, a), partsColor);
                }

                GUI.DragWindow();

                if (GetAnyMouseButtonDown())
                {
                    Vector2 v2Tmp = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                    if (!rect.Contains(v2Tmp))
                    {
                        func(new Color32(r, g, b, a), partsColor);
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
