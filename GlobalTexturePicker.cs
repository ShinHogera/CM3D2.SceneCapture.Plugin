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
            private bool firstSet = false;

            private List<ImageInfo> imageFiles;
            private Texture2D currentTexture;

            public TexturePickerWindow(int iWIndowID)
            {
                WINDOW_ID = iWIndowID;

                imageFiles = new List<ImageInfo>();
            }

            public void ChangeImage(ImageInfo info)
            {
                if( !imageFiles.Any() )
                    return;

                currentTexture = info.tex;
                func(currentTexture, info.abbrevPath);
            }

            private static Texture2D ReadTexture(string fullPath)
            {
                byte[] bytes = File.ReadAllBytes(fullPath);
                Texture2D tex = new Texture2D(4, 4);
                tex.LoadImage(bytes);
                tex.name = fullPath;
                return tex;
            }

            private void UpdateImages(List<string> imageDirectories)
            {
                imageFiles = new List<ImageInfo>();

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
                    FileInfo[] files = info.GetFiles("*.png", SearchOption.AllDirectories).OrderBy(p => p.FullName).ToArray();
                    foreach(FileInfo file in files)
                    {
                        string abbrevPath = file.FullName.Replace(ConstantValues.BaseConfigDir + @"\", "");
                        abbrevPath = abbrevPath.Replace(ConstantValues.BaseConfigDirSybaris + @"\", "");
                        ImageInfo imageInfo = new ImageInfo()
                            {
                                fullPath = file.FullName,
                                abbrevPath = abbrevPath,
                                tex = ReadTexture(file.FullName),
                            };
                        imageFiles.Add(imageInfo);
                    }
                }
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
                gsButton.alignment = TextAnchor.MiddleLeft;

                fMargin = iFontSize * 0.3f;
                scrollPosition = new Vector2(0, 0);

                func = f;

                show = true;

                this.UpdateImages(imageDirectories);

                if( this.firstSet && this.imageFiles.Count > 0 )
                {
                    this.ChangeImage(this.imageFiles[0]);
                    this.firstSet = false;
                }
            }

            private void fileButton(ImageInfo info, ref Rect rectItem)
            {
                rectItem.width = gsButton.fontSize * 2f;
                rectItem.height = rectItem.width;
                rectItem.x = gsButton.fontSize * 0.5f;
                GUI.DrawTexture( rectItem, info.tex );

                rectItem.x += rectItem.width + fMargin;
                rectItem.width = rect.width - rectItem.width - fMargin * 9;
                if( GUI.Button(rectItem, info.abbrevPath, gsButton) )
                {
                    this.ChangeImage(info);
                }
                rectItem.y += rectItem.height;
            }

            public void GuiFunc(int winId)
            {
                float windowHeight = this.rect.height;
                if( this.rect.height > Screen.height * 0.7f )
                {
                    windowHeight = Screen.height * 0.7f;
                }

                int iFontSize = gsLabel.fontSize;

                Rect rectScroll = new Rect(0, fMargin * 2, this.rect.width - fMargin, windowHeight - fMargin * 4);
                Rect rectScrollView = new Rect(0, 0, this.rect.width - 7 * fMargin, guiScrollHeight);

                scrollPosition = GUI.BeginScrollView(rectScroll, scrollPosition, rectScrollView, false, true);

                Rect rectItem = new Rect(iFontSize * 0.5f, iFontSize * 0.5f, iFontSize * 1.5f, iFontSize * 1.5f);
                if( this.currentTexture != null )
                    GUI.DrawTexture( rectItem, this.currentTexture );

                int i = 0;
                foreach(var imageFilePair in imageFiles)
                {
                    this.fileButton(imageFilePair, ref rectItem);
                    i += 1;
                }
                GUI.EndScrollView();

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
                    for (int j = 0; i < 3; i++)
                    {
                        m |= Input.GetMouseButtonDown(j);
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

            internal class ImageInfo
            {
                public ImageInfo() {}

                public string fullPath;
                public string abbrevPath;
                public Texture2D tex;
            }
        }
    }
}
