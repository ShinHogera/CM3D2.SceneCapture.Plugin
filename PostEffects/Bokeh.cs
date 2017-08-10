using UnityEngine;
using UnityEngine.Serialization;

namespace CM3D2.SceneCapture.Plugin
{
    public class Bokeh : BaseEffect
    {
        #region Editable properties

        public Transform _pointOfFocus;

        public Transform pointOfFocus {
            get { return _pointOfFocus; }
            set { _pointOfFocus = value; }
        }

        public float _focusDistance = 10.0f;

        public float focusDistance {
            get { return _focusDistance; }
            set { _focusDistance = value; }
        }

        public float _fNumber = 1.4f;

        public float fNumber {
            get { return _fNumber; }
            set { _fNumber = value; }
        }

        public bool _useCameraFov = true;

        public bool useCameraFov {
            get { return _useCameraFov; }
            set { _useCameraFov = value; }
        }

        public float _focalLength = 0.05f;

        public float focalLength {
            get { return _focalLength; }
            set { _focalLength = value; }
        }

        public enum KernelSize { Small, Medium, Large, VeryLarge }

        public KernelSize _kernelSize = KernelSize.Medium;

        public KernelSize kernelSize {
            get { return _kernelSize; }
            set { _kernelSize = value; }
        }

        #endregion

        #region Debug properties

        public bool _visualize = false;
        public bool visualize {
            get { return _visualize; }
            set { _visualize = value; }
        }

        #endregion

        #region Private members

        // Height of the 35mm full-frame format (36mm x 24mm)
        const float kFilmHeight = 0.024f;

        Camera TargetCamera {
            get { return GetComponent<Camera>(); }
        }

        float CalculateFocusDistance()
        {
            if (_pointOfFocus == null) return _focusDistance;
            var cam = TargetCamera.transform;
            return Vector3.Dot(_pointOfFocus.position - cam.position, cam.forward);
        }

        float CalculateFocalLength()
        {
            if (!_useCameraFov) return _focalLength;
            var fov = TargetCamera.fieldOfView * Mathf.Deg2Rad;
            return 0.5f * kFilmHeight / Mathf.Tan(0.5f * fov);
        }

        float CalculateMaxCoCRadius(int screenHeight)
        {
            // Estimate the allowable maximum radius of CoC from the kernel
            // size (the equation below was empirically derived).
            var radiusInPixels = (float)_kernelSize * 4 + 6;

            // Applying a 5% limit to the CoC radius to keep the size of
            // TileMax/NeighborMax small enough.
            return Mathf.Min(0.05f, radiusInPixels / screenHeight);
        }

        void SetUpShaderParameters(RenderTexture source)
        {
            var s1 = CalculateFocusDistance();
            var f = CalculateFocalLength();
            s1 = Mathf.Max(s1, f);
            Material.SetFloat("_Distance", s1);

            var coeff = f * f / (_fNumber * (s1 - f) * kFilmHeight * 2);
            Material.SetFloat("_LensCoeff", coeff);

            var maxCoC = CalculateMaxCoCRadius(source.height);
            Material.SetFloat("_MaxCoC", maxCoC);
            Material.SetFloat("_RcpMaxCoC", 1 / maxCoC);

            var rcpAspect = (float)source.height / source.width;
            Material.SetFloat("_RcpAspect", rcpAspect);
        }

        #endregion

        #region MonoBehaviour functions

        void OnEnable()
        {
            TargetCamera.depthTextureMode |= DepthTextureMode.Depth;
        }

        void OnDestroy()
        {
            // Destroy the temporary objects.
            if (Material != null)
                if (Application.isPlaying)
                    Destroy(Material);
                else
                    DestroyImmediate(Material);
        }

        void Update()
        {
            if (_focusDistance < 0.01f) _focusDistance = 0.01f;
            if (_fNumber < 0.1f) _fNumber = 0.1f;
        }

        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // If the material hasn't been initialized because of system
            // incompatibility, just blit and return.
            if (Material == null)
            {
                Graphics.Blit(source, destination);
                // Try to disable itself if it's Player.
                // if (Application.isPlaying) enabled = false;
                return;
            }

            var width = source.width;
            var height = source.height;
            var format = RenderTextureFormat.ARGBHalf;

            SetUpShaderParameters(source);

            // Focus range visualization
            if (_visualize)
            {
                Graphics.Blit(source, destination, Material, 7);
                return;
            }

            // Pass #1 - Downsampling, prefiltering and CoC calculation
            var rt1 = RenderTexture.GetTemporary(width / 2, height / 2, 0, format);
            source.filterMode = FilterMode.Point;
            Graphics.Blit(source, rt1, Material, 0);

            // Pass #2 - Bokeh simulation
            var rt2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, format);
            rt1.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rt1, rt2, Material, 1 + (int)_kernelSize);

            // Pass #3 - Additional blur
            rt2.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rt2, rt1, Material, 5);

            // Pass #4 - Upsampling and composition
            Material.SetTexture("_BlurTex", rt1);
            Graphics.Blit(source, destination, Material, 6);

            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }

        #endregion
    }
}
