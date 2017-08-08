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
    internal class FisheyeDef
    {
        public static Fisheye fisheyeEffect;

        public static float strengthX { get; set; }
        public static float strengthY { get; set; }

        static FisheyeDef()
        {
            if(fisheyeEffect == null)
            {
                fisheyeEffect = Util.GetComponentVar<Fisheye, FisheyeDef>(fisheyeEffect);
            }

            strengthX = 0.05f;
            strengthY = 0.05f;
        }

        public static void InitMemberByInstance(Fisheye fish)
        {
            strengthX = fish.strengthX;
            strengthY = fish.strengthY;
        }

        public static void Update(FisheyePane fisheyePane)
        {
            if(fisheyeEffect == null)
            {
                fisheyeEffect = Util.GetComponentVar<Fisheye, FisheyeDef>(fisheyeEffect);
            }

            if (Instances.needEffectWindowReload == true)
                fisheyePane.IsEnabled = fisheyeEffect.enabled;
            else
                fisheyeEffect.enabled = fisheyePane.IsEnabled;

            fisheyeEffect.strengthX = fisheyePane.StrengthXValue;
            fisheyeEffect.strengthY = fisheyePane.StrengthYValue;
        }

        public static void Reset()
        {
            if (fisheyeEffect == null)
                return;

            fisheyeEffect.strengthX = strengthX;
            fisheyeEffect.strengthY = strengthY;
        }
    }
}
