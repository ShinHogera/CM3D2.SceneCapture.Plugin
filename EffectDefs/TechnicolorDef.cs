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
    internal class TechnicolorDef
    {
        public static Technicolor technicolorEffect;

        public static float exposure { get; set; }
        public static Vector3 balance { get; set; }
        public static float amount { get; set; }

        static TechnicolorDef()
        {
            if(technicolorEffect == null)
            {
                technicolorEffect = Util.GetComponentVar<Technicolor, TechnicolorDef>(technicolorEffect);
            }

            exposure = 4f;
	    balance = new Vector3(0.25f, 0.25f, 0.25f);
            amount = 0.5f;
        }

        public static void InitMemberByInstance(Technicolor t)
        {
            exposure = t.exposure;
            balance = t.balance;
            amount = t.amount;
        }

        public static void Update(TechnicolorPane technicolorPane)
        {
            if (Instances.needEffectWindowReload == true)
                technicolorPane.IsEnabled = technicolorEffect.enabled;
            else
                technicolorEffect.enabled = technicolorPane.IsEnabled;

            technicolorEffect.exposure = technicolorPane.ExposureValue;
            technicolorEffect.balance = technicolorPane.BalanceValue;
            technicolorEffect.amount = technicolorPane.AmountValue;
        }

        public static void Reset()
        {
            if (technicolorEffect == null)
                return;

            technicolorEffect.exposure = exposure;
            technicolorEffect.balance = balance;
            technicolorEffect.amount = amount;
        }
    }
}
