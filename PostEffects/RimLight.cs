using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CM3D2.SceneCapture.Plugin
{
    // requires GBufferUtils
    public class RimLight : BaseEffect
    {
        public Color color = new Color(0.75f, 0.75f, 1.0f, 0.0f);
        public float intensity = 1.0f;
        public float fresnelBias = 0.0f;
        public float fresnelScale = 5.0f;
        public float fresnelPow = 5.0f;

        public bool edgeHighlighting = true;
        public float edgeIntensity = 0.3f;

        // [Range(0.0f, .99f)]
        public float edgeThreshold = 0.8f;

        public float edgeRadius = 1.0f;
        public bool mulSmoothness = true;
        // public Shader m_shader;
        // Material m_material;

        public Vector4 GetLinearColor()
        {
            return new Vector4(
                Mathf.GammaToLinearSpace(color.r),
                Mathf.GammaToLinearSpace(color.g),
                Mathf.GammaToLinearSpace(color.b),
                1.0f
            );
        }

        void Reset()
        {
            GetComponent<GBufferUtils>().m_enable_inv_matrices = true;
        }

        void OnDestroy()
        {
            // if (Material != null)
            // {
            //     Object.DestroyImmediate(m_material);
            // }
        }

        [ImageEffectOpaque]
        protected override void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            // if (m_material == null)
            // {
            //     m_material = new Material(Shader);
            // }

            if (edgeHighlighting)
            {
                Material.EnableKeyword("ENABLE_EDGE_HIGHLIGHTING");
            }
            else
            {
                Material.DisableKeyword("ENABLE_EDGE_HIGHLIGHTING");
            }

            if (mulSmoothness)
            {
                Material.EnableKeyword("ENABLE_SMOOTHNESS_ATTENUAION");
            }
            else
            {
                Material.DisableKeyword("ENABLE_SMOOTHNESS_ATTENUAION");
            }

            Material.SetVector("_Color", GetLinearColor());
            Material.SetVector("_Params1", new Vector4(fresnelBias, fresnelScale, fresnelPow, intensity));
            Material.SetVector("_Params2", new Vector4(edgeIntensity, edgeThreshold, edgeRadius, 0.0f));
            Graphics.Blit(src, dst, Material);
        }
    }
}
