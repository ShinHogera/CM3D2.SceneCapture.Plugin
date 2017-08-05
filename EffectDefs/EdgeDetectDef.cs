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
    internal class EdgeDetectDef
    {
        public static EdgeDetectEffectNormals edgeDetectEffect;

        public static EdgeDetectMode mode { get; set; }
        public static float sensitivityNormals { get; set; }
        public static float sensitivityDepth { get; set; }
        public static float sampleDist { get; set; }
        public static float lumThreshhold { get; set; }
        public static float edgeExp { get; set; }
        public static float edgesOnly { get; set; }
        public static Color edgesOnlyBgColor { get; set; }

        static EdgeDetectDef()
        {
            if(edgeDetectEffect == null)
            {
                edgeDetectEffect = Util.GetComponentVar<EdgeDetectEffectNormals, EdgeDetectDef>(edgeDetectEffect);
            }

            mode = EdgeDetectMode.SobelDepthThin;
            sensitivityNormals = 1f;
            sensitivityDepth = 1f;
            sampleDist = 1f;
            lumThreshhold = 0.2f;
            edgeExp = 1f;
            edgesOnly = 0f;
            edgesOnlyBgColor = Color.white;
        }

        public static void InitMemberByInstance(EdgeDetectEffectNormals ed)
        {
            mode = ed.mode;
            sensitivityNormals = ed.sensitivityNormals;
            sensitivityDepth = ed.sensitivityDepth;
            sampleDist = ed.sampleDist;
            lumThreshhold = ed.lumThreshhold;
            edgeExp = ed.edgeExp;
            edgesOnly = ed.edgesOnly;
            edgesOnlyBgColor = ed.edgesOnlyBgColor;
        }

        public static void Update(EdgeDetectPane edgeDetectPane)
        {
            if (Instances.needEffectWindowReload == true)
                edgeDetectPane.IsEnabled = edgeDetectEffect.enabled;
            else
                edgeDetectEffect.enabled = edgeDetectPane.IsEnabled;

            edgeDetectEffect.mode = edgeDetectPane.ModeValue;
            edgeDetectEffect.sensitivityNormals = edgeDetectPane.SensitivityNormalsValue;
            edgeDetectEffect.sensitivityDepth = edgeDetectPane.SensitivityDepthValue;
            edgeDetectEffect.sampleDist = edgeDetectPane.SampleDistValue;
            edgeDetectEffect.lumThreshhold = edgeDetectPane.LumThresholdValue;
            edgeDetectEffect.edgeExp = edgeDetectPane.EdgeExpValue;
            edgeDetectEffect.edgesOnly = edgeDetectPane.EdgesOnlyValue;
            edgeDetectEffect.edgesOnlyBgColor = edgeDetectPane.EdgesOnlyBgColorValue;
        }

        public static void Reset()
        {
            if (edgeDetectEffect == null)
                return;

            edgeDetectEffect.mode = mode;
            edgeDetectEffect.sensitivityNormals = sensitivityNormals;
            edgeDetectEffect.sensitivityDepth = sensitivityDepth;
            edgeDetectEffect.sampleDist = sampleDist;
            edgeDetectEffect.lumThreshhold = lumThreshhold;
            edgeDetectEffect.edgeExp = edgeExp;
            edgeDetectEffect.edgesOnly = edgesOnly;
            edgeDetectEffect.edgesOnlyBgColor = edgesOnlyBgColor;

            OnLoad();
        }

        public static void OnLoad()
        {
            if (edgeDetectEffect == null)
                return;

            edgeDetectEffect.SetCameraFlag();
        }
    }
}
