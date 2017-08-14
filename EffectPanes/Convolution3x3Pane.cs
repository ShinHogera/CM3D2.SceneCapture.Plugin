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
    internal class Convolution3x3Pane : BasePane
    {
        public Convolution3x3Pane( int fontSize ) : base( fontSize, Translation.GetText("Panes", "Convolution3x3") ) {}

        override public void SetupPane()
        {
            this.convolutionMatrixLabel = new CustomLabel();
            this.convolutionMatrixLabel.Text = Translation.GetText("Convolution3x3", "convolutionMatrix");
            this.ChildControls.Add( this.convolutionMatrixLabel );

            this.divisorSlider = new CustomSlider( Convolution3x3Def.convolution3x3Effect.divisor, 0.0f, 10f, 4 );
            this.divisorSlider.Text = Translation.GetText("Convolution3x3", "divisor");
            this.ChildControls.Add( this.divisorSlider );

            this.amountSlider = new CustomSlider( Convolution3x3Def.convolution3x3Effect.amount, 0.0f, 1f, 4 );
            this.amountSlider.Text = Translation.GetText("Convolution3x3", "amount");
            this.ChildControls.Add( this.amountSlider );

            this.kernelTopXField = new CustomTextField( Convolution3x3Def.convolution3x3Effect.kernelTop.x.ToString() );
            this.kernelTopXField.Text = "";
            this.ChildControls.Add( this.kernelTopXField );

            this.kernelTopYField = new CustomTextField( Convolution3x3Def.convolution3x3Effect.kernelTop.y.ToString() );
            this.kernelTopYField.Text = "";
            this.ChildControls.Add( this.kernelTopYField );

            this.kernelTopZField = new CustomTextField( Convolution3x3Def.convolution3x3Effect.kernelTop.z.ToString() );
            this.kernelTopZField.Text = "";
            this.ChildControls.Add( this.kernelTopZField );

            this.kernelMiddleXField = new CustomTextField( Convolution3x3Def.convolution3x3Effect.kernelMiddle.x.ToString() );
            this.kernelMiddleXField.Text = "";
            this.ChildControls.Add( this.kernelMiddleXField );

            this.kernelMiddleYField = new CustomTextField( Convolution3x3Def.convolution3x3Effect.kernelMiddle.y.ToString() );
            this.kernelMiddleYField.Text = "";
            this.ChildControls.Add( this.kernelMiddleYField );

            this.kernelMiddleZField = new CustomTextField( Convolution3x3Def.convolution3x3Effect.kernelMiddle.z.ToString() );
            this.kernelMiddleZField.Text = "";
            this.ChildControls.Add( this.kernelMiddleZField );

            this.kernelBottomXField = new CustomTextField( Convolution3x3Def.convolution3x3Effect.kernelBottom.x.ToString() );
            this.kernelBottomXField.Text = "";
            this.ChildControls.Add( this.kernelBottomXField );

            this.kernelBottomYField = new CustomTextField( Convolution3x3Def.convolution3x3Effect.kernelBottom.y.ToString() );
            this.kernelBottomYField.Text = "";
            this.ChildControls.Add( this.kernelBottomYField );

            this.kernelBottomZField = new CustomTextField( Convolution3x3Def.convolution3x3Effect.kernelBottom.z.ToString() );
            this.kernelBottomZField.Text = "";
            this.ChildControls.Add( this.kernelBottomZField );
        }

        override public void ShowPane()
        {
            this.convolutionMatrixLabel.Left = this.Left + ControlBase.FixedMargin;
            this.convolutionMatrixLabel.Top = this.Top + this.ControlHeight + ControlBase.FixedMargin;
            this.convolutionMatrixLabel.Width = (this.Width / 3) - ControlBase.FixedMargin / 4;
            this.convolutionMatrixLabel.Height = this.ControlHeight;
            this.convolutionMatrixLabel.OnGUI();
            this.convolutionMatrixLabel.Visible = true;

            GUIUtil.AddGUIButtonAfter(this, this.kernelTopXField, this.convolutionMatrixLabel, 3);
            GUIUtil.AddGUIButton(this, this.kernelTopYField, this.kernelTopXField, 3);
            GUIUtil.AddGUIButton(this, this.kernelTopZField, this.kernelTopYField, 3);
            GUIUtil.AddGUIButtonAfter(this, this.kernelMiddleXField, this.kernelTopXField, 3);
            GUIUtil.AddGUIButton(this, this.kernelMiddleYField, this.kernelMiddleXField, 3);
            GUIUtil.AddGUIButton(this, this.kernelMiddleZField, this.kernelMiddleYField, 3);
            GUIUtil.AddGUIButtonAfter(this, this.kernelBottomXField, this.kernelMiddleXField, 3);
            GUIUtil.AddGUIButton(this, this.kernelBottomYField, this.kernelBottomXField, 3);
            GUIUtil.AddGUIButton(this, this.kernelBottomZField, this.kernelBottomYField, 3);
            GUIUtil.AddGUISlider(this, this.divisorSlider);
            GUIUtil.AddGUISlider(this, this.amountSlider);
        }

        override public void Reset()
        {
            Convolution3x3Def.Reset();
        }

        #region Properties
        public Vector3 KernelTopValue
        {
            get
            {
                Vector3 vec = Convolution3x3Def.kernelTop;
                float fOut;
                if(float.TryParse(this.kernelTopXField.Value, out fOut))
                    vec.x = fOut;
                if(float.TryParse(this.kernelTopYField.Value, out fOut))
                    vec.y = fOut;
                if(float.TryParse(this.kernelTopZField.Value, out fOut))
                    vec.z = fOut;
                return vec;
            }
        }
        public Vector3 KernelMiddleValue
        {
            get
            {
                Vector3 vec = Convolution3x3Def.kernelMiddle;
                float fOut;
                if(float.TryParse(this.kernelMiddleXField.Value, out fOut))
                    vec.x = fOut;
                if(float.TryParse(this.kernelMiddleYField.Value, out fOut))
                    vec.y = fOut;
                if(float.TryParse(this.kernelMiddleZField.Value, out fOut))
                    vec.z = fOut;
                return vec;
            }
        }
        public Vector3 KernelBottomValue
        {
            get
            {
                Vector3 vec = Convolution3x3Def.kernelBottom;
                float fOut;
                if(float.TryParse(this.kernelBottomXField.Value, out fOut))
                    vec.x = fOut;
                if(float.TryParse(this.kernelBottomYField.Value, out fOut))
                    vec.y = fOut;
                if(float.TryParse(this.kernelBottomZField.Value, out fOut))
                    vec.z = fOut;
                return vec;
            }
        }
        public float DivisorValue
        {
            get
            {
                return this.divisorSlider.Value;
            }
        }

        public float AmountValue
        {
            get
            {
                return this.amountSlider.Value;
            }
        }

        #endregion

        #region Fields
        private CustomLabel convolutionMatrixLabel = null;
        private CustomTextField kernelTopXField = null;
        private CustomTextField kernelTopYField = null;
        private CustomTextField kernelTopZField = null;
        private CustomTextField kernelMiddleXField = null;
        private CustomTextField kernelMiddleYField = null;
        private CustomTextField kernelMiddleZField = null;
        private CustomTextField kernelBottomXField = null;
        private CustomTextField kernelBottomYField = null;
        private CustomTextField kernelBottomZField = null;
        private CustomSlider divisorSlider = null;
        private CustomSlider amountSlider = null;
        #endregion
    }
}
