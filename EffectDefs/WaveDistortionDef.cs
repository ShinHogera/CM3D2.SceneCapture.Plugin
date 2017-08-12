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
    internal class WaveDistortionDef
    {
        public static WaveDistortion waveDistortionEffect;

        public static float amplitude { get; set; }
        public static float waves { get; set; }
        public static float colorGlitch { get; set; }
        public static float phase { get; set; }

        public WaveDistortionDef()
        {
            if( waveDistortionEffect == null)
            {
                waveDistortionEffect = Util.GetComponentVar<WaveDistortion, WaveDistortionDef>(waveDistortionEffect);
            }

            amplitude = 0.6f;
            waves = 5f;
            colorGlitch = 0.35f;
            phase = 0.35f;
        }

        public void InitMemberByInstance(WaveDistortion waveDistortion)
        {
            amplitude = waveDistortion.amplitude;
            waves = waveDistortion.waves;
            colorGlitch = waveDistortion.colorGlitch;
            phase = waveDistortion.phase;
        }

        public static void Update(WaveDistortionPane waveDistortionPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                waveDistortionPane.IsEnabled = waveDistortionEffect.enabled;
            }
            else
            {
                waveDistortionEffect.enabled = waveDistortionPane.IsEnabled;
            }

            waveDistortionEffect.amplitude = waveDistortionPane.AmplitudeValue;
            waveDistortionEffect.waves = waveDistortionPane.WavesValue;
            waveDistortionEffect.colorGlitch = waveDistortionPane.ColorGlitchValue;
            waveDistortionEffect.phase = waveDistortionPane.PhaseValue;
        }

        public static void Reset()
        {
            if( waveDistortionEffect == null )
                return;

            waveDistortionEffect.amplitude = amplitude;
            waveDistortionEffect.waves = waves;
            waveDistortionEffect.colorGlitch = colorGlitch;
            waveDistortionEffect.phase = phase;
        }
    }
}

