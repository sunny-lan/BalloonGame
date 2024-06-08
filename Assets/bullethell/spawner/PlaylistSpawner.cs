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
			ordering.Shuffle();

		List<Coroutine> wait = null;
		if(simultaneous)wait = new ();

		for(int i=0;i<children.Length;i++)
		{
			var child = children[ordering[i]];
			if (simultaneous)
			{
				wait.Add(StartCoroutine(child.Fire()));
			}
			else
			{
				yield return child.Fire();
			}
		}

		if(wait!=null)
		foreach (var child in wait) yield return child;
	}

}
