using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsPattern : BulletHellObj
{
	public enum Dir
	{
		Clockwise,
		CounterClockwise
	}

	[SerializeField] Laser horizontal;
	[SerializeField] Laser vertical;

	public float waitBeforeRotate = 1f;
	public float rotationTime = 2f;
	public float startRotAngle = 20, endRotAngle = 35;
	public float waitAfterRotate = 1f;
	public float intervalBetweenIterations = 1f;
	public int numIterations = 5;

	[SerializeField] AnimationCurve rotationCurve;

	[SerializeField]
	GenericDictionary<Dir, float> rotDirectionProbability = new()
	{
		[Dir.CounterClockwise] = 0.5f,
		[Dir.Clockwise] = 0.5f
	};

	public bool alternate = true;


	protected override void Start()
	{
		base.Start();
		horizontal.gameObject.SetActive(false);
		vertical.gameObject.SetActive(false);
	}



	public override IEnumerator Fire()
	{

		var direction = rotDirectionProbability.SelectByProbability() switch
		{
			Dir.Clockwise => Vector3.forward,
			Dir.CounterClockwise => Vector3.back,
			_ => throw new System.NotImplementedException(),
		};

		for (int iteration = 0; iteration < numIterations; iteration++)
		{
			var totalRotAngle = Mathf.Lerp(startRotAngle, endRotAngle, iteration / (numIterations - 1));

			yield return FireSingle(totalRotAngle, direction);
			yield return new WaitForSeconds(waitAfterRotate);
			if (alternate) direction = -direction;
		}
	}

	private IEnumerator FireSingle(float totalRotAngle, Vector3 direction)
	{
		var playerPos = GameManager.LevelManager.Player.transform.position;
		horizontal.transform.position = new(playerPos.x - 50, playerPos.y, 0);
		vertical.transform.position = new(playerPos.x, playerPos.y - 50, 0);
		horizontal.transform.right = Vector3.right;
		vertical.transform.right = Vector3.up;

		vertical.FireTime = horizontal.FireTime = waitBeforeRotate + rotationTime + waitAfterRotate;

		horizontal.gameObject.SetActive(true);
		vertical.gameObject.SetActive(true);

		var h = StartCoroutine(horizontal.Fire());
		var v = StartCoroutine(vertical.Fire());

		yield return new WaitForSeconds(waitBeforeRotate + horizontal.PreviewFullTime + horizontal.PreviewAnimTime);


		var lastAnimVal = rotationCurve.Evaluate(0);
		for (float t = 0; t <= rotationTime; t += Time.deltaTime)
		{
			var animProg = t / rotationTime;
			SetProg( animProg);

			yield return null;
		}
		SetProg(1);

		yield return new WaitForSeconds(waitAfterRotate);

		yield return h;
		yield return v;

		horizontal.gameObject.SetActive(false);
		vertical.gameObject.SetActive(false);

		void SetProg( float animProg)
		{
			var newAnimVal = rotationCurve.Evaluate(animProg);

			horizontal.transform.RotateAround(playerPos, direction, (newAnimVal - lastAnimVal) * totalRotAngle);
			vertical.transform.RotateAround(playerPos, -direction, (newAnimVal - lastAnimVal) * totalRotAngle);
			lastAnimVal = newAnimVal;
		}
	}
}
