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
    internal class DynamicLookupDef
    {
        public static DynamicLookup dynamicLookupEffect;

        public static Color white { get; set; }
        public static Color black { get; set; }
        public static Color red { get; set; }
        public static Color green { get; set; }
        public static Color blue { get; set; }
        public static Color yellow { get; set; }
        public static Color magenta { get; set; }
        public static Color cyan { get; set; }
        public static float amount { get; set; }

        static DynamicLookupDef()
        {
            if(dynamicLookupEffect == null)
            {
                dynamicLookupEffect = Util.GetComponentVar<DynamicLookup, DynamicLookupDef>(dynamicLookupEffect);
            }
            white = new Color(1f, 1f, 1f);
            black = new Color(0f, 0f, 0f);
            red = new Color(1f, 0f, 0f);
            green = new Color(0f, 1f, 0f);
            blue = new Color(0f, 0f, 1f);
            yellow = new Color(1f, 1f, 0f);
            magenta = new Color(1f, 0f, 1f);
            cyan = new Color(0f, 1f, 1f);
            amount = 1f;
        }

        public static void InitMemberByInstance(DynamicLookup dl)
        {
            white = dl.white;
            black = dl.black;
            red = dl.red;
            green = dl.green;
            blue = dl.blue;
            yellow = dl.yellow;
            magenta = dl.magenta;
            cyan = dl.cyan;
            amount = dl.amount;
        }

        public static void Update(DynamicLookupPane dynamicLookupPane)
        {
            if (Instances.needEffectWindowReload == true)
                dynamicLookupPane.IsEnabled = dynamicLookupEffect.enabled;
            else
                dynamicLookupEffect.enabled = dynamicLookupPane.IsEnabled;

            dynamicLookupEffect.white = dynamicLookupPane.WhiteValue;
            dynamicLookupEffect.black = dynamicLookupPane.BlackValue;
            dynamicLookupEffect.red = dynamicLookupPane.RedValue;
            dynamicLookupEffect.green = dynamicLookupPane.GreenValue;
            dynamicLookupEffect.blue = dynamicLookupPane.BlueValue;
            dynamicLookupEffect.yellow = dynamicLookupPane.YellowValue;
            dynamicLookupEffect.magenta = dynamicLookupPane.MagentaValue;
            dynamicLookupEffect.cyan = dynamicLookupPane.CyanValue;
            dynamicLookupEffect.amount = dynamicLookupPane.AmountValue;
        }

        public static void Reset()
        {
            if (dynamicLookupEffect == null)
                return;

            dynamicLookupEffect.white = white;
            dynamicLookupEffect.black = black;
            dynamicLookupEffect.red = red;
            dynamicLookupEffect.green = green;
            dynamicLookupEffect.blue = blue;
            dynamicLookupEffect.yellow = yellow;
            dynamicLookupEffect.magenta = magenta;
            dynamicLookupEffect.cyan = cyan;
            dynamicLookupEffect.amount = amount;
        }
    }
}
