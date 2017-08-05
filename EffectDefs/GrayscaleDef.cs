namespace CM3D2.SceneCapture.Plugin
{
    internal class GrayscaleDef
    {
        private static GrayscaleEffect grayscaleEffect;

        static GrayscaleDef()
        {
            if(grayscaleEffect == null)
            {
                grayscaleEffect = Util.GetComponentVar<GrayscaleEffect, GrayscaleEffect>(grayscaleEffect);
            }
        }

        public static void Update(GrayscalePane grayscalePane)
        {
            if (Instances.needEffectWindowReload == true)
                grayscalePane.IsEnabled = grayscaleEffect.enabled;
            else
                grayscaleEffect.enabled = grayscalePane.IsEnabled;
        }

        public static void Reset()
        {

        }

        public static void OnLoad()
        {

        }
    }
}
