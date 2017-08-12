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
    internal class BleachBypassDef
    {
        public static BleachBypass bleachBypassEffect;

        public static float amount { get; set; }

        public BleachBypassDef()
        {
            if( bleachBypassEffect == null)
            {
                bleachBypassEffect = Util.GetComponentVar<BleachBypass, BleachBypassDef>(bleachBypassEffect);
            }

            amount = 1f;
        }

        public void InitMemberByInstance(BleachBypass bleachBypass)
        {
            amount = bleachBypass.amount;
        }

        public static void Update(BleachBypassPane bleachBypassPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                bleachBypassPane.IsEnabled = bleachBypassEffect.enabled;
            }
            else
            {
                bleachBypassEffect.enabled = bleachBypassPane.IsEnabled;
            }

            bleachBypassEffect.amount = bleachBypassPane.AmountValue;
        }

        public static void Reset()
        {
            if( bleachBypassEffect == null )
                return;

            bleachBypassEffect.amount = amount;
        }
    }
}

