using UnityEngine;

namespace CM3D2.SceneCapture.Plugin
{
	public class DynamicLookup : BaseEffect
	{
		public Color white = new Color(1f, 1f, 1f);
		public Color black = new Color(0f, 0f, 0f);
		public Color red = new Color(1f, 0f, 0f);
		public Color green = new Color(0f, 1f, 0f);
		public Color blue = new Color(0f, 0f, 1f);
		public Color yellow = new Color(1f, 1f, 0f);
		public Color magenta = new Color(1f, 0f, 1f);
		public Color cyan = new Color(0f, 1f, 1f);
		public float amount = 1f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetColor("_White", white);
			Material.SetColor("_Black", black);
			Material.SetColor("_Red", red);
			Material.SetColor("_Green", green);
			Material.SetColor("_Blue", blue);
			Material.SetColor("_Yellow", yellow);
			Material.SetColor("_Magenta", magenta);
			Material.SetColor("_Cyan", cyan);
			Material.SetFloat("_Amount", amount);
			Graphics.Blit(source, destination, Material, CLib.IsLinearColorSpace() ? 1 : 0);
		}
	}
}
