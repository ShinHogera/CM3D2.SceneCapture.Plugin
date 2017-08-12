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
    internal class WiggleDef
    {
        public static Wiggle wiggleEffect;

        public static Wiggle.Algorithm mode { get; set; }
        public static float timer { get; set; }
        public static float speed { get; set; }
        public static float frequency { get; set; }
        public static float amplitude { get; set; }
        public static bool automaticTimer { get; set; }

        public WiggleDef()
        {
            if( wiggleEffect == null)
            {
                wiggleEffect = Util.GetComponentVar<Wiggle, WiggleDef>(wiggleEffect);
            }

            mode = Wiggle.Algorithm.Complex;
            timer = 0f;
            speed = 1f;
            frequency = 12f;
            amplitude = 0.01f;
            automaticTimer = true;
        }

        public void InitMemberByInstance(Wiggle wiggle)
        {
            mode = wiggle.mode;
            timer = wiggle.timer;
            speed = wiggle.speed;
            frequency = wiggle.frequency;
            amplitude = wiggle.amplitude;
            automaticTimer = wiggle.automaticTimer;
        }

        public static void Update(WigglePane wigglePane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                wigglePane.IsEnabled = wiggleEffect.enabled;
            }
            else
            {
                wiggleEffect.enabled = wigglePane.IsEnabled;
            }

            wiggleEffect.mode = wigglePane.ModeValue;

            if( wigglePane.SpeedValue == 0 )
                wiggleEffect.timer = wigglePane.TimerValue;
            else
                wigglePane.TimerValue = wiggleEffect.timer;

            wiggleEffect.speed = wigglePane.SpeedValue;
            wiggleEffect.frequency = wigglePane.FrequencyValue;
            wiggleEffect.amplitude = wigglePane.AmplitudeValue;
            wiggleEffect.automaticTimer = wigglePane.AutomaticTimerValue;
        }

        public static void Reset()
        {
            if( wiggleEffect == null )
                return;

            wiggleEffect.mode = mode;
            wiggleEffect.timer = timer;
            wiggleEffect.speed = speed;
            wiggleEffect.frequency = frequency;
            wiggleEffect.amplitude = amplitude;
            wiggleEffect.automaticTimer = automaticTimer;
        }
    }
}

