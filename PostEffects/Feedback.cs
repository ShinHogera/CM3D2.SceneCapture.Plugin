using UnityEngine;
using UnityEngine.Rendering;

namespace CM3D2.SceneCapture.Plugin
{
    public class Feedback : BaseEffect
    {
        #region Editable properties

        /// Feedback color
        public Color color {
            get { return _color; }
            set { _color = value; }
        }

        public Color _color = Color.white;

        /// Horizontal offset for feedback
        public float offsetX {
            get { return _offsetX; }
            set { _offsetX = value; }
        }

        public float _offsetX = 0;

        /// Vertical offset for feedback
        public float offsetY {
            get { return _offsetY; }
            set { _offsetY = value; }
        }

        public float _offsetY = 0;

        /// Center-axis rotation for feedback
        public float rotation {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public float _rotation = 0;

        /// Scale factor for feedback
        public float scale {
            get { return _scale; }
            set { _scale = value; }
        }

        public float _scale = 1;

        /// Disables bilinear filter
        public bool jaggies {
            get { return _jaggies; }
            set { _jaggies = value; }
        }

        public bool _jaggies = false;

        #endregion

        #region Private members

        Mesh _mesh;

        RenderTexture _delayBuffer;
        CommandBuffer _command;

        // 2D rotation matrix
        Vector4 rotationMatrixAsVector {
            get {
                var angle = -Mathf.Deg2Rad * _rotation;
                var sin = Mathf.Sin(angle);
                var cos = Mathf.Cos(angle);
                return new Vector4(cos, sin, -sin, cos);
            }
        }

        // Initialize the delay buffer and the feedback command.
        void StartFeedback()
        {
            var camera = GetComponent<Camera>();

            // Initialize the delay buffer.
            _delayBuffer = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight);
            _delayBuffer.wrapMode = TextureWrapMode.Clamp;
            Material.SetTexture("_MainTex", _delayBuffer);

            // Create a feedback command and attach it to the camera.
            if (_command == null) {
                _command = new CommandBuffer();
                _command.name = "Kino.Feedback";
            }
            _command.Clear();
            _command.DrawMesh(_mesh, Matrix4x4.identity, Material, 0, 0);
            camera.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, _command);
        }

        #endregion

        #region MonoBehaviour Functions

        protected override void OnDisable()
        {
            // Do nothing if it hasn't been initialized.
            if (_delayBuffer == null) return;

            // Release the delay buffer.
            RenderTexture.ReleaseTemporary(_delayBuffer);
            _delayBuffer = null;

            // Detach the command buffer from the camera.
            var camera = GetComponent<Camera>();
            camera.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, _command);
        }

        void OnDestroy()
        {
            if (Application.isPlaying)
                Destroy(Material);
            else
                DestroyImmediate(Material);
        }

        void Update()
        {
            // Do nothing if it hasn't been initialized.
            if (_delayBuffer == null) return;

            // Release temporary objects when the screen resolution is changed.
            var camera = GetComponent<Camera>();
            if (camera.pixelWidth != _delayBuffer.width ||
                camera.pixelHeight != _delayBuffer.height)
            {
                OnDisable();
                return;
            }

            // Update the shader/texture properties.
            Material.SetColor("_Color", _color);
            Material.SetVector("_Offset", new Vector2(_offsetX, _offsetY) * -0.05f);
            Material.SetVector("_Rotation", rotationMatrixAsVector);
            Material.SetFloat("_Scale", 2 - _scale);
            _delayBuffer.filterMode = _jaggies ? FilterMode.Point : FilterMode.Bilinear;
        }

        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Lazy initialization of the delay buffer.
            if (_delayBuffer == null) StartFeedback();

            Update();

            // Copy the content of the frame to the delay buffer.
            Graphics.Blit(source, _delayBuffer);
            Graphics.Blit(source, destination);
        }

        #endregion
    }
}
