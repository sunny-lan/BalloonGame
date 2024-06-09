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
	[SerializeField] AnimationCurve curve;

	protected override void Start()
	{
		base.Start();
		if (transform.childCount > 0)
			target ??= transform.GetChild(0);
	}

	public float lineOffset = 0.3f;

	public override IEnumerator Fire()
	{
		var originPos = target.position;
		yield return new WaitForSeconds(Mathf.Min(trackingPeriod, initialWaitTime));

		var destPos = GameManager.LevelManager.Player.transform.position;

		yield return new WaitForSeconds(initialWaitTime - Mathf.Min(trackingPeriod, initialWaitTime));

		for (float t = 0; t <= moveTime; t += Time.deltaTime)
		{
			var animProg = t / moveTime;
			var pp = GameManager.LevelManager.Player.transform.position;

			if (t + initialWaitTime < trackingPeriod)
			{
				destPos = pp;
			}

			var trueDestPos = (destPos - originPos).normalized * Mathf.Max(0, (pp-originPos).magnitude + lineOffset) + originPos;

			target.position = Vector3.Lerp(originPos, trueDestPos, curve.Evaluate(animProg));

			yield return null;
		}//
	}

}
