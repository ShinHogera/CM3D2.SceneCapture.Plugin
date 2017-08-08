using UnityEngine;
namespace CM3D2.SceneCapture.Plugin
{
    internal class SepiaDef
    {
        private static SepiaToneEffect sepiaEffect;

        static SepiaDef() {
            Debug.Log("new sep");
            if(sepiaEffect == null)
            {
                sepiaEffect = Util.GetComponentVar<SepiaToneEffect, SepiaToneEffect>(sepiaEffect);
            }
            Debug.Log(sepiaEffect == null);
        }

        public static void Update(SepiaPane sepiaPane)
        {
            Debug.Log(sepiaEffect == null);
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
