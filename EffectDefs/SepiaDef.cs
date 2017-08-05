namespace CM3D2.SceneCapture.Plugin
{
    internal class SepiaDef
    {
        private static SepiaToneEffect sepiaEffect;

        static SepiaDef()
        {
            if(sepiaEffect == null)
            {
                sepiaEffect = Util.GetComponentVar<SepiaToneEffect, SepiaToneEffect>(sepiaEffect);
            }
        }

        public static void Update(SepiaPane sepiaPane)
        {
            if (Instances.needEffectWindowReload == true)
                sepiaPane.IsEnabled = sepiaEffect.enabled;
            else
                sepiaEffect.enabled = sepiaPane.IsEnabled;
        }

        public static void Reset()
        {

        }

        public static void OnLoad()
        {
                
        }
    }
}
