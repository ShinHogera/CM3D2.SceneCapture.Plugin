using UnityEngine;

namespace CM3D2.SceneCapture.Plugin
{
	public class Technicolor : BaseEffect
	{
		public float exposure = 4f;
		public Vector3 balance = new Vector3(0.25f, 0.25f, 0.25f);
		public float amount = 0.5f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetFloat("_Exposure", 8f - exposure);
			Material.SetVector("_Balance", Vector3.one - balance);
			Material.SetFloat("_Amount", amount);
			Graphics.Blit(source, destination, Material);
		}
	}
}
