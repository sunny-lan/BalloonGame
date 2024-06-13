using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaylistSpawner : BulletHellObj
{
	public bool simultaneous = false;
	public bool randomizeOrder = false;
	public bool waitForChildren = true;
	public float delayBetween = 0;

	static System.Random rng = new();
	int[] ordering;

	public int currentIndex { get; private set; }
	public int totalIndex => children.Length;

	protected override void Start()
	{
		base.Start();
		ordering = Enumerable.Range(0, children.Length).ToArray();
	}

	public override IEnumerator Fire()
	{
		if (randomizeOrder)
			ordering.Shuffle();

		List<Coroutine> wait = null;
		if (simultaneous && waitForChildren) wait = new();

		for (int i = 0; i < children.Length;)
		{
			var child = children[ordering[i]];
			if (child.isActiveAndEnabled)
			{
				currentIndex = i;
				if (simultaneous)
				{
					wait.Add(StartCoroutine(child.Fire()));
				}
				else
				{
					yield return child.Fire();
				}
			}

			i++;
			if (child.isActiveAndEnabled)
				yield return new WaitForSeconds(delayBetween);
		}

		currentIndex = children.Length;

		if (wait != null)
			foreach (var child in wait) yield return child;
	}

}
