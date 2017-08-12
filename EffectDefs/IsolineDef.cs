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

        public IsolineDef()
        {
            if( isolineEffect == null)
            {
                isolineEffect = Util.GetComponentVar<Isoline, IsolineDef>(isolineEffect);
            }

            lineColor = new Color(1, 1, 1);
            luminanceBlending = 1f;
            fallOffDepth = 0f;
            backgroundColor = new Color(1, 1, 1);
            axis = new Vector3(1, 1, 1);
        }

        public void InitMemberByInstance(Isoline isoline)
        {
            lineColor = isoline.lineColor;
            luminanceBlending = isoline.luminanceBlending;
            fallOffDepth = isoline.fallOffDepth;
            backgroundColor = isoline.backgroundColor;
            axis = isoline.axis;
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
        
            isolineEffect.lineColor = isolinePane.LineColorValue;
            isolineEffect.luminanceBlending = isolinePane.LuminanceBlendingValue;
            isolineEffect.fallOffDepth = isolinePane.FallOffDepthValue;
            isolineEffect.backgroundColor = isolinePane.BackgroundColorValue;
            isolineEffect.axis = isolinePane.AxisValue;
        }

        public static void Reset()
        {
            if( isolineEffect == null )
                return;
        
            isolineEffect.lineColor = lineColor;
            isolineEffect.luminanceBlending = luminanceBlending;
            isolineEffect.fallOffDepth = fallOffDepth;
            isolineEffect.backgroundColor = backgroundColor;
            isolineEffect.axis = axis;
        }
    }
}

