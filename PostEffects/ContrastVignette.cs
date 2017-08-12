namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class ContrastVignette : BaseEffect
	{
		public Vector2 center = new Vector2(0.5f, 0.5f);

		// [Range(-100f, 100f)]
		public float sharpness = 32f;

		// [Range(0f, 100f)]
		public float darkness = 28f;

		// [Range(0f, 200f)]
		public float contrast = 20.0f;

		public Vector3 contrastCoeff = new Vector3(0.5f, 0.5f, 0.5f);

		// [Range(0f, 200f)]
		public float edgeBlending = 0f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Params", new Vector4(sharpness * 0.01f, darkness * 0.02f, contrast * 0.01f, edgeBlending * 0.01f));
			Material.SetVector("_Coeffs", contrastCoeff);
			Material.SetVector("_Center", center);
			Graphics.Blit(source, destination, Material);
		}
	}
}
