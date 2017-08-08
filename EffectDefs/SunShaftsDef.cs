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
    internal class SunShaftsDef
    {
        public static SunShafts sunShaftsEffect;

        public static SunShaftsResolution resolution { get; set; }
        public static ShaftsScreenBlendMode screenBlendMode { get; set; }
        public static bool useDepthTexture { get; set; }

        public static Transform sunTransform { get; set; }
        public static Color sunColor { get; set; }

        public static float maxRadius { get; set; }
        public static int radialBlurIterations { get; set; }

        public static float sunShaftBlurRadius { get; set; }
        public static float sunShaftIntensity { get; set; }

        public static float useSkyBoxAlpha { get; set; }

        //
        public static Texture2D texSunColor { get; set; }
        public static DragManager drag { get; set; }
        public static bool isDrag { get; set; }
        //

        static SunShaftsDef()
        {
            if(sunShaftsEffect == null)
            {
                sunShaftsEffect = Util.GetComponentVar<SunShafts, SunShaftsDef>(sunShaftsEffect);
            }

            useDepthTexture = false;
            resolution = SunShaftsResolution.Normal;
            screenBlendMode = ShaftsScreenBlendMode.Screen;

            sunColor = Color.white;

            maxRadius = 0.75f;
            radialBlurIterations = 2;

            sunShaftBlurRadius = 2.5f;
            sunShaftIntensity = 1.15f;

            useSkyBoxAlpha = 0.75f;

            texSunColor = new Texture2D(1, 1);
            texSunColor.SetPixel(0, 0, Color.white);
            texSunColor.Apply();

            drag = new DragManager();
            sunTransform = drag.goDrag.transform;
        }

        public static void InitMemberByInstance(SunShafts sunShafts)
        {
            useDepthTexture = false;
            resolution = sunShafts.resolution;
            screenBlendMode = sunShafts.screenBlendMode;

            sunTransform = sunShafts.sunTransform;
            sunColor = sunShafts.sunColor;

            maxRadius = sunShafts.maxRadius;
            radialBlurIterations = sunShafts.radialBlurIterations;

            sunShaftBlurRadius = sunShafts.sunShaftBlurRadius;
            sunShaftIntensity = sunShafts.sunShaftIntensity;

            useSkyBoxAlpha = sunShafts.useSkyBoxAlpha;

            texSunColor.SetPixel(0, 0, sunColor);
            texSunColor.Apply();
        }

        public static void Update(SunShaftsPane sunShaftsPane)
        {
            if(sunShaftsEffect == null)
            {
                sunShaftsEffect = Util.GetComponentVar<SunShafts, SunShaftsDef>(sunShaftsEffect);
            }

            if (Instances.needEffectWindowReload == true)
                sunShaftsPane.IsEnabled = sunShaftsEffect.enabled;
            else
                sunShaftsEffect.enabled = sunShaftsPane.IsEnabled;

            sunShaftsEffect.resolution = sunShaftsPane.ResolutionValue;
            sunShaftsEffect.screenBlendMode = sunShaftsPane.ScreenBlendModeValue;

            sunShaftsEffect.sunTransform = drag.goDrag.transform;
            sunShaftsEffect.sunColor = sunShaftsPane.SunColorValue;

            sunShaftsEffect.maxRadius = sunShaftsPane.MaxRadiusValue;
            sunShaftsEffect.radialBlurIterations = sunShaftsPane.RadialBlurIterationsValue;

            sunShaftsEffect.sunShaftBlurRadius = sunShaftsPane.SunShaftBlurRadiusValue;
            sunShaftsEffect.sunShaftIntensity = sunShaftsPane.SunShaftIntensityValue;

            sunShaftsEffect.useSkyBoxAlpha = sunShaftsPane.UseSkyBoxAlphaValue;
        }

        public static void Reset()
        {
            if (sunShaftsEffect == null)
                return;

            sunShaftsEffect.useDepthTexture = false;
            sunShaftsEffect.resolution = resolution;
            sunShaftsEffect.screenBlendMode = screenBlendMode;

            //sunShaftsEffect.sunTransform = sunTransform;
            sunShaftsEffect.sunColor = sunColor;

            sunShaftsEffect.maxRadius = maxRadius;
            sunShaftsEffect.radialBlurIterations = radialBlurIterations;

            sunShaftsEffect.sunShaftBlurRadius = sunShaftBlurRadius;
            sunShaftsEffect.sunShaftIntensity = sunShaftIntensity;

            sunShaftsEffect.useSkyBoxAlpha = useSkyBoxAlpha;

            OnLoad();
        }


        public static void OnLoad()
        {
            if (sunShaftsEffect == null)
                return;

            texSunColor.SetPixel(0, 0, sunShaftsEffect.sunColor);
            texSunColor.Apply();

            sunShaftsEffect.useDepthTexture = false;
            sunShaftsEffect.sunTransform = sunTransform;
        }

        public static void InitExtra(SunShafts sunshafts)
        {
            sunshafts.useDepthTexture = false;
            sunshafts.sunTransform = sunTransform;
        }

        public static void StartDrag()
        {
            SunShaftsDef.isDrag = true;
            drag.StartDrag();
        }

        public static void StoptDrag()
        {
            SunShaftsDef.isDrag = false;
            drag.StopDrag();
        }


        public static void Update()
        {
            if (!SunShaftsDef.isDrag)
                return;
        }
    }
}
