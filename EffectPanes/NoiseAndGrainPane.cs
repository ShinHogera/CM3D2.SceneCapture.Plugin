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
    internal class NoiseAndGrainPane : BasePane
    {
        public NoiseAndGrainPane( int fontSize) : base( fontSize, Translation.GetText("Panes", "NoiseAndGrain") ) {}

        override public void SetupPane()
        {
            this.dx11GrainCheckbox = new CustomToggleButton( false, "toggle" );
            this.dx11GrainCheckbox.Text = Translation.GetText("NoiseAndGrain", "dx11Grain");
            this.ChildControls.Add( this.dx11GrainCheckbox );

            this.monochromeCheckbox = new CustomToggleButton( false, "toggle" );
            this.monochromeCheckbox.Text = Translation.GetText("NoiseAndGrain", "monochrome");
            this.ChildControls.Add( this.monochromeCheckbox );

            this.filterModeBox = new CustomComboBox( NOISEANDGRAIN_MODES );
            this.filterModeBox.Text = Translation.GetText("NoiseAndGrain", "filterMode");
            this.filterModeBox.SelectedIndex = (int)NoiseAndGrainDef.noiseAndGrainEffect.filterMode;
            this.ChildControls.Add( this.filterModeBox );

            this.intensityMultiplierSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.intensityMultiplier, 0f, 10f, 2 );
            this.intensityMultiplierSlider.Text = Translation.GetText("NoiseAndGrain", "intensityMultiplier");
            this.ChildControls.Add( this.intensityMultiplierSlider );
            this.generalIntensitySlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.generalIntensity, 0f, 10f, 2 );
            this.generalIntensitySlider.Text = Translation.GetText("NoiseAndGrain", "generalIntensity");
            this.ChildControls.Add( this.generalIntensitySlider );
            this.blackIntensitySlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.blackIntensity, 0f, 10f, 2 );
            this.blackIntensitySlider.Text = Translation.GetText("NoiseAndGrain", "blackIntensity");
            this.ChildControls.Add( this.blackIntensitySlider );
            this.whiteIntensitySlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.whiteIntensity, 0f, 10f, 2 );
            this.whiteIntensitySlider.Text = Translation.GetText("NoiseAndGrain", "whiteIntensity");
            this.ChildControls.Add( this.whiteIntensitySlider );
            this.midGreySlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.midGrey, 0f, 1f, 4 );
            this.midGreySlider.Text = Translation.GetText("NoiseAndGrain", "midGrey");
            this.ChildControls.Add( this.midGreySlider );
            this.softnessSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.softness, 0f, 1f, 4 );
            this.softnessSlider.Text = Translation.GetText("NoiseAndGrain", "softness");
            this.ChildControls.Add( this.softnessSlider );

            this.monochromeTilingSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.monochromeTiling, 0f, 100f, 2 );
            this.monochromeTilingSlider.Text = Translation.GetText("NoiseAndGrain", "monochromeTiling");
            this.ChildControls.Add( this.monochromeTilingSlider );
            this.tilingRedSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.tiling.x, 0f, 100f, 2 );
            this.tilingRedSlider.Text = Translation.GetText("NoiseAndGrain", "tilingRed");
            this.ChildControls.Add( this.tilingRedSlider );
            this.tilingGreenSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.tiling.y, 0f, 100f, 2 );
            this.tilingGreenSlider.Text = Translation.GetText("NoiseAndGrain", "tilingGreen");
            this.ChildControls.Add( this.tilingGreenSlider );
            this.tilingBlueSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.tiling.z, 0f, 100f, 2 );
            this.tilingBlueSlider.Text = Translation.GetText("NoiseAndGrain", "tilingBlue");
            this.ChildControls.Add( this.tilingBlueSlider );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.dx11GrainCheckbox);
            GUIUtil.AddGUICheckbox(this, this.monochromeCheckbox);
            GUIUtil.AddGUISlider(this, this.intensityMultiplierSlider);
            GUIUtil.AddGUISlider(this, this.blackIntensitySlider);
            GUIUtil.AddGUISlider(this, this.whiteIntensitySlider);
            GUIUtil.AddGUISlider(this, this.midGreySlider);

            if( this.dx11GrainCheckbox.Value == false )
            {
                GUIUtil.AddGUICheckbox(this, this.filterModeBox);
            }

            GUIUtil.AddGUISlider(this, this.softnessSlider);

            if( this.monochromeCheckbox.Value == true )
            {
                GUIUtil.AddGUISlider(this, this.monochromeTilingSlider);
            }
            else
            {
                GUIUtil.AddGUISlider(this, this.tilingRedSlider);
                GUIUtil.AddGUISlider(this, this.tilingGreenSlider);
                GUIUtil.AddGUISlider(this, this.tilingBlueSlider);
            }
        }

        override public void Reset()
        {

        }

        #region Properties
        public FilterMode FilterModeValue
        {
            get
            {
                return (FilterMode)Enum.Parse( typeof( FilterMode ), this.filterModeBox.SelectedItem);
            }
        }

        public bool Dx11GrainValue
        {
            get
            {
                return this.dx11GrainCheckbox.Value;
            }
        }

        public bool MonochromeValue
        {
            get
            {
                return this.monochromeCheckbox.Value;
            }
        }

        public float IntensityMultiplierValue
        {
            get
            {
                return this.intensityMultiplierSlider.Value;
            }
        }

        public float GeneralIntensityValue
        {
            get
            {
                return this.generalIntensitySlider.Value;
            }
        }

        public float BlackIntensityValue
        {
            get
            {
                return this.blackIntensitySlider.Value;
            }
        }

        public float WhiteIntensityValue
        {
            get
            {
                return this.whiteIntensitySlider.Value;
            }
        }

        public float MidGreyValue
        {
            get
            {
                return this.midGreySlider.Value;
            }
        }

        public float SoftnessValue
        {
            get
            {
                return this.softnessSlider.Value;
            }
        }

        public float MonochromeTilingValue
        {
            get
            {
                return this.monochromeTilingSlider.Value;
            }
        }

        public Vector3 TilingValue
        {
            get
            {
                return new Vector3(this.tilingRedSlider.Value, this.tilingGreenSlider.Value, this.tilingBlueSlider.Value);
            }
        }
        #endregion

        #region Fields
        private static readonly string[] NOISEANDGRAIN_MODES = new string[] { "Point", "Bilinear", "Trilinear" };

        private CustomToggleButton dx11GrainCheckbox = null;
        private CustomToggleButton monochromeCheckbox = null;

        private CustomSlider intensityMultiplierSlider = null;
        private CustomSlider generalIntensitySlider = null;
        private CustomSlider blackIntensitySlider = null;
        private CustomSlider whiteIntensitySlider = null;
        private CustomSlider midGreySlider = null;
        private CustomComboBox filterModeBox = null;
        private CustomSlider softnessSlider = null;

        private CustomSlider monochromeTilingSlider = null;
        private CustomSlider tilingRedSlider = null;
        private CustomSlider tilingGreenSlider = null;
        private CustomSlider tilingBlueSlider = null;
        #endregion
    }
}
