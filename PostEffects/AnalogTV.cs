namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class AnalogTV : BaseEffect
	{
		public bool automaticPhase = true;

		public float phase = 0.5f;

		public bool convertToGrayscale = false;

		// [Range(0f, 1f)]
		public float noiseIntensity = 0.5f;

		// [Range(0f, 10f)]
		public float scanlinesIntensity = 2f;

		// [Range(0, 4096)]
		public int scanlinesCount = 768;

		public float scanlinesOffset = 0f;

		public bool verticalScanlines = false;

		// [Range(-2f, 2f)]
		public float distortion = 0.2f;

		// [Range(-2f, 2f)]
		public float cubicDistortion = 0.6f;

		// [Range(0.01f, 2f)]
		public float scale = 0.8f;

		protected virtual void Update()
		{
			if (automaticPhase)
			{
				// Reset the Phase after a while, some GPUs don't like big numbers
				if (phase > 1000f)
					phase = 10f;

				phase += Time.deltaTime * 0.25f;
			}
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Params1", new Vector4(noiseIntensity, scanlinesIntensity, scanlinesCount, scanlinesOffset));
			Material.SetVector("_Params2", new Vector4(phase, distortion, cubicDistortion, scale));

			int pass = verticalScanlines ? 2 : 0;
			pass += convertToGrayscale ? 1 : 0;

			Graphics.Blit(source, destination, Material, pass);
		}
	}
}
