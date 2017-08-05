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
    public static class GlobalCurveWindow
    {
        static Texture2D textureBack { get; set; }
        static Texture2D textureBackNarrow { get; set; }

        static GlobalCurveWindow() {
            // instantiate textures first, before using them in window
            {
                textureBack = new Texture2D(128, 128);

                Color[] color = new Color[128 * 128];
                for (int i = 0; i < color.Length; i++)
                {
                    color[i] = Color.black;
                }
                textureBack.SetPixels(color);
                textureBack.Apply();
            }

            {
                textureBackNarrow = new Texture2D(128, 32);
                Color[] color = new Color[128 * 32];
                for (int i = 0; i < color.Length; i++)
                {
                    color[i] = Color.black;
                }
                textureBackNarrow.SetPixels(color);
                textureBackNarrow.Apply();
            }

            curve = new CurveWindow(302);
            gsWin = new GUIStyle("box");
            gsWin.fontSize = Util.GetPix(12);
            gsWin.alignment = TextAnchor.UpperRight;
        }

        public static Texture2D CreateCurveTexture(AnimationCurve curve, bool bNarrow)
        {
            Texture2D tex;
            if (bNarrow)
                tex = (Texture2D)UnityEngine.Object.Instantiate(GlobalCurveWindow.textureBackNarrow);
            else
                tex = (Texture2D)UnityEngine.Object.Instantiate(GlobalCurveWindow.textureBack);
            int width = tex.width;
            int height = tex.height;
            Color[] color = tex.GetPixels();
            for (int x = 0; x < width; x++)
            {
                float f = Mathf.Clamp01(curve.Evaluate(x / (float)width));
                color[x + (int)(f * (height - 1)) * width] = Color.green;
            }
            tex.SetPixels(color);
            tex.Apply();
            return tex;
        }

        public static void Update()
        {
            if(curve.show)
            {
                curve.rect = GUI.Window(curve.WINDOW_ID, curve.rect, curve.GuiFunc, string.Empty, gsWin);
            }
        }

        public static bool Visible
        {
            get
            {
                return curve.show;
            }
        }

        public static void Set(Vector2 p, float fWidth, int iFontSize, AnimationCurve _curve, Action<AnimationCurve> f)
        {
            curve.Set(p, fWidth, iFontSize, _curve, f);
        }

        private static GUIStyle gsWin;
        private static CurveWindow curve;

        internal class CurveWindow
        {
            public readonly int WINDOW_ID;

            public Rect rect { get; set; }
            private float fMargin { get; set; }
            private float fRightPos { get; set; }
            private float fUpPos { get; set; }

            public bool show { get; private set; }
            public bool narrowSlider { get; set; }

            public Action<AnimationCurve> func { get; private set; }

            private static GUIStyle gsLabel { get; set; }
            private static GUIStyle gsButton { get; set; }
            private static GUIStyle gsText { get; set; }

            private Texture2D texture { get; set; }
            private AnimationCurve curve { get; set; }
            private Keyframe[] keys { get; set; }

            private float[] fCurve { get; set; }

            private string[] sValues { get; set; }

            private bool isGuiTranslation = false;

            public CurveWindow(int iWIndowID)
            {
                WINDOW_ID = iWIndowID;

                texture = (Texture2D)UnityEngine.Object.Instantiate(GlobalCurveWindow.textureBack);

                fCurve = new float[4];
                keys = new Keyframe[2];

                sValues = new string[4];

                gsLabel = new GUIStyle("label");
                gsLabel.alignment = TextAnchor.MiddleLeft;

                gsButton = new GUIStyle("button");
                gsButton.alignment = TextAnchor.MiddleCenter;

                gsText = new GUIStyle("textarea");
                gsText.alignment = TextAnchor.UpperLeft;
            }

            public void Set(Vector2 p, float fWidth, int iFontSize, AnimationCurve _curve, Action<AnimationCurve> f)
            {
                rect = new Rect(p.x - fWidth, p.y, fWidth, 0f);
                fRightPos = p.x + fWidth;
                fUpPos = p.y;

                gsLabel.fontSize = iFontSize;
                gsButton.fontSize = iFontSize;
                gsText.fontSize = iFontSize;

                fMargin = iFontSize * 0.3f;

                func = f;

                curve = _curve;
                keys[0] = _curve.keys[0];
                keys[1] = _curve.keys[1];

                fCurve[0] = keys[0].outTangent;
                fCurve[1] = keys[0].value;
                fCurve[2] = keys[1].inTangent;
                fCurve[3] = keys[1].value;

                sValues[0] = fCurve[0].ToString();
                sValues[1] = fCurve[1].ToString();
                sValues[2] = fCurve[2].ToString();
                sValues[3] = fCurve[3].ToString();

                texture = GlobalCurveWindow.CreateCurveTexture(curve, false);

                show = true;
            }

            private void CreateCurve()
            {
                keys[0].outTangent = fCurve[0];
                keys[0].value = fCurve[1];
                keys[1].inTangent = fCurve[2];
                keys[1].value = fCurve[3];
                curve = new AnimationCurve(keys);
                texture = GlobalCurveWindow.CreateCurveTexture(curve, false);
            }

            public void GuiFunc(int winId)
            {
                int iFontSize = gsLabel.fontSize;
                Rect rectItem = new Rect(iFontSize * 0.5f, iFontSize * 0.5f, iFontSize, rect.width - iFontSize * 3);

                float fTmp;

                fTmp = GUI.VerticalSlider(rectItem, fCurve[1], 1f, 0f);
                if (fTmp != fCurve[1])
                {
                    fCurve[1] = fTmp;
                    sValues[1] = fTmp.ToString();
                }

                rectItem.x = rect.width - rectItem.width - iFontSize * 0.5f;
                fTmp = GUI.VerticalSlider(rectItem, fCurve[3], 1f, 0f);
                if (fTmp != fCurve[3])
                {
                    fCurve[3] = fTmp;
                    sValues[3] = fTmp.ToString();
                }

                rectItem.x = rectItem.width + iFontSize * 0.5f;
                rectItem.width = rectItem.height;
                GUI.DrawTexture(rectItem, texture);

                rectItem.x = iFontSize * 0.5f;
                rectItem.width = (rect.width - iFontSize) / 2f;
                rectItem.y += rectItem.height;
                rectItem.height = iFontSize * 1.5f;
                string sTmp = Util.DrawTextFieldF(rectItem, sValues[1], gsText);
                if (sTmp != sValues[1])
                {
                    if (float.TryParse(sTmp, out fTmp))
                    {
                        fCurve[1] = Mathf.Clamp01(fTmp);
                        sTmp = fCurve[1].ToString();
                    }
                    sValues[1] = sTmp;
                }

                rectItem.x += rectItem.width;
                sTmp = Util.DrawTextFieldF(rectItem, sValues[3], gsText);
                if (sTmp != sValues[3])
                {
                    if (float.TryParse(sTmp, out fTmp))
                    {
                        fCurve[3] = Mathf.Clamp01(fTmp);
                        sTmp = fCurve[3].ToString();
                    }

                    sValues[3] = sTmp;
                }

                //

                rectItem.x = iFontSize * 0.5f;
                rectItem.width = iFontSize * 4;
                rectItem.y += rectItem.height + fMargin;
                GUI.Label(rectItem, "Start", gsLabel);

                rectItem.width = rect.width - rectItem.width - iFontSize;
                rectItem.x = rect.width - rectItem.width - iFontSize * 0.5f;
                sTmp = Util.DrawTextFieldF(rectItem, sValues[0], gsText);

                if (sTmp != sValues[0])
                {
                    if (float.TryParse(sTmp, out fTmp))
                        fCurve[0] = fTmp;

                    sValues[0] = sTmp;
                }

                float fMax = narrowSlider ? 1f : 10f;

                rectItem.x = iFontSize * 0.5f;
                rectItem.width = rect.width - iFontSize * 3;
                rectItem.y += rectItem.height;
                fTmp = GUI.HorizontalSlider(rectItem, fCurve[0], -fMax, fMax);
                if (fTmp != fCurve[0])
                {
                    fCurve[0] = fTmp;
                    sValues[0] = fTmp.ToString();
                }

                rectItem.x += rectItem.width;
                rectItem.width = iFontSize * 2;
                if (GUI.Button(rectItem, ConstantValues.GUIBUTTON_DEF, gsButton))
                {
                    fCurve[0] = 1f;
                    sValues[0] = fCurve[0].ToString();
                }


                rectItem.x = iFontSize * 0.5f;
                rectItem.width = iFontSize * 4;
                rectItem.y += rectItem.height + fMargin;
                GUI.Label(rectItem, "End", gsLabel);

                rectItem.width = rect.width - rectItem.width - iFontSize;
                rectItem.x = rect.width - rectItem.width - iFontSize * 0.5f;
                sTmp = Util.DrawTextFieldF(rectItem, sValues[2], gsText);
                if (sTmp != sValues[2])
                {
                    if (float.TryParse(sTmp, out fTmp))
                        fCurve[2] = fTmp;

                    sValues[2] = sTmp;
                }

                rectItem.x = iFontSize * 0.5f;
                rectItem.width = rect.width - iFontSize * 3;
                rectItem.y += rectItem.height;
                fTmp = GUI.HorizontalSlider(rectItem, fCurve[2], -fMax, fMax);
                if (fTmp != fCurve[2])
                {
                    fCurve[2] = fTmp;
                    sValues[2] = fTmp.ToString();
                }

                rectItem.x += rectItem.width;
                rectItem.width = iFontSize * 2;
                if (GUI.Button(rectItem, ConstantValues.GUIBUTTON_DEF, gsButton))
                {
                    fCurve[2] = 1f;
                    sValues[2] = fCurve[2].ToString();
                }


                rectItem.width = iFontSize * 5f;
                rectItem.x = iFontSize * 0.5f;
                rectItem.y += rectItem.height + fMargin;
                if (GUI.Button(rectItem, isGuiTranslation ? (narrowSlider ? "幅を広く" : "幅を狭く") : (narrowSlider ? "Broad" : "Narrow"), gsButton))
                {
                    narrowSlider = !narrowSlider;
                }


                rectItem.x = rect.width - iFontSize * 5.5f;
                if (GUI.Button(rectItem, isGuiTranslation ? "全値反転" : "Reverse", gsButton))
                {
                    fCurve[1] = 1f - fCurve[1];
                    sValues[1] = fCurve[1].ToString();

                    fCurve[3] = 1f - fCurve[3];
                    sValues[3] = fCurve[3].ToString();

                    fCurve[0] *= -1;
                    sValues[0] = fCurve[0].ToString();

                    fCurve[2] *= -1;
                    sValues[2] = fCurve[2].ToString();
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
                    CreateCurve();
                    func(curve);
                }

                GUI.DragWindow();

                if (GetAnyMouseButtonDown())
                {
                    Vector2 v2Tmp = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                    if (!rect.Contains(v2Tmp))
                    {
                        func(curve);
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
