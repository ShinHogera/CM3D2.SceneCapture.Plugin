using UnityEngine;

namespace CM3D2.SceneCapture.Plugin
{
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class BaseEffect : MonoBehaviour
	{
		public Shader Shader;

		protected Material m_Material;
		public Material Material
		{
			get
			{
				if (m_Material == null)
				{
					m_Material = new Material(Shader);
					m_Material.hideFlags = HideFlags.HideAndDontSave;
				}

				return m_Material;
			}
		}

		protected virtual void Start()
		{
			// Disable if we don't support image effects
			if (!SystemInfo.supportsImageEffects)
			{
				Debug.LogWarning("Image effects aren't supported on this device");
				enabled = false;
				return;
			}

			// Disable the image effect if the shader can't run on the users graphics card
			if (!Shader || !Shader.isSupported)
			{
				Debug.LogWarning("The shader is null or unsupported on this device");
				enabled = false;
			}
		}

		protected virtual void OnDisable()
		{
			if (m_Material)
				DestroyImmediate(m_Material);
		}

		public void Apply(Texture source, RenderTexture destination)
		{
			if (source is RenderTexture)
			{
				OnRenderImage(source as RenderTexture, destination);
				return;
			}

			RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(source, rt);
			OnRenderImage(rt, destination);
			RenderTexture.ReleaseTemporary(rt);
		}

		protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination) { }
	}
}
