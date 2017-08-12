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
    internal class LookupFilterDef
    {
        public static LookupFilter lookupFilterEffect;

        public static float amount { get; set; }
        public static bool forceCompatibility { get; set; }
        public static Texture lookupTexture { get; set; }
        public static string lookupTextureFile { get; set; }

        public LookupFilterDef()
        {
            if( lookupFilterEffect == null)
            {
                lookupFilterEffect = Util.GetComponentVar<LookupFilter, LookupFilterDef>(lookupFilterEffect);
            }

            amount = 1f;
            forceCompatibility = false;

            // TODO: Default
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            lookupTexture = (Texture)tex;
            lookupTextureFile = "";
        }

        public void InitMemberByInstance(LookupFilter lookupFilter)
        {
            amount = lookupFilter.amount;
            forceCompatibility = lookupFilter.forceCompatibility;
            lookupTexture = lookupFilter.lookupTexture;
        }

        public static void Update(LookupFilterPane lookupFilterPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                lookupFilterPane.IsEnabled = lookupFilterEffect.enabled;
            }
            else
            {
                lookupFilterEffect.enabled = lookupFilterPane.IsEnabled;
            }

            lookupFilterEffect.amount = lookupFilterPane.AmountValue;
            lookupFilterEffect.forceCompatibility = lookupFilterPane.ForceCompatibilityValue;
            lookupFilterEffect.lookupTexture = lookupFilterPane.LookupTextureValue;

            lookupTextureFile = lookupFilterPane.LookupTextureFileValue;
        }

        public static void Reset()
        {
            if( lookupFilterEffect == null )
                return;

            lookupFilterEffect.amount = amount;
            lookupFilterEffect.forceCompatibility = forceCompatibility;
            lookupFilterEffect.lookupTexture = (Texture2D)lookupTexture;
        }
    }
}

