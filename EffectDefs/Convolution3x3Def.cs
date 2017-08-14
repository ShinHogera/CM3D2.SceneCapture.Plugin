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
    internal class Convolution3x3Def
    {
        public static Convolution3x3 convolution3x3Effect;

        public static Vector3 kernelTop { get; set; }
        public static Vector3 kernelMiddle { get; set; }
        public static Vector3 kernelBottom { get; set; }
        public static float divisor { get; set; }
        public static float amount { get; set; }

        public Convolution3x3Def()
        {
            if( convolution3x3Effect == null)
            {
                convolution3x3Effect = Util.GetComponentVar<Convolution3x3, Convolution3x3Def>(convolution3x3Effect);
            }

            kernelTop = Vector3.zero;
            kernelMiddle = Vector3.up;
            kernelBottom = Vector3.zero;
            divisor = 1f;
            amount = 1f;
        }

        public void InitMemberByInstance(Convolution3x3 convolution3x3)
        {
            kernelTop = convolution3x3.kernelTop;
            kernelMiddle = convolution3x3.kernelMiddle;
            kernelBottom = convolution3x3.kernelBottom;
            divisor = convolution3x3.divisor;
            amount = convolution3x3.amount;
        }

        public static void Update(Convolution3x3Pane convolution3x3Pane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                convolution3x3Pane.IsEnabled = convolution3x3Effect.enabled;
            }
            else
            {
                convolution3x3Effect.enabled = convolution3x3Pane.IsEnabled;
            }

            convolution3x3Effect.kernelTop = convolution3x3Pane.KernelTopValue;
            convolution3x3Effect.kernelMiddle = convolution3x3Pane.KernelMiddleValue;
            convolution3x3Effect.kernelBottom = convolution3x3Pane.KernelBottomValue;
            convolution3x3Effect.divisor = convolution3x3Pane.DivisorValue;
            convolution3x3Effect.amount = convolution3x3Pane.AmountValue;
        }

        public static void Reset()
        {
            if( convolution3x3Effect == null )
                return;

            convolution3x3Effect.kernelTop = kernelTop;
            convolution3x3Effect.kernelMiddle = kernelMiddle;
            convolution3x3Effect.kernelBottom = kernelBottom;
            convolution3x3Effect.divisor = divisor;
            convolution3x3Effect.amount = amount;
        }
    }
}

