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
    internal class VignettingDef
    {
        public static Vignetting vignettingEffect;

        public static float intensity { get; set; }
        public static float blur { get; set; }
        public static float blurSpread { get; set; }

        public static float luminanceDependency { get; set; }
        public static Vignetting.AberrationMode mode { get; set; }
        public static float chromaticAberration { get; set; }
        public static float axialAberration { get; set; }
        public static float blurDistance { get; set; }

        static VignettingDef()
        {
            if(vignettingEffect == null)
            {
                vignettingEffect = Util.GetComponentVar<Vignetting, VignettingDef>(vignettingEffect);
            }

            mode = Vignetting.AberrationMode.Simple;
            intensity = 0f;
            blur = 0.82f;
            blurSpread = 4.19f;
            luminanceDependency = 0.494f;
            chromaticAberration = -0.75f;
            axialAberration = 1.18f;
            blurDistance = 1.71f;
        }

        public static void InitMemberByInstance(Vignetting vignet)
        {
            mode = vignet.mode;
            intensity = vignet.intensity;
            blur = vignet.blur;
            blurSpread = vignet.blurSpread;
            luminanceDependency = vignet.luminanceDependency;
            chromaticAberration = vignet.chromaticAberration;
            axialAberration = vignet.axialAberration;
            blurDistance = vignet.blurDistance;
        }

        public static void Update(VignettingPane vignettingPane)
        {
            if(vignettingEffect == null)
            {
                vignettingEffect = Util.GetComponentVar<Vignetting, VignettingDef>(vignettingEffect);
            }

            if (Instances.needEffectWindowReload == true)
                vignettingPane.IsEnabled = vignettingEffect.enabled;
            else
                vignettingEffect.enabled = vignettingPane.IsEnabled;

            vignettingEffect.mode = vignettingPane.ModeValue;
            vignettingEffect.intensity = vignettingPane.IntensityValue;
            vignettingEffect.blur = vignettingPane.BlurValue;
            vignettingEffect.blurSpread = vignettingPane.BlurSpreadValue;
            vignettingEffect.luminanceDependency = vignettingPane.LuminanceDependencyValue;
            vignettingEffect.chromaticAberration = vignettingPane.ChromaticAberrationValue;
            vignettingEffect.axialAberration = vignettingPane.AxialAberrationValue;
            vignettingEffect.blurDistance = vignettingPane.BlurDistanceValue;

        }

        public static void Reset()
        {
            if (vignettingEffect == null)
                return;

            vignettingEffect.mode = mode;
            vignettingEffect.intensity = intensity;
            vignettingEffect.blur = blur;
            vignettingEffect.blurSpread = blurSpread;
            vignettingEffect.luminanceDependency = luminanceDependency;
            vignettingEffect.chromaticAberration = chromaticAberration;
            vignettingEffect.axialAberration = axialAberration;
            vignettingEffect.blurDistance = blurDistance;
        }
    }
}
