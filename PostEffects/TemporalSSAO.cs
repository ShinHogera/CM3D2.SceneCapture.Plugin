using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CM3D2.SceneCapture.Plugin
{
    // requires GBufferUtils
    public class TemporalSSAO : BaseEffect
    {
        public enum SampleCount
        {
            Low,
            Medium,
            High,
        }
        public enum DebugOption
        {
            Off,
            ShowAO,
            ShowVelocity,
            ShowViewNormal,
        }

        public SampleCount sampleCount = SampleCount.Medium;
        // [Range(1,8)]
        public int downsampling = 3;
        // [Range(0.0f, 5.0f)]
        public float radius = 0.25f;
        // [Range(0.0f, 8.0f)]
        public float intensity = 1.5f;
        // [Range(0.0f, 8.0f)]
        public float blurSize = 0.5f;
        public bool dangerousSamples = true;
        public float maxAccumulation = 100.0f;

        // public Shader Shader;

        // Material Material;
        Mesh m_quad;
        RenderTexture[] m_ao_buffer = new RenderTexture[2];


        public static RenderTexture CreateRenderTexture(int w, int h, int d, RenderTextureFormat f)
        {
            RenderTexture r = new RenderTexture(w, h, d, f);
            r.filterMode = FilterMode.Point;
            r.useMipMap = false;
            r.generateMips = false;
            r.wrapMode = TextureWrapMode.Clamp;
            r.Create();
            return r;
        }

        void Reset()
        {
            // Shader = AssetDatabase.LoadAssetAtPath<Shader>("Assets/Ist/TemporalSSAO/Shaders/TemporalSSAO.shader");
            var gbu = GetComponent<GBufferUtils>();
            gbu.m_enable_inv_matrices = true;
            gbu.m_enable_prev_depth = true;
            gbu.m_enable_velocity = true;
        }

        void Awake()
        {
            var cam = GetComponent<Camera>();
            if (cam.renderingPath != RenderingPath.DeferredShading)
            {
                Debug.Log("ScreenSpaceReflections: Rendering path must be deferred.");
                cam.renderingPath = RenderingPath.DeferredShading;
            }
        }

        void OnDestroy()
        {
            Object.DestroyImmediate(Material);
        }

        protected override void OnDisable()
        {
            ReleaseRenderTargets();
        }

        void ReleaseRenderTargets()
        {
            for (int i = 0; i < m_ao_buffer.Length; ++i)
            {
                if (m_ao_buffer[i] != null)
                {
                    m_ao_buffer[i].Release();
                    m_ao_buffer[i] = null;
                }
            }
        }

        void UpdateRenderTargets()
        {
            Camera cam = GetComponent<Camera>();

            Vector2 reso = new Vector2(cam.pixelWidth, cam.pixelHeight) / downsampling;
            if (m_ao_buffer[0] != null && m_ao_buffer[0].width != (int)reso.x)
            {
                ReleaseRenderTargets();
            }
            if (m_ao_buffer[0] == null || !m_ao_buffer[0].IsCreated())
            {
                for (int i = 0; i < m_ao_buffer.Length; ++i)
                {
                    m_ao_buffer[i] = CreateRenderTexture((int)reso.x, (int)reso.y, 0, RenderTextureFormat.RGHalf);
                    m_ao_buffer[i].filterMode = FilterMode.Bilinear;
                    Graphics.SetRenderTarget(m_ao_buffer[i]);
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
                // Material.hideFlags = HideFlags.DontSave;

                m_quad = MeshUtils.GenerateQuad();
            }
            UpdateRenderTargets();

            switch(sampleCount)
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

            if (dangerousSamples)
            {
                Material.EnableKeyword("ENABLE_DANGEROUS_SAMPLES");
            }
            else
            {
                Material.DisableKeyword("ENABLE_DANGEROUS_SAMPLES");
            }

            Material.SetVector("_Params0", new Vector4(radius, intensity, maxAccumulation, 0.0f));
            Material.SetTexture("_AOBuffer", m_ao_buffer[1]);
            Material.SetTexture("_MainTex", src);

            // accumulate ao
            Graphics.SetRenderTarget(m_ao_buffer[0]);
            Material.SetPass(0);
            Graphics.DrawMeshNow(m_quad, Matrix4x4.identity);

            if(blurSize > 0.0f)
            {
                int w = (int)(m_ao_buffer[0].width * 1.0f);
                int h = (int)(m_ao_buffer[0].height * 1.0f);
                var tmp1 = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.RGHalf);
                var tmp2 = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.RGHalf);
                tmp1.filterMode = FilterMode.Trilinear;
                tmp2.filterMode = FilterMode.Trilinear;

                // horizontal blur
                Graphics.SetRenderTarget(tmp1);
                Material.SetTexture("_AOBuffer", m_ao_buffer[0]);
                Material.SetVector("_BlurOffset", new Vector4(blurSize / src.width, 0.0f, 0.0f, 0.0f));
                Material.EnableKeyword("BLUR_HORIZONTAL");
                Material.SetPass(1);
                Graphics.DrawMeshNow(m_quad, Matrix4x4.identity);
                Material.DisableKeyword("BLUR_HORIZONTAL");

                // vertical blur
                Graphics.SetRenderTarget(tmp2);
                Material.SetTexture("_AOBuffer", tmp1);
                Material.SetVector("_BlurOffset", new Vector4(0.0f, blurSize / src.height, 0.0f, 0.0f));
                Material.EnableKeyword("BLUR_VERTICAL");
                Material.SetPass(1);
                Graphics.DrawMeshNow(m_quad, Matrix4x4.identity);
                Material.DisableKeyword("BLUR_VERTICAL");

                // combine
                Graphics.SetRenderTarget(dst);
                Material.SetTexture("_AOBuffer", tmp2);
                Graphics.Blit(src, dst, Material, 2);

                RenderTexture.ReleaseTemporary(tmp2);
                RenderTexture.ReleaseTemporary(tmp1);
            }
            else
            {
                // combine
                Graphics.SetRenderTarget(dst);
                Material.SetTexture("_AOBuffer", m_ao_buffer[0]);
                Graphics.Blit(src, dst, Material, 2);
            }

            Swap(ref m_ao_buffer[0], ref m_ao_buffer[1]);
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
