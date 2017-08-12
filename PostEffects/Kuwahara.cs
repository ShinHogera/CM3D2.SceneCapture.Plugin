namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class Kuwahara : BaseEffect
	{
		// [Range(1, 6)]
		public int radius = 3;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			radius = Mathf.Clamp(radius, 1, 6);
			Material.SetVector("_PSize", new Vector2(1f / (float)source.width, 1f / (float)source.height));
			Graphics.Blit(source, destination, Material, radius - 1);
		}
	}
}
