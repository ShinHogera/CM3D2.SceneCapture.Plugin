using UnityEngine;

namespace CM3D2.SceneCapture.Plugin
{
	public class HueFocus : BaseEffect
	{
		public float hue = 0f;
		public float range = 30f;
		public float boost = 0.5f;
		public float amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			float h = hue / 360f;
			float r = range / 180f;
			Material.SetVector("_Range", new Vector2(h - r, h + r));
			Material.SetVector("_Params", new Vector3(h, boost + 1f, amount));
			Graphics.Blit(source, destination, Material);
		}
	}
}
