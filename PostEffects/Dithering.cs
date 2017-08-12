namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class Dithering : BaseEffect
	{
		public bool ShowOriginal = false;

		public bool ConvertToGrayscale = false;

		// [Range(0f, 1f)]
		public float RedLuminance = 0.299f;

		// [Range(0f, 1f)]
		public float GreenLuminance = 0.587f;

		// [Range(0f, 1f)]
		public float BlueLuminance = 0.114f;

		// [Range(0f, 1f)]
		public float Amount = 1f;

		protected Texture2D m_DitherPattern;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			if (m_DitherPattern == null)
				m_DitherPattern = Resources.Load<Texture2D>("Misc/DitherPattern");

			Material.SetTexture("_Pattern", m_DitherPattern);
			Material.SetVector("_Params", new Vector4(RedLuminance, GreenLuminance, BlueLuminance, Amount));

			int pass = ShowOriginal ? 4 : 0;
			pass += ConvertToGrayscale ? 2 : 0;
			pass += CLib.IsLinearColorSpace() ? 1 : 0;

			Graphics.Blit(source, destination, Material, pass);
		}
	}
}
