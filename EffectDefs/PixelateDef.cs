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
    internal class PixelateDef
    {
        public static Pixelate pixelateEffect;

        public static float scale { get; set; }
        public static bool automaticRatio { get; set; }
        public static float ratio { get; set; }
        public static Pixelate.SizeMode mode { get; set; }

        public PixelateDef()
        {
            if( pixelateEffect == null)
            {
                pixelateEffect = Util.GetComponentVar<Pixelate, PixelateDef>(pixelateEffect);
            }

            scale = 80.0f;
            automaticRatio = true;
            ratio = 1.0f;
            mode = Pixelate.SizeMode.ResolutionIndependent;
        }

        public void InitMemberByInstance(Pixelate pixelate)
        {
            scale = pixelate.scale;
            automaticRatio = pixelate.automaticRatio;
            ratio = pixelate.ratio;
            mode = pixelate.mode;
        }

        public static void Update(PixelatePane pixelatePane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                pixelatePane.IsEnabled = pixelateEffect.enabled;
            }
            else
            {
                pixelateEffect.enabled = pixelatePane.IsEnabled;
            }

            pixelateEffect.scale = pixelatePane.ScaleValue;
            pixelateEffect.automaticRatio = pixelatePane.AutomaticRatioValue;
            pixelateEffect.ratio = pixelatePane.RatioValue;
            pixelateEffect.mode = pixelatePane.ModeValue;
        }

        public static void Reset()
        {
            if( pixelateEffect == null )
                return;

            pixelateEffect.scale = scale;
            pixelateEffect.automaticRatio = automaticRatio;
            pixelateEffect.ratio = ratio;
            pixelateEffect.mode = mode;
        }
    }
}

