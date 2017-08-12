namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class BleachBypass : BaseEffect
	{
		// [Range(0f, 1f)]
		public float amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetFloat("_Amount", amount);
			Graphics.Blit(source, destination, Material);
		}
	}
}
