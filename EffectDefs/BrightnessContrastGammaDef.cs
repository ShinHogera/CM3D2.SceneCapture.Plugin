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
    internal class BrightnessContrastGammaDef
    {
        public static BrightnessContrastGamma brightnessContrastGammaEffect;

        public static float brightness { get; set; }
        public static float contrast { get; set; }
        public static Vector3 contrastCoeff { get; set; }
        public static float gamma { get; set; }

        public BrightnessContrastGammaDef()
        {
            if( brightnessContrastGammaEffect == null)
            {
                brightnessContrastGammaEffect = Util.GetComponentVar<BrightnessContrastGamma, BrightnessContrastGammaDef>(brightnessContrastGammaEffect);
            }

            brightness = 0f;
            contrast = 0f;
            contrastCoeff = new Vector3(0.5f, 0.5f, 0.5f);
            gamma = 1f;
        }

        public void InitMemberByInstance(BrightnessContrastGamma brightnessContrastGamma)
        {
            brightness = brightnessContrastGamma.brightness;
            contrast = brightnessContrastGamma.contrast;
            contrastCoeff = brightnessContrastGamma.contrastCoeff;
            gamma = brightnessContrastGamma.gamma;
        }

        public static void Update(BrightnessContrastGammaPane brightnessContrastGammaPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                brightnessContrastGammaPane.IsEnabled = brightnessContrastGammaEffect.enabled;
            }
            else
            {
                brightnessContrastGammaEffect.enabled = brightnessContrastGammaPane.IsEnabled;
            }

            brightnessContrastGammaEffect.brightness = brightnessContrastGammaPane.BrightnessValue;
            brightnessContrastGammaEffect.contrast = brightnessContrastGammaPane.ContrastValue;
            brightnessContrastGammaEffect.contrastCoeff = brightnessContrastGammaPane.ContrastCoeffValue;
            brightnessContrastGammaEffect.gamma = brightnessContrastGammaPane.GammaValue;
        }

        public static void Reset()
        {
            if( brightnessContrastGammaEffect == null )
                return;

            brightnessContrastGammaEffect.brightness = brightness;
            brightnessContrastGammaEffect.contrast = contrast;
            brightnessContrastGammaEffect.contrastCoeff = contrastCoeff;
            brightnessContrastGammaEffect.gamma = gamma;
        }
    }
}

