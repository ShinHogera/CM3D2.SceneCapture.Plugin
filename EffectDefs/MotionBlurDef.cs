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
    internal class MotionBlurDef
    {
        public static MotionBlur motionBlurEffect;

        public static bool extraBlur { get; set; }
        public static float blurAmount { get; set; }

        static MotionBlurDef()
        {
            if(motionBlurEffect == null)
            {
                motionBlurEffect = Util.GetComponentVar<MotionBlur, MotionBlurDef>(motionBlurEffect);
            }

            extraBlur = false;
            blurAmount = 0.8f;
        }

        public static void InitMemberByInstance(MotionBlur mb)
        {
            extraBlur = mb.extraBlur;
            blurAmount = mb.blurAmount;
        }

        public static void Update(MotionBlurPane motionBlurPane)
        {
            if (Instances.needEffectWindowReload == true)
                motionBlurPane.IsEnabled = motionBlurEffect.enabled;
            else
                motionBlurEffect.enabled = motionBlurPane.IsEnabled;

            motionBlurEffect.extraBlur = motionBlurPane.ExtraBlurValue;
            motionBlurEffect.blurAmount = motionBlurPane.BlurAmountValue;
        }

        public static void Reset()
        {
            if (motionBlurEffect == null)
                return;

            motionBlurEffect.extraBlur = extraBlur;
            motionBlurEffect.blurAmount = blurAmount;
        }
    }
}
