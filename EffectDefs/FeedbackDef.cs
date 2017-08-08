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
    internal class FeedbackDef
    {
        public static Feedback feedbackEffect;

        public static Color color { get; set; }
        public static float offsetX { get; set; }
        public static float offsetY { get; set; }
        public static float rotation { get; set; }
        public static float scale { get; set; }
        public static bool jaggies { get; set; }

        static FeedbackDef()
        {
            if(feedbackEffect == null)
            {
                feedbackEffect = Util.GetComponentVar<Feedback, FeedbackDef>(feedbackEffect);
            }

            color = Color.white;
            offsetX = 0f;
            offsetY = 0f;
            rotation = 0f;
            scale = 1f;
            jaggies = false;
        }

        public static void InitMemberByInstance(Feedback feedback)
        {
            color = feedback.color;
            offsetX = feedback.offsetX;
            offsetY = feedback.offsetY;
            rotation = feedback.rotation;
            scale = feedback.scale;
            jaggies = feedback.jaggies;
        }

        public static void Update(FeedbackPane feedbackPane)
        {
            if(feedbackEffect == null)
            {
                feedbackEffect = Util.GetComponentVar<Feedback, FeedbackDef>(feedbackEffect);
            }

            if (Instances.needEffectWindowReload == true)
                feedbackPane.IsEnabled = feedbackEffect.enabled;
            else
                feedbackEffect.enabled = feedbackPane.IsEnabled;

            feedbackEffect.color = feedbackPane.ColorValue;
            feedbackEffect.offsetX = feedbackPane.OffsetXValue;
            feedbackEffect.offsetY = feedbackPane.OffsetYValue;
            feedbackEffect.rotation = feedbackPane.RotationValue;
            feedbackEffect.scale = feedbackPane.ScaleValue;
            feedbackEffect.jaggies = feedbackPane.JaggiesValue;
        }

        public static void Reset()
        {
            if (feedbackEffect == null)
                return;

            feedbackEffect.color = color;
            feedbackEffect.offsetX = offsetX;
            feedbackEffect.offsetY = offsetY;
            feedbackEffect.rotation = rotation;
            feedbackEffect.scale = scale;
            feedbackEffect.jaggies = jaggies;
        }
    }
}
