namespace CM3D2.SceneCapture.Plugin
{
	using UnityEngine;

	public class Wiggle : BaseEffect
	{
		public enum Algorithm
		{
			Simple,
			Complex
		}

		public Algorithm mode = Algorithm.Complex;

		public float timer = 0f;

		public float speed = 1f;

		public float frequency = 12f;

		public float amplitude = 0.01f;

		public bool automaticTimer = true;

		protected virtual void Update()
		{
			if (automaticTimer)
			{
				// Reset the timer after a while, some GPUs don't like big numbers
				if (timer > 1000f)
					timer -= 1000f;

				timer += speed * Time.deltaTime;
			}
		}

		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Material.SetVector("_Params", new Vector3(frequency, amplitude, timer * (mode == Algorithm.Complex ? 0.1f : 1f)));
			Graphics.Blit(source, destination, Material, (int)mode);
		}
	}
}
