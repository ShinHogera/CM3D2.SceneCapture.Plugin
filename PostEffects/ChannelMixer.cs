namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class ChannelMixer : BaseEffect
	{
		public Vector3 red = new Vector3(100f, 0f, 0f);
		public Vector3 green = new Vector3(0f, 100f, 0f);
		public Vector3 blue = new Vector3(0f, 0f, 100f);
		public Vector3 constant = new Vector3(0f, 0f, 0f);

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Red", new Vector4(red.x * 0.01f, green.x * 0.01f, blue.x * 0.01f));
			Material.SetVector("_Green", new Vector4(red.y * 0.01f, green.y * 0.01f, blue.y * 0.01f));
			Material.SetVector("_Blue", new Vector4(red.z * 0.01f, green.z * 0.01f, blue.z * 0.01f));
			Material.SetVector("_Constant", new Vector4(constant.x * 0.01f, constant.y * 0.01f, constant.z * 0.01f));

			Graphics.Blit(source, destination, Material);
		}
	}
}
