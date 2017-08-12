namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class WhiteBalance : BaseEffect
	{
		public enum BalanceMode
		{
			Simple,
			Complex
		}

		public Color white = new Color(0.5f, 0.5f, 0.5f);

		public BalanceMode mode = BalanceMode.Complex;

		protected virtual void Reset()
		{
			white = CLib.IsLinearColorSpace() ?
				new Color(0.72974005284f, 0.72974005284f, 0.72974005284f) :
				new Color(0.5f, 0.5f, 0.5f);
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetColor("_White", white);
			Graphics.Blit(source, destination, Material, (int)mode);
		}
	}
}
