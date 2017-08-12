namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class ComicBook : BaseEffect
	{
		public float stripAngle = 0.6f;

		// [Min(0f)]
		public float stripDensity = 180f;

		// [Range(0f, 1f)]
		public float stripThickness = 0.5f;

		public Vector2 stripLimits = new Vector2(0.25f, 0.4f);

		public Color stripInnerColor = new Color(0.3f, 0.3f, 0.3f);

		public Color stripOuterColor = new Color(0.8f, 0.8f, 0.8f);

		public Color fillColor = new Color(0.1f, 0.1f, 0.1f);

		public Color backgroundColor = Color.white;

		public bool edgeDetection = false;

		// [Min(0.01f)]
		public float edgeThreshold = 5f;

		public Color edgeColor = Color.black;

		// [Range(0f, 1f)]
		public float amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_StripParams", new Vector4(Mathf.Cos(stripAngle), Mathf.Sin(stripAngle), stripLimits.x, stripLimits.y));
			Material.SetVector("_StripParams2", new Vector3(stripDensity * 10f, stripThickness, amount));
			Material.SetColor("_StripInnerColor", stripInnerColor);
			Material.SetColor("_StripOuterColor", stripOuterColor);

			Material.SetColor("_FillColor", fillColor);
			Material.SetColor("_BackgroundColor", backgroundColor);

			if (edgeDetection)
			{
				Material.SetFloat("_EdgeThreshold", 1f / (edgeThreshold * 100f));
				Material.SetColor("_EdgeColor", edgeColor);
				Graphics.Blit(source, destination, Material, 1);
			}
			else
			{
				Graphics.Blit(source, destination, Material, 0);
			}
		}
	}
}
