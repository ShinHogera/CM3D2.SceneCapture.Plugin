namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class LookupFilter : BaseEffect
	{
		public Texture2D lookupTexture;

		public float amount = 1f;

		public bool forceCompatibility = false;

            public bool needsUpdate { get; set; }

		protected Texture3D m_Lut3D;
		protected string m_BaseTextureName;
		protected bool m_Use2DLut = false;

		// public Shader Shader2D;
		// public Shader Shader3D;

		protected override void Start()
		{
		}

		protected override void OnDisable()
		{
			if (Material != null)
				DestroyImmediate(Material);

			if (m_Lut3D)
				DestroyImmediate(m_Lut3D);

			m_BaseTextureName = "";
		}

		protected void Reset()
		{
			m_BaseTextureName = "";
		}

		protected void SetIdentityLut()
		{
			int dim = 16;
			Color[] newC = new Color[dim * dim * dim];
			float oneOverDim = 1.0f / (1.0f * dim - 1.0f);

			for (int i = 0; i < dim; i++)
			{
				for (int j = 0; j < dim; j++)
				{
					for (int k = 0; k < dim; k++)
					{
						newC[i + (j * dim) + (k * dim * dim)] = new Color((i * 1.0f) * oneOverDim, (j * 1.0f) * oneOverDim, (k * 1.0f) * oneOverDim, 1.0f);
					}
				}
			}

			if (m_Lut3D)
				DestroyImmediate(m_Lut3D);

			m_Lut3D = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
			m_Lut3D.hideFlags = HideFlags.HideAndDontSave;
			m_Lut3D.SetPixels(newC);
			m_Lut3D.Apply();
			m_BaseTextureName = "";
		}

		public bool ValidDimensions(Texture2D tex2D)
		{
			if (tex2D == null || tex2D.height != Mathf.FloorToInt(Mathf.Sqrt(tex2D.width)))
				return false;

			return true;
		}

		protected void ConvertBaseTexture()
		{
			if (!ValidDimensions(lookupTexture))
			{
				Debug.LogWarning("The given 2D texture " + lookupTexture.name + " cannot be used as a 3D LUT. Pick another texture or adjust dimension to e.g. 256x16.");
				return;
			}

			m_BaseTextureName = lookupTexture.name;

			int dim = lookupTexture.height;

			Color[] c = lookupTexture.GetPixels();
			Color[] newC = new Color[c.Length];

			for (int i = 0; i < dim; i++)
			{
				for (int j = 0; j < dim; j++)
				{
					for (int k = 0; k < dim; k++)
					{
						int j_ = dim - j - 1;
						newC[i + (j * dim) + (k * dim * dim)] = c[k * dim + i + j_ * dim * dim];
					}
				}
			}

			if (m_Lut3D)
				DestroyImmediate(m_Lut3D);

			m_Lut3D = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
			m_Lut3D.hideFlags = HideFlags.HideAndDontSave;
			m_Lut3D.wrapMode = TextureWrapMode.Clamp;
			m_Lut3D.SetPixels(newC);
			m_Lut3D.Apply();

                        needsUpdate = false;
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

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (lookupTexture == null || amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

                        RenderLut3D(source, destination);
		}

		protected virtual void RenderLut3D(RenderTexture source, RenderTexture destination)
		{
			if (needsUpdate || lookupTexture.name != m_BaseTextureName)
				ConvertBaseTexture();

			if (m_Lut3D == null)
				SetIdentityLut();

			Material.SetTexture("_LookupTex", m_Lut3D);
			float lutSize = (float)m_Lut3D.width;
			Material.SetVector("_Params", new Vector3(
					(lutSize - 1f) / (1f * lutSize),
					1f / (2f * lutSize),
					amount
				));

			Graphics.Blit(source, destination, Material, CLib.IsLinearColorSpace() ? 1 : 0);
		}
	}
}
