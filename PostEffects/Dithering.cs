namespace CM3D2.SceneCapture.Plugin
{
    using UnityEngine;

    public class Dithering : BaseEffect
    {
        public bool showOriginal = false;

        public bool convertToGrayscale = false;

        // [Range(0f, 1f)]
        public float redLuminance = 0.299f;

        // [Range(0f, 1f)]
        public float greenLuminance = 0.587f;

        // [Range(0f, 1f)]
        public float blueLuminance = 0.114f;

        // [Range(0f, 1f)]
        public float amount = 1f;

        public Texture2D m_DitherPattern { get; set; }

        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (amount <= 0f)
            {
                Graphics.Blit(source, destination);
                return;
            }

            if (m_DitherPattern == null) {}
            // m_DitherPattern = Resources.Load<Texture2D>("Misc/DitherPattern");

            Material.SetTexture("_Pattern", m_DitherPattern);
            Material.SetVector("_Params", new Vector4(redLuminance, greenLuminance, blueLuminance, amount));

            int pass = showOriginal ? 4 : 0;
            pass += convertToGrayscale ? 2 : 0;
            pass += CLib.IsLinearColorSpace() ? 1 : 0;

            Graphics.Blit(source, destination, Material, pass);
        }
    }
}
