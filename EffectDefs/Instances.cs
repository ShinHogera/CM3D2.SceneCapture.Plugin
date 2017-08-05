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
        public static bool needEffectWindowReload { get; set; }

        public static void Create()
        {
            lights = new List<LightInfo>();
            needLightReload = false;
            needEffectWindowReload = false;
        }

        public static void SetLights(List<LightInfo> lights)
        {
            Instances.lights = lights;
        }

        public static List<LightInfo> GetLights()
        {
            return Instances.lights;
        }

        public static void ClearLights()
        {
            Instances.lights.Clear();
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


        public static XDocument Save()
        {
            return new XDocument(new XElement("Preset",
                                              SaveEffects(),
                                              SaveLights()));
        }

        private static void LoadEffects(XDocument xml)
        {
            try
            {
                XElement effects = xml.Element("Preset").Element("Effects");
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
                Instances.needEffectWindowReload = true;
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
                Instances.ClearLights();
                XElement lightsElem = xml.Element("Preset").Element("Lights");
                List<LightInfo> lightsList = new List<LightInfo>();
                foreach(XElement light in lightsElem.Elements())
                {
                    Debug.Log(light == null);
                    Debug.Log(light.ToString());
                    lightsList.Add(Instances.LoadLight(light));
                }
                Instances.SetLights(lightsList);
                Instances.needLightReload = true;
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

        public static void Load(string filename)
        {
            XDocument xml = XDocument.Load(filename);
            Instances.LoadEffects(xml);
            Instances.LoadLights(xml);
        }
    }
}

