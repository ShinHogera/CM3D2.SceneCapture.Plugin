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
    internal class TemporalSSAODef
    {
        public static TemporalSSAO temporalSSAOEffect;

        public static TemporalSSAO.SampleCount SampleCount { get; set; }
        public static int downsampling { get; set; }
        public static float radius { get; set; }
        public static float intensity { get; set; }
        public static float blurSize { get; set; }
        public static bool dangerousSamples { get; set; }
        public static float maxAccumulation { get; set; }

        public TemporalSSAODef()
        {
            if( temporalSSAOEffect == null)
            {
                temporalSSAOEffect = Util.GetComponentVar<TemporalSSAO, TemporalSSAODef>(temporalSSAOEffect);
            }

            SampleCount = TemporalSSAO.SampleCount.Medium;
            downsampling = 3;
            radius = 0.25f;
            intensity = 1.5f;
            blurSize = 0.5f;
            dangerousSamples = true;
            maxAccumulation = 100.0f;
        }

        public void InitMemberByInstance(TemporalSSAO temporalSSAO)
        {
            SampleCount = temporalSSAO.sampleCount;
            downsampling = temporalSSAO.downsampling;
            radius = temporalSSAO.radius;
            intensity = temporalSSAO.intensity;
            blurSize = temporalSSAO.blurSize;
            dangerousSamples = temporalSSAO.dangerousSamples;
            maxAccumulation = temporalSSAO.maxAccumulation;
        }

        public static void Update(TemporalSSAOPane temporalSSAOPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                temporalSSAOPane.IsEnabled = temporalSSAOEffect.enabled;
            }
            else
            {
                temporalSSAOEffect.enabled = temporalSSAOPane.IsEnabled;
            }

            temporalSSAOEffect.sampleCount = temporalSSAOPane.SampleCountValue;
            temporalSSAOEffect.downsampling = temporalSSAOPane.DownsamplingValue;
            temporalSSAOEffect.radius = temporalSSAOPane.RadiusValue;
            temporalSSAOEffect.intensity = temporalSSAOPane.IntensityValue;
            temporalSSAOEffect.blurSize = temporalSSAOPane.BlurSizeValue;
            temporalSSAOEffect.dangerousSamples = temporalSSAOPane.DangerousSamplesValue;
            temporalSSAOEffect.maxAccumulation = temporalSSAOPane.MaxAccumulationValue;
        }

        public static void Reset()
        {
            if( temporalSSAOEffect == null )
                return;

            temporalSSAOEffect.sampleCount = SampleCount;
            temporalSSAOEffect.downsampling = downsampling;
            temporalSSAOEffect.radius = radius;
            temporalSSAOEffect.intensity = intensity;
            temporalSSAOEffect.blurSize = blurSize;
            temporalSSAOEffect.dangerousSamples = dangerousSamples;
            temporalSSAOEffect.maxAccumulation = maxAccumulation;
        }
    }
}

