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
    /// <summary>
    ///   Combobox window shared across all CustomComboBox objects.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Attempting to create a Window within a ScrollView causes weird things to happen.
    ///     This class lets the CustomComboBox objects access a window that's rendered outside the ScrollView region, which allows it to operate correctly.
    ///     It's a hacky workaround.
    ///   </para>
    /// </remarks>
    public static class GlobalComboBox
    {
        static GlobalComboBox() {
            combo = new ComboBox(300);
            gsWin = new GUIStyle("box");
            gsWin.fontSize = Util.GetPix(12);
            gsWin.alignment = TextAnchor.UpperRight;

        }

        public static void Update()
        {
            if(combo.show)
            {
                combo.rect = GUI.Window(combo.WINDOW_ID, combo.rect, combo.GuiFunc, string.Empty, gsWin);
            }
        }

        public static bool Visible
        {
            get
            {
                return combo.show;
            }
        }

        public static void Set(Rect r, GUIContent[] s, int i, Action<int> f)
        {
            combo.Set(r, s, i, f);
        }

        private static GUIStyle gsWin;
        private static ComboBox combo;

        internal class ComboBox
        {
            public readonly int WINDOW_ID;

            public Rect rect { get; set; }
            private Rect rectItem { get; set; }
            public bool show { get; private set; }
            private GUIContent[] sItems { get; set; }

            private GUIStyle gsSelectionGrid { get; set; }
            private GUIStyleState gssBlack { get; set; }
            private GUIStyleState gssWhite { get; set; }

            public Action<int> func { get; private set; }

            public ComboBox(int iWIndowID)
            {
                WINDOW_ID = iWIndowID;
            }

            public void Set(Rect r, GUIContent[] s, int i, Action<int> f)
            {
                rect = r;
                sItems = s;

                gsSelectionGrid = new GUIStyle();
                gsSelectionGrid.fontSize = i;

                gssBlack = new GUIStyleState();
                gssBlack.textColor = Color.white;
                gssBlack.background = Texture2D.blackTexture;

                gssWhite = new GUIStyleState();
                gssWhite.textColor = Color.black;
                gssWhite.background = Texture2D.whiteTexture;

                gsSelectionGrid.normal = gssBlack;
                gsSelectionGrid.hover = gssWhite;

                rectItem = new Rect(0f, 0f, rect.width, rect.height);

                func = f;

                show = true;
            }

            public void GuiFunc(int winId)
            {
                int iTmp = -1;
                iTmp = GUI.SelectionGrid(rectItem, -1, sItems, 1, gsSelectionGrid);
                if (iTmp >= 0)
                {
                    func(iTmp);
                    show = false;
                }

                if (GetAnyMouseButtonDown())
                {
                    Vector2 v2Tmp = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                    if (!rect.Contains(v2Tmp))
                        show = false;
                }
            }

            private bool GetAnyMouseButtonDown()
            {
                return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
            }
        }
    }
}
