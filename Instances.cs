using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace CM3D2.SceneCapture.Plugin
{
    internal class Instances {
        private static List<LightInfo> lights = null;
        public static bool needLightReload { get; set; }
        private static List<ModelInfo> models = null;
        public static bool needModelReload { get; set; }
        public static bool needMiscReload { get; set; }
        public static bool needEffectWindowReload { get; set; }
        public static string background { get; set; }

        public static bool loadEffects { get; set; }
        public static bool loadLights { get; set; }
        public static bool loadModels { get; set; }
        public static bool loadCamera { get; set; }
        public static bool loadMisc { get; set; }

        public Instances()
        {
            Debug.Log("new instances");
            lights = new List<LightInfo>();
            models = new List<ModelInfo>();

            needLightReload = false;
            needModelReload = false;
            needEffectWindowReload = false;
            needMiscReload = false;

            background = ConstantValues.Background.Keys.First();
            loadEffects = true;
            loadLights = true;
            loadModels = true;
            loadCamera = true;
            loadMisc = true;

            new ColorCorrectionCurvesDef();
            new SepiaDef();
            new GrayscaleDef();
            new ContrastDef();
            new EdgeDetectDef();
            new CreaseDef();
            new AntialiasingDef();
            new NoiseAndGrainDef();
            new BlurDef();
            new DepthOfFieldDef();
            new MotionBlurDef();
            new BloomDef();
            new GlobalFogDef();
            new TiltShiftHdrDef();
            new FisheyeDef();
            new VignettingDef();
            new SunShaftsDef();
            new LensDistortionBlurDef();
            new LetterboxDef();
            new HueFocusDef();
            new ChannelSwapDef();
            new TechnicolorDef();
            new DynamicLookupDef();
            new AnalogGlitchDef();
            new DigitalGlitchDef();
            new BokehDef();
            new ObscuranceDef();
            new AnalogTVDef();
            new BleachBypassDef();
            new BlendDef();
            new BrightnessContrastGammaDef();
            new ChannelMixerDef();
            new ComicBookDef();
            new ContrastVignetteDef();
            new Convolution3x3Def();
            new DitheringDef();
            new DoubleVisionDef();
            new HalftoneDef();
            new IsolineDef();
            new KuwaharaDef();
            new LookupFilterDef();
            new PixelateDef();
            new RGBSplitDef();
            new ShadowsMidtonesHighlightsDef();
            new WaveDistortionDef();
            new WhiteBalanceDef();
            new WiggleDef();
        }

        public static void SetLights(List<LightInfo> lights)
        {
            Instances.lights = lights;
        }

        public static void SetModels(List<ModelInfo> models)
        {
            Instances.models = models;
        }

        public static List<LightInfo> GetLights()
        {
            return Instances.lights;
        }

        public static List<ModelInfo> GetModels()
        {
            return Instances.models;
        }

        public static void ClearEffects()
        {
            Instances.LoadEffects(XDocument.Parse("<Preset><Effects /></Preset>"));
        }

        public static void ClearLights()
        {
            Instances.lights.Clear();

            Light mainLight = GameMain.Instance.MainLight.GetComponent<Light>();

            mainLight.intensity = 0.95f;
            mainLight.range = 10;
            mainLight.color = new Color(1, 1, 1, 1);
            mainLight.transform.eulerAngles = new Vector3(40, 180, 18);
            mainLight.spotAngle = 30;
            mainLight.shadows = LightShadows.Soft;
            mainLight.shadowStrength = 0.098f;
            mainLight.shadowBias = 0.01f;
            mainLight.shadowNormalBias = 0.4f;
        }

        public static void ClearModels()
        {
            Instances.models.Clear();
        }

        public static void ResetCamera()
        {
            // Taken from photo mode defaults
            GameMain.Instance.MainCamera.SetTargetPos(new Vector3(-0.05539433f, 0.95894f, 0.1269088f), true);
            GameMain.Instance.MainCamera.SetDistance(3f, true);
            GameMain.Instance.MainCamera.SetAroundAngle(new Vector2(-180f,11.5528f), true);
            GameMain.Instance.MainCamera.SetTargetOffset(new Vector3(0.0f, 0.0f, 0.0f), true);
            GameMain.Instance.MainCamera.gameObject.GetComponent<Camera>().fieldOfView = 35f;
        }

        public static void ClearMisc()
        {
            Instances.background = "夜伽: サロン:夜";
        }

        public static void ResetAll()
        {
            ClearEffects();
            ClearLights();
            ClearModels();
            ResetCamera();
            ClearMisc();

            needLightReload = true;
            needMiscReload = true;
            needModelReload = true;
            needEffectWindowReload = true;
        }

        public static XElement SaveEffects()
        {
            var xml = new XElement("Effects",
                                   SerializeStatic.SaveDef(typeof(AntialiasingDef), typeof(AntialiasingAsPostEffect)),
                                   SerializeStatic.SaveDef(typeof(BloomDef), typeof(Bloom)),
                                   SerializeStatic.SaveDef(typeof(BlurDef), typeof(Blur)),
                                   SerializeStatic.SaveDef(typeof(ChannelSwapDef), typeof(ChannelSwapper)),
                                   SerializeStatic.SaveDef(typeof(ColorCorrectionCurvesDef), typeof(ColorCorrectionCurves)),
                                   SerializeStatic.SaveDef(typeof(ContrastDef), typeof(ContrastEnhance)),
                                   SerializeStatic.SaveDef(typeof(CreaseDef), typeof(Crease)),
                                   SerializeStatic.SaveDef(typeof(DepthOfFieldDef), typeof(DepthOfFieldScatter)),
                                   SerializeStatic.SaveDef(typeof(DynamicLookupDef), typeof(DynamicLookup)),
                                   SerializeStatic.SaveDef(typeof(EdgeDetectDef), typeof(EdgeDetectEffectNormals)),
                                   SerializeStatic.SaveDef(typeof(FisheyeDef), typeof(Fisheye)),
                                   SerializeStatic.SaveDef(typeof(GlobalFogDef), typeof(GlobalFog)),
                                   SerializeStatic.SaveDef(typeof(GrayscaleDef), typeof(GrayscaleEffect)),
                                   SerializeStatic.SaveDef(typeof(HueFocusDef), typeof(HueFocus)),
                                   SerializeStatic.SaveDef(typeof(LensDistortionBlurDef), typeof(LensDistortionBlur)),
                                   SerializeStatic.SaveDef(typeof(LetterboxDef), typeof(Letterbox)),
                                   SerializeStatic.SaveDef(typeof(MotionBlurDef), typeof(MotionBlur)),
                                   SerializeStatic.SaveDef(typeof(NoiseAndGrainDef), typeof(NoiseAndGrain)),
                                   SerializeStatic.SaveDef(typeof(SepiaDef), typeof(SepiaToneEffect)),
                                   // SerializeStatic.SaveDef(typeof(SSAODef), typeof(AmbientObscurance)),
                                   SerializeStatic.SaveDef(typeof(SunShaftsDef), typeof(SunShafts)),
                                   SerializeStatic.SaveDef(typeof(TechnicolorDef), typeof(Technicolor)),
                                   SerializeStatic.SaveDef(typeof(TiltShiftHdrDef), typeof(TiltShiftHdr)),
                                   SerializeStatic.SaveDef(typeof(VignettingDef), typeof(Vignetting)),
                                   SerializeStatic.SaveDef(typeof(AnalogGlitchDef), typeof(AnalogGlitch)),
                                   SerializeStatic.SaveDef(typeof(DigitalGlitchDef), typeof(DigitalGlitch)),
                                   SerializeStatic.SaveDef(typeof(BokehDef), typeof(Bokeh)),
                                   SerializeStatic.SaveDef(typeof(ObscuranceDef), typeof(Obscurance)),
                                   SerializeStatic.SaveDef(typeof(AnalogTVDef), typeof(AnalogTV)),
                                   SerializeStatic.SaveDef(typeof(BleachBypassDef), typeof(BleachBypass)),
                                   SerializeStatic.SaveDef(typeof(BlendDef), typeof(Blend)),
                                   SerializeStatic.SaveDef(typeof(BrightnessContrastGammaDef), typeof(BrightnessContrastGamma)),
                                   SerializeStatic.SaveDef(typeof(ChannelMixerDef), typeof(ChannelMixer)),
                                   SerializeStatic.SaveDef(typeof(ComicBookDef), typeof(ComicBook)),
                                   SerializeStatic.SaveDef(typeof(ContrastVignetteDef), typeof(ContrastVignette)),
                                   SerializeStatic.SaveDef(typeof(Convolution3x3Def), typeof(Convolution3x3)),
                                   SerializeStatic.SaveDef(typeof(DitheringDef), typeof(Dithering)),
                                   SerializeStatic.SaveDef(typeof(DoubleVisionDef), typeof(DoubleVision)),
                                   SerializeStatic.SaveDef(typeof(HalftoneDef), typeof(Halftone)),
                                   SerializeStatic.SaveDef(typeof(IsolineDef), typeof(Isoline)),
                                   SerializeStatic.SaveDef(typeof(KuwaharaDef), typeof(Kuwahara)),
                                   SerializeStatic.SaveDef(typeof(LookupFilterDef), typeof(LookupFilter)),
                                   SerializeStatic.SaveDef(typeof(PixelateDef), typeof(Pixelate)),
                                   SerializeStatic.SaveDef(typeof(RGBSplitDef), typeof(RGBSplit)),
                                   SerializeStatic.SaveDef(typeof(ShadowsMidtonesHighlightsDef), typeof(ShadowsMidtonesHighlights)),
                                   SerializeStatic.SaveDef(typeof(WaveDistortionDef), typeof(WaveDistortion)),
                                   SerializeStatic.SaveDef(typeof(WhiteBalanceDef), typeof(WhiteBalance)),
                                   SerializeStatic.SaveDef(typeof(WiggleDef), typeof(Wiggle)));

            xml.Elements().Where(e => e.Name == "null").Remove();

            return xml;
        }

        public static XElement SaveLights()
        {
            var xml = new XElement("Lights");
            foreach(LightInfo light in lights)
            {
                var elem = new XElement("Light",
                                        new XElement("Position", Util.ConvertVector3ToString(light.position)),
                                        new XElement("EulerAngles", Util.ConvertVector3ToString(light.eulerAngles)),
                                        new XElement("Intensity", light.intensity.ToString()),
                                        new XElement("Range", light.range.ToString()),
                                        new XElement("SpotAngle", light.spotAngle.ToString()),
                                        new XElement("Color", Util.ConvertColor32ToString((Color32)light.color)),
                                        new XElement("Type", ((int)light.type).ToString()),
                                        new XElement("Enabled", light.enabled.ToString()));
                xml.Add(elem);
            }
            return xml;
        }

        public static XElement SaveModels()
        {
            var xml = new XElement("Models");
            Debug.Log(models == null);
            foreach(ModelInfo model in models)
            {
                Debug.Log(model.position);
                Debug.Log(model.rotation);
                Debug.Log(model.localScale);
                Debug.Log(model.modelName);
                var elem = new XElement("Model",
                                        new XElement("Position", Util.ConvertVector3ToString(model.position)),
                                        new XElement("Rotation", Util.ConvertQuaternionToString(model.rotation)),
                                        new XElement("LocalScale", Util.ConvertVector3ToString(model.localScale)),
                                        new XElement("ModelName", model.modelName),
                                        new XElement("ModelIconName", model.modelIconName));
                xml.Add(elem);
            }
            return xml;
        }

        public static XElement SaveCamera()
        {
            GameObject cameraObj = GameMain.Instance.MainCamera.gameObject;
            var xml = new XElement("Camera",
                                   new XElement("Position", Util.ConvertVector3ToString(GameMain.Instance.MainCamera.GetTargetPos())),
                                   new XElement("Rotation", Util.ConvertVector3ToString(cameraObj.transform.eulerAngles)),
                                   new XElement("Distance", GameMain.Instance.MainCamera.GetDistance().ToString()),
                                   new XElement("FieldOfView", cameraObj.GetComponent<Camera>().fieldOfView.ToString()));

            return xml;
        }

        public static XElement SaveMisc()
        {
            var xml = new XElement("Misc",
                                   new XElement("Background", background),
                                   new XElement("Version", SceneCapture.GetPluginVersion()));
            return xml;
        }

        public static XDocument Save()
        {
            return new XDocument(new XElement("Preset",
                                              SaveEffects(),
                                              SaveLights(),
                                              SaveModels(),
                                              SaveCamera(),
                                              SaveMisc()));
        }

        private static void LoadEffects(XDocument xml)
        {
            try
            {
                Instances.needEffectWindowReload = true;
                XElement effects = xml.Element("Preset").Element("Effects");

                if(effects == null)
                    return;

                SerializeStatic.LoadDef(effects, typeof(AntialiasingDef), typeof(AntialiasingAsPostEffect));
                SerializeStatic.LoadDef(effects, typeof(BloomDef), typeof(Bloom));
                SerializeStatic.LoadDef(effects, typeof(BlurDef), typeof(Blur));
                SerializeStatic.LoadDef(effects, typeof(ChannelSwapDef), typeof(ChannelSwapper));
                SerializeStatic.LoadDef(effects, typeof(ColorCorrectionCurvesDef), typeof(ColorCorrectionCurves));
                SerializeStatic.LoadDef(effects, typeof(ContrastDef), typeof(ContrastEnhance));
                SerializeStatic.LoadDef(effects, typeof(CreaseDef), typeof(Crease));
                SerializeStatic.LoadDef(effects, typeof(DepthOfFieldDef), typeof(DepthOfFieldScatter));
                SerializeStatic.LoadDef(effects, typeof(DynamicLookupDef), typeof(DynamicLookup));
                SerializeStatic.LoadDef(effects, typeof(EdgeDetectDef), typeof(EdgeDetectEffectNormals));
                SerializeStatic.LoadDef(effects, typeof(FisheyeDef), typeof(Fisheye));
                SerializeStatic.LoadDef(effects, typeof(GlobalFogDef), typeof(GlobalFog));
                SerializeStatic.LoadDef(effects, typeof(GrayscaleDef), typeof(GrayscaleEffect));
                SerializeStatic.LoadDef(effects, typeof(HueFocusDef), typeof(HueFocus));
                SerializeStatic.LoadDef(effects, typeof(LensDistortionBlurDef), typeof(LensDistortionBlur));
                SerializeStatic.LoadDef(effects, typeof(LetterboxDef), typeof(Letterbox));
                SerializeStatic.LoadDef(effects, typeof(MotionBlurDef), typeof(MotionBlur));
                SerializeStatic.LoadDef(effects, typeof(NoiseAndGrainDef), typeof(NoiseAndGrain));
                SerializeStatic.LoadDef(effects, typeof(SepiaDef), typeof(SepiaToneEffect));
                // SerializeStatic.LoadDef(effects, typeof(SSAODef), typeof(AmbientObscurance));
                SerializeStatic.LoadDef(effects, typeof(SunShaftsDef), typeof(SunShafts));
                SerializeStatic.LoadDef(effects, typeof(TechnicolorDef), typeof(Technicolor));
                SerializeStatic.LoadDef(effects, typeof(TiltShiftHdrDef), typeof(TiltShiftHdr));
                SerializeStatic.LoadDef(effects, typeof(VignettingDef), typeof(Vignetting));
                SerializeStatic.LoadDef(effects, typeof(AnalogGlitchDef), typeof(AnalogGlitch));
                SerializeStatic.LoadDef(effects, typeof(DigitalGlitchDef), typeof(DigitalGlitch));
                SerializeStatic.LoadDef(effects, typeof(BokehDef), typeof(Bokeh));
                SerializeStatic.LoadDef(effects, typeof(ObscuranceDef), typeof(Obscurance));
                SerializeStatic.LoadDef(effects, typeof(AnalogTVDef), typeof(AnalogTV));
                SerializeStatic.LoadDef(effects, typeof(BleachBypassDef), typeof(BleachBypass));
                SerializeStatic.LoadDef(effects, typeof(BlendDef), typeof(Blend));
                SerializeStatic.LoadDef(effects, typeof(BrightnessContrastGammaDef), typeof(BrightnessContrastGamma));
                SerializeStatic.LoadDef(effects, typeof(ChannelMixerDef), typeof(ChannelMixer));
                SerializeStatic.LoadDef(effects, typeof(ComicBookDef), typeof(ComicBook));
                SerializeStatic.LoadDef(effects, typeof(ContrastVignetteDef), typeof(ContrastVignette));
                SerializeStatic.LoadDef(effects, typeof(Convolution3x3Def), typeof(Convolution3x3));
                SerializeStatic.LoadDef(effects, typeof(DitheringDef), typeof(Dithering));
                SerializeStatic.LoadDef(effects, typeof(DoubleVisionDef), typeof(DoubleVision));
                SerializeStatic.LoadDef(effects, typeof(HalftoneDef), typeof(Halftone));
                SerializeStatic.LoadDef(effects, typeof(IsolineDef), typeof(Isoline));
                SerializeStatic.LoadDef(effects, typeof(KuwaharaDef), typeof(Kuwahara));
                SerializeStatic.LoadDef(effects, typeof(LookupFilterDef), typeof(LookupFilter));
                SerializeStatic.LoadDef(effects, typeof(PixelateDef), typeof(Pixelate));
                SerializeStatic.LoadDef(effects, typeof(RGBSplitDef), typeof(RGBSplit));
                SerializeStatic.LoadDef(effects, typeof(ShadowsMidtonesHighlightsDef), typeof(ShadowsMidtonesHighlights));
                SerializeStatic.LoadDef(effects, typeof(WaveDistortionDef), typeof(WaveDistortion));
                SerializeStatic.LoadDef(effects, typeof(WhiteBalanceDef), typeof(WhiteBalance));
                SerializeStatic.LoadDef(effects, typeof(WiggleDef), typeof(Wiggle));
            }
            catch (Exception e)
            {
                Debug.LogError( e );
            }
        }

        private static void LoadLights(XDocument xml)
        {
            try
            {
                Instances.needLightReload = true;
                Instances.ClearLights();
                XElement lightsElem = xml.Element("Preset").Element("Lights");

                if(lightsElem == null)
                    return;

                List<LightInfo> lightsList = new List<LightInfo>();

                foreach(XElement light in lightsElem.Elements())
                {
                    lightsList.Add(Instances.LoadLight(light));
                }
                Instances.SetLights(lightsList);
            }
            catch (Exception e)
            {
                Debug.LogError( e );
            }
        }

        private static LightInfo LoadLight(XElement singleLight)
        {
            try
            {
                LightInfo light = new LightInfo();
                Color cOut;
                float fOut;
                Vector3 v3Out;

                v3Out = Util.ConvertStringToVector3(singleLight.Element("Position").Value.ToString());
                light.position = v3Out;

                v3Out = Util.ConvertStringToVector3(singleLight.Element("EulerAngles").Value.ToString());
                light.eulerAngles = v3Out;

                cOut = Util.ConvertStringToColor32(singleLight.Element("Color").Value.ToString());
                light.color = cOut;

                light.type = (LightType)Enum.Parse( typeof( LightType ), singleLight.Element("Type").Value.ToString());

                float.TryParse(singleLight.Element("Intensity").Value.ToString(), out fOut);
                light.intensity = fOut;

                float.TryParse(singleLight.Element("Range").Value.ToString(), out fOut);
                light.range = fOut;

                float.TryParse(singleLight.Element("SpotAngle").Value.ToString(), out fOut);
                light.spotAngle = fOut;

                light.enabled = true;

                return light;
            }
            catch (Exception e)
            {
                Debug.LogError( e );
            }
            return null;
        }

        private static void LoadModels(XDocument xml)
        {
            try
            {
                Instances.needModelReload = true;

                Instances.ClearModels();
                XElement modelsElem = xml.Element("Preset").Element("Models");

                if(modelsElem == null)
                    return;

                List<ModelInfo> modelsList = new List<ModelInfo>();
                foreach(XElement model in modelsElem.Elements())
                {
                    modelsList.Add(Instances.LoadModel(model));
                }
                Instances.SetModels(modelsList);
            }
            catch (Exception e)
            {
                Debug.LogError( e );
            }
        }

        private static ModelInfo LoadModel(XElement singleModel)
        {
            try
            {
                ModelInfo model = new ModelInfo();
                Vector3 v3Out;
                Quaternion qOut;

                v3Out = Util.ConvertStringToVector3(singleModel.Element("Position").Value.ToString());
                model.position = v3Out;

                qOut = Util.ConvertStringToQuaternion(singleModel.Element("Rotation").Value.ToString());
                model.rotation = qOut;

                v3Out = Util.ConvertStringToVector3(singleModel.Element("LocalScale").Value.ToString());
                model.localScale = v3Out;

                model.modelName = singleModel.Element("ModelName").Value.ToString();
                model.modelIconName = singleModel.Element("ModelIconName").Value.ToString();

                return model;
            }
            catch (Exception e)
            {
                Debug.LogError( e );
            }
            return null;
        }
        private static void LoadCamera(XDocument xml)
        {
            try
            {
                XElement cameraElem = xml.Element("Preset").Element("Camera");

                if(cameraElem == null)
                    return;

                GameObject cameraObj = GameMain.Instance.MainCamera.gameObject;
                float fOut;
                Vector3 v3Out;

                v3Out = Util.ConvertStringToVector3(cameraElem.Element("Position").Value.ToString());
                GameMain.Instance.MainCamera.SetTargetPos(v3Out, true);

                v3Out = Util.ConvertStringToVector3(cameraElem.Element("Rotation").Value.ToString());
                cameraObj.transform.eulerAngles = v3Out;

                float.TryParse(cameraElem.Element("Distance").Value.ToString(), out fOut);
                GameMain.Instance.MainCamera.SetDistance(fOut, true);

                float.TryParse(cameraElem.Element("FieldOfView").Value.ToString(), out fOut);
                cameraObj.GetComponent<Camera>().fieldOfView = fOut;
            }
            catch (Exception e)
            {
                Debug.LogError( e );
            }
        }

        private static void LoadMisc(XDocument xml)
        {
            try
            {
                Instances.needMiscReload = true;

                Instances.ClearMisc();
                XElement miscElem = xml.Element("Preset").Element("Misc");

                if(miscElem == null)
                    return;

                Instances.background = miscElem.Element("Background").Value.ToString();
            }
            catch (Exception e)
            {
                Debug.LogError( e );
            }
        }

        public static void Load(string filename)
        {
            XDocument xml = XDocument.Load(filename);
            Instances.ResetAll();

            if(loadEffects)
                Instances.LoadEffects(xml);

            if(loadLights)
                Instances.LoadLights(xml);

            if(loadModels)
                Instances.LoadModels(xml);

            if(loadCamera)
                Instances.LoadCamera(xml);

            if(loadMisc)
                Instances.LoadMisc(xml);
        }
    }
}

