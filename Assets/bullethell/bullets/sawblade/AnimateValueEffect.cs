using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimateValueEffect : BulletHellObj
{
	[SerializeField]
	UnityEvent<float> Setter;

	public float time = 1f;
	public float amplitude = 1f;
	public bool reverse = false;

	[SerializeField]
	AnimationCurve curve;

	public override IEnumerator Fire()
	{
		for (float t = 0; t <= time; t += Time.deltaTime)
		{
			var prog = t / time;
			if (reverse) prog = 1 - prog;
			Setter.Invoke(curve.Evaluate(prog) * amplitude);
			yield return null;
		}
	}
}
