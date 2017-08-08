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
    internal class ObscuranceDef
    {
        public static Obscurance obscuranceEffect;

        public static float intensity { get; set; }
        public static float radius { get; set; }
        public static Obscurance.SampleCount sampleCount { get; set; }
        public static int sampleCountValue { get; set; }
        public static bool downsampling { get; set; }
        public static Obscurance.OcclusionSource occlusionSource { get; set; }
        public static bool ambientOnly { get; set; }

        static ObscuranceDef()
        {
            if(obscuranceEffect == null)
            {
                obscuranceEffect = Util.GetComponentVar<Obscurance, ObscuranceDef>(obscuranceEffect);
            }

            intensity = 1;
            radius = 0.3f;
            sampleCount = Obscurance.SampleCount.Medium;
            sampleCountValue = 24;
            downsampling = false;
            occlusionSource = Obscurance.OcclusionSource.GBuffer;
            ambientOnly = false;
        }

        public static void InitMemberByInstance(Obscurance ob)
        {
            intensity = ob.intensity;
            radius = ob.radius;
            sampleCount = ob.sampleCount;
            sampleCountValue = ob.sampleCountValue;
            downsampling = ob.downsampling;
            occlusionSource = ob.occlusionSource;
            ambientOnly = ob.ambientOnly;
        }

        public static void Update(ObscurancePane obscurancePane)
        {
            if(obscuranceEffect == null)
            {
                obscuranceEffect = Util.GetComponentVar<Obscurance, ObscuranceDef>(obscuranceEffect);
            }

            if (Instances.needEffectWindowReload == true)
                obscurancePane.IsEnabled = obscuranceEffect.enabled;
            else
                obscuranceEffect.enabled = obscurancePane.IsEnabled;

            obscuranceEffect.intensity = obscurancePane.IntensityValue;
            obscuranceEffect.radius = obscurancePane.RadiusValue;
            obscuranceEffect.sampleCount = obscurancePane.SampleCountValue;
            obscuranceEffect.sampleCountValue = obscurancePane.SampleCountValueValue;
            obscuranceEffect.downsampling = obscurancePane.DownsamplingValue;
            obscuranceEffect.occlusionSource = obscurancePane.OcclusionSourceValue;
            obscuranceEffect.ambientOnly = obscurancePane.AmbientOnlyValue;
        }

        public static void Reset()
        {
            if (obscuranceEffect == null)
                return;

            obscuranceEffect.intensity = intensity;
            obscuranceEffect.radius = radius;
            obscuranceEffect.sampleCount = sampleCount;
            obscuranceEffect.sampleCountValue = sampleCountValue;
            obscuranceEffect.downsampling = downsampling;
            obscuranceEffect.occlusionSource = occlusionSource;
            obscuranceEffect.ambientOnly = ambientOnly;
        }
    }
}
