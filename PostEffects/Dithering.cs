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

        protected Texture2D ditherPattern;

        protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (amount <= 0f)
            {
                Graphics.Blit(source, destination);

                return;
            }

                // m_DitherPattern = Resources.Load<Texture2D>("Misc/DitherPattern");
                ditherPattern = new Texture2D(8, 8);
                ditherPattern.LoadImage(System.IO.File.ReadAllBytes( System.IO.Directory.GetCurrentDirectory() + @"\DitherPattern.png"));
                ditherPattern.Apply();
            GetComponent<Camera>().renderingPath = RenderingPath.DeferredShading;


            Material.SetTexture("_Pattern", ditherPattern);
            Material.SetVector("_Params", new Vector4(redLuminance, greenLuminance, blueLuminance, amount));

            int pass = showOriginal ? 4 : 0;
            pass += convertToGrayscale ? 2 : 0;
            pass += CLib.IsLinearColorSpace() ? 1 : 0;

            Graphics.Blit(source, destination, Material, pass);
        }
    }
}
