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
      
        public static float dood { get; set; }      
        public static Light.LightType lightType { get; set; }      
        public static bool isWorking { get; set; }      
        public static Color backgroundColor { get; set; }
        public IsolineDef()
        {
            if( isolineEffect == null)
            {
                isolineEffect = Util.GetComponentVar<Isoline, IsolineDef>(isolineEffect);
            }

            dood = 0f;
            lightType = Light.LightType.Directional;
            isWorking = false;
            backgroundColor = new Color(1, 1, 1);
        }

        public void InitMemberByInstance(Isoline isoline)
        {
            dood = isoline.dood;
            lightType = isoline.lightType;
            isWorking = isoline.isWorking;
            backgroundColor = isoline.backgroundColor;
        }

        public static void Update(IsolinePane isolinePane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                isolinePane.IsEnabled = isolineEffect.enabled;
            }
            else
            {
                isolineEffect.enabled = isolinePane.IsEnabled;
            }
        
            isolineEffect.dood = isolinePane.DoodValue;
            isolineEffect.lightType = isolinePane.LightTypeValue;
            isolineEffect.isWorking = isolinePane.IsWorkingValue;
            isolineEffect.backgroundColor = isolinePane.BackgroundColorValue;
        }

        public static void Reset()
        {
            if( isolineEffect == null )
                return;
        
            isolineEffect.dood = dood;
            isolineEffect.lightType = lightType;
            isolineEffect.isWorking = isWorking;
            isolineEffect.backgroundColor = backgroundColor;
        }
    }
}

