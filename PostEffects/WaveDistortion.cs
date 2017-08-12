namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class WaveDistortion : BaseEffect
	{
		// [Range(0f, 1f)]
		public float amplitude = 0.6f;

		public float waves = 5f;

		// [Range(0f, 5f)]
		public float colorGlitch = 0.35f;

		public float phase = 0.35f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float fp = CLib.Frac(phase);

			if (fp == 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_Params", new Vector4(
					Amplitude,
					Waves,
					ColorGlitch,
					fp
				));

			Graphics.Blit(source, destination, Material);
		}
	}
}
