using UnityEngine;

namespace CM3D2.SceneCapture.Plugin
{
	public class Letterbox : BaseEffect
	{
            public float aspectWidth = 21f;
            public float aspectHeight = 9f;
            public Color fillColor = Color.black;

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
                        float aspect = aspectWidth / aspectHeight;
			float w = (float)source.width;
			float h = (float)source.height;
			float currentAspect = w / h;
			float offset = 0f;
			int pass = 0;

			Material.SetColor("_FillColor", fillColor);

			if (currentAspect < aspect)
			{
				offset = (h - w / aspect) * 0.5f / h;
			}
			else if (currentAspect > aspect)
			{
				offset = (w - h * aspect) * 0.5f / w;
				pass = 1;
			}
			else
			{
				Graphics.Blit(source, destination);
				return;
			}

			Material.SetVector("_Offsets", new Vector2(offset, 1f - offset));
			Graphics.Blit(source, destination, Material, pass);
		}
	}
}
