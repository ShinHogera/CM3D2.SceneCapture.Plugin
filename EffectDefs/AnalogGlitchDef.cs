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
    internal class AnalogGlitchDef
    {
        public static AnalogGlitch analogGlitchEffect;

        public static float scanLineJitter { get; set; }
        public static float verticalJump { get; set; }
        public static float horizontalShake { get; set; }
        public static float colorDrift { get; set; }

        static AnalogGlitchDef()
        {
            if(analogGlitchEffect == null)
            {
                analogGlitchEffect = Util.GetComponentVar<AnalogGlitch, AnalogGlitchDef>(analogGlitchEffect);
            }
            scanLineJitter = 0.0f;
            verticalJump = 0.0f;
            horizontalShake = 0.0f;
            colorDrift = 0.0f;
        }

        public static void InitMemberByInstance(AnalogGlitch c)
        {
            scanLineJitter = c.scanLineJitter;
            verticalJump = c.verticalJump;
            horizontalShake = c.horizontalShake;
            colorDrift = c.colorDrift;
        }

        public static void Update(AnalogGlitchPane analogGlitchPane)
        {
            if (Instances.needEffectWindowReload == true)
                analogGlitchPane.IsEnabled = analogGlitchEffect.enabled;
            else
                analogGlitchEffect.enabled = analogGlitchPane.IsEnabled;

            analogGlitchEffect.scanLineJitter = analogGlitchPane.ScanLineJitterValue;
            analogGlitchEffect.verticalJump = analogGlitchPane.VerticalJumpValue;
            analogGlitchEffect.horizontalShake = analogGlitchPane.HorizontalShakeValue;
            analogGlitchEffect.colorDrift = analogGlitchPane.ColorDriftValue;
        }

        public static void Reset()
        {
            if (analogGlitchEffect == null)
                return;

            analogGlitchEffect.scanLineJitter = scanLineJitter;
            analogGlitchEffect.verticalJump = verticalJump;
            analogGlitchEffect.horizontalShake = horizontalShake;
            analogGlitchEffect.colorDrift = colorDrift;
        }
    }
}
