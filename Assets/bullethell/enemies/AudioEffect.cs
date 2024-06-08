using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : BulletHellObj
{
	[SerializeField] AudioClip clip;
	[SerializeField] AudioSource source;
	public float length = 9999;

	protected override void Awake()
	{
		base.Awake();
		source ??= GetComponent<AudioSource>();	
	}

	public override IEnumerator Fire()
	{
		source.clip = clip;
		source.Play();
		source.SetScheduledEndTime(AudioSettings.dspTime + length);
		yield break;
	}
}
