

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
}