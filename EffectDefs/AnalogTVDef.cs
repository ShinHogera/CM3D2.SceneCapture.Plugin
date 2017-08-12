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
    internal class AnalogTVDef
    {
        public static AnalogTV analogTVEffect;

        public static bool automaticPhase { get; set; }
        public static float phase { get; set; }
        public static bool convertToGrayscale { get; set; }
        public static float noiseIntensity { get; set; }
        public static float scanlinesIntensity { get; set; }
        public static int scanlinesCount { get; set; }
        public static float scanlinesOffset { get; set; }
        public static bool verticalScanlines { get; set; }
        public static float distortion { get; set; }
        public static float cubicDistortion { get; set; }
        public static float scale { get; set; }

        public AnalogTVDef()
        {
            if( analogTVEffect == null)
            {
                analogTVEffect = Util.GetComponentVar<AnalogTV, AnalogTVDef>(analogTVEffect);
            }

            automaticPhase = true;
            phase = 0.5f;
            convertToGrayscale = false;
            noiseIntensity = 0.5f;
            scanlinesIntensity = 2f;
            scanlinesCount = 768;
            scanlinesOffset = 0f;
            verticalScanlines = false;
            distortion = 0.2f;
            cubicDistortion = 0.6f;
            scale = 0.8f;
        }

        public void InitMemberByInstance(AnalogTV analogTV)
        {
            automaticPhase = analogTV.automaticPhase;
            phase = analogTV.phase;
            convertToGrayscale = analogTV.convertToGrayscale;
            noiseIntensity = analogTV.noiseIntensity;
            scanlinesIntensity = analogTV.scanlinesIntensity;
            scanlinesCount = analogTV.scanlinesCount;
            scanlinesOffset = analogTV.scanlinesOffset;
            verticalScanlines = analogTV.verticalScanlines;
            distortion = analogTV.distortion;
            cubicDistortion = analogTV.cubicDistortion;
            scale = analogTV.scale;
        }

        public static void Update(AnalogTVPane analogTVPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                analogTVPane.IsEnabled = analogTVEffect.enabled;
            }
            else
            {
                analogTVEffect.enabled = analogTVPane.IsEnabled;
            }

            analogTVEffect.automaticPhase = analogTVPane.AutomaticPhaseValue;
            analogTVEffect.phase = analogTVPane.PhaseValue;
            analogTVEffect.convertToGrayscale = analogTVPane.ConvertToGrayscaleValue;
            analogTVEffect.noiseIntensity = analogTVPane.NoiseIntensityValue;
            analogTVEffect.scanlinesIntensity = analogTVPane.ScanlinesIntensityValue;
            analogTVEffect.scanlinesCount = analogTVPane.ScanlinesCountValue;
            analogTVEffect.scanlinesOffset = analogTVPane.ScanlinesOffsetValue;
            analogTVEffect.verticalScanlines = analogTVPane.VerticalScanlinesValue;
            analogTVEffect.distortion = analogTVPane.DistortionValue;
            analogTVEffect.cubicDistortion = analogTVPane.CubicDistortionValue;
            analogTVEffect.scale = analogTVPane.ScaleValue;
        }

        public static void Reset()
        {
            if( analogTVEffect == null )
                return;

            analogTVEffect.automaticPhase = automaticPhase;
            analogTVEffect.phase = phase;
            analogTVEffect.convertToGrayscale = convertToGrayscale;
            analogTVEffect.noiseIntensity = noiseIntensity;
            analogTVEffect.scanlinesIntensity = scanlinesIntensity;
            analogTVEffect.scanlinesCount = scanlinesCount;
            analogTVEffect.scanlinesOffset = scanlinesOffset;
            analogTVEffect.verticalScanlines = verticalScanlines;
            analogTVEffect.distortion = distortion;
            analogTVEffect.cubicDistortion = cubicDistortion;
            analogTVEffect.scale = scale;
        }
    }
}

