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
    internal class LensDistortionBlurDef
    {
        public static LensDistortionBlur lensDistortionBlurEffect;

        public static LensDistortionBlur.QualityPreset quality { get; set; }
        public static int samples { get; set; }
        public static float distortion { get; set; }
        public static float cubicDistortion { get; set; }
        public static float scale { get; set; }

        static LensDistortionBlurDef()
        {
            if(lensDistortionBlurEffect == null)
            {
                lensDistortionBlurEffect = Util.GetComponentVar<LensDistortionBlur, LensDistortionBlurDef>(lensDistortionBlurEffect);
            }

            quality = LensDistortionBlur.QualityPreset.Medium;
            samples = 10;
            distortion = 0.2f;
            cubicDistortion = 0.6f;
            scale = 0.8f;
        }

        public static void InitMemberByInstance(LensDistortionBlur blur)
        {
            quality = blur.quality;
            samples = blur.samples;
            distortion = blur.distortion;
            cubicDistortion = blur.cubicDistortion;
            scale = blur.scale;
        }

        public static void Update(LensDistortionBlurPane lensDistortionBlurPane)
        {
            if(lensDistortionBlurEffect == null)
            {
                lensDistortionBlurEffect = Util.GetComponentVar<LensDistortionBlur, LensDistortionBlurDef>(lensDistortionBlurEffect);
            }

            if (Instances.needEffectWindowReload == true)
                lensDistortionBlurPane.IsEnabled = lensDistortionBlurEffect.enabled;
            else
                lensDistortionBlurEffect.enabled = lensDistortionBlurPane.IsEnabled;

            lensDistortionBlurEffect.quality = lensDistortionBlurPane.QualityValue;
            lensDistortionBlurEffect.samples = lensDistortionBlurPane.SamplesValue;
            lensDistortionBlurEffect.distortion = lensDistortionBlurPane.DistortionValue;
            lensDistortionBlurEffect.cubicDistortion = lensDistortionBlurPane.CubicDistortionValue;
            lensDistortionBlurEffect.scale = lensDistortionBlurPane.ScaleValue;
        }

        public static void Reset()
        {
            if (lensDistortionBlurEffect == null)
                return;

            lensDistortionBlurEffect.quality = quality;
            lensDistortionBlurEffect.samples = samples;
            lensDistortionBlurEffect.distortion = distortion;
            lensDistortionBlurEffect.cubicDistortion = cubicDistortion;
            lensDistortionBlurEffect.scale = scale;
        }
    }
}
