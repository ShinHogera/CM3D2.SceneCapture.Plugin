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
    internal class EdgeDetectPane : BasePane
    {
        public EdgeDetectPane( int fontSize ) : base( fontSize, "Edge Detect" ) {}

        override public void SetupPane()
        {
            this.edgeDetectModeBox = new CustomComboBox( EDGEDETECT_MODES );
            this.edgeDetectModeBox.SelectedIndex = (int)EdgeDetectDef.edgeDetectEffect.mode;
            this.ChildControls.Add( this.edgeDetectModeBox );

            this.sensitivityNormalsSlider = new CustomSlider( EdgeDetectDef.edgeDetectEffect.sensitivityNormals, 0f, 10f, 1 );
            this.sensitivityNormalsSlider.Text = "Sensitivity Normals";
            this.ChildControls.Add( this.sensitivityNormalsSlider );

            this.edgeExpSlider = new CustomSlider( EdgeDetectDef.edgeDetectEffect.edgeExp, 0f, 1f, 1 );
            this.edgeExpSlider.Text = "Edges Exponent";
            this.ChildControls.Add( this.edgeExpSlider );

            this.sensitivityDepthSlider = new CustomSlider( EdgeDetectDef.edgeDetectEffect.sensitivityDepth, 0f, 10f, 1 );
            this.sensitivityDepthSlider.Text = "Sensitivity Depth";
            this.ChildControls.Add( this.sensitivityDepthSlider );

            this.lumThresholdSlider = new CustomSlider( EdgeDetectDef.edgeDetectEffect.lumThreshhold, 0f, 4f, 1 );
            this.lumThresholdSlider.Text = "Luminance Threshold";
            this.ChildControls.Add( this.lumThresholdSlider );

            this.sampleDistSlider = new CustomSlider( EdgeDetectDef.edgeDetectEffect.sampleDist, 0f, 10f, 1 );
            this.sampleDistSlider.Text = "Sample Distance";
            this.ChildControls.Add( this.sampleDistSlider );

            this.edgesOnlySlider = new CustomSlider( EdgeDetectDef.edgeDetectEffect.edgesOnly, 0f, 1f, 1 );
            this.edgesOnlySlider.Text = "Edges Only";
            this.ChildControls.Add( this.edgesOnlySlider );

            this.edgesOnlyBgColorPicker = new CustomColorPicker( EdgeDetectDef.edgeDetectEffect.edgesOnlyBgColor );
            this.edgesOnlyBgColorPicker.Text = "Edges Only Background";
            this.ChildControls.Add( this.edgesOnlyBgColorPicker );
        }

        override public void ShowPane()
        {
            GUIUtil.AddGUICheckbox(this, this.edgeDetectModeBox);
            switch( this.ModeValue )
            {
                case EdgeDetectMode.TriangleDepthNormals:
                case EdgeDetectMode.RobertsCrossDepthNormals:
                    GUIUtil.AddGUISlider(this, this.sensitivityNormalsSlider);
                    GUIUtil.AddGUISlider(this, this.sensitivityDepthSlider);
                    break;

                case EdgeDetectMode.SobelDepth:
                case EdgeDetectMode.SobelDepthThin:
                    GUIUtil.AddGUISlider(this, this.edgeExpSlider);
                    break;

                case EdgeDetectMode.TriangleLuminance:
                    GUIUtil.AddGUISlider(this, this.lumThresholdSlider);
                    break;

                default:
                    break;
            }
            GUIUtil.AddGUISlider(this, this.sampleDistSlider);
            GUIUtil.AddGUISlider(this, this.edgesOnlySlider);
            GUIUtil.AddGUICheckbox(this, this.edgesOnlyBgColorPicker);
        }

        override public void Reset()
        {
            
        }

        #region Properties
        public EdgeDetectMode ModeValue
        {
            get
            {
                return (EdgeDetectMode)Enum.Parse( typeof( EdgeDetectMode ), this.edgeDetectModeBox.SelectedItem);
            }
        }

        public float SensitivityNormalsValue
        {
            get
            {
                return this.sensitivityNormalsSlider.Value;
            }
        }

        public float SensitivityDepthValue
        {
            get
            {
                return this.sensitivityDepthSlider.Value;
            }
        }

        public float SampleDistValue
        {
            get
            {
                return this.sampleDistSlider.Value;
            }
        }

        public float LumThresholdValue
        {
            get
            {
                return this.lumThresholdSlider.Value;
            }
        }

        public float EdgeExpValue
        {
            get
            {
                return this.edgeExpSlider.Value;
            }
        }

        public float EdgesOnlyValue
        {
            get
            {
                return this.edgesOnlySlider.Value;
            }
        }

        public Color EdgesOnlyBgColorValue
        {
            get
            {
                return this.edgesOnlyBgColorPicker.Value;
            }
        }
        #endregion

        #region Fields
        private static readonly string[] EDGEDETECT_MODES = new string[] { "TriangleDepthNormals", "RobertsCrossDepthNormals", "SobelDepth", "SobelDepthThin", "TriangleLuminance" };

        private CustomComboBox edgeDetectModeBox = null;
        private CustomSlider sensitivityNormalsSlider = null;
        private CustomSlider sensitivityDepthSlider = null;
        private CustomSlider lumThresholdSlider = null;
        private CustomSlider edgeExpSlider = null;
        private CustomSlider sampleDistSlider = null;
        private CustomSlider edgesOnlySlider = null;
        private CustomColorPicker edgesOnlyBgColorPicker = null;
    }
    #endregion
}
