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
    internal class NoiseAndGrainDef
    {
        public static NoiseAndGrain noiseAndGrainEffect;

        public static bool dx11Grain { get; set; }
        public static bool monochrome { get; set; }

        public static float intensityMultiplier { get; set; }
        public static float generalIntensity { get; set; }
        public static float blackIntensity { get; set; }
        public static float whiteIntensity { get; set; }
        public static float midGrey { get; set; }
        public static FilterMode filterMode { get; set; }
        public static float softness { get; set; }

        public static float monochromeTiling { get; set; }
        public static Vector3 tiling { get; set; }

        static NoiseAndGrainDef()
        {
            if(noiseAndGrainEffect == null)
            {
                noiseAndGrainEffect = Util.GetComponentVar<NoiseAndGrain, NoiseAndGrainDef>(noiseAndGrainEffect);
            }

            dx11Grain = false;
            monochrome = false;
            monochromeTiling = 64f;
            intensityMultiplier = 0.25f;
            generalIntensity = 0.5f;
            blackIntensity = 1f;
            whiteIntensity = 1f;
            midGrey = 0.2f;
            filterMode = FilterMode.Bilinear;
            softness = 0f;
            tiling = new Vector3(64f, 64f, 64f);
        }

        public static void InitMemberByInstance(NoiseAndGrain nag)
        {
            dx11Grain = nag.dx11Grain;
            monochrome = nag.monochrome;
            monochromeTiling = nag.monochromeTiling;
            intensityMultiplier = nag.intensityMultiplier;
            generalIntensity = nag.generalIntensity;
            blackIntensity = nag.blackIntensity;
            whiteIntensity = nag.whiteIntensity;
            midGrey = nag.midGrey;
            filterMode = nag.filterMode;
            softness = nag.softness;
            tiling = nag.tiling;
        }

        public static void InitExtra(NoiseAndGrain nag)
        {
            nag.noiseTexture = new Texture2D(8, 8);
            nag.noiseTexture.LoadImage(ConstantValues.byteNoiseAndGrainPng);
            nag.noiseTexture.Apply();
        }

        public static void Update(NoiseAndGrainPane noiseAndGrainPane)
        {
            if (Instances.needEffectWindowReload == true)
                noiseAndGrainPane.IsEnabled = noiseAndGrainEffect.enabled;
            else
                noiseAndGrainEffect.enabled = noiseAndGrainPane.IsEnabled;

            noiseAndGrainEffect.dx11Grain = noiseAndGrainPane.Dx11GrainValue;
            noiseAndGrainEffect.monochrome = noiseAndGrainPane.MonochromeValue;
            noiseAndGrainEffect.monochromeTiling = noiseAndGrainPane.MonochromeTilingValue;
            noiseAndGrainEffect.intensityMultiplier = noiseAndGrainPane.IntensityMultiplierValue;
            noiseAndGrainEffect.generalIntensity = noiseAndGrainPane.GeneralIntensityValue;
            noiseAndGrainEffect.blackIntensity = noiseAndGrainPane.BlackIntensityValue;
            noiseAndGrainEffect.whiteIntensity = noiseAndGrainPane.WhiteIntensityValue;
            noiseAndGrainEffect.midGrey = noiseAndGrainPane.MidGreyValue;
            noiseAndGrainEffect.filterMode = noiseAndGrainPane.FilterModeValue;
            noiseAndGrainEffect.softness = noiseAndGrainPane.SoftnessValue;
            noiseAndGrainEffect.tiling = noiseAndGrainPane.TilingValue;
        }

        public static void Reset()
        {
            if (noiseAndGrainEffect == null)
                return;

            noiseAndGrainEffect.dx11Grain = dx11Grain;
            noiseAndGrainEffect.monochrome = monochrome;
            noiseAndGrainEffect.monochromeTiling = monochromeTiling;
            noiseAndGrainEffect.intensityMultiplier = intensityMultiplier;
            noiseAndGrainEffect.generalIntensity = generalIntensity;
            noiseAndGrainEffect.blackIntensity = blackIntensity;
            noiseAndGrainEffect.whiteIntensity = whiteIntensity;
            noiseAndGrainEffect.midGrey = midGrey;
            noiseAndGrainEffect.filterMode = filterMode;
            noiseAndGrainEffect.softness = softness;
            noiseAndGrainEffect.tiling = tiling;
        }
    }
}
