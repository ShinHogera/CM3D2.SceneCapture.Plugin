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
    internal class SSAODef
    {
        public static AmbientObscurance ssaoEffect;

        public static float intensity { get; set; }
        public static float radius { get; set; }
        public static int blurIterations { get; set; }
        public static float blurFilterDistance { get; set; }
        public static int downsample { get; set; }

        static SSAODef()
        {
            if(ssaoEffect == null)
            {
                ssaoEffect = Util.GetComponentVar<AmbientObscurance, SSAODef>(ssaoEffect);
            }

            intensity = 0.5f;
            radius = 0.2f;
            blurIterations = 1;
            blurFilterDistance = 1.25f;
            downsample = 0;
        }

        public static void InitMemberByInstance(AmbientObscurance c)
        {
            intensity = c.intensity;
            radius = c.radius;
            blurIterations = c.blurIterations;
            blurFilterDistance = c.blurFilterDistance;
            downsample = c.downsample;
        }

        public static void Update(SSAOPane ssaoPane)
        {
            if(ssaoEffect == null)
            {
                ssaoEffect = Util.GetComponentVar<AmbientObscurance, SSAODef>(ssaoEffect);
            }

            if (Instances.needEffectWindowReload == true)
                ssaoPane.IsEnabled = ssaoEffect.enabled;
            else
                ssaoEffect.enabled = ssaoPane.IsEnabled;

            ssaoEffect.intensity = ssaoPane.IntensityValue;
            ssaoEffect.radius = ssaoPane.RadiusValue;
            ssaoEffect.blurIterations = ssaoPane.BlurIterationsValue;
            ssaoEffect.blurFilterDistance = ssaoPane.BlurFilterDistanceValue;
            ssaoEffect.downsample = ssaoPane.DownsampleValue;
        }

        public static void Reset()
        {
            if (ssaoEffect == null)
                return;

            ssaoEffect.intensity = intensity;
            ssaoEffect.radius = radius;
            ssaoEffect.blurIterations = blurIterations;
            ssaoEffect.blurFilterDistance = blurFilterDistance;
            ssaoEffect.downsample = downsample;
        }
    }
}
