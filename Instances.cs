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

        public Instances()
        {
            Debug.Log("new instances");
            lights = new List<LightInfo>();
            needLightReload = false;
            models = new List<ModelInfo>();
            needModelReload = false;
            needEffectWindowReload = false;
            background = ConstantValues.Background.Keys.First();

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

        public static void ClearLights()
        {
            Instances.lights.Clear();
        }

        public static void ClearModels()
        {
            Instances.models.Clear();
        }

        public static void ClearMisc()
        {
            Instances.background = ConstantValues.Background.Keys.First();
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
                                   // SerializeStatic.SaveDef(typeof(FeedbackDef), typeof(Feedback)),
                                   SerializeStatic.SaveDef(typeof(ObscuranceDef), typeof(Obscurance))
                                   );

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

        public static XElement SaveMisc()
        {
            var xml = new XElement("Misc",
                                   new XElement("Version", SceneCapture.GetPluginVersion()),
                                   new XElement("Background", background));
            return xml;
        }

        public static XDocument Save()
        {
            return new XDocument(new XElement("Preset",
                                              SaveEffects(),
                                              SaveLights(),
                                              SaveModels(),
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
                // SerializeStatic.LoadDef(effects, typeof(FeedbackDef), typeof(Feedback));
                SerializeStatic.LoadDef(effects, typeof(ObscuranceDef), typeof(Obscurance));
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
            Instances.LoadEffects(xml);
            Instances.LoadLights(xml);
            Instances.LoadModels(xml);
            Instances.LoadMisc(xml);
        }
    }
}

