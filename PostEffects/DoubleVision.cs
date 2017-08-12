namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class DoubleVision : BaseEffect
	{
		public Vector2 displace = new Vector2(0.7f, 0.0f);

		// [Range(0f, 1f)]
		public float amount = 1.0f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (amount <= 0f || displace == Vector2.zero)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_Displace", new Vector2(displace.x / (float)source.width, displace.y / (float)source.height));
			Material.SetFloat("_Amount", amount);
			Graphics.Blit(source, destination, Material);
		}
	}
}
