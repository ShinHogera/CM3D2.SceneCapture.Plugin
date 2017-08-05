using UnityEngine;
using System;

namespace CM3D2.SceneCapture.Plugin
{
	public class LensDistortionBlur : BaseEffect
	{
		public enum QualityPreset
		{
			Low = 4,
			Medium = 8,
			High = 12,
			Custom
		}

		public QualityPreset quality = QualityPreset.Medium;
		public int samples = 10;
		public float distortion = 0.2f;
		public float cubicDistortion = 0.6f;
		public float scale = 0.8f;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
                    try{
			int lSamples = quality == QualityPreset.Custom ? samples : (int)quality;
			Material.SetVector("_Params", new Vector4(lSamples, distortion / lSamples, cubicDistortion / lSamples, scale));
			Graphics.Blit(source, destination, Material);
                    } catch(Exception e) { Debug.Log(e.ToString());}
		}
	}
}
