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
    internal class RGBSplitDef
    {
        public static RGBSplit rGBSplitEffect;

        public static float amount { get; set; }
        public static float angle { get; set; }

        public RGBSplitDef()
        {
            if( rGBSplitEffect == null)
            {
                rGBSplitEffect = Util.GetComponentVar<RGBSplit, RGBSplitDef>(rGBSplitEffect);
            }

            amount = 0f;
            angle = 0f;
        }

        public void InitMemberByInstance(RGBSplit rGBSplit)
        {
            amount = rGBSplit.amount;
            angle = rGBSplit.angle;
        }

        public static void Update(RGBSplitPane rGBSplitPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                rGBSplitPane.IsEnabled = rGBSplitEffect.enabled;
            }
            else
            {
                rGBSplitEffect.enabled = rGBSplitPane.IsEnabled;
            }

            rGBSplitEffect.amount = rGBSplitPane.AmountValue;
            rGBSplitEffect.angle = rGBSplitPane.AngleValue;
        }

        public static void Reset()
        {
            if( rGBSplitEffect == null )
                return;

            rGBSplitEffect.amount = amount;
            rGBSplitEffect.angle = angle;
        }
    }
}

