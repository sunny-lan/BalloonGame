using System.Collections;
using UnityEngine;

public class RotateToPlayer : BulletHellObj
{
    [SerializeField] Transform target;

	public float duration = 0.4f;
	public float speed = 90f;
	public float trackingDuration = 0.2f;

	public override IEnumerator Fire()
	{
		Quaternion getPlayerFacing()
		{
			Quaternion destAngle;
			var playerPos = GameManager.LevelManager.Player.transform.position;
			var delta = playerPos - target.position;

			destAngle = Quaternion.Euler(new(0, 0, Mathf.Atan2(
				delta.y, delta.x
			) * Mathf.Rad2Deg));
			return destAngle;
		}

		Quaternion destAngle = getPlayerFacing();

		for (float t = 0; t <= duration; t += Time.deltaTime)
		{
			if (t <= trackingDuration)
			{
				destAngle = getPlayerFacing();
			}

			target.rotation = Quaternion.RotateTowards(target.rotation, destAngle, Time.deltaTime * speed);
			yield return null;
		}

	}
}
