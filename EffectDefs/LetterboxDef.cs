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
    internal class LetterboxDef
    {
        public static Letterbox letterboxEffect;

        public static float aspectWidth { get; set; }
        public static float aspectHeight { get; set; }
        public static Color fillColor { get; set; }

        static LetterboxDef()
        {
            if(letterboxEffect == null)
            {
                letterboxEffect = Util.GetComponentVar<Letterbox, LetterboxDef>(letterboxEffect);
            }

            aspectWidth = 21f;
            aspectHeight = 9f;
            fillColor = Color.black;
        }

        public static void InitMemberByInstance(Letterbox lb)
        {
            aspectWidth = lb.aspectWidth;
            aspectHeight = lb.aspectHeight;
            fillColor = lb.fillColor;
        }

        public static void Update(LetterboxPane letterboxPane)
        {
            if(letterboxEffect == null)
            {
                letterboxEffect = Util.GetComponentVar<Letterbox, LetterboxDef>(letterboxEffect);
            }

            if (Instances.needEffectWindowReload == true)
                letterboxPane.IsEnabled = letterboxEffect.enabled;
            else
                letterboxEffect.enabled = letterboxPane.IsEnabled;

            letterboxEffect.aspectWidth = letterboxPane.AspectWidthValue;
            letterboxEffect.aspectHeight = letterboxPane.AspectHeightValue;
            letterboxEffect.fillColor = letterboxPane.FillColorValue;
        }

        public static void Reset()
        {
            if (letterboxEffect == null)
                return;

            letterboxEffect.aspectWidth = aspectWidth;
            letterboxEffect.aspectHeight = aspectHeight;
            letterboxEffect.fillColor = fillColor;
        }
    }
}
