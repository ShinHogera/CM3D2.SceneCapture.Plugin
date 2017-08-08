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
    internal class DepthOfFieldDef
    {
        public static DepthOfFieldScatter depthOfFieldEffect;

        public static bool visualizeFocus { get; set; }
        public static float focalLength { get; set; }
        public static float focalSize { get; set; }
        public static Transform focalTransform { get; set; }
        public static float aperture { get; set; }

        public static DepthOfFieldScatter.BlurType blurType { get; set; }
        public static DepthOfFieldScatter.BlurSampleCount blurSampleCount { get; set; }

        public static float maxBlurSize { get; set; }
        public static bool highResolution { get; set; }

        public static bool nearBlur { get; set; }
        public static float foregroundOverlap { get; set; }

        public static float dx11BokehScale { get; set; }
        public static float dx11BokehIntensity { get; set; }
        public static float dx11BokehThreshhold { get; set; }
        public static float dx11SpawnHeuristic { get; set; }

        //
        public static bool transformFromMaid { get; set; }
        public static int focusedMaid { get; set; }
        //

        static DepthOfFieldDef()
        {
            if(depthOfFieldEffect == null)
            {
                depthOfFieldEffect = Util.GetComponentVar<DepthOfFieldScatter, DepthOfFieldDef>(depthOfFieldEffect);
            }

            visualizeFocus = false;
            focalLength= 10f;
            focalSize = 0.05f;
            //focalTransform;
            aperture = 11.5f;

            blurType = DepthOfFieldScatter.BlurType.DiscBlur;
            blurSampleCount = DepthOfFieldScatter.BlurSampleCount.High;

            maxBlurSize = 2f;
            highResolution = false;

            nearBlur = false;
            foregroundOverlap = 1f;

            dx11BokehScale= 1.2f;
            dx11BokehIntensity = 2.5f;

            dx11BokehThreshhold = 0.5f;
            dx11SpawnHeuristic = 0.0875f;

            focusedMaid = -1;
        }

        public static void InitMemberByInstance(DepthOfFieldScatter depth)
        {
            visualizeFocus = depth.visualizeFocus;
            focalLength = depth.focalLength;
            focalSize = depth.focalSize;
            focalTransform = depth.focalTransform;
            aperture = depth.aperture;
            blurType = depth.blurType;
            blurSampleCount = depth.blurSampleCount;

            maxBlurSize = depth.maxBlurSize;
            highResolution = depth.highResolution;

            nearBlur = depth.nearBlur;
            foregroundOverlap = depth.foregroundOverlap;

            dx11BokehScale = depth.dx11BokehScale;
            dx11BokehIntensity = depth.dx11BokehIntensity;

            dx11BokehThreshhold = depth.dx11BokehThreshhold;
            dx11SpawnHeuristic = depth.dx11SpawnHeuristic;
        }

        public static void InitExtra(DepthOfFieldScatter depth)
        {
            depth.dx11BokehTexture = new Texture2D(64, 64, TextureFormat.DXT5, false);
            depth.dx11BokehTexture.LoadImage(ConstantValues.byteHexShapePng);
            depth.dx11BokehTexture.Apply();
        }

        public static void Update(DepthOfFieldPane depthOfFieldPane)
        {
            // FIXME: Becomes null after scene transition?
            if(depthOfFieldEffect == null)
            {
                depthOfFieldEffect = Util.GetComponentVar<DepthOfFieldScatter, DepthOfFieldDef>(depthOfFieldEffect);
            }

            if (Instances.needEffectWindowReload == true)
                depthOfFieldPane.IsEnabled = depthOfFieldEffect.enabled;
            else
                depthOfFieldEffect.enabled = depthOfFieldPane.IsEnabled;

            depthOfFieldEffect.visualizeFocus = depthOfFieldPane.VisualizeFocusValue;
            depthOfFieldEffect.focalLength = depthOfFieldPane.FocalLengthValue;
            depthOfFieldEffect.focalSize = depthOfFieldPane.FocalSizeValue;
            depthOfFieldEffect.aperture = depthOfFieldPane.ApertureValue;

            depthOfFieldEffect.blurType = depthOfFieldPane.BlurTypeValue;
            depthOfFieldEffect.blurSampleCount = depthOfFieldPane.BlurSampleCountValue;

            depthOfFieldEffect.maxBlurSize = depthOfFieldPane.MaxBlurSizeValue;
            depthOfFieldEffect.highResolution = depthOfFieldPane.HighResolutionValue;

            depthOfFieldEffect.nearBlur = depthOfFieldPane.NearBlurValue;
            depthOfFieldEffect.foregroundOverlap = depthOfFieldPane.ForegroundOverlapValue;

            depthOfFieldEffect.dx11BokehScale = depthOfFieldPane.Dx11BokehScaleValue;
            depthOfFieldEffect.dx11BokehIntensity = depthOfFieldPane.Dx11BokehIntensityValue;

            depthOfFieldEffect.dx11BokehThreshhold = depthOfFieldPane.Dx11BokehThreshholdValue;
            depthOfFieldEffect.dx11SpawnHeuristic = depthOfFieldPane.Dx11SpawnHeuristicValue;

            focusedMaid = -1;
        }

        public static void Reset()
        {
            if (depthOfFieldEffect == null)
                return;

            depthOfFieldEffect.visualizeFocus = visualizeFocus;
            depthOfFieldEffect.focalLength = focalLength;
            depthOfFieldEffect.focalSize = focalSize;
            depthOfFieldEffect.aperture = aperture;

            depthOfFieldEffect.blurType = blurType;
            depthOfFieldEffect.blurSampleCount = blurSampleCount;

            depthOfFieldEffect.maxBlurSize = maxBlurSize;
            depthOfFieldEffect.highResolution = highResolution;

            depthOfFieldEffect.nearBlur = nearBlur;
            depthOfFieldEffect.foregroundOverlap = foregroundOverlap;

            depthOfFieldEffect.dx11BokehScale = dx11BokehScale;
            depthOfFieldEffect.dx11BokehIntensity = dx11BokehIntensity;

            depthOfFieldEffect.dx11BokehThreshhold = dx11BokehThreshhold;
            depthOfFieldEffect.dx11SpawnHeuristic = dx11SpawnHeuristic;

            depthOfFieldEffect.focalTransform = focalTransform;
            focusedMaid = -1;
        }

        public static void SetTransform()
        {
            depthOfFieldEffect.focalTransform = null;
            DepthOfFieldDef.focusedMaid = -1;
        }
        public static void SetTransform(MaidManager _mm)
        {
            if (_mm == null)
                return;

            depthOfFieldEffect.focalTransform = _mm.GetTransform();
            DepthOfFieldDef.focusedMaid = _mm.iCurrent;
        }

        public static void OnMaidManagerFind(MaidManager mm)
        {
            if (depthOfFieldEffect == null)
                return;

            mm.Select(DepthOfFieldDef.focusedMaid);
            depthOfFieldEffect.focalTransform = mm.GetTransform();
        }
    }
}
