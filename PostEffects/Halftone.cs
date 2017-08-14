namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class Halftone : BaseEffect
	{
		// [Min(0f)]
		public float scale = 12f;

		// [Min(0f)]
		public float dotSize = 1.35f;

		public float angle = 1.2f;

		// [Range(0f, 1f)]
		public float smoothness = 0.080f;

		public Vector2 center = new Vector2(0.5f, 0.5f);

		public bool desaturate = false;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Center", new Vector2(center.x * (float)source.width, center.y * (float)source.height));
			Material.SetVector("_Params", new Vector3(scale, dotSize, smoothness));

			Matrix4x4 m = new Matrix4x4();
			m.SetRow(0, CMYKRot(angle + 0.261799388f));
			m.SetRow(1, CMYKRot(angle + 1.30899694f));
			m.SetRow(2, CMYKRot(angle));
			m.SetRow(3, CMYKRot(angle + 0.785398163f));
			Material.SetMatrix("_MatRot", m);

			Graphics.Blit(source, destination, Material, desaturate ? 1 : 0);
		}

		Vector4 CMYKRot(float angle)
		{
			float ca = Mathf.Cos(angle);
			float sa = Mathf.Sin(angle);
			return new Vector4(ca, -sa, sa, ca);
		}
	}
}
