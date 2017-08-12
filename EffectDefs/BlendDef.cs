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
    internal class BlendDef
    {
        public static Blend blendEffect;

        public static float amount { get; set; }
        public static Blend.BlendingMode mode { get; set; }
        public static Texture texture { get; set; }
        public static string textureFile { get; set; }

        public BlendDef()
        {
            if( blendEffect == null)
            {
                blendEffect = Util.GetComponentVar<Blend, BlendDef>(blendEffect);
            }

            amount = 1f;
            mode = Blend.BlendingMode.Darken;
            texture = (Texture) new Texture2D(4, 4);

            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            texture = (Texture)tex;
            textureFile = "";
        }

        public void InitMemberByInstance(Blend blend)
        {
            amount = blend.amount;
            mode = blend.mode;
            texture = blend.texture;
        }

        public static void Update(BlendPane blendPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                blendPane.IsEnabled = blendEffect.enabled;
            }
            else
            {
                blendEffect.enabled = blendPane.IsEnabled;
            }

            blendEffect.amount = blendPane.AmountValue;
            blendEffect.mode = blendPane.ModeValue;
            blendEffect.texture = blendPane.TextureValue;

            textureFile = blendPane.TextureFileValue;
        }

        public static void Reset()
        {
            if( blendEffect == null )
                return;

            blendEffect.amount = amount;
            blendEffect.mode = mode;
            blendEffect.texture = texture;
        }
    }
}

