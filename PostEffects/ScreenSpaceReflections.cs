using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CM3D2.SceneCapture.Plugin
{
    // requires GBufferUtils
    public class ScreenSpaceReflections : BaseEffect
    {
        public enum SampleCount
        {
            Low,
            Medium,
            High,
        }

        public SampleCount sampleCount = SampleCount.Medium;
        // [Range(1,8)]
        public int downsampling = 2;
        // [Range(0.0f, 2.0f)]
        public float intensity = 1.0f;
        // [Range(0.0f, 1.0f)]
        public float rayDiffusion = 0.01f;
        // [Range(0.0f, 8.0f)]
        public float blurSize = 1.0f;

        public float raymarchDistance = 2.5f;
        public float falloffDistance = 2.5f;
        public float rayHitDistance = 0.15f;
        public float maxAccumulation = 25.0f;
        public float stepBoost = 0.0f;
        public bool dangerousSamples = true;
        public bool preRaymarchPass = true;
        // public Shader Shader;

        // Material Material;
        Mesh m_quad = null;
        RenderTexture[] m_reflection_buffers = new RenderTexture[2];
        RenderTexture[] m_accumulation_buffers = new RenderTexture[2];
        RenderBuffer[] m_rb = new RenderBuffer[2];


        public static RenderTexture CreateRenderTexture(int w, int h, int d, RenderTextureFormat f)
        {
            RenderTexture r = new RenderTexture(w, h, d, f);
            r.filterMode = FilterMode.Bilinear;
            r.useMipMap = false;
            r.generateMips = false;
            r.Create();
            return r;
        }

        void Reset()
        {
            // Shader = AssetDatabase.LoadAssetAtPath<Shader>("Assets/Ist/ScreenSpaceReflections/Shaders/ScreenSpaceReflections.shader");
            var gbu = GetComponent<GBufferUtils>();
            gbu.m_enable_inv_matrices = true;
            gbu.m_enable_prev_depth = true;
            gbu.m_enable_velocity = true;
        }

        void Awake()
        {
#if UNITY_EDITOR
            var cam = GetComponent<Camera>();
            if (cam.renderingPath != RenderingPath.DeferredShading &&
                (cam.renderingPath == RenderingPath.UsePlayerSettings && PlayerSettings.renderingPath != RenderingPath.DeferredShading))
            {
                Debug.Log("ScreenSpaceReflections: Rendering path must be deferred.");
            }
#endif // UNITY_EDITOR
        }

        void OnDestroy()
        {
            // Object.DestroyImmediate(Material);
        }

        protected override void OnDisable()
        {
            ReleaseRenderTargets();
        }

        void ReleaseRenderTargets()
        {
            for (int i = 0; i < m_reflection_buffers.Length; ++i)
            {
                if (m_reflection_buffers[i] != null)
                {
                    m_reflection_buffers[i].Release();
                    m_reflection_buffers[i] = null;
                }
                if (m_accumulation_buffers[i] != null)
                {
                    m_accumulation_buffers[i].Release();
                    m_accumulation_buffers[i] = null;
                }
            }
        }

        void UpdateRenderTargets()
        {
            Camera cam = GetComponent<Camera>();

            Vector2 reso = new Vector2(cam.pixelWidth, cam.pixelHeight) / downsampling;
            if (m_reflection_buffers[0] != null && m_reflection_buffers[0].width != (int)reso.x)
            {
                ReleaseRenderTargets();
            }
            if (m_reflection_buffers[0] == null || !m_reflection_buffers[0].IsCreated())
            {
                for (int i = 0; i < m_reflection_buffers.Length; ++i)
                {
                    m_reflection_buffers[i] = CreateRenderTexture((int)reso.x, (int)reso.y, 0, RenderTextureFormat.ARGB32);
                    Graphics.SetRenderTarget(m_reflection_buffers[i]);
                    GL.Clear(false, true, Color.black);

                    m_accumulation_buffers[i] = CreateRenderTexture((int)reso.x, (int)reso.y, 0, RenderTextureFormat.R8);
                    Graphics.SetRenderTarget(m_accumulation_buffers[i]);
                    GL.Clear(false, true, Color.black);
                }
            }
        }

        [ImageEffectOpaque]
        protected override void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            GetComponent<GBufferUtils>().UpdateVelocityBuffer();

            if (m_quad == null)
            {
                // Material = new Material(Shader);
                // TODO
                // m_quad = MeshUtils.GenerateQuad();
            }
            UpdateRenderTargets();

            switch (sampleCount)
            {
                case SampleCount.Low:
                    Material.EnableKeyword("SAMPLES_LOW");
                    Material.DisableKeyword("SAMPLES_MEDIUM");
                    Material.DisableKeyword("SAMPLES_HIGH");
                    break;
                case SampleCount.Medium:
                    Material.DisableKeyword("SAMPLES_LOW");
                    Material.EnableKeyword("SAMPLES_MEDIUM");
                    Material.DisableKeyword("SAMPLES_HIGH");
                    break;
                case SampleCount.High:
                    Material.DisableKeyword("SAMPLES_LOW");
                    Material.DisableKeyword("SAMPLES_MEDIUM");
                    Material.EnableKeyword("SAMPLES_HIGH");
                    break;
            }
            if(preRaymarchPass) { Material.EnableKeyword("ENABLE_PREPASS"); }
            else                    { Material.DisableKeyword("ENABLE_PREPASS"); }

            if(dangerousSamples) { Material.EnableKeyword("ENABLE_DANGEROUS_SAMPLES"); }
            else                    { Material.DisableKeyword("ENABLE_DANGEROUS_SAMPLES"); }

            Material.SetVector("_Params0", new Vector4(intensity, raymarchDistance, rayDiffusion, falloffDistance));
            Material.SetVector("_Params1", new Vector4(maxAccumulation, rayHitDistance, stepBoost, 0.0f));
            Material.SetTexture("_ReflectionBuffer", m_reflection_buffers[1]);
            Material.SetTexture("_AccumulationBuffer", m_accumulation_buffers[1]);
            Material.SetTexture("_MainTex", src);



            // accumulate reflection
            m_rb[0] = m_reflection_buffers[0].colorBuffer;
            m_rb[1] = m_accumulation_buffers[0].colorBuffer;
            if (preRaymarchPass)
            {
                var prepass_buffer = RenderTexture.GetTemporary(
                    m_reflection_buffers[0].width / 4,
                    m_reflection_buffers[0].height / 4, 0, RenderTextureFormat.RHalf);
                prepass_buffer.filterMode = FilterMode.Bilinear;

                // raymarch in low-resolution buffer
                Graphics.SetRenderTarget(prepass_buffer);
                Material.SetPass(3);
                Graphics.DrawMeshNow(m_quad, Matrix4x4.identity);

                // continue raymarch from pre-march result
                Graphics.SetRenderTarget(m_rb, m_reflection_buffers[0].depthBuffer);
                Material.SetTexture("_PrePassBuffer", prepass_buffer);
                Material.SetPass(0);
                Graphics.DrawMeshNow(m_quad, Matrix4x4.identity);

                RenderTexture.ReleaseTemporary(prepass_buffer);
            }
            else
            {
                Graphics.SetRenderTarget(m_rb, m_reflection_buffers[0].depthBuffer);
                Material.SetPass(0);
                Graphics.DrawMeshNow(m_quad, Matrix4x4.identity);
            }


            var tmp1 = RenderTexture.GetTemporary(m_reflection_buffers[0].width, m_reflection_buffers[0].height, 0, RenderTextureFormat.ARGB32);
            var tmp2 = RenderTexture.GetTemporary(m_reflection_buffers[0].width, m_reflection_buffers[0].height, 0, RenderTextureFormat.ARGB32);
            tmp1.filterMode = FilterMode.Bilinear;
            tmp2.filterMode = FilterMode.Bilinear;

            if(blurSize > 0.0f)
            {
                // horizontal blur
                Graphics.SetRenderTarget(tmp1);
                Material.SetTexture("_ReflectionBuffer", m_reflection_buffers[0]);
                Material.SetVector("_BlurOffset", new Vector4(blurSize / src.width, 0.0f, 0.0f, 0.0f));
                Material.SetPass(1);
                Graphics.DrawMeshNow(m_quad, Matrix4x4.identity);

                // vertical blur
                Graphics.SetRenderTarget(tmp2);
                Material.SetTexture("_ReflectionBuffer", tmp1);
                Material.SetVector("_BlurOffset", new Vector4(0.0f, blurSize / src.height, 0.0f, 0.0f));
                Material.SetPass(1);
                Graphics.DrawMeshNow(m_quad, Matrix4x4.identity);
            }

            // combine
            Graphics.SetRenderTarget(dst);
            Material.SetTexture("_ReflectionBuffer", blurSize > 0.0f ? tmp2 : m_reflection_buffers[0]);
            Material.SetTexture("_AccumulationBuffer", m_accumulation_buffers[0]);
            Graphics.Blit(src, dst, Material, 2);

            RenderTexture.ReleaseTemporary(tmp2);
            RenderTexture.ReleaseTemporary(tmp1);


            Swap(ref m_reflection_buffers[0], ref m_reflection_buffers[1]);
            Swap(ref m_accumulation_buffers[0], ref m_accumulation_buffers[1]);
        }

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T tmp;
            tmp = lhs;
            lhs = rhs;
            rhs = tmp;
        }
    }
}
