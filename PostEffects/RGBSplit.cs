namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class RGBSplit : BaseEffect
	{
		public float amount = 0f;

		public float angle = 0f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (amount == 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_Params", new Vector3(
					amount * 0.001f,
					Mathf.Sin(angle),
					Mathf.Cos(angle)
				));

			Graphics.Blit(source, destination, Material);
		}
	}
}
