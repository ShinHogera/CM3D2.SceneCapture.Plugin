namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class ShadowsMidtonesHighlights : BaseEffect
	{
		public enum ColorMode
		{
			LiftGammaGain = 0,
			OffsetGammaSlope = 1
		}

		public ColorMode mode = ColorMode.LiftGammaGain;

		public Color shadows = new Color(1f, 1f, 1f, 0.5f);

		public Color midtones = new Color(1f, 1f, 1f, 0.5f);

		public Color highlights = new Color(1f, 1f, 1f, 0.5f);

		// [Range(0f, 1f)]
		public float amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			float multiplier;
			Material.SetVector("_Shadows", shadows * (shadows.a * 2));
			multiplier = 1f + (1f - (midtones.r * 0.299f + midtones.g * 0.587f + midtones.b * 0.114f));
			Material.SetVector("_Midtones", (midtones * multiplier) * (midtones.a * 2f));
			multiplier = 1f + (1f - (highlights.r * 0.299f + highlights.g * 0.587f + highlights.b * 0.114f));
			Material.SetVector("_Highlights", (highlights * multiplier) * (highlights.a * 2f));

			Material.SetFloat("_Amount", amount);

			Graphics.Blit(source, destination, Material, (int)mode);
		}
	}
}
