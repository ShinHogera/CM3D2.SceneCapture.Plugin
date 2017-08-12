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
    internal class ContrastVignetteDef
    {
        public static ContrastVignette contrastVignetteEffect;

        public static Vector2 center { get; set; }
        public static float sharpness { get; set; }
        public static float darkness { get; set; }
        public static float contrast { get; set; }
        public static Vector3 contrastCoeff { get; set; }
        public static float edgeBlending { get; set; }

        public ContrastVignetteDef()
        {
            if( contrastVignetteEffect == null)
            {
                contrastVignetteEffect = Util.GetComponentVar<ContrastVignette, ContrastVignetteDef>(contrastVignetteEffect);
            }

            center = new Vector2(0.5f, 0.5f);
            sharpness = 32f;
            darkness = 28f;
            contrast = 20.0f;
            contrastCoeff = new Vector3(0.5f, 0.5f, 0.5f);
            edgeBlending = 0f;
        }

        public void InitMemberByInstance(ContrastVignette contrastVignette)
        {
            center = contrastVignette.center;
            sharpness = contrastVignette.sharpness;
            darkness = contrastVignette.darkness;
            contrast = contrastVignette.contrast;
            contrastCoeff = contrastVignette.contrastCoeff;
            edgeBlending = contrastVignette.edgeBlending;
        }

        public static void Update(ContrastVignettePane contrastVignettePane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                contrastVignettePane.IsEnabled = contrastVignetteEffect.enabled;
            }
            else
            {
                contrastVignetteEffect.enabled = contrastVignettePane.IsEnabled;
            }

            contrastVignetteEffect.center = contrastVignettePane.CenterValue;
            contrastVignetteEffect.sharpness = contrastVignettePane.SharpnessValue;
            contrastVignetteEffect.darkness = contrastVignettePane.DarknessValue;
            contrastVignetteEffect.contrast = contrastVignettePane.ContrastValue;
            contrastVignetteEffect.contrastCoeff = contrastVignettePane.ContrastCoeffValue;
            contrastVignetteEffect.edgeBlending = contrastVignettePane.EdgeBlendingValue;
        }

        public static void Reset()
        {
            if( contrastVignetteEffect == null )
                return;

            contrastVignetteEffect.center = center;
            contrastVignetteEffect.sharpness = sharpness;
            contrastVignetteEffect.darkness = darkness;
            contrastVignetteEffect.contrast = contrast;
            contrastVignetteEffect.contrastCoeff = contrastCoeff;
            contrastVignetteEffect.edgeBlending = edgeBlending;
        }
    }
}

