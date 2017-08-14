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
    internal class DitheringDef
    {
        public static Dithering ditheringEffect;

        public static bool showOriginal { get; set; }
        public static bool convertToGrayscale { get; set; }
        public static float redLuminance { get; set; }
        public static float greenLuminance { get; set; }
        public static float blueLuminance { get; set; }
        public static float amount { get; set; }

        public DitheringDef()
        {
            if( ditheringEffect == null)
            {
                ditheringEffect = Util.GetComponentVar<Dithering, DitheringDef>(ditheringEffect);
            }

            showOriginal = false;
            convertToGrayscale = false;
            redLuminance = 0.299f;
            greenLuminance = 0.587f;
            blueLuminance = 0.114f;
            amount = 1f;
        }

        public void InitMemberByInstance(Dithering dithering)
        {
            showOriginal = dithering.showOriginal;
            convertToGrayscale = dithering.convertToGrayscale;
            redLuminance = dithering.redLuminance;
            greenLuminance = dithering.greenLuminance;
            blueLuminance = dithering.blueLuminance;
            amount = dithering.amount;
        }

        public static void InitExtra(Dithering dithering)
        {
            Texture2D ditherTex = new Texture2D(64, 64);
            ditherTex.LoadImage(ConstantValues.ditheringPng);
            ditherTex.Apply();
            dithering.m_DitherPattern = ditherTex;
        }

        public static void Update(DitheringPane ditheringPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                ditheringPane.IsEnabled = ditheringEffect.enabled;
            }
            else
            {
                ditheringEffect.enabled = ditheringPane.IsEnabled;
            }

            ditheringEffect.showOriginal = ditheringPane.ShowOriginalValue;
            ditheringEffect.convertToGrayscale = ditheringPane.ConvertToGrayscaleValue;
            ditheringEffect.redLuminance = ditheringPane.RedLuminanceValue;
            ditheringEffect.greenLuminance = ditheringPane.GreenLuminanceValue;
            ditheringEffect.blueLuminance = ditheringPane.BlueLuminanceValue;
            ditheringEffect.amount = ditheringPane.AmountValue;
        }

        public static void Reset()
        {
            if( ditheringEffect == null )
                return;

            ditheringEffect.showOriginal = showOriginal;
            ditheringEffect.convertToGrayscale = convertToGrayscale;
            ditheringEffect.redLuminance = redLuminance;
            ditheringEffect.greenLuminance = greenLuminance;
            ditheringEffect.blueLuminance = blueLuminance;
            ditheringEffect.amount = amount;
        }
    }
}

