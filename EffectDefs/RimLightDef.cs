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
    internal class RimLightDef
    {
        public static RimLight rimLightEffect;

        public static Color color { get; set; }
        public static float intensity { get; set; }
        public static float fresnelBias { get; set; }
        public static float fresnelScale { get; set; }
        public static float fresnelPow { get; set; }
        public static bool edgeHighlighting { get; set; }
        public static float edgeIntensity { get; set; }
        public static float edgeThreshold { get; set; }
        public static float edgeRadius { get; set; }
        public static bool mulSmoothness { get; set; }

        public RimLightDef()
        {
            if( rimLightEffect == null)
            {
                rimLightEffect = Util.GetComponentVar<RimLight, RimLightDef>(rimLightEffect);
            }

            color = new Color(0.75f, 0.75f, 1.0f, 0.0f);
            intensity = 1.0f;
            fresnelBias = 0.0f;
            fresnelScale = 5.0f;
            fresnelPow = 5.0f;
            edgeHighlighting = true;
            edgeIntensity = 0.3f;
            edgeThreshold = 0.8f;
            edgeRadius = 1.0f;
            mulSmoothness = true;
        }

        public void InitMemberByInstance(RimLight rimLight)
        {
            color = rimLight.color;
            intensity = rimLight.intensity;
            fresnelBias = rimLight.fresnelBias;
            fresnelScale = rimLight.fresnelScale;
            fresnelPow = rimLight.fresnelPow;
            edgeHighlighting = rimLight.edgeHighlighting;
            edgeIntensity = rimLight.edgeIntensity;
            edgeThreshold = rimLight.edgeThreshold;
            edgeRadius = rimLight.edgeRadius;
            mulSmoothness = rimLight.mulSmoothness;
        }

        public static void Update(RimLightPane rimLightPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                rimLightPane.IsEnabled = rimLightEffect.enabled;
            }
            else
            {
                rimLightEffect.enabled = rimLightPane.IsEnabled;
            }

            rimLightEffect.color = rimLightPane.ColorValue;
            rimLightEffect.intensity = rimLightPane.IntensityValue;
            rimLightEffect.fresnelBias = rimLightPane.FresnelBiasValue;
            rimLightEffect.fresnelScale = rimLightPane.FresnelScaleValue;
            rimLightEffect.fresnelPow = rimLightPane.FresnelPowValue;
            rimLightEffect.edgeHighlighting = rimLightPane.EdgeHighlightingValue;
            rimLightEffect.edgeIntensity = rimLightPane.EdgeIntensityValue;
            rimLightEffect.edgeThreshold = rimLightPane.EdgeThresholdValue;
            rimLightEffect.edgeRadius = rimLightPane.EdgeRadiusValue;
            rimLightEffect.mulSmoothness = rimLightPane.MulSmoothnessValue;
        }

        public static void Reset()
        {
            if( rimLightEffect == null )
                return;

            rimLightEffect.color = color;
            rimLightEffect.intensity = intensity;
            rimLightEffect.fresnelBias = fresnelBias;
            rimLightEffect.fresnelScale = fresnelScale;
            rimLightEffect.fresnelPow = fresnelPow;
            rimLightEffect.edgeHighlighting = edgeHighlighting;
            rimLightEffect.edgeIntensity = edgeIntensity;
            rimLightEffect.edgeThreshold = edgeThreshold;
            rimLightEffect.edgeRadius = edgeRadius;
            rimLightEffect.mulSmoothness = mulSmoothness;
        }
    }
}

