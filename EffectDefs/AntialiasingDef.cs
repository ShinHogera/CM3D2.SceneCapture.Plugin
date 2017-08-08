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
    internal class AntialiasingDef
    {
        public static AntialiasingAsPostEffect antialiasingEffect;

            public static AAMode mode { get; set; }
            public static bool dlaaSharp { get; set; }
            public static bool showGeneratedNormals { get; set; }
            public static float blurRadius { get; set; }
            public static float edgeSharpness { get; set; }
            public static float edgeThreshold { get; set; }
            public static float edgeThresholdMin { get; set; }
            public static float offsetScale { get; set; }

        static AntialiasingDef()
        {
            if(antialiasingEffect == null)
            {
                antialiasingEffect = Util.GetComponentVar<AntialiasingAsPostEffect, AntialiasingDef>(antialiasingEffect);
            }

            mode = AAMode.FXAA3Console;
            dlaaSharp = false;
            showGeneratedNormals = false;
            blurRadius = 18f;
            edgeSharpness = 4f;
            edgeThreshold = 0.2f;
            edgeThresholdMin = 0.05f;
            offsetScale = 0.2f;
        }

        public static void InitMemberByInstance(AntialiasingAsPostEffect antialiasingEffect)
        {
            mode = antialiasingEffect.mode;
            dlaaSharp = antialiasingEffect.dlaaSharp;
            showGeneratedNormals = antialiasingEffect.showGeneratedNormals;
            blurRadius = antialiasingEffect.blurRadius;
            edgeSharpness = antialiasingEffect.edgeSharpness;
            edgeThreshold = antialiasingEffect.edgeThreshold;
            edgeThresholdMin = antialiasingEffect.edgeThresholdMin;
            offsetScale = antialiasingEffect.offsetScale;
        }

        public static void Update(AntialiasingPane antialiasingPane)
        {
            if(antialiasingEffect == null)
            {
                antialiasingEffect = Util.GetComponentVar<AntialiasingAsPostEffect, AntialiasingDef>(antialiasingEffect);
            }

            if (Instances.needEffectWindowReload == true)
                antialiasingPane.IsEnabled = antialiasingEffect.enabled;
            else
                antialiasingEffect.enabled = antialiasingPane.IsEnabled;

            antialiasingEffect.mode = antialiasingPane.ModeValue;
            antialiasingEffect.dlaaSharp = antialiasingPane.DlaaSharpValue;
            antialiasingEffect.showGeneratedNormals = antialiasingPane.ShowGeneratedNormalsValue;
            antialiasingEffect.blurRadius = antialiasingPane.BlurRadiusValue;
            antialiasingEffect.edgeSharpness = antialiasingPane.EdgeSharpnessValue;
            antialiasingEffect.edgeThreshold = antialiasingPane.EdgeThresholdValue;
            antialiasingEffect.edgeThresholdMin = antialiasingPane.EdgeThresholdMinValue;
            antialiasingEffect.offsetScale = antialiasingPane.OffsetScaleValue;
        }

        public static void Reset()
        {
            if (antialiasingEffect == null)
                return;

            antialiasingEffect.mode = mode;
            antialiasingEffect.dlaaSharp = dlaaSharp;
            antialiasingEffect.showGeneratedNormals = showGeneratedNormals;
            antialiasingEffect.blurRadius = blurRadius;
            antialiasingEffect.edgeSharpness = edgeSharpness;
            antialiasingEffect.edgeThreshold = edgeThreshold;
            antialiasingEffect.edgeThresholdMin = edgeThresholdMin;
            antialiasingEffect.offsetScale = offsetScale;
        }
    }
}
