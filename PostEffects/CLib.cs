using UnityEngine;

namespace CM3D2.SceneCapture.Plugin
{
	public static class CLib
	{
		public const float PI_2 = Mathf.PI / 2f;
		public const float PI2 = Mathf.PI * 2f;

		public static float Frac(float f)
		{
			return f - Mathf.Floor(f);
		}

		public static bool IsLinearColorSpace()
		{
			return QualitySettings.activeColorSpace == ColorSpace.Linear;
		}

		public static bool Approximately(float source, float about, float range)
		{
			return ((Mathf.Abs(source - about) < range));
		}
	}
}
