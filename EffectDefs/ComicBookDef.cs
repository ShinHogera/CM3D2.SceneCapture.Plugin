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
    internal class ComicBookDef
    {
        public static ComicBook comicBookEffect;

        public static float stripAngle { get; set; }
        public static float stripDensity { get; set; }
        public static float stripThickness { get; set; }
        public static Vector2 stripLimits { get; set; }
        public static Color stripInnerColor { get; set; }
        public static Color stripOuterColor { get; set; }
        public static Color fillColor { get; set; }
        public static Color backgroundColor { get; set; }
        public static bool edgeDetection { get; set; }
        public static float edgeThreshold { get; set; }
        public static Color edgeColor { get; set; }
        public static float amount { get; set; }

        public ComicBookDef()
        {
            if( comicBookEffect == null)
            {
                comicBookEffect = Util.GetComponentVar<ComicBook, ComicBookDef>(comicBookEffect);
            }

            stripAngle = 0.6f;
            stripDensity = 180f;
            stripThickness = 0.5f;
            stripLimits = new Vector2(0.25f, 0.4f);
            stripInnerColor = new Color(0.3f, 0.3f, 0.3f);
            stripOuterColor = new Color(0.8f, 0.8f, 0.8f);
            fillColor = new Color(0.1f, 0.1f, 0.1f);
            backgroundColor = Color.white;
            edgeDetection = false;
            edgeThreshold = 5f;
            edgeColor = Color.black;
            amount = 1f;
        }

        public void InitMemberByInstance(ComicBook comicBook)
        {
            stripAngle = comicBook.stripAngle;
            stripDensity = comicBook.stripDensity;
            stripThickness = comicBook.stripThickness;
            stripLimits = comicBook.stripLimits;
            stripInnerColor = comicBook.stripInnerColor;
            stripOuterColor = comicBook.stripOuterColor;
            fillColor = comicBook.fillColor;
            backgroundColor = comicBook.backgroundColor;
            edgeDetection = comicBook.edgeDetection;
            edgeThreshold = comicBook.edgeThreshold;
            edgeColor = comicBook.edgeColor;
            amount = comicBook.amount;
        }

        public static void Update(ComicBookPane comicBookPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                comicBookPane.IsEnabled = comicBookEffect.enabled;
            }
            else
            {
                comicBookEffect.enabled = comicBookPane.IsEnabled;
            }

            comicBookEffect.stripAngle = comicBookPane.StripAngleValue;
            comicBookEffect.stripDensity = comicBookPane.StripDensityValue;
            comicBookEffect.stripThickness = comicBookPane.StripThicknessValue;
            comicBookEffect.stripLimits = comicBookPane.StripLimitsValue;
            comicBookEffect.stripInnerColor = comicBookPane.StripInnerColorValue;
            comicBookEffect.stripOuterColor = comicBookPane.StripOuterColorValue;
            comicBookEffect.fillColor = comicBookPane.FillColorValue;
            comicBookEffect.backgroundColor = comicBookPane.BackgroundColorValue;
            comicBookEffect.edgeDetection = comicBookPane.EdgeDetectionValue;
            comicBookEffect.edgeThreshold = comicBookPane.EdgeThresholdValue;
            comicBookEffect.edgeColor = comicBookPane.EdgeColorValue;
            comicBookEffect.amount = comicBookPane.AmountValue;
        }

        public static void Reset()
        {
            if( comicBookEffect == null )
                return;

            comicBookEffect.stripAngle = stripAngle;
            comicBookEffect.stripDensity = stripDensity;
            comicBookEffect.stripThickness = stripThickness;
            comicBookEffect.stripLimits = stripLimits;
            comicBookEffect.stripInnerColor = stripInnerColor;
            comicBookEffect.stripOuterColor = stripOuterColor;
            comicBookEffect.fillColor = fillColor;
            comicBookEffect.backgroundColor = backgroundColor;
            comicBookEffect.edgeDetection = edgeDetection;
            comicBookEffect.edgeThreshold = edgeThreshold;
            comicBookEffect.edgeColor = edgeColor;
            comicBookEffect.amount = amount;
        }
    }
}

