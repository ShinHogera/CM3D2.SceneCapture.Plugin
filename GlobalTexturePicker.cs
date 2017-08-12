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
    public static class GlobalTexturePicker
    {
        static GlobalTexturePicker() {
            texturePicker = new TexturePickerWindow(300);
            gsWin = new GUIStyle("box");
            gsWin.fontSize = Util.GetPix(12);
            gsWin.alignment = TextAnchor.UpperRight;
        }

        public static void Update()
        {
            if(texturePicker.show)
            {
                texturePicker.rect = GUI.Window(texturePicker.WINDOW_ID, texturePicker.rect, texturePicker.GuiFunc, string.Empty, gsWin);
            }
        }

        public static bool Visible
        {
            get
            {
                return texturePicker.show;
            }
        }

        public static void Set(Vector2 p, float fWidth, int iFontSize, List<string> imageDirectories, Action<Texture2D, string> f)
        {
            texturePicker.Set(p, fWidth, iFontSize, imageDirectories, f);
        }

        private static GUIStyle gsWin;
        private static TexturePickerWindow texturePicker;

        internal class TexturePickerWindow
        {
            public readonly int WINDOW_ID;

            public Rect rect { get; set; }
            private float fWidth { get; set; }
            private float fMargin { get; set; }
            private float fRightPos { get; set; }
            private float fUpPos { get; set; }
            private float guiScrollHeight { get; set; }

            public bool show { get; private set; }

            public Action<Texture2D, string> func { get; private set; }

            private GUIStyle gsLabel { get; set; }
            private GUIStyle gsButton { get; set; }

            private Vector2 scrollPosition { get; set; }

            private int index;
            private List<KeyValuePair<string, string>> imageFiles;
            private Texture2D currentTexture;

            public TexturePickerWindow(int iWIndowID)
            {
                WINDOW_ID = iWIndowID;

                index = 0;
                imageFiles = new List<KeyValuePair<string, string>>();
            }

            public void ChangeImage()
            {
                if( !imageFiles.Any() )
                    return;

                string fileName = imageFiles[index].Key;
                string abbrevPath = imageFiles[index].Value;
                Debug.Log("loadin " + fileName);
                byte[] bytes = File.ReadAllBytes(fileName);
                currentTexture = new Texture2D(4, 4);
                currentTexture.LoadImage(bytes);
                func(currentTexture, abbrevPath);
            }

            private void SelectNext()
            {
                if( !imageFiles.Any() )
                    return;

                index += 1;
                if( index >= imageFiles.Count )
                    index = 0;

                this.ChangeImage();
            }

            private void SelectPrev()
            {
                if( !imageFiles.Any() )
                    return;

                index -= 1;
                if( index < 0 )
                    index = imageFiles.Count - 1;

                this.ChangeImage();
            }

            private void UpdateImages(List<string> imageDirectories)
            {
                imageFiles = new List<KeyValuePair<string, string>>();
                index = 0;

                foreach(string directory in imageDirectories)
                {
                    string fullPath = ConstantValues.BaseConfigDir + @"\" + directory;
                    if( !Directory.Exists(fullPath))
                    {
                        fullPath = ConstantValues.BaseConfigDirSybaris + @"\" + directory;
                        if( !Directory.Exists(fullPath) ) {
                            continue;
                        }
                    }

                    DirectoryInfo info = new DirectoryInfo(fullPath);
                    FileInfo[] files = info.GetFiles("*.png").OrderBy(p => p.Name).ToArray();
                    foreach(FileInfo file in files)
                    {
                        string abbrevPath = file.FullName.Replace(ConstantValues.BaseConfigDir + @"\", "");
                        abbrevPath = abbrevPath.Replace(ConstantValues.BaseConfigDirSybaris + @"\", "");
                        var pair = new KeyValuePair<string, string>(file.FullName, abbrevPath);
                        imageFiles.Add(pair);
                    }
                }

                this.ChangeImage();
            }

            public void Set(Vector2 p, float fWidth, int iFontSize, List<string> imageDirectories, Action<Texture2D, string> f)
            {
                rect = new Rect(p.x - fWidth, p.y, fWidth, 0f);
                fRightPos = p.x + fWidth;
                fUpPos = p.y;
                this.fWidth = fWidth;

                gsLabel = new GUIStyle("label");
                gsLabel.fontSize = iFontSize;
                gsLabel.alignment = TextAnchor.MiddleLeft;

                gsButton = new GUIStyle("button");
                gsButton.fontSize = iFontSize;
                gsButton.alignment = TextAnchor.MiddleCenter;

                fMargin = iFontSize * 0.3f;
                scrollPosition = new Vector2(0, 0);

                func = f;

                show = true;

                this.UpdateImages(imageDirectories);
            }

            public void GuiFunc(int winId)
            {
                float windowHeight = this.rect.height;
                if( this.rect.height > Screen.height * 0.7f )
                {
                    windowHeight = Screen.height * 0.7f;
                }

                int iFontSize = gsLabel.fontSize;

                Rect rectItem = new Rect(iFontSize * 0.5f, iFontSize * 0.5f, iFontSize * 1.5f, iFontSize * 1.5f);
                if( this.currentTexture != null )
                    GUI.DrawTexture( rectItem, this.currentTexture );

                rectItem.width = iFontSize * 3;
                rectItem.y += rectItem.height + fMargin;
                if (GUI.Button(rectItem, "<", gsButton))
                {
                    this.SelectPrev();
                }
                rectItem.x += rectItem.width;
                if (GUI.Button(rectItem, ">", gsButton))
                {
                    this.SelectNext();
                }
                rectItem.x += rectItem.width;
                rectItem.width = iFontSize * 20;
                GUI.Label(rectItem, "Name", gsLabel);

                rectItem.y += 100;

                float fHeight = rectItem.y + rectItem.height + fMargin;
                if (fHeight > Screen.height * 0.7f) {
                    fHeight = Screen.height * 0.7f;
                }

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

                guiScrollHeight = rectItem.y + rectItem.height;

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
                        enableGameGui = !rect.Contains(mousePos);
                    }
                    GameMain.Instance.MainCamera.SetControl(enableGameGui);
                    UICamera.InputEnable = enableGameGui;
                }

                GUI.DragWindow();

                // Vector2 screenSize = new Vector2(Screen.width, Screen.height);
                // if (guiScrollHeight > screenSize.y * 0.7f)
                // {
                //     rect = new Rect(rect.x, rect.y, rect.width, screenSize.y * 0.7f);
                // }
                // else
                // {
                //     rect = new Rect(rect.x, rect.y, rect.width, guiScrollHeight);
                // }

                if (GetAnyMouseButtonDown())
                {
                    Vector2 v2Tmp = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                    if (!rect.Contains(v2Tmp))
                    {
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
