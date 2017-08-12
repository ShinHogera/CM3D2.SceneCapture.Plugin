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
    internal class ShadowsMidtonesHighlightsDef
    {
        public static ShadowsMidtonesHighlights shadowsMidtonesHighlightsEffect;

        public static ShadowsMidtonesHighlights.ColorMode mode { get; set; }
        public static Color shadows { get; set; }
        public static Color midtones { get; set; }
        public static Color highlights { get; set; }
        public static float amount { get; set; }

        public ShadowsMidtonesHighlightsDef()
        {
            if( shadowsMidtonesHighlightsEffect == null)
            {
                shadowsMidtonesHighlightsEffect = Util.GetComponentVar<ShadowsMidtonesHighlights, ShadowsMidtonesHighlightsDef>(shadowsMidtonesHighlightsEffect);
            }

            mode = ShadowsMidtonesHighlights.ColorMode.LiftGammaGain;
            shadows = new Color(1f, 1f, 1f, 0.5f);
            midtones = new Color(1f, 1f, 1f, 0.5f);
            highlights = new Color(1f, 1f, 1f, 0.5f);
            amount = 1f;
        }

        public void InitMemberByInstance(ShadowsMidtonesHighlights shadowsMidtonesHighlights)
        {
            mode = shadowsMidtonesHighlights.mode;
            shadows = shadowsMidtonesHighlights.shadows;
            midtones = shadowsMidtonesHighlights.midtones;
            highlights = shadowsMidtonesHighlights.highlights;
            amount = shadowsMidtonesHighlights.amount;
        }

        public static void Update(ShadowsMidtonesHighlightsPane shadowsMidtonesHighlightsPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                shadowsMidtonesHighlightsPane.IsEnabled = shadowsMidtonesHighlightsEffect.enabled;
            }
            else
            {
                shadowsMidtonesHighlightsEffect.enabled = shadowsMidtonesHighlightsPane.IsEnabled;
            }

            shadowsMidtonesHighlightsEffect.mode = shadowsMidtonesHighlightsPane.ModeValue;
            shadowsMidtonesHighlightsEffect.shadows = shadowsMidtonesHighlightsPane.ShadowsValue;
            shadowsMidtonesHighlightsEffect.midtones = shadowsMidtonesHighlightsPane.MidtonesValue;
            shadowsMidtonesHighlightsEffect.highlights = shadowsMidtonesHighlightsPane.HighlightsValue;
            shadowsMidtonesHighlightsEffect.amount = shadowsMidtonesHighlightsPane.AmountValue;
        }

        public static void Reset()
        {
            if( shadowsMidtonesHighlightsEffect == null )
                return;

            shadowsMidtonesHighlightsEffect.mode = mode;
            shadowsMidtonesHighlightsEffect.shadows = shadows;
            shadowsMidtonesHighlightsEffect.midtones = midtones;
            shadowsMidtonesHighlightsEffect.highlights = highlights;
            shadowsMidtonesHighlightsEffect.amount = amount;
        }
    }
}

