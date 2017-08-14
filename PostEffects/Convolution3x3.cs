namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class Convolution3x3 : BaseEffect
	{
		public Vector3 kernelTop = Vector3.zero;
		public Vector3 kernelMiddle = Vector3.up;
		public Vector3 kernelBottom = Vector3.zero;

		public float divisor = 1f;

		// [Range(0f, 1f)]
		public float amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_PSize", new Vector2(1f / (float)source.width, 1f / (float)source.height));
			Material.SetVector("_KernelT", kernelTop / divisor);
			Material.SetVector("_KernelM", kernelMiddle / divisor);
			Material.SetVector("_KernelB", kernelBottom / divisor);
			Material.SetFloat("_Amount", amount);
			Graphics.Blit(source, destination, Material);
		}
	}
}
