using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaylistSpawner : BulletHellObj
{
	public bool simultaneous = false;

	public override IEnumerator Fire()
	{
		foreach(var child in children)
		{
			if (simultaneous)
			{
				StartCoroutine(child.Fire());
			}
			else
			{
				yield return child.Fire();
			}
		}
	}

}
