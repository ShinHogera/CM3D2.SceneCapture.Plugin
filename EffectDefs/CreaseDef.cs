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
    internal class CreaseDef
    {
        public static Crease creaseEffect;

        public static float intensity { get; set; }
        public static int softness { get; set; }
        public static float spread { get; set; }

        static CreaseDef()
        {
            if(creaseEffect == null)
            {
                creaseEffect = Util.GetComponentVar<Crease, CreaseDef>(creaseEffect);
            }
            intensity = 0.5f;
            softness = 1;
            spread = 1f;
        }


        public static void InitMemberByInstance(Crease c)
        {
            intensity = c.intensity;
            softness = c.softness;
            spread = c.spread;
        }

        public static void Update(CreasePane creasePane)
        {
            if (Instances.needEffectWindowReload == true)
                creasePane.IsEnabled = creaseEffect.enabled;
            else
                creaseEffect.enabled = creasePane.IsEnabled;

            creaseEffect.intensity = creasePane.IntensityValue;
            creaseEffect.softness = creasePane.SoftnessValue;
            creaseEffect.spread = creasePane.SpreadValue;
        }

        public static void Reset()
        {
            if (creaseEffect == null)
                return;

            creaseEffect.intensity = intensity;
            creaseEffect.softness = softness;
            creaseEffect.spread = spread;
        }
    }
}
