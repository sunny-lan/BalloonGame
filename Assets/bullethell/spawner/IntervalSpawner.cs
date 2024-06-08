using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalSpawner : BulletHellObj
{
	public float initialDelay = 1;
	public float interval = 1f;
	public bool simultaneous = true;
	public int waves = -1;


	public override IEnumerator Fire()
	{
		yield return new WaitForSeconds(initialDelay);

		var wait = new List<Coroutine>();
		for (int i = 0; i < waves || waves == -1; i++)
		{
			if (child != null)
				if (simultaneous)
				{
					var c = StartCoroutine(child.Fire());
					wait.Add(c);
				}
				else
				{
					yield return child.Fire();
				}

			yield return new WaitForSeconds(interval);
		}

		foreach (var c in wait) { yield return c; }
	}
}
