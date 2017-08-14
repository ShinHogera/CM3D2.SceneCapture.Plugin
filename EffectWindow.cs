using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.SceneCapture.Plugin
{
    #region EffectWindow

    internal class EffectWindow : ScrollablePane
    {
        #region Methods

        public EffectWindow( int fontSize ) : base ( fontSize ) {}

        override public void Awake()
        {
            try
            {
                this.colorCorrectionCurvesPane = new ColorCorrectionCurvesPane( this.FontSize );
                this.ChildControls.Add( this.colorCorrectionCurvesPane );

                this.sepiaPane = new SepiaPane( this.FontSize );
                this.ChildControls.Add( this.sepiaPane );

                this.grayscalePane = new GrayscalePane( this.FontSize );
                this.ChildControls.Add( this.grayscalePane );

                this.contrastPane = new ContrastPane( this.FontSize );
                this.ChildControls.Add( this.contrastPane );

                this.edgeDetectPane = new EdgeDetectPane( this.FontSize );
                this.ChildControls.Add( this.edgeDetectPane );

                // this.ssaoPane = new SSAOPane( this.FontSize );
                // this.ChildControls.Add( this.ssaoPane );

                this.creasePane = new CreasePane( this.FontSize );
                this.ChildControls.Add( this.creasePane );

                this.antialiasingPane = new AntialiasingPane( this.FontSize );
                this.ChildControls.Add( this.antialiasingPane );

                this.sunShaftsPane = new SunShaftsPane( this.FontSize );
                this.ChildControls.Add( this.sunShaftsPane );

                this.noiseAndGrainPane = new NoiseAndGrainPane( this.FontSize );
                this.ChildControls.Add( this.noiseAndGrainPane );

                this.blurPane = new BlurPane( this.FontSize );
                this.ChildControls.Add( this.blurPane );

                this.depthOfFieldPane = new DepthOfFieldPane( this.FontSize );
                this.ChildControls.Add( this.depthOfFieldPane );

                this.motionBlurPane = new MotionBlurPane( this.FontSize );
                this.ChildControls.Add( this.motionBlurPane );

                this.tiltShiftHdrPane = new TiltShiftHdrPane( this.FontSize );
                this.ChildControls.Add( this.tiltShiftHdrPane );

                this.fisheyePane = new FisheyePane( this.FontSize );
                this.ChildControls.Add( this.fisheyePane );

                this.vignettingPane = new VignettingPane( this.FontSize );
                this.ChildControls.Add( this.vignettingPane );

                this.lensDistortionBlurPane = new LensDistortionBlurPane( this.FontSize );
                this.ChildControls.Add( this.lensDistortionBlurPane );

                this.letterboxPane = new LetterboxPane( this.FontSize );
                this.ChildControls.Add( this.letterboxPane );

                this.technicolorPane = new TechnicolorPane( this.FontSize );
                this.ChildControls.Add( this.technicolorPane );

                this.dynamicLookupPane = new DynamicLookupPane( this.FontSize );
                this.ChildControls.Add( this.dynamicLookupPane );

                this.hueFocusPane = new HueFocusPane( this.FontSize );
                this.ChildControls.Add( this.hueFocusPane );

                this.channelSwapPane = new ChannelSwapPane( this.FontSize );
                this.ChildControls.Add( this.channelSwapPane );

                this.analogGlitchPane = new AnalogGlitchPane( this.FontSize );
                this.ChildControls.Add( this.analogGlitchPane );

                this.digitalGlitchPane = new DigitalGlitchPane( this.FontSize );
                this.ChildControls.Add( this.digitalGlitchPane );

                this.bloomPane = new BloomPane( this.FontSize );
                this.ChildControls.Add( this.bloomPane );

                this.globalFogPane = new GlobalFogPane( this.FontSize );
                this.ChildControls.Add( this.globalFogPane );

                this.bokehPane = new BokehPane( this.FontSize );
                this.ChildControls.Add( this.bokehPane );

                this.obscurancePane = new ObscurancePane( this.FontSize );
                this.ChildControls.Add( this.obscurancePane );

                this.analogTVPane = new AnalogTVPane( this.FontSize );
                this.ChildControls.Add( this.analogTVPane );

                this.bleachBypassPane = new BleachBypassPane( this.FontSize );
                this.ChildControls.Add( this.bleachBypassPane );

                this.blendPane = new BlendPane( this.FontSize );
                this.ChildControls.Add( this.blendPane );

                this.brightnessContrastGammaPane = new BrightnessContrastGammaPane( this.FontSize );
                this.ChildControls.Add( this.brightnessContrastGammaPane );

                this.channelMixerPane = new ChannelMixerPane( this.FontSize );
                this.ChildControls.Add( this.channelMixerPane );

                this.comicBookPane = new ComicBookPane( this.FontSize );
                this.ChildControls.Add( this.comicBookPane );

                this.contrastVignettePane = new ContrastVignettePane( this.FontSize );
                this.ChildControls.Add( this.contrastVignettePane );

                this.doubleVisionPane = new DoubleVisionPane( this.FontSize );
                this.ChildControls.Add( this.doubleVisionPane );

                this.halftonePane = new HalftonePane( this.FontSize );
                this.ChildControls.Add( this.halftonePane );

                this.isolinePane = new IsolinePane( this.FontSize );
                this.ChildControls.Add( this.isolinePane );

                this.kuwaharaPane = new KuwaharaPane( this.FontSize );
                this.ChildControls.Add( this.kuwaharaPane );

                this.lookupFilterPane = new LookupFilterPane( this.FontSize );
                this.ChildControls.Add( this.lookupFilterPane );

                this.pixelatePane = new PixelatePane( this.FontSize );
                this.ChildControls.Add( this.pixelatePane );

                this.rgbSplitPane = new RGBSplitPane( this.FontSize );
                this.ChildControls.Add( this.rgbSplitPane );

                this.shadowsMidtonesHighlightsPane = new ShadowsMidtonesHighlightsPane( this.FontSize );
                this.ChildControls.Add( this.shadowsMidtonesHighlightsPane );

                this.waveDistortionPane = new WaveDistortionPane( this.FontSize );
                this.ChildControls.Add( this.waveDistortionPane );

                this.whiteBalancePane = new WhiteBalancePane( this.FontSize );
                this.ChildControls.Add( this.whiteBalancePane );

                this.wigglePane = new WigglePane( this.FontSize );
                this.ChildControls.Add( this.wigglePane );

                this.ditheringPane = new DitheringPane( this.FontSize );
                this.ChildControls.Add( this.ditheringPane );

                this.convolution3x3Pane = new Convolution3x3Pane( this.FontSize );
                this.ChildControls.Add( this.convolution3x3Pane );
            }
            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }
        override public void Update()
        {
            try
            {
                if( Instances.needEffectWindowReload )
                {
                    this.ChildControls.Clear();
                    this.Awake();
                }

                ColorCorrectionCurvesDef.Update(this.colorCorrectionCurvesPane);
                SepiaDef.Update(this.sepiaPane);
                GrayscaleDef.Update(this.grayscalePane);
                ContrastDef.Update(this.contrastPane);
                EdgeDetectDef.Update(this.edgeDetectPane);
                // SSAODef.Update(this.ssaoPane);
                CreaseDef.Update(this.creasePane);
                AntialiasingDef.Update(this.antialiasingPane);
                NoiseAndGrainDef.Update(this.noiseAndGrainPane);
                BlurDef.Update(this.blurPane);
                DepthOfFieldDef.Update(this.depthOfFieldPane);
                MotionBlurDef.Update(this.motionBlurPane);
                BloomDef.Update(this.bloomPane);
                GlobalFogDef.Update(this.globalFogPane);
                TiltShiftHdrDef.Update(this.tiltShiftHdrPane);
                FisheyeDef.Update(this.fisheyePane);
                VignettingDef.Update(this.vignettingPane);
                SunShaftsDef.Update(this.sunShaftsPane);
                LensDistortionBlurDef.Update(this.lensDistortionBlurPane);
                LetterboxDef.Update(this.letterboxPane);
                HueFocusDef.Update(this.hueFocusPane);
                ChannelSwapDef.Update(this.channelSwapPane);
                TechnicolorDef.Update(this.technicolorPane);
                DynamicLookupDef.Update(this.dynamicLookupPane);
                AnalogGlitchDef.Update(this.analogGlitchPane);
                DigitalGlitchDef.Update(this.digitalGlitchPane);
                BokehDef.Update(this.bokehPane);
                FeedbackDef.Update(this.feedbackPane);
                ObscuranceDef.Update(this.obscurancePane);
                AnalogTVDef.Update(this.analogTVPane);
                BleachBypassDef.Update(this.bleachBypassPane);
                BlendDef.Update(this.blendPane);
                BrightnessContrastGammaDef.Update(this.brightnessContrastGammaPane);
                ChannelMixerDef.Update(this.channelMixerPane);
                ComicBookDef.Update(this.comicBookPane);
                ContrastVignetteDef.Update(this.contrastVignettePane);
                Convolution3x3Def.Update(this.convolution3x3Pane);
                DitheringDef.Update(this.ditheringPane);
                DoubleVisionDef.Update(this.doubleVisionPane);
                HalftoneDef.Update(this.halftonePane);
                IsolineDef.Update(this.isolinePane);
                KuwaharaDef.Update(this.kuwaharaPane);
                LookupFilterDef.Update(this.lookupFilterPane);
                PixelateDef.Update(this.pixelatePane);
                RGBSplitDef.Update(this.rgbSplitPane);
                ShadowsMidtonesHighlightsDef.Update(this.shadowsMidtonesHighlightsPane);
                WaveDistortionDef.Update(this.waveDistortionPane);
                WhiteBalanceDef.Update(this.whiteBalancePane);
                WiggleDef.Update(this.wigglePane);

                if( Instances.needEffectWindowReload )
                {
                    Instances.needEffectWindowReload = false;
                }
            }

            catch( Exception e )
            {
                Debug.LogError( e.ToString() );
            }
        }

        override public void ShowPane()
        {
            // because bloom is complicated to handle, since it is always enabled
            this.bloomPane.Update();

            // 光源名ラベル
            this.colorCorrectionCurvesPane.Left = this.Left + ControlBase.FixedMargin;
            this.colorCorrectionCurvesPane.Top = this.Top + ControlBase.FixedMargin;
            this.colorCorrectionCurvesPane.Width = this.Width - ControlBase.FixedMargin * 4;
            this.colorCorrectionCurvesPane.Height = this.ControlHeight;
            this.colorCorrectionCurvesPane.OnGUI();

            GUIUtil.AddGUICheckbox(this, this.brightnessContrastGammaPane,  this.colorCorrectionCurvesPane);
            GUIUtil.AddGUICheckbox(this, this.shadowsMidtonesHighlightsPane,  this.brightnessContrastGammaPane);
            GUIUtil.AddGUICheckbox(this, this.lookupFilterPane,  this.shadowsMidtonesHighlightsPane);
            GUIUtil.AddGUICheckbox(this, this.whiteBalancePane,  this.lookupFilterPane);
            GUIUtil.AddGUICheckbox(this, this.channelMixerPane, this.whiteBalancePane);
            GUIUtil.AddGUICheckbox(this, this.bleachBypassPane, this.channelMixerPane);
            GUIUtil.AddGUICheckbox(this, this.sepiaPane, this.bleachBypassPane);
            GUIUtil.AddGUICheckbox(this, this.grayscalePane, this.sepiaPane);
            GUIUtil.AddGUICheckbox(this, this.contrastPane, this.grayscalePane);
            GUIUtil.AddGUICheckbox(this, this.edgeDetectPane, this.contrastPane);
            // GUIUtil.AddGUICheckbox(this, this.ssaoPane, this.edgeDetectPane);
            GUIUtil.AddGUICheckbox(this, this.creasePane, this.edgeDetectPane);
            GUIUtil.AddGUICheckbox(this, this.antialiasingPane, this.creasePane);
            GUIUtil.AddGUICheckbox(this, this.noiseAndGrainPane, this.antialiasingPane);
            GUIUtil.AddGUICheckbox(this, this.blurPane, this.noiseAndGrainPane);
            GUIUtil.AddGUICheckbox(this, this.depthOfFieldPane, this.blurPane);
            GUIUtil.AddGUICheckbox(this, this.motionBlurPane, this.depthOfFieldPane);
            GUIUtil.AddGUICheckbox(this, this.bloomPane, this.motionBlurPane);
            GUIUtil.AddGUICheckbox(this, this.globalFogPane, this.bloomPane);
            GUIUtil.AddGUICheckbox(this, this.tiltShiftHdrPane, this.globalFogPane);
            GUIUtil.AddGUICheckbox(this, this.fisheyePane, this.tiltShiftHdrPane);
            GUIUtil.AddGUICheckbox(this, this.vignettingPane, this.fisheyePane);
            GUIUtil.AddGUICheckbox(this, this.sunShaftsPane, this.vignettingPane);

            GUIUtil.AddGUICheckbox(this, this.dynamicLookupPane, this.sunShaftsPane);
            GUIUtil.AddGUICheckbox(this, this.hueFocusPane, this.dynamicLookupPane);
            GUIUtil.AddGUICheckbox(this, this.channelSwapPane, this.hueFocusPane);
            GUIUtil.AddGUICheckbox(this, this.technicolorPane, this.channelSwapPane);
            GUIUtil.AddGUICheckbox(this, this.lensDistortionBlurPane, this.technicolorPane);
            GUIUtil.AddGUICheckbox(this, this.letterboxPane, this.lensDistortionBlurPane);
            GUIUtil.AddGUICheckbox(this, this.analogGlitchPane, this.letterboxPane);
            GUIUtil.AddGUICheckbox(this, this.digitalGlitchPane, this.analogGlitchPane);
            GUIUtil.AddGUICheckbox(this, this.bokehPane, this.digitalGlitchPane);
            GUIUtil.AddGUICheckbox(this, this.obscurancePane, this.bokehPane);
            GUIUtil.AddGUICheckbox(this, this.analogTVPane, this.obscurancePane);
            GUIUtil.AddGUICheckbox(this, this.blendPane, this.analogTVPane);
            GUIUtil.AddGUICheckbox(this, this.comicBookPane, this.blendPane);
            GUIUtil.AddGUICheckbox(this, this.contrastVignettePane, this.comicBookPane);
            GUIUtil.AddGUICheckbox(this, this.convolution3x3Pane, this.contrastVignettePane);
            GUIUtil.AddGUICheckbox(this, this.ditheringPane, this.convolution3x3Pane);
            GUIUtil.AddGUICheckbox(this, this.doubleVisionPane, this.ditheringPane);
            GUIUtil.AddGUICheckbox(this, this.halftonePane, this.doubleVisionPane);
            GUIUtil.AddGUICheckbox(this, this.isolinePane, this.halftonePane);
            GUIUtil.AddGUICheckbox(this, this.kuwaharaPane, this.isolinePane);
            GUIUtil.AddGUICheckbox(this, this.pixelatePane, this.kuwaharaPane);
            GUIUtil.AddGUICheckbox(this, this.rgbSplitPane, this.pixelatePane);
            GUIUtil.AddGUICheckbox(this, this.waveDistortionPane, this.rgbSplitPane);
            GUIUtil.AddGUICheckbox(this, this.wigglePane, this.waveDistortionPane);

            // ウィンドウ高さ調整
            this.Height = GUIUtil.GetHeightForParent(this);
        }

        #region Fields

        ///-------------------------------------------------------------------------
        /// <summary>光源削除ボタンクリックイベント</summary>
        /// <param name="sender">イベント送信者</param>
        /// <param name="args">イベント引数(未使用)</param>
        ///-------------------------------------------------------------------------
        private ColorCorrectionCurvesPane colorCorrectionCurvesPane = null;
        private SepiaPane sepiaPane = null;
        private GrayscalePane grayscalePane = null;
        private ContrastPane contrastPane = null;
        private EdgeDetectPane edgeDetectPane = null;
        // private SSAOPane ssaoPane = null;
        private CreasePane creasePane = null;
        private AntialiasingPane antialiasingPane = null;
        private NoiseAndGrainPane noiseAndGrainPane = null;
        private BlurPane blurPane = null;
        private DepthOfFieldPane depthOfFieldPane = null;
        private MotionBlurPane motionBlurPane = null;
        private BloomPane bloomPane = null;
        private GlobalFogPane globalFogPane = null;
        private TiltShiftHdrPane tiltShiftHdrPane = null;
        private FisheyePane fisheyePane = null;
        private VignettingPane vignettingPane = null;
        private SunShaftsPane sunShaftsPane = null;

        private LensDistortionBlurPane lensDistortionBlurPane = null;
        private ChannelSwapPane channelSwapPane = null;
        private LetterboxPane letterboxPane = null;
        private DynamicLookupPane dynamicLookupPane = null;
        private HueFocusPane hueFocusPane = null;
        private TechnicolorPane technicolorPane = null;
        private AnalogGlitchPane analogGlitchPane = null;
        private DigitalGlitchPane digitalGlitchPane = null;
        private BokehPane bokehPane = null;
        private FeedbackPane feedbackPane = null;
        private ObscurancePane obscurancePane = null;
        private AnalogTVPane analogTVPane = null;
        private BleachBypassPane bleachBypassPane = null;
        private BlendPane blendPane = null;
        private BrightnessContrastGammaPane brightnessContrastGammaPane = null;
        private ChannelMixerPane channelMixerPane = null;
        private ComicBookPane comicBookPane = null;
        private ContrastVignettePane contrastVignettePane = null;
        private Convolution3x3Pane convolution3x3Pane = null;
        private DitheringPane ditheringPane = null;
        private DoubleVisionPane doubleVisionPane = null;
        private HalftonePane halftonePane = null;
        private IsolinePane isolinePane = null;
        private KuwaharaPane kuwaharaPane = null;
        private LookupFilterPane lookupFilterPane = null;
        private PixelatePane pixelatePane = null;
        private RGBSplitPane rgbSplitPane = null;
        private ShadowsMidtonesHighlightsPane shadowsMidtonesHighlightsPane = null;
        private WaveDistortionPane waveDistortionPane = null;
        private WigglePane wigglePane = null;
        private WhiteBalancePane whiteBalancePane = null;
        #endregion

        #endregion
    }
    #endregion
}
