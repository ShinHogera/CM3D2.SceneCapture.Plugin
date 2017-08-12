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
    internal class HalftoneDef
    {
        public static Halftone halftoneEffect;

        public static float scale { get; set; }
        public static float dotSize { get; set; }
        public static float angle { get; set; }
        public static float smoothness { get; set; }
        public static Vector2 center { get; set; }
        public static bool desaturate { get; set; }

        public HalftoneDef()
        {
            if( halftoneEffect == null)
            {
                halftoneEffect = Util.GetComponentVar<Halftone, HalftoneDef>(halftoneEffect);
            }

            scale = 12f;
            dotSize = 1.35f;
            angle = 1.2f;
            smoothness = 0.080f;
            center = new Vector2(0.5f, 0.5f);
            desaturate = false;
        }

        public void InitMemberByInstance(Halftone halftone)
        {
            scale = halftone.scale;
            dotSize = halftone.dotSize;
            angle = halftone.angle;
            smoothness = halftone.smoothness;
            center = halftone.center;
            desaturate = halftone.desaturate;
        }

        public static void Update(HalftonePane halftonePane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                halftonePane.IsEnabled = halftoneEffect.enabled;
            }
            else
            {
                halftoneEffect.enabled = halftonePane.IsEnabled;
            }

            halftoneEffect.scale = halftonePane.ScaleValue;
            halftoneEffect.dotSize = halftonePane.DotSizeValue;
            halftoneEffect.angle = halftonePane.AngleValue;
            halftoneEffect.smoothness = halftonePane.SmoothnessValue;
            halftoneEffect.center = halftonePane.CenterValue;
            halftoneEffect.desaturate = halftonePane.DesaturateValue;
        }

        public static void Reset()
        {
            if( halftoneEffect == null )
                return;

            halftoneEffect.scale = scale;
            halftoneEffect.dotSize = dotSize;
            halftoneEffect.angle = angle;
            halftoneEffect.smoothness = smoothness;
            halftoneEffect.center = center;
            halftoneEffect.desaturate = desaturate;
        }
    }
}

