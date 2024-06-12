

using UnityEngine;

public static class VectorExt
{
	public static Vector4 WithW(this Vector3 v, float w)
	{
		return new (v.x, v.y, v.z, w);
	}
	public static Vector3 WithZ(this Vector2 v, float z)
	{
		return new (v.x, v.y, z);
	}
	public static Vector2 XY(this Vector3 v)
	{
		return new(v.x, v.y);
	}
	public static Vector2 Abs(this Vector2 v)
	{
		return new(Mathf.Abs(v.x), Mathf.Abs(v.y));
	}

	public static Vector2 ClosestPointOn(this Vector2 v, Bounds b)
	{
		return new(
			Mathf.Clamp(v.x, b.min.x, b.max.x),
			Mathf.Clamp(v.y, b.min.y, b.max.y)
		);
	}
}