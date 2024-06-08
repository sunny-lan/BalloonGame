using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : BulletHellObj
{
	[SerializeField] CameraShake shake;

	public float amt = 0.05f, duration = 0.1f;
	public bool simultaneous = true;

	protected override void Start()
	{
		base.Start();
		shake ??= GetComponent<CameraShake>();
		shake ??= GameManager.LevelManager.CameraShake;
	}

	public override IEnumerator Fire()
	{
		if (simultaneous)
			StartCoroutine(shake.Shake(duration, amt));
		else
			yield return shake.Shake(duration, amt);
	}

}
