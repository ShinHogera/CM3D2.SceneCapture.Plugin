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
    internal class DoubleVisionDef
    {
        public static DoubleVision doubleVisionEffect;

        public static Vector2 displace { get; set; }

        public static float amount { get; set; }

        public DoubleVisionDef()
        {
            if( doubleVisionEffect == null)
            {
                doubleVisionEffect = Util.GetComponentVar<DoubleVision, DoubleVisionDef>(doubleVisionEffect);
            }

            displace = new Vector2(0.7f, 0.0f);
            amount = 1.0f;
        }

        public void InitMemberByInstance(DoubleVision doubleVision)
        {
            displace = doubleVision.displace;
            amount = doubleVision.amount;
        }

        public static void Update(DoubleVisionPane doubleVisionPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                doubleVisionPane.IsEnabled = doubleVisionEffect.enabled;
            }
            else
            {
                doubleVisionEffect.enabled = doubleVisionPane.IsEnabled;
            }

            doubleVisionEffect.displace = doubleVisionPane.DisplaceValue;
            doubleVisionEffect.amount = doubleVisionPane.AmountValue;
        }

        public static void Reset()
        {
            if( doubleVisionEffect == null )
                return;

            doubleVisionEffect.displace = displace;
            doubleVisionEffect.amount = amount;
        }
    }
}

