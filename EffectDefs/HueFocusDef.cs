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
    internal class HueFocusDef
    {
        public static HueFocus hueFocusEffect;

        public static float hue { get; set; }
        public static float range { get; set; }
        public static float boost { get; set; }
        public static float amount { get; set; }

        static HueFocusDef()
        {
            if(hueFocusEffect == null)
            {
                hueFocusEffect = Util.GetComponentVar<HueFocus, HueFocusDef>(hueFocusEffect);
            }
            hue = 0f;
            range = 30f;
            boost = 0.5f;
            amount = 1f;
        }

        public static void InitMemberByInstance(HueFocus hf)
        {
            hue = hf.hue;
            range = hf.range;
            boost = hf.boost;
            amount = hf.amount;
        }

        public static void Update(HueFocusPane hueFocusPane)
        {
            if (Instances.needEffectWindowReload == true)
                hueFocusPane.IsEnabled = hueFocusEffect.enabled;
            else
                hueFocusEffect.enabled = hueFocusPane.IsEnabled;

            hueFocusEffect.hue = hueFocusPane.HueValue;
            hueFocusEffect.range = hueFocusPane.RangeValue;
            hueFocusEffect.boost = hueFocusPane.BoostValue;
            hueFocusEffect.amount = hueFocusPane.AmountValue;
        }

        public static void Reset()
        {
            if (hueFocusEffect == null)
                return;

            hueFocusEffect.hue = hue;
            hueFocusEffect.range = range;
            hueFocusEffect.boost = boost;
            hueFocusEffect.amount = amount;
        }
    }
}
