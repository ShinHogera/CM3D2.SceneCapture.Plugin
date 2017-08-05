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
    internal class GlobalFogDef
    {
        public static GlobalFog globalFogEffect;

        public static GlobalFog.FogMode fogMode { get; set; }
        public static float globalDensity { get; set; }
        public static float height { get; set; }
        public static float heightScale { get; set; }
        public static float startDistance { get; set; }
        public static Color globalFogColor { get; set; }

        //
        public static Texture2D texGlobalFogColor { get; set; }
        public static bool AdjustHeightScale { get; set; }
        public static bool AdjustStartDistance { get; set; }
        //

        static GlobalFogDef()
        {
            fogMode = GlobalFog.FogMode.AbsoluteYAndDistance;
            globalDensity = 1f;
            height = 0f;
            heightScale = 100f;
            startDistance = 200f;
            globalFogColor = new Color(0.5f, 0.5f, 0.5f, 1f);

            texGlobalFogColor = new Texture2D(1, 1);
            texGlobalFogColor.SetPixel(0, 0, globalFogColor);
            texGlobalFogColor.Apply();
        }

        public static void InitMemberByInstance(GlobalFog fog)
        {
            fogMode = fog.fogMode;
            globalDensity = fog.globalDensity;
            height = fog.height;
            heightScale = fog.heightScale;
            startDistance = fog.startDistance;
            globalFogColor = fog.globalFogColor;

            texGlobalFogColor.SetPixel(0, 0, globalFogColor);
            texGlobalFogColor.Apply();
        }

        public static void Setup()
        {
            try
            {
                globalFogEffect = Util.GetComponentVar<GlobalFog, GlobalFogDef>(globalFogEffect);
            }
            catch (Exception e) {
                Debug.Log("Can't set fog!");
                Debug.LogError( e );
            }
        }

        public static void Update(GlobalFogPane globalFogPane)
        {
            if(globalFogEffect == null)
            {
                Setup();
            }

            if (Instances.needEffectWindowReload == true)
                globalFogPane.IsEnabled = globalFogEffect.enabled;
            else
                globalFogEffect.enabled = globalFogPane.IsEnabled;

            globalFogEffect.fogMode = globalFogPane.FogModeValue;
            globalFogEffect.globalDensity = globalFogPane.GlobalDensityValue;
            globalFogEffect.height = globalFogPane.HeightValue;
            globalFogEffect.heightScale = globalFogPane.HeightScaleValue;
            globalFogEffect.startDistance = globalFogPane.StartDistanceValue;
            globalFogEffect.globalFogColor = globalFogPane.GlobalFogColorValue;
        }

        public static void Reset()
        {
            if (globalFogEffect == null)
                return;

            globalFogEffect.fogMode = fogMode;
            globalFogEffect.globalDensity = globalDensity;
            globalFogEffect.height = height;
            globalFogEffect.heightScale = heightScale;
            globalFogEffect.startDistance = startDistance;
            globalFogEffect.globalFogColor = globalFogColor;

            OnLoad();
        }

        public static void OnLoad()
        {
            if (globalFogEffect == null)
                return;

            texGlobalFogColor.SetPixel(0, 0, globalFogEffect.globalFogColor);
            texGlobalFogColor.Apply();
        }
    }
}
