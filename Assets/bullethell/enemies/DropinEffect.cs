using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropinEffect : BulletHellObj
{
	[SerializeField] AnimationCurve dropInY;
	[SerializeField] AnimationCurve alpha;
	[SerializeField] Transform targetPos;
	[SerializeField] SpriteRenderer targetSpriteRenderer;

	public float dropInTime = 0.3f;
	public float impactDuration = 0.2f;
	public float impactAmt = 0.2f;
	public float waitAfter = 0.2f;

	protected override void Start()
	{
		base.Start();
		targetSpriteRenderer.gameObject.SetActive(false);
	}

	public override IEnumerator Fire()
	{

		targetSpriteRenderer.color = new(1, 1, 1, 0);
		targetSpriteRenderer.gameObject.SetActive(true);
		for (float t = 0; t <= dropInTime; t += Time.deltaTime)
		{
			var animProg = t / dropInTime;

			targetSpriteRenderer.color = new(1, 1, 1, alpha.Evaluate(animProg));
			targetSpriteRenderer.transform.position = targetPos.position + dropInY.Evaluate(animProg) * Vector3.up;

			yield return new WaitForEndOfFrame();
		}

		targetSpriteRenderer.color = new(1, 1, 1, 1);
		targetSpriteRenderer.transform.position = targetPos.position;

		StartCoroutine(GameManager.LevelManager.CameraShake.Shake(impactDuration, impactAmt));
		yield return new WaitForSeconds(waitAfter);
	}

}
