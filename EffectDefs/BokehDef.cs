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
    internal class BokehDef
    {
        public static Bokeh bokehEffect;

        public static Transform pointOfFocus { get; set; }
        public static float focusDistance { get; set; }
        public static float fNumber { get; set; }
        public static bool useCameraFov { get; set; }
        public static float focalLength { get; set; }
        public static Bokeh.KernelSize kernelSize { get; set; }
        public static bool visualize { get; set; }
        // TODO: Support maid focus

        public static bool isDrag { get; set; }
        public static bool transformFromMaid { get; set; }
        public static int focusedMaid { get; set; }

        static BokehDef()
        {
            if(bokehEffect == null)
            {
                bokehEffect = Util.GetComponentVar<Bokeh, BokehDef>(bokehEffect);
            }

            focusDistance = 10.0f;
            fNumber = 1.4f;
            useCameraFov = true;
            focalLength = 0.05f;
            kernelSize = Bokeh.KernelSize.Medium;
            visualize = false;

            pointOfFocus = null;

            focusedMaid = -1;
        }

        public static void InitMemberByInstance(Bokeh bokeh)
        {
            pointOfFocus = bokehEffect.pointOfFocus;
            focusDistance = bokehEffect.focusDistance;
            fNumber = bokehEffect.fNumber;
            useCameraFov = bokehEffect.useCameraFov;
            focalLength = bokehEffect.focalLength;
            kernelSize = bokehEffect.kernelSize;
            visualize = bokehEffect.visualize;
        }

        public static void Update(BokehPane bokehPane)
        {
            if (Instances.needEffectWindowReload == true)
                bokehPane.IsEnabled = bokehEffect.enabled;
            else
                bokehEffect.enabled = bokehPane.IsEnabled;

            bokehEffect.focusDistance = bokehPane.FocusDistanceValue;
            bokehEffect.fNumber = bokehPane.FNumberValue;
            bokehEffect.useCameraFov = bokehPane.UseCameraFovValue;
            bokehEffect.focalLength = bokehPane.FocalLengthValue;
            bokehEffect.kernelSize = bokehPane.KernelSizeValue;
            bokehEffect.visualize = bokehPane.VisualizeValue;
        }

        public static void Reset()
        {
            if (bokehEffect == null)
                return;

            bokehEffect.pointOfFocus = pointOfFocus;
            bokehEffect.focusDistance = focusDistance;
            bokehEffect.fNumber = fNumber;
            bokehEffect.useCameraFov = useCameraFov;
            bokehEffect.focalLength = focalLength;
            bokehEffect.kernelSize = kernelSize;
            bokehEffect.visualize = visualize;
        }

        public static void SetTransform()
        {
            bokehEffect.pointOfFocus = null;
            focusedMaid = -1;
        }

        public static void SetTransform(MaidManager _mm)
        {
            if (_mm == null)
                return;

            bokehEffect.pointOfFocus = _mm.GetTransform();
            BokehDef.focusedMaid = _mm.iCurrent;
        }

        public static void OnMaidManagerFind(MaidManager mm)
        {
            if (bokehEffect == null)
                return;

            mm.Select(BokehDef.focusedMaid);
            bokehEffect.pointOfFocus = mm.GetTransform();
        }
    }
}
