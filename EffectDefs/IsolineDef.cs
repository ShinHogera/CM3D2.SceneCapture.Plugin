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
    internal class IsolineDef
    {
        public static Isoline isolineEffect;

        public static Color lineColor { get; set; }
        public static float luminanceBlending { get; set; }
        public static float fallOffDepth { get; set; }
        public static Color backgroundColor { get; set; }
        public static Vector3 axis { get; set; }

        static IsolineDef()
        {
            if(isolineEffect == null)
            {
                isolineEffect = Util.GetComponentVar<Isoline, IsolineDef>(isolineEffect);
            }

            strengthX = 0.05f;
            strengthY = 0.05f;
        }

        public static void InitMemberByInstance(Isoline iso)
        {
            strengthX = fish.strengthX;
            strengthY = fish.strengthY;
        }

        public static void Update(IsolinePane isolinePane)
        {
            if (Instances.needEffectWindowReload == true)
                isolinePane.IsEnabled = isolineEffect.enabled;
            else
                isolineEffect.enabled = isolinePane.IsEnabled;

            isolineEffect.strengthX = isolinePane.StrengthXValue;
            isolineEffect.strengthY = isolinePane.StrengthYValue;
        }

        public static void Reset()
        {
            if (isolineEffect == null)
                return;

            isolineEffect.strengthX = strengthX;
            isolineEffect.strengthY = strengthY;
        }
    }
}
