//
// KinoIsoline - Isoline effect
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using UnityEngine;

namespace CM3D2.SceneCapture.Plugin
{
    public class Isoline : BaseEffect
    {
        #region Public Properties

        // Line color
        // [SerializeField, ColorUsage(true, true, 0, 8, 0.125f, 3)]
        public Color _lineColor = Color.white;

        public Color lineColor {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        // Luminance blending ratio
        // [SerializeField, Range(0, 1)]
        public float _luminanceBlending = 1;

        public float luminanceBlending {
            get { return _luminanceBlending; }
            set { _luminanceBlending = value; }
        }

        // Depth fall-off
        // [SerializeField]
        public float _fallOffDepth = 40;

        public float fallOffDepth {
            get { return _fallOffDepth; }
            set { _fallOffDepth = value; }
        }

        // Background color
        // [SerializeField]
        public Color _backgroundColor = Color.black;

        public Color backgroundColor {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        // Slicing axis
        // [SerializeField]
        public Vector3 _axis = Vector3.one * 0.577f;

        public Vector3 axis {
            get { return _axis; }
            set { _axis = value; }
        }

        // Contour interval
        // [SerializeField]
        public float _interval = 0.25f;

        public float interval {
            get { return _interval; }
            set { _interval = value; }
        }

        // Offset
        // [SerializeField]
        public Vector3 _offset;

        public Vector3 offset {
            get { return _offset; }
            set { _offset = value; }
        }

        // Distortion frequency
        // [SerializeField]
        public float _distortionFrequency = 1;

        public float distortionFrequency {
            get { return _distortionFrequency; }
            set { _distortionFrequency = value; }
        }

        // Distortion amount
        // [SerializeField]
        public float _distortionAmount = 0;

        public float distortionAmount {
            get { return _distortionAmount; }
            set { _distortionAmount = value; }
        }

        // Modulation mode
        public enum ModulationMode {
            None, Frac, Sin, Noise
        }

        // [SerializeField]
        public ModulationMode _modulationMode = ModulationMode.None;

        public ModulationMode modulationMode {
            get { return _modulationMode; }
            set { _modulationMode = value; }
        }

        // Modulation axis
        // [SerializeField]
        public Vector3 _modulationAxis = Vector3.forward;

        public Vector3 modulationAxis {
            get { return _modulationAxis; }
            set { _modulationAxis = value; }
        }

        // Modulation frequency
        // [SerializeField]
        public float _modulationFrequency = 0.2f;

        public float modulationFrequency {
            get { return _modulationFrequency; }
            set { _modulationFrequency = value; }
        }

        // Modulation speed
        // [SerializeField]
        public float _modulationSpeed = 1;

        public float modulationSpeed {
            get { return _modulationSpeed; }
            set { _modulationSpeed = value; }
        }

        // Modulation exponent
        // [SerializeField, Range(1, 50)]
        public float _modulationExponent = 24;

        public float modulationExponent {
            get { return _modulationExponent; }
            set { _modulationExponent = value; }
        }

        #endregion

        #region IsolineScroller Properties
        public Vector3 _direction = Vector3.one * 0.577f;

        public Vector3 direction {
            get { return _direction; }
            set { _direction = value; }
        }

        public float _speed = 0.2f;

        public float speed {
            get { return _speed; }
            set { _speed = value; }
        }

        #endregion

        #region Private Properties

        float _modulationTime;

        #endregion

        #region MonoBehaviour Functions

        void OnEnable()
        {
            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
        }

        void Update()
        {
            _modulationTime += Time.deltaTime * _modulationSpeed;

            var delta = _direction.normalized * _speed * Time.deltaTime;
            this.offset += delta;
        }

        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // if (Material == null) {
            //     Material = new Material(_shader);
            //     Material.hideFlags = HideFlags.DontSave;
            // }

            var matrix = GetComponent<Camera>().cameraToWorldMatrix;
            Material.SetMatrix("_InverseView", matrix);

            Material.SetColor("_Color", _lineColor);
            Material.SetFloat("_FallOffDepth", _fallOffDepth);
            Material.SetFloat("_Blend", _luminanceBlending);
            Material.SetColor("_BgColor", _backgroundColor);

            Material.SetVector("_Axis", _axis.normalized);
            Material.SetFloat("_Density", 1.0f / _interval);
            Material.SetVector("_Offset", _offset);

            Material.SetFloat("_DistFreq", _distortionFrequency);
            Material.SetFloat("_DistAmp", _distortionAmount);

            if (_distortionAmount > 0)
                Material.EnableKeyword("DISTORTION");
            else
                Material.DisableKeyword("DISTORTION");

            Material.DisableKeyword("MODULATION_FRAC");
            Material.DisableKeyword("MODULATION_SIN");
            Material.DisableKeyword("MODULATION_NOISE");

            if (_modulationMode == ModulationMode.Frac)
                Material.EnableKeyword("MODULATION_FRAC");
            else if (_modulationMode == ModulationMode.Sin)
                Material.EnableKeyword("MODULATION_SIN");
            else if (_modulationMode == ModulationMode.Noise)
                Material.EnableKeyword("MODULATION_NOISE");

            var modFreq = _modulationFrequency;
            if (_modulationMode == ModulationMode.Sin)
                modFreq *= Mathf.PI * 2;

            Material.SetVector("_ModAxis", _modulationAxis.normalized);
            Material.SetFloat("_ModFreq", modFreq);
            Material.SetFloat("_ModTime", _modulationTime);
            Material.SetFloat("_ModExp", _modulationExponent);

            Graphics.Blit(source, destination, Material);
        }

        #endregion
    }
}
