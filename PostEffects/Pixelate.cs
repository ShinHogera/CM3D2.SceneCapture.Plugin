namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class Pixelate : BaseEffect
	{
		public enum SizeMode
		{
			ResolutionIndependent,
			PixelPerfect
		}

		// [Range(1f, 1024f)]
		public float scale = 80.0f;

		public bool automaticRatio = true;

		public float ratio = 1.0f;

		public SizeMode mode = SizeMode.ResolutionIndependent;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float newScale = this.scale;

			if (mode == SizeMode.PixelPerfect)
				newScale = (float)source.width / this.scale;

			Material.SetVector("_Params", new Vector2(
					newScale,
					automaticRatio ? ((float)source.width / (float)source.height) : ratio
				));

			Graphics.Blit(source, destination, Material);
		}
	}
}
