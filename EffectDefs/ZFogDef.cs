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
    internal class ZFogDef
    {
        public static ZFog zFogEffect;

        public static Color color1 { get; set; }
        public static float hdr1 { get; set; }
        public static float near1 { get; set; }
        public static float far1 { get; set; }
        public static float pow1 { get; set; }
        public static Color color2 { get; set; }
        public static float hdr2 { get; set; }
        public static float near2 { get; set; }
        public static float far2 { get; set; }
        public static float pow2 { get; set; }

        public ZFogDef()
        {
            if( zFogEffect == null)
            {
                zFogEffect = Util.GetComponentVar<ZFog, ZFogDef>(zFogEffect);
            }

            color1 = new Color(0.75f, 0.75f, 1.0f, 1.0f);
            hdr1 = 1.0f;
            near1 = 10.0f;
            far1 = 30.0f;
            pow1 = 1.0f;
            color2 = new Color(0.75f, 0.75f, 1.0f, 1.0f);
            hdr2 = 1.0f;
            near2 = 10.0f;
            far2 = 30.0f;
            pow2 = 1.0f;
        }

        public void InitMemberByInstance(ZFog zFog)
        {
            color1 = zFog.color1;
            hdr1 = zFog.hdr1;
            near1 = zFog.near1;
            far1 = zFog.far1;
            pow1 = zFog.pow1;
            color2 = zFog.color2;
            hdr2 = zFog.hdr2;
            near2 = zFog.near2;
            far2 = zFog.far2;
            pow2 = zFog.pow2;
        }

        public static void Update(ZFogPane zFogPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                zFogPane.IsEnabled = zFogEffect.enabled;
            }
            else
            {
                zFogEffect.enabled = zFogPane.IsEnabled;
            }

            zFogEffect.color1 = zFogPane.Color1Value;
            zFogEffect.hdr1 = zFogPane.Hdr1Value;
            zFogEffect.near1 = zFogPane.Near1Value;
            zFogEffect.far1 = zFogPane.Far1Value;
            zFogEffect.pow1 = zFogPane.Pow1Value;
            zFogEffect.color2 = zFogPane.Color2Value;
            zFogEffect.hdr2 = zFogPane.Hdr2Value;
            zFogEffect.near2 = zFogPane.Near2Value;
            zFogEffect.far2 = zFogPane.Far2Value;
            zFogEffect.pow2 = zFogPane.Pow2Value;
        }

        public static void Reset()
        {
            if( zFogEffect == null )
                return;

            zFogEffect.color1 = color1;
            zFogEffect.hdr1 = hdr1;
            zFogEffect.near1 = near1;
            zFogEffect.far1 = far1;
            zFogEffect.pow1 = pow1;
            zFogEffect.color2 = color2;
            zFogEffect.hdr2 = hdr2;
            zFogEffect.near2 = near2;
            zFogEffect.far2 = far2;
            zFogEffect.pow2 = pow2;
        }
    }
}

