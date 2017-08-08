using UnityEngine;
namespace CM3D2.SceneCapture.Plugin
{
    internal class SepiaDef
    {
        private static SepiaToneEffect sepiaEffect;

        static SepiaDef() {
            if(sepiaEffect == null)
            {
                sepiaEffect = Util.GetComponentVar<SepiaToneEffect, SepiaToneEffect>(sepiaEffect);
            }
            Debug.Log(sepiaEffect == null);
        }

        public static void Update(SepiaPane sepiaPane)
        {
            // FIXME: Becomes null after scene transition?
            if(sepiaEffect == null)
            {
                sepiaEffect = Util.GetComponentVar<SepiaToneEffect, SepiaToneEffect>(sepiaEffect);
            }

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
