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
    internal class ScreenSpaceReflectionsDef
    {
        public static ScreenSpaceReflections screenSpaceReflectionsEffect;

        public static ScreenSpaceReflections.SampleCount SampleCount { get; set; }
        public static int downsampling { get; set; }
        public static float intensity { get; set; }
        public static float rayDiffusion { get; set; }
        public static float blurSize { get; set; }
        public static float raymarchDistance { get; set; }
        public static float falloffDistance { get; set; }
        public static float rayHitDistance { get; set; }
        public static float maxAccumulation { get; set; }
        public static float stepBoost { get; set; }
        public static bool dangerousSamples { get; set; }
        public static bool preRaymarchPass { get; set; }

        public ScreenSpaceReflectionsDef()
        {
            if( screenSpaceReflectionsEffect == null)
            {
                screenSpaceReflectionsEffect = Util.GetComponentVar<ScreenSpaceReflections, ScreenSpaceReflectionsDef>(screenSpaceReflectionsEffect);
            }

            SampleCount = ScreenSpaceReflections.SampleCount.Medium;
            downsampling = 2;
            intensity = 1.0f;
            rayDiffusion = 0.01f;
            blurSize = 1.0f;
            raymarchDistance = 2.5f;
            falloffDistance = 2.5f;
            rayHitDistance = 0.15f;
            maxAccumulation = 25.0f;
            stepBoost = 0.0f;
            dangerousSamples = true;
            preRaymarchPass = true;
        }

        public void InitMemberByInstance(ScreenSpaceReflections screenSpaceReflections)
        {
            SampleCount = screenSpaceReflections.sampleCount;
            downsampling = screenSpaceReflections.downsampling;
            intensity = screenSpaceReflections.intensity;
            rayDiffusion = screenSpaceReflections.rayDiffusion;
            blurSize = screenSpaceReflections.blurSize;
            raymarchDistance = screenSpaceReflections.raymarchDistance;
            falloffDistance = screenSpaceReflections.falloffDistance;
            rayHitDistance = screenSpaceReflections.rayHitDistance;
            maxAccumulation = screenSpaceReflections.maxAccumulation;
            stepBoost = screenSpaceReflections.stepBoost;
            dangerousSamples = screenSpaceReflections.dangerousSamples;
            preRaymarchPass = screenSpaceReflections.preRaymarchPass;
        }

        public static void Update(ScreenSpaceReflectionsPane screenSpaceReflectionsPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                screenSpaceReflectionsPane.IsEnabled = screenSpaceReflectionsEffect.enabled;
            }
            else
            {
                screenSpaceReflectionsEffect.enabled = screenSpaceReflectionsPane.IsEnabled;
            }

            screenSpaceReflectionsEffect.sampleCount = screenSpaceReflectionsPane.SampleCountValue;
            screenSpaceReflectionsEffect.downsampling = screenSpaceReflectionsPane.DownsamplingValue;
            screenSpaceReflectionsEffect.intensity = screenSpaceReflectionsPane.IntensityValue;
            screenSpaceReflectionsEffect.rayDiffusion = screenSpaceReflectionsPane.RayDiffusionValue;
            screenSpaceReflectionsEffect.blurSize = screenSpaceReflectionsPane.BlurSizeValue;
            screenSpaceReflectionsEffect.raymarchDistance = screenSpaceReflectionsPane.RaymarchDistanceValue;
            screenSpaceReflectionsEffect.falloffDistance = screenSpaceReflectionsPane.FalloffDistanceValue;
            screenSpaceReflectionsEffect.rayHitDistance = screenSpaceReflectionsPane.RayHitDistanceValue;
            screenSpaceReflectionsEffect.maxAccumulation = screenSpaceReflectionsPane.MaxAccumulationValue;
            screenSpaceReflectionsEffect.stepBoost = screenSpaceReflectionsPane.StepBoostValue;
            screenSpaceReflectionsEffect.dangerousSamples = screenSpaceReflectionsPane.DangerousSamplesValue;
            screenSpaceReflectionsEffect.preRaymarchPass = screenSpaceReflectionsPane.PreRaymarchPassValue;
        }

        public static void Reset()
        {
            if( screenSpaceReflectionsEffect == null )
                return;

            screenSpaceReflectionsEffect.sampleCount = SampleCount;
            screenSpaceReflectionsEffect.downsampling = downsampling;
            screenSpaceReflectionsEffect.intensity = intensity;
            screenSpaceReflectionsEffect.rayDiffusion = rayDiffusion;
            screenSpaceReflectionsEffect.blurSize = blurSize;
            screenSpaceReflectionsEffect.raymarchDistance = raymarchDistance;
            screenSpaceReflectionsEffect.falloffDistance = falloffDistance;
            screenSpaceReflectionsEffect.rayHitDistance = rayHitDistance;
            screenSpaceReflectionsEffect.maxAccumulation = maxAccumulation;
            screenSpaceReflectionsEffect.stepBoost = stepBoost;
            screenSpaceReflectionsEffect.dangerousSamples = dangerousSamples;
            screenSpaceReflectionsEffect.preRaymarchPass = preRaymarchPass;
        }
    }
}

