using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : BulletHellObj
{
	[SerializeField] AudioClip clip;
	[SerializeField] AudioSource source;

	public override IEnumerator Fire()
	{
		source.PlayOneShot(clip);
		yield break;
	}
}
