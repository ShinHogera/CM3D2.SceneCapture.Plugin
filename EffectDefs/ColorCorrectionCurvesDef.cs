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
    internal class ColorCorrectionCurvesDef
    {
        public static ColorCorrectionCurves colorCurvesEffect;

        public static ColorCorrectionMode mode { get; set; }
        public static bool selectiveCc { get; set; }
        public static float saturation { get; set; }
        public static AnimationCurve redChannel { get; set; }
        public static AnimationCurve greenChannel { get; set; }
        public static AnimationCurve blueChannel { get; set; }

        public static AnimationCurve depthRedChannel { get; set; }
        public static AnimationCurve depthGreenChannel { get; set; }
        public static AnimationCurve depthBlueChannel { get; set; }
        public static AnimationCurve zCurve { get; set; }

        public static Color selectiveFromColor { get; set; }
        public static Color selectiveToColor { get; set; }

        //public static bool updateTextures { get; set; }
        public static bool useDepthCorrection { get; set; }

        static ColorCorrectionCurvesDef()
        {
            if(colorCurvesEffect == null)
            {
                colorCurvesEffect = Util.GetComponentVar<ColorCorrectionCurves, ColorCorrectionCurvesDef>(colorCurvesEffect);
            }

            mode = ColorCorrectionMode.Simple;
            selectiveCc = false;
            saturation = 1f;

            Keyframe[] keys = new Keyframe[2];
            keys[0] = new Keyframe(0f, 0f, 0f, 1f);
            keys[1] = new Keyframe(1f, 1f, 1f, 0f);

            redChannel = new AnimationCurve(keys);
            greenChannel = new AnimationCurve(keys);
            blueChannel = new AnimationCurve(keys);

            depthRedChannel = new AnimationCurve(keys);
            depthGreenChannel = new AnimationCurve(keys);
            depthBlueChannel = new AnimationCurve(keys);
            zCurve = new AnimationCurve(keys);

            selectiveFromColor = Color.white;
            selectiveToColor = Color.white;

            useDepthCorrection = false;

        }

        public static void InitMemberByInstance(ColorCorrectionCurves colorcurves)
        {
            mode = colorcurves.mode;
            selectiveCc = colorcurves.selectiveCc;
            saturation = colorcurves.saturation;

            redChannel = colorcurves.redChannel;
            greenChannel = colorcurves.greenChannel;
            blueChannel = colorcurves.blueChannel;

            depthRedChannel = colorcurves.depthRedChannel;
            depthGreenChannel = colorcurves.depthGreenChannel;
            depthBlueChannel = colorcurves.depthBlueChannel;
            zCurve = colorcurves.zCurve;

            selectiveFromColor = colorcurves.selectiveFromColor;
            selectiveToColor = colorcurves.selectiveToColor;

            useDepthCorrection = colorcurves.useDepthCorrection;
        }

        public static void InitExtra(ColorCorrectionCurves colorcurves)
        {
            Keyframe[] keys = new Keyframe[2];
            keys[0] = new Keyframe(0f, 0f, 0f, 1f);
            keys[1] = new Keyframe(1f, 1f, 1f, 0f);

            colorcurves.redChannel = new AnimationCurve(keys);
            colorcurves.greenChannel = new AnimationCurve(keys);
            colorcurves.blueChannel = new AnimationCurve(keys);

            colorcurves.depthRedChannel = new AnimationCurve(keys);
            colorcurves.depthGreenChannel = new AnimationCurve(keys);
            colorcurves.depthBlueChannel = new AnimationCurve(keys);
            colorcurves.zCurve = new AnimationCurve(keys);
        }

        public static void Update(ColorCorrectionCurvesPane colorCorrectionCurvesPane)
        {
            if (Instances.needEffectWindowReload == true)
                colorCorrectionCurvesPane.IsEnabled = colorCurvesEffect.enabled;
            else
                colorCurvesEffect.enabled = colorCorrectionCurvesPane.IsEnabled;

            colorCurvesEffect.mode = colorCorrectionCurvesPane.ModeValue;
            colorCurvesEffect.selectiveCc = colorCorrectionCurvesPane.SelectiveCcValue;
            colorCurvesEffect.saturation = colorCorrectionCurvesPane.SaturationValue;

            colorCurvesEffect.redChannel = colorCorrectionCurvesPane.RedChannelValue;
            colorCurvesEffect.greenChannel = colorCorrectionCurvesPane.GreenChannelValue;
            colorCurvesEffect.blueChannel = colorCorrectionCurvesPane.BlueChannelValue;

            colorCurvesEffect.depthRedChannel = colorCorrectionCurvesPane.DepthRedChannelValue;
            colorCurvesEffect.depthGreenChannel = colorCorrectionCurvesPane.DepthGreenChannelValue;
            colorCurvesEffect.depthBlueChannel = colorCorrectionCurvesPane.DepthBlueChannelValue;
            colorCurvesEffect.zCurve = colorCorrectionCurvesPane.ZCurveValue;

            colorCurvesEffect.selectiveFromColor = colorCorrectionCurvesPane.SelectiveFromColorValue;
            colorCurvesEffect.selectiveToColor = colorCorrectionCurvesPane.SelectiveToColorValue;

            colorCurvesEffect.useDepthCorrection = colorCorrectionCurvesPane.UseDepthCorrectionValue;

            colorCurvesEffect.UpdateParameters();
        }

        public static void Reset()
        {
            if (colorCurvesEffect == null)
                return;

            colorCurvesEffect.mode = mode;
            colorCurvesEffect.selectiveCc = selectiveCc;
            colorCurvesEffect.saturation = saturation;

            colorCurvesEffect.redChannel = redChannel;
            colorCurvesEffect.greenChannel = greenChannel;
            colorCurvesEffect.blueChannel = blueChannel;

            colorCurvesEffect.depthRedChannel = depthRedChannel;
            colorCurvesEffect.depthGreenChannel = depthGreenChannel;
            colorCurvesEffect.depthBlueChannel = depthBlueChannel;
            colorCurvesEffect.zCurve = zCurve;

            colorCurvesEffect.selectiveFromColor = selectiveFromColor;
            colorCurvesEffect.selectiveToColor = selectiveToColor;

            colorCurvesEffect.useDepthCorrection = useDepthCorrection;

            OnLoad();
        }

        public static void OnLoad()
        {
            if (colorCurvesEffect == null)
                return;

            colorCurvesEffect.UpdateParameters();
        }
    }
}
