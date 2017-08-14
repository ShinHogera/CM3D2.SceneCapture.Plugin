namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class Blend : BaseEffect
	{
		public enum BlendingMode
		{
			Darken = 0,
			Multiply = 1,
			ColorBurn = 2,
			LinearBurn = 3,
			DarkerColor = 4,

			Lighten = 6,
			Screen = 7,
			ColorDodge = 8,
			LinearDodge = 9,
			LighterColor = 10,

			Overlay = 12,
			SoftLight = 13,
			HardLight = 14,
			VividLight = 15,
			LinearLight = 16,
			PinLight = 17,
			HardMix = 18,

			Difference = 20,
			Exclusion = 21,
			Subtract = 22,
			Divide = 23
		}

		public Texture texture;

		// [Range(0f, 1f)]
		public float amount = 1f;

		public BlendingMode mode = 0;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (texture == null || amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetTexture("_OverlayTex", texture);
			Material.SetFloat("_Amount", amount);
			Graphics.Blit(source, destination, Material, (int)mode);
		}
	}
}
