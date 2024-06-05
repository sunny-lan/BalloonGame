

using UnityEngine;

public static class VectorExt
{
	public static Vector4 WithW(this Vector3 v, float w)
	{
		return new Vector4(v.x, v.y, v.z, w);
	}
}