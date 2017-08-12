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
    internal class KuwaharaDef
    {
        public static Kuwahara kuwaharaEffect;

        public static int radius { get; set; }

        public KuwaharaDef()
        {
            if( kuwaharaEffect == null)
            {
                kuwaharaEffect = Util.GetComponentVar<Kuwahara, KuwaharaDef>(kuwaharaEffect);
            }

            radius = 3;
        }

        public void InitMemberByInstance(Kuwahara kuwahara)
        {
            radius = kuwahara.radius;
        }

        public static void Update(KuwaharaPane kuwaharaPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                kuwaharaPane.IsEnabled = kuwaharaEffect.enabled;
            }
            else
            {
                kuwaharaEffect.enabled = kuwaharaPane.IsEnabled;
            }

            kuwaharaEffect.radius = kuwaharaPane.RadiusValue;
        }

        public static void Reset()
        {
            if( kuwaharaEffect == null )
                return;

            kuwaharaEffect.radius = radius;
        }
    }
}

