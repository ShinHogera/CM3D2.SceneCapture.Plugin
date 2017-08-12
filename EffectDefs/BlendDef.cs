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
    internal class BlendDef
    {
        public static Blend blendEffect;

        public static float amount { get; set; }
        public static Blend.BlendingMode mode { get; set; }

        public BlendDef()
        {
            if( blendEffect == null)
            {
                blendEffect = Util.GetComponentVar<Blend, BlendDef>(blendEffect);
            }

            amount = 1f;
            mode = Blend.BlendingMode.Darken;
        }

        public void InitMemberByInstance(Blend blend)
        {
            amount = blend.amount;
            mode = blend.mode;
        }

        public static void Update(BlendPane blendPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                blendPane.IsEnabled = blendEffect.enabled;
            }
            else
            {
                blendEffect.enabled = blendPane.IsEnabled;
            }

            blendEffect.amount = blendPane.AmountValue;
            blendEffect.mode = blendPane.ModeValue;
        }

        public static void Reset()
        {
            if( blendEffect == null )
                return;

            blendEffect.amount = amount;
            blendEffect.mode = mode;
        }
    }
}

