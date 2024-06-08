using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalBulletSpawner : BulletHellObj
{
	public float startAngle = 0;
	public float endAngle = 360;
	public float interval = 40;
	public float delayBetween = 0;
	public bool simultaneous = true;
	public float startDistance = 0.2f;

	public override IEnumerator Fire()
	{
		var lst = new List<Coroutine>();
		for (float theta = startAngle; theta <= endAngle; theta += interval)
		{
			var thetaRad = Mathf.Deg2Rad * theta;
			child.transform.position = new Vector3(Mathf.Cos(thetaRad), Mathf.Sin(thetaRad), 0) * startDistance;
			child.transform.localEulerAngles = new(0, 0, theta);

			if (simultaneous)
			{
				var x = StartCoroutine(child.Fire());
				lst.Add(x);
			}
			else
			{
				yield return child.Fire();
			}

			yield return new WaitForSeconds(delayBetween);
		}

		foreach (var x in lst) yield return x;
	}//a

}
