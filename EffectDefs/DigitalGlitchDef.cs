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
    internal class DigitalGlitchDef
    {
        public static DigitalGlitch digitalGlitchEffect;

        public static float intensity { get; set; }

        static DigitalGlitchDef()
        {
            if(digitalGlitchEffect == null)
            {
                digitalGlitchEffect = Util.GetComponentVar<DigitalGlitch, DigitalGlitchDef>(digitalGlitchEffect);
            }

            intensity = 0;
        }

        public static void InitMemberByInstance(DigitalGlitch d)
        {
            intensity = d.intensity;
        }

        public static void Update(DigitalGlitchPane digitalGlitchPane)
        {
            if (Instances.needEffectWindowReload == true)
                digitalGlitchPane.IsEnabled = digitalGlitchEffect.enabled;
            else
                digitalGlitchEffect.enabled = digitalGlitchPane.IsEnabled;

            digitalGlitchEffect.intensity = digitalGlitchPane.IntensityValue;
        }

        public static void Reset()
        {
            if (digitalGlitchEffect == null)
                return;

            digitalGlitchEffect.intensity = intensity;
        }
    }
}
