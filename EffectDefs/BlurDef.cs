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
    internal class BlurDef
    {
        public static Blur blurEffect;

        // [Range(0, 2)]
        public static int downsample { get; set; }
        // [Range(0, 10)]
        public static float blurSize { get; set; }
        // [Range(1, 4)]
        public static int blurIterations { get; set; }

        static BlurDef()
        {
            if(blurEffect == null)
            {
                blurEffect = Util.GetComponentVar<Blur, BlurDef>(blurEffect);
            }

            //blurType = Blur.BlurType.StandardGauss;
            downsample = 1;
            blurSize = 3f;
            blurIterations = 2;
        }

        public static void InitMemberByInstance(Blur blur)
        {
            if(blurEffect == null)
            {
                blurEffect = Util.GetComponentVar<Blur, BlurDef>(blurEffect);
            }

            blurIterations = blur.blurIterations;
            blurSize = blur.blurSize;
            downsample = blur.downsample;
        }

        public static void Update(BlurPane blurPane)
        {
            if (Instances.needEffectWindowReload == true)
                blurPane.IsEnabled = blurEffect.enabled;
            else
                blurEffect.enabled = blurPane.IsEnabled;

            blurEffect.blurIterations = blurPane.BlurIterationsValue;
            blurEffect.blurSize = blurPane.BlurSizeValue;
            blurEffect.downsample = blurPane.DownsampleValue;
        }

        public static void Reset()
        {
            if (blurEffect == null)
                return;

            //blurEffect.blurType = blurType;
            blurEffect.blurIterations = blurIterations;
            blurEffect.blurSize = blurSize;
            blurEffect.downsample = downsample;
        }
    }
}
