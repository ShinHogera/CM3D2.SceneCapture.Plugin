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
        public NoiseAndGrainPane( int fontSize) : base( fontSize, "Noise And Grain" ) {}

        override public void SetupPane()
        {
            this.dx11GrainCheckbox = new CustomToggleButton( false, "toggle" );
            this.dx11GrainCheckbox.Text = "DirectX11 Grain";
            this.ChildControls.Add( this.dx11GrainCheckbox );

            this.monochromeCheckbox = new CustomToggleButton( false, "toggle" );
            this.monochromeCheckbox.Text = "Monochrome";
            this.ChildControls.Add( this.monochromeCheckbox );

            this.filterModeBox = new CustomComboBox( NOISEANDGRAIN_MODES );
            this.filterModeBox.Text = "Mode";
            this.filterModeBox.SelectedIndex = (int)NoiseAndGrainDef.noiseAndGrainEffect.filterMode;
            this.ChildControls.Add( this.filterModeBox );

            this.intensityMultiplierSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.intensityMultiplier, 0f, 10f, 1 );
            this.intensityMultiplierSlider.Text = "Intensity Multiplier";
            this.ChildControls.Add( this.intensityMultiplierSlider );
            this.generalIntensitySlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.generalIntensity, 0f, 10f, 1 );
            this.generalIntensitySlider.Text = "General Intensity";
            this.ChildControls.Add( this.generalIntensitySlider );
            this.blackIntensitySlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.blackIntensity, 0f, 10f, 1 );
            this.blackIntensitySlider.Text = "Black Intensity";
            this.ChildControls.Add( this.blackIntensitySlider );
            this.whiteIntensitySlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.whiteIntensity, 0f, 10f, 1 );
            this.whiteIntensitySlider.Text = "White Intensity";
            this.ChildControls.Add( this.whiteIntensitySlider );
            this.midGreySlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.midGrey, 0f, 1f, 1 );
            this.midGreySlider.Text = "Mid Grey";
            this.ChildControls.Add( this.midGreySlider );
            this.softnessSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.softness, 0f, 1f, 1 );
            this.softnessSlider.Text = "Softness";
            this.ChildControls.Add( this.softnessSlider );

            this.monochromeTilingSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.monochromeTiling, 0f, 100f, 1 );
            this.monochromeTilingSlider.Text = "Monochrome Tiling";
            this.ChildControls.Add( this.monochromeTilingSlider );
            this.tilingRedSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.tiling.x, 0f, 100f, 1 );
            this.tilingRedSlider.Text = "Red Tiling";
            this.ChildControls.Add( this.tilingRedSlider );
            this.tilingGreenSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.tiling.y, 0f, 100f, 1 );
            this.tilingGreenSlider.Text = "Green Tiling";
            this.ChildControls.Add( this.tilingGreenSlider );
            this.tilingBlueSlider = new CustomSlider( NoiseAndGrainDef.noiseAndGrainEffect.tiling.z, 0f, 100f, 1 );
            this.tilingBlueSlider.Text = "Blue Tiling";
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
