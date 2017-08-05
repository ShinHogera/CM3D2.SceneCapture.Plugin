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
    internal class ContrastDef
    {
        public static ContrastEnhance contrastEffect;

        public static float intensity { get; set; }
        public static float threshhold { get; set; }
        public static float blurSpread { get; set; }

        static ContrastDef()
        {
            if(contrastEffect == null)
            {
                contrastEffect = Util.GetComponentVar<ContrastEnhance, ContrastDef>(contrastEffect);
            }

            intensity = 0.5f;
            threshhold = 0f;
            blurSpread = 1f;
        }

        public static void InitMemberByInstance(ContrastEnhance c)
        {
            intensity = c.intensity;
            threshhold = c.threshhold;
            blurSpread = c.blurSpread;
        }

        public static void Update(ContrastPane contrastPane)
        {
            if (Instances.needEffectWindowReload == true)
                contrastPane.IsEnabled = contrastEffect.enabled;
            else
                contrastEffect.enabled = contrastPane.IsEnabled;

            contrastEffect.intensity = contrastPane.IntensityValue;
            contrastEffect.threshhold = contrastPane.ThreshholdValue;
            contrastEffect.blurSpread = contrastPane.BlurSpreadValue;
        }

        public static void Reset()
        {
            if (contrastEffect == null)
                return;

            contrastEffect.intensity = intensity;
            contrastEffect.threshhold = threshhold;
            contrastEffect.blurSpread = blurSpread;
        }

        public static void OnLoad()
        {

        }
    }
}
