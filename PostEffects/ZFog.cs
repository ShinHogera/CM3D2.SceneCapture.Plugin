using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CM3D2.SceneCapture.Plugin
{
    // requires GBufferUtils
    public class ZFog : BaseEffect
    {
        public Color color1 = new Color(0.75f, 0.75f, 1.0f, 1.0f);
        public float hdr1 = 1.0f;
        public float near1 = 10.0f;
        public float far1 = 30.0f;
        public float pow1 = 1.0f;

        public Color color2 = new Color(0.75f, 0.75f, 1.0f, 1.0f);
        public float hdr2 = 1.0f;
        public float near2 = 10.0f;
        public float far2 = 30.0f;
        public float pow2 = 1.0f;


        // public Shader Shader;
        // Material Material;

        public Vector4 GetLinearColor1()
        {
            return new Vector4(
                Mathf.GammaToLinearSpace(color1.r*hdr1),
                Mathf.GammaToLinearSpace(color1.g*hdr1),
                Mathf.GammaToLinearSpace(color1.b*hdr1),
                color1.a
            );
        }

        public Vector4 GetLinearColor2()
        {
            return new Vector4(
                Mathf.GammaToLinearSpace(color2.r * hdr2),
                Mathf.GammaToLinearSpace(color2.g * hdr2),
                Mathf.GammaToLinearSpace(color2.b * hdr2),
                color2.a
            );
        }

#if UNITY_EDITOR
        void Reset()
        {
            // Shader = AssetDatabase.LoadAssetAtPath<Shader>("Assets/Ist/RimLight/Shaders/ZFog.shader");
            GetComponent<GBufferUtils>().m_enable_inv_matrices = true;
        }
#endif // UNITY_EDITOR

        void OnDestroy()
        {
            // if (Material != null)
            // {
            //     Object.DestroyImmediate(Material);
            // }
        }

        [ImageEffectOpaque]
        protected override void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            // if (Material == null)
            // {
            //     Material = new Material(Shader);
            // }

            Material.SetVector("_Color1", GetLinearColor1());
            Material.SetVector("_Params1", new Vector4(near1, far1, pow1, 0.0f));
            Material.SetVector("_Color2", GetLinearColor2());
            Material.SetVector("_Params2", new Vector4(near2, far2, pow2, 0.0f));
            Graphics.Blit(src, dst, Material);
        }
    }
}
