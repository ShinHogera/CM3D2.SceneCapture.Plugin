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
    internal class WhiteBalanceDef
    {
        public static WhiteBalance whiteBalanceEffect;

        public static Color white { get; set; }
        public static WhiteBalance.BalanceMode mode { get; set; }

        public WhiteBalanceDef()
        {
            if( whiteBalanceEffect == null)
            {
                whiteBalanceEffect = Util.GetComponentVar<WhiteBalance, WhiteBalanceDef>(whiteBalanceEffect);
            }

            white = new Color(0.5f, 0.5f, 0.5f);
            mode = WhiteBalance.BalanceMode.Complex;
        }

        public void InitMemberByInstance(WhiteBalance whiteBalance)
        {
            white = whiteBalance.white;
            mode = whiteBalance.mode;
        }

        public static void Update(WhiteBalancePane whiteBalancePane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                whiteBalancePane.IsEnabled = whiteBalanceEffect.enabled;
            }
            else
            {
                whiteBalanceEffect.enabled = whiteBalancePane.IsEnabled;
            }

            whiteBalanceEffect.white = whiteBalancePane.WhiteValue;
            whiteBalanceEffect.mode = whiteBalancePane.ModeValue;
        }

        public static void Reset()
        {
            if( whiteBalanceEffect == null )
                return;

            whiteBalanceEffect.white = white;
            whiteBalanceEffect.mode = mode;
        }
    }
}

