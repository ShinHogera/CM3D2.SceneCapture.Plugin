using UnityEngine;

namespace CM3D2.SceneCapture.Plugin
{
    public class AnalogGlitch : BaseEffect
    {
        #region Public Properties

        // Scan line jitter

        public float _scanLineJitter = 0;

        public float scanLineJitter {
            get { return _scanLineJitter; }
            set { _scanLineJitter = value; }
        }

        // Vertical jump

        public float _verticalJump = 0;

        public float verticalJump {
            get { return _verticalJump; }
            set { _verticalJump = value; }
        }

        // Horizontal shake

        public float _horizontalShake = 0;

        public float horizontalShake {
            get { return _horizontalShake; }
            set { _horizontalShake = value; }
        }

        // Color drift

        public float _colorDrift = 0;

        public float colorDrift {
            get { return _colorDrift; }
            set { _colorDrift = value; }
        }

        #endregion

        #region Private Properties

        float _verticalJumpTime;

        #endregion

        #region MonoBehaviour Functions

        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _verticalJumpTime += Time.deltaTime * _verticalJump * 11.3f;

            var sl_thresh = Mathf.Clamp01(1.0f - _scanLineJitter * 1.2f);
            var sl_disp = 0.002f + Mathf.Pow(_scanLineJitter, 3) * 0.05f;
            Material.SetVector("_ScanLineJitter", new Vector2(sl_disp, sl_thresh));

            var vj = new Vector2(_verticalJump, _verticalJumpTime);
            Material.SetVector("_VerticalJump", vj);

            Material.SetFloat("_HorizontalShake", _horizontalShake * 0.2f);

            var cd = new Vector2(_colorDrift * 0.04f, Time.time * 606.11f);
            Material.SetVector("_ColorDrift", cd);

            Graphics.Blit(source, destination, Material);
        }

        #endregion
    }
}
