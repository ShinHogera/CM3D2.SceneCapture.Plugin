namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class BrightnessContrastGamma : BaseEffect
	{
		// [Range(-100f, 100f)]
		public float brightness = 0f;

		// [Range(-100f, 100f)]
		public float contrast = 0f;

		public Vector3 contrastCoeff = new Vector3(0.5f, 0.5f, 0.5f);

		// [Range(0.1f, 9.9f)]
		public float gamma = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (brightness == 0f && contrast == 0f && gamma == 1f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_BCG", new Vector4((brightness + 100f) * 0.01f, (contrast + 100f) * 0.01f, 1.0f / gamma));
			Material.SetVector("_Coeffs", contrastCoeff);
			Graphics.Blit(source, destination, Material);
		}
	}
}
