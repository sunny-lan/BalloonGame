using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaylistSpawner : BulletHellObj
{
	public bool simultaneous = false;
	public bool randomizeOrder = false;

	static System.Random rng = new ();
	int[] ordering;

	protected override void Start()
	{
		base.Start();
		ordering = Enumerable.Range(0, children.Length).ToArray();
	}

	public override IEnumerator Fire()
	{
		if(randomizeOrder)
			rng.Shuffle(ordering);

		for(int i=0;i<children.Length;i++)
		{
			var child = children[ordering[i]];
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
