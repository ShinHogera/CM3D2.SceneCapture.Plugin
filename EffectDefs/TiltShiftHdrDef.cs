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
    internal class TiltShiftHdrDef
    {
        public static TiltShiftHdr tiltShiftHdrEffect;

        // [Range(0, 15)]
        public static float blurArea { get; set; }
        // [Range(0, 25)]
        public static float maxBlurSize { get; set; }
        // [Range(0, 1)]
        public static int downsample { get; set; }

        public static TiltShiftHdr.TiltShiftMode mode { get; set; }
        public static TiltShiftHdr.TiltShiftQuality quality { get; set; }

        static TiltShiftHdrDef()
        {
            if(tiltShiftHdrEffect == null)
            {
                tiltShiftHdrEffect = Util.GetComponentVar<TiltShiftHdr, TiltShiftHdrDef>(tiltShiftHdrEffect);
            }
            mode = TiltShiftHdr.TiltShiftMode.TiltShiftMode;
            quality = TiltShiftHdr.TiltShiftQuality.Normal;
            blurArea = 1f;
            maxBlurSize = 5f;
            downsample = 0;
        }

        public static void InitMemberByInstance(TiltShiftHdr tilt)
        {
            mode = tilt.mode;
            quality = tilt.quality;
            blurArea = tilt.blurArea;
            downsample = tilt.downsample;
            maxBlurSize = tilt.maxBlurSize;
        }

        public static void Update(TiltShiftHdrPane tiltShiftHdrPane)
        {
            if (Instances.needEffectWindowReload == true)
                tiltShiftHdrPane.IsEnabled = tiltShiftHdrEffect.enabled;
            else
                tiltShiftHdrEffect.enabled = tiltShiftHdrPane.IsEnabled;

            tiltShiftHdrEffect.mode = tiltShiftHdrPane.ModeValue;
            tiltShiftHdrEffect.quality = tiltShiftHdrPane.QualityValue;
            tiltShiftHdrEffect.blurArea = tiltShiftHdrPane.BlurAreaValue;
            tiltShiftHdrEffect.downsample = tiltShiftHdrPane.DownsampleValue;
            tiltShiftHdrEffect.maxBlurSize = tiltShiftHdrPane.MaxBlurSizeValue;
        }

        public static void Reset()
        {
            if (tiltShiftHdrEffect == null)
                return;

            tiltShiftHdrEffect.mode = mode;
            tiltShiftHdrEffect.quality = quality;
            tiltShiftHdrEffect.blurArea = blurArea;
            tiltShiftHdrEffect.downsample = downsample;
            tiltShiftHdrEffect.maxBlurSize = maxBlurSize;
        }
    }
}
