using UnityEngine;

namespace CM3D2.SceneCapture.Plugin
{
	public class ChannelSwapper : BaseEffect
	{
		public enum Channel
		{
			Red,
			Green,
			Blue
		}

		public Channel redSource = Channel.Red;
		public Channel greenSource = Channel.Green;
		public Channel blueSource = Channel.Blue;

		static Vector4[] m_Channels = new Vector4[]
		{
			new Vector4(1f, 0f, 0f, 0f),
			new Vector4(0f, 1f, 0f, 0f),
			new Vector4(0f, 0f, 1f, 0f)
		};

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Red", m_Channels[(int)redSource]);
			Material.SetVector("_Green", m_Channels[(int)greenSource]);
			Material.SetVector("_Blue", m_Channels[(int)blueSource]);
			Graphics.Blit(source, destination, Material);
		}
	}
}
