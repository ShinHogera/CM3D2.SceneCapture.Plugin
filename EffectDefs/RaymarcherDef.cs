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
    internal class RaymarcherDef
    {
        public static Raymarcher raymarcherEffect;

        public static bool screenSpace { get; set; }
        public static bool enableAdaptive { get; set; }
        public static bool enableTemporal { get; set; }
        public static bool dbgShowSteps { get; set; }
        public static int scene { get; set; }
        public static Color fogColor { get; set; }

        public RaymarcherDef()
        {
            if( raymarcherEffect == null)
            {
                raymarcherEffect = Util.GetComponentVar<Raymarcher, RaymarcherDef>(raymarcherEffect);
            }

            screenSpace = true;
            enableAdaptive = true;
            enableTemporal = true;
            dbgShowSteps = false;
            scene = 0;
            fogColor = new Color(0.16f, 0.13f, 0.20f);
        }

        public void InitMemberByInstance(Raymarcher raymarcher)
        {
            screenSpace = raymarcher.screenSpace;
            enableAdaptive = raymarcher.enableAdaptive;
            enableTemporal = raymarcher.enableTemporal;
            dbgShowSteps = raymarcher.dbgShowSteps;
            scene = raymarcher.scene;
            fogColor = raymarcher.fogColor;
        }

        public static void Update(RaymarcherPane raymarcherPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                raymarcherPane.IsEnabled = raymarcherEffect.enabled;
            }
            else
            {
                raymarcherEffect.enabled = raymarcherPane.IsEnabled;
            }

            raymarcherEffect.screenSpace = raymarcherPane.ScreenSpaceValue;
            raymarcherEffect.enableAdaptive = raymarcherPane.EnableAdaptiveValue;
            raymarcherEffect.enableTemporal = raymarcherPane.EnableTemporalValue;
            raymarcherEffect.dbgShowSteps = raymarcherPane.DbgShowStepsValue;
            raymarcherEffect.scene = raymarcherPane.SceneValue;
            raymarcherEffect.fogColor = raymarcherPane.FogColorValue;
        }

        public static void Reset()
        {
            if( raymarcherEffect == null )
                return;

            raymarcherEffect.screenSpace = screenSpace;
            raymarcherEffect.enableAdaptive = enableAdaptive;
            raymarcherEffect.enableTemporal = enableTemporal;
            raymarcherEffect.dbgShowSteps = dbgShowSteps;
            raymarcherEffect.scene = scene;
            raymarcherEffect.fogColor = fogColor;
        }
    }
}

