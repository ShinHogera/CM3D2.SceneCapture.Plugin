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
    internal class BloomDef
    {
        public static Bloom bloomEffect;

        public static Bloom.HDRBloomMode hdr { get; set; }
        public static Bloom.BloomScreenBlendMode screenBlendMode { get; set; }
        public static Bloom.TweakMode tweakMode { get; set; }
        public static Bloom.BloomQuality quality { get; set; }

        public static int bloomBlurIterations { get; set; }
        public static float bloomIntensity { get; set; }
        public static float bloomThreshhold { get; set; }

        public static float blurWidth { get; set; }

        public static float flareRotation { get; set; }

        public static float hollyStretchWidth { get; set; }
        public static int hollywoodFlareBlurIterations { get; set; }

        public static Bloom.LensFlareStyle lensflareMode { get; set; }
        public static float lensflareIntensity { get; set; }
        public static float lensFlareSaturation { get; set; }
        public static float lensflareThreshhold { get; set; }
        public static Color flareColorA { get; set; }
        public static Color flareColorB { get; set; }
        public static Color flareColorC { get; set; }
        public static Color flareColorD { get; set; }
        //public static Texture2D lensFlareVignetteMask;
        public static float sepBlurSpread { get; set; }

        public static Color bloomThreshholdColor { get; set; }

        //
        public static Bloom.HDRBloomMode _hdr { get; set; }
        public static Bloom.BloomScreenBlendMode _screenBlendMode { get; set; }
        public static Bloom.TweakMode _tweakMode { get; set; }
        public static Bloom.BloomQuality _quality { get; set; }

        public static int _bloomBlurIterations { get; set; }
        public static float _bloomIntensity { get; set; }
        public static float _bloomThreshhold { get; set; }

        public static float _blurWidth { get; set; }

        public static float _flareRotation { get; set; }

        public static float _hollyStretchWidth { get; set; }
        public static int _hollywoodFlareBlurIterations { get; set; }

        public static Bloom.LensFlareStyle _lensflareMode { get; set; }
        public static float _lensflareIntensity { get; set; }
        public static float _lensFlareSaturation { get; set; }
        public static float _lensflareThreshhold { get; set; }
        public static Color _flareColorA { get; set; }
        public static Color _flareColorB { get; set; }
        public static Color _flareColorC { get; set; }
        public static Color _flareColorD { get; set; }
        public static float _sepBlurSpread { get; set; }

        public static Color _bloomThreshholdColor { get; set; }

        //
        public static bool backedUp { get; set; }
        public static bool enable { get; set; }
        public static bool enabledInPane { get; set; }
        public static Texture2D texThhreshholdColor { get; set; }
        public static Texture2D texFlareColorA { get; set; }
        public static Texture2D texFlareColorB { get; set; }
        public static Texture2D texFlareColorC { get; set; }
        public static Texture2D texFlareColorD { get; set; }

        static BloomDef()
        {
            hdr = Bloom.HDRBloomMode.Auto;
            screenBlendMode = Bloom.BloomScreenBlendMode.Screen;
            tweakMode = Bloom.TweakMode.Basic;
            quality = Bloom.BloomQuality.High;

            bloomBlurIterations = 3;
            bloomIntensity = 2.85f;
            bloomThreshhold = 0.7f;
            blurWidth = 1f;
            flareRotation = 0f;
            hollyStretchWidth = 2.5f;
            hollywoodFlareBlurIterations = 2;
            lensflareIntensity = 0f;

            lensflareMode = Bloom.LensFlareStyle.Anamorphic;
            lensFlareSaturation = 0.75f;
            lensflareThreshhold = 0.3f;
            sepBlurSpread = 3.48f;
            bloomThreshholdColor = Color.white;

            flareColorA = Color.white;
            flareColorB = Color.white;
            flareColorC = Color.white;
            flareColorD = Color.white;

            texThhreshholdColor = new Texture2D(1, 1);
            texThhreshholdColor.SetPixel(0, 0, Color.white);
            texThhreshholdColor.Apply();

            texFlareColorA = new Texture2D(1, 1);
            texFlareColorA.SetPixel(0, 0, Color.white);
            texFlareColorA.Apply();
            texFlareColorB = new Texture2D(1, 1);
            texFlareColorB.SetPixel(0, 0, Color.white);
            texFlareColorB.Apply();
            texFlareColorC = new Texture2D(1, 1);
            texFlareColorC.SetPixel(0, 0, Color.white);
            texFlareColorC.Apply();
            texFlareColorD = new Texture2D(1, 1);
            texFlareColorD.SetPixel(0, 0, Color.white);
            texFlareColorD.Apply();

            enable = false;
            backedUp = false;
            enabledInPane = false;
        }

        public static void InitMemberByInstance(Bloom bloom)
        {
            hdr = bloom.hdr;
            screenBlendMode = bloom.screenBlendMode;
            tweakMode = bloom.tweakMode;
            quality = bloom.quality;

            bloomBlurIterations = bloom.bloomBlurIterations;
            bloomIntensity = bloom.bloomIntensity;
            bloomThreshhold = bloom.bloomThreshhold;
            blurWidth = bloom.blurWidth;
            flareRotation = bloom.flareRotation;
            hollyStretchWidth = bloom.hollyStretchWidth;
            hollywoodFlareBlurIterations = bloom.hollywoodFlareBlurIterations;
            lensflareIntensity = bloom.lensflareIntensity;

            lensflareMode = bloom.lensflareMode;
            lensFlareSaturation = bloom.lensFlareSaturation;
            lensflareThreshhold = bloom.lensflareThreshhold;
            sepBlurSpread = bloom.sepBlurSpread;
            bloomThreshholdColor = bloom.bloomThreshholdColor;

            flareColorA = bloom.flareColorA;
            flareColorB = bloom.flareColorB;
            flareColorC = bloom.flareColorC;
            flareColorD = bloom.flareColorD;

            texThhreshholdColor.SetPixel(0, 0, bloomThreshholdColor);
            texThhreshholdColor.Apply();

            texFlareColorA.SetPixel(0, 0, flareColorA);
            texFlareColorA.Apply();
            texFlareColorB.SetPixel(0, 0, flareColorB);
            texFlareColorB.Apply();
            texFlareColorC.SetPixel(0, 0, flareColorC);
            texFlareColorC.Apply();
            texFlareColorD.SetPixel(0, 0, flareColorD);
            texFlareColorD.Apply();

            BloomDef.enable = true;
        }

        public static void InitExtra(Bloom bloom)
        {
            BloomDef.enable = true;
        }

        public static float ConvertIntensity(float f)
        {
            return (float)((double)f * 100 * 0.00999999977648258);
        }

        public static void Setup()
        {
            try
            {
                bloomEffect = Util.GetComponentVar<Bloom, BloomDef>(bloomEffect);
                BackUp();
            }
            catch (Exception e) {
                Debug.Log("Can't set bloom!");
                Debug.Log( e );
            }
        }

        public static void Update(BloomPane bloomPane)
        {
            // this has to happen after the game has initialized, due to built-in bloom
            if(bloomEffect == null)
            {
                Setup();
            }

            if (Instances.needEffectWindowReload == true)
            {
                bloomPane.IsEnabled = enabledInPane;
            }

            if(!BloomDef.enabledInPane)
            {
                return;
            }

            if (Instances.needEffectWindowReload == true)
            {
                // Maximum 'real' bloom value is 2.85, while the CMSystem max is 100, so 100/2.85 ~= 35
                float intensity = bloomEffect.bloomIntensity * 35.0877192982f;
                GameMain.Instance.CMSystem.BloomValue = (int)(intensity);
                bloomPane.BloomIntensityValue = intensity;
            }
            else
            {
                GameMain.Instance.CMSystem.BloomValue = (int)(bloomPane.BloomIntensityValue);
            }

            bloomEffect.hdr = bloomPane.HdrValue;
            bloomEffect.screenBlendMode = bloomPane.ScreenBlendModeValue;
            bloomEffect.tweakMode = bloomPane.TweakModeValue;
            bloomEffect.quality = bloomPane.QualityValue;

            bloomEffect.bloomBlurIterations = bloomPane.BloomBlurIterationsValue;
            bloomEffect.bloomThreshhold = bloomPane.BloomThreshholdValue;
            bloomEffect.blurWidth = bloomPane.BlurWidthValue;
            bloomEffect.flareRotation = bloomPane.FlareRotationValue;
            bloomEffect.hollyStretchWidth = bloomPane.HollyStretchWidthValue;
            bloomEffect.hollywoodFlareBlurIterations = bloomPane.HollywoodFlareBlurIterationsValue;
            bloomEffect.lensflareIntensity = bloomPane.LensflareIntensityValue;

            bloomEffect.lensflareMode = bloomPane.LensflareModeValue;
            bloomEffect.lensFlareSaturation = bloomPane.LensFlareSaturationValue;
            bloomEffect.lensflareThreshhold = bloomPane.LensflareThreshholdValue;
            bloomEffect.sepBlurSpread = bloomPane.SepBlurSpreadValue;
            bloomEffect.bloomThreshholdColor = bloomPane.BloomThreshholdColorValue;

            bloomEffect.flareColorA = bloomPane.FlareColorAValue;
            bloomEffect.flareColorB = bloomPane.FlareColorBValue;
            bloomEffect.flareColorC = bloomPane.FlareColorCValue;
            bloomEffect.flareColorD = bloomPane.FlareColorDValue;

            texThhreshholdColor.SetPixel(0, 0, bloomThreshholdColor);
            texThhreshholdColor.Apply();

            texFlareColorA.SetPixel(0, 0, flareColorA);
            texFlareColorA.Apply();
            texFlareColorB.SetPixel(0, 0, flareColorB);
            texFlareColorB.Apply();
            texFlareColorC.SetPixel(0, 0, flareColorC);
            texFlareColorC.Apply();
            texFlareColorD.SetPixel(0, 0, flareColorD);
            texFlareColorD.Apply();
        }

        public static void Reset()
        {
            if (bloomEffect == null)
                return;

            bloomEffect.hdr = hdr;
            bloomEffect.screenBlendMode = screenBlendMode;
            bloomEffect.tweakMode = tweakMode;
            bloomEffect.quality = quality;

            bloomEffect.bloomBlurIterations = bloomBlurIterations;
            bloomEffect.bloomIntensity = bloomIntensity;
            bloomEffect.bloomThreshhold = bloomThreshhold;
            bloomEffect.blurWidth = blurWidth;
            bloomEffect.flareRotation = flareRotation;
            bloomEffect.hollyStretchWidth = hollyStretchWidth;
            bloomEffect.hollywoodFlareBlurIterations = hollywoodFlareBlurIterations;
            bloomEffect.lensflareIntensity = lensflareIntensity;

            bloomEffect.lensflareMode = lensflareMode;
            bloomEffect.lensFlareSaturation = lensFlareSaturation;
            bloomEffect.lensflareThreshhold = lensflareThreshhold;
            bloomEffect.sepBlurSpread = sepBlurSpread;
            bloomEffect.bloomThreshholdColor = bloomThreshholdColor;

            bloomEffect.flareColorA = flareColorA;
            bloomEffect.flareColorB = flareColorB;
            bloomEffect.flareColorC = flareColorC;
            bloomEffect.flareColorD = flareColorD;

            GameMain.Instance.CMSystem.BloomValue = (int)Math.Ceiling(bloomIntensity * (100 / 2.85));

            OnLoad();
        }

        public static void OnLoad()
        {
            if (bloomEffect == null)
                return;

            texThhreshholdColor.SetPixel(0, 0, bloomEffect.bloomThreshholdColor);
            texThhreshholdColor.Apply();

            texFlareColorA.SetPixel(0, 0, bloomEffect.flareColorA);
            texFlareColorA.Apply();
            texFlareColorB.SetPixel(0, 0, bloomEffect.flareColorB);
            texFlareColorB.Apply();
            texFlareColorC.SetPixel(0, 0, bloomEffect.flareColorC);
            texFlareColorC.Apply();
            texFlareColorD.SetPixel(0, 0, bloomEffect.flareColorD);
            texFlareColorD.Apply();

            GameMain.Instance.CMSystem.BloomValue = (int)Math.Ceiling(bloomEffect.bloomIntensity * (100 / 2.85));
        }

        public static void BackUp()
        {
            Debug.Log("Backup");
            if (bloomEffect == null)
                return;

            if (backedUp)
                return;

            _hdr = bloomEffect.hdr;
            _screenBlendMode = bloomEffect.screenBlendMode;
            _tweakMode = bloomEffect.tweakMode;
            _quality = bloomEffect.quality;

            _bloomIntensity = bloomEffect.bloomIntensity;
            _bloomThreshhold = bloomEffect.bloomThreshhold;

            _blurWidth = bloomEffect.blurWidth;

            _flareRotation = bloomEffect.flareRotation;

            _hollyStretchWidth = bloomEffect.hollyStretchWidth;
            _hollywoodFlareBlurIterations = bloomEffect.hollywoodFlareBlurIterations;

            _lensflareMode = bloomEffect.lensflareMode;
            _lensflareIntensity = bloomEffect.lensflareIntensity;
            _lensFlareSaturation = bloomEffect.lensFlareSaturation;
            _lensflareThreshhold = bloomEffect.lensflareThreshhold;
            _flareColorA = bloomEffect.flareColorA;
            _flareColorB = bloomEffect.flareColorB;
            _flareColorC = bloomEffect.flareColorC;
            _flareColorD = bloomEffect.flareColorD;
            _sepBlurSpread = bloomEffect.sepBlurSpread;

            _bloomThreshholdColor = bloomEffect.bloomThreshholdColor;
            backedUp = true;
        }

        public static void Restore()
        {
            if (bloomEffect == null)
                return;

            bloomEffect.hdr = _hdr;
            bloomEffect.screenBlendMode = _screenBlendMode;
            bloomEffect.tweakMode = _tweakMode;
            bloomEffect.quality = _quality;

            bloomEffect.bloomIntensity = _bloomIntensity;
            bloomEffect.bloomThreshhold = _bloomThreshhold;

            bloomEffect.blurWidth = _blurWidth;

            bloomEffect.flareRotation = _flareRotation;

            bloomEffect.hollyStretchWidth = _hollyStretchWidth;
            bloomEffect.hollywoodFlareBlurIterations = _hollywoodFlareBlurIterations;

            bloomEffect.lensflareMode = _lensflareMode;
            bloomEffect.lensflareIntensity = _lensflareIntensity;
            bloomEffect.lensFlareSaturation = _lensFlareSaturation;
            bloomEffect.lensflareThreshhold = _lensflareThreshhold;
            bloomEffect.flareColorA = _flareColorA;
            bloomEffect.flareColorB = _flareColorB;
            bloomEffect.flareColorC = _flareColorC;
            bloomEffect.flareColorD = _flareColorD;
            bloomEffect.sepBlurSpread = _sepBlurSpread;

            bloomEffect.bloomThreshholdColor = _bloomThreshholdColor;

            GameMain.Instance.CMSystem.BloomValue = (int)Math.Ceiling(_bloomIntensity * (100 / 2.85));
        }
    }
}
