using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoomToPlayer : BulletHellObj
{
	public float trackingPeriod = 0;
	public float initialWaitTime = 0.2f;
	public float moveTime = 0.5f;

	[SerializeField] Transform target;

	protected override void Start()
	{
		base.Start();
		if (transform.childCount > 0)
			target ??= transform.GetChild(0);
	}

	public override IEnumerator Fire()
	{
		var destPos = GameManager.LevelManager.Player.transform.position;
		var originPos = target.position;

		yield return new WaitForSeconds(initialWaitTime);
		for (float t = 0; t <= moveTime; t += Time.deltaTime)
		{
			var animProg = t / moveTime;
			if (animProg < trackingPeriod)
			{
				destPos = GameManager.LevelManager.Player.transform.position;
			}

			target.position = Vector3.Lerp(originPos, destPos, animProg);

			yield return null;
		}//
	}

}
