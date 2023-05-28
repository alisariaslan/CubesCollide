using UnityEngine;

namespace Assets.Scripts
{
	public static class Helper
	{
		public static Vector2 AsXZ(this Vector3 v3) => new Vector2(v3.x, v3.z);
		public static Vector3 AsXZ(this Vector2 v2) => new Vector3(v2.x, 0f, v2.y);

		public static Vector2 AsXZRaw(this Vector3 v3)
		{
			var x = v3.x;
			var z = v3.z;

			if (x > 0)
				x = 1;
			else if (x < 0)
				x = -1;
			else
				x = 0;

			if (z > 0)
				z = 1;
			else if (z < 0)
				z = -1;
			else
				z = 0;

			return new Vector2(x, z);
		}

		public static Vector2 AsXZRawReversed(this Vector3 v3)
		{
			var x = v3.x;
			var z = v3.z;

			if (x > 0)
				x = -1;
			else if (x < 0)
				x = 1;
			else
				x = 0;

			if (z > 0)
				z = -1;
			else if (z < 0)
				z = 1;
			else
				z = 0;

			return new Vector2(x, z);
		}

		public static Color RandomColor()
		{
			return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
		}
	}
}
