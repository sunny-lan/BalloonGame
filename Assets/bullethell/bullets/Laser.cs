using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : BulletHellObj
{
	[SerializeField] GameObject body;
	[SerializeField] GameObject preview;
	[SerializeField] SpriteRenderer previewSR;
	[SerializeField] SpriteRenderer bodySR;
	[SerializeField] SpriteRenderer baseSR;

	[SerializeField] AudioClip laserLeadup;
	[SerializeField] AudioClip laserFire;
	[SerializeField] AnimationCurve previewXScaling;
	[SerializeField] AnimationCurve previewYScaling;
	[SerializeField] AnimationCurve previewAlpha;
	[SerializeField] AnimationCurve previewBodyAlpha;

	public LayerMask hits;
	public float DamageOverTime = 100;
	public float PreviewAnimTime = 0.5f;
	public float PreviewFullTime = 0.5f;
	public  float FireTime;

	private AudioSource audioSrc;
	public float shakeDuration = 0.2f;
	public float shakeAmount = 0.1f;

	public bool Firing { get; private set; } = false;

	protected override void Awake()
	{
		base.Awake();
		audioSrc = GetComponent<AudioSource>();
	}

	protected override void Start()
	{
		base.Start();
		body.SetActive(false);
		preview.SetActive(false);
		baseSR.gameObject.SetActive(false);	
	}



	public override IEnumerator Fire()
	{
		bodySR.color = new(1, 1, 1, 0);
		body.SetActive(true);
		preview.transform.localScale = new(0, 0, 1);
		preview.SetActive(true);
		baseSR.gameObject.SetActive(true);
		audioSrc.PlayOneShot(laserLeadup);
		
		for (float t = 0; t <= PreviewAnimTime; t += Time.deltaTime)
		{
			var animProgress = t / PreviewAnimTime;
			preview.transform.localScale = new(
				previewXScaling.Evaluate(animProgress),
				previewYScaling.Evaluate(animProgress),1);
			baseSR.color= previewSR.color = new Color(1,0,0,previewAlpha.Evaluate(animProgress));
			bodySR.color = new(1, 1, 1, previewBodyAlpha.Evaluate(animProgress));
			

			yield return null;
		}
		preview.transform.localScale = new(1, 1, 1);
		baseSR.color = previewSR.color = new Color(1, 0, 0,1);

		yield return new WaitForSeconds(PreviewFullTime);

		audioSrc.Stop();
		audioSrc.PlayOneShot(laserFire);
		body.SetActive(true);
		preview.SetActive(false);
		bodySR.color = new(1, 1, 1, 1);
		StartCoroutine(GameManager.LevelManager.CameraShake.Shake(shakeDuration, shakeAmount));
		Firing = true;

		for(float t = 0; t <= FireTime; t += Time.deltaTime)
		{
			var hit = Physics2D.Raycast(transform.position, transform.right, float.PositiveInfinity, hits);
			if (hit.collider != null)
			{
				if(hit.collider.GetComponent<PlayerController>() is PlayerController player)
				{
					player.DoDamage(DamageOverTime * Time.deltaTime);
				}
			}

			yield return null;
		}

		Firing = false;
		body.SetActive(false);
		baseSR.gameObject.SetActive(false);
	}
}
