using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
	[SerializeField] HealthBar healthBar;

	[SerializeField] float lifeLostShakeDuration = 0.5f;
	[SerializeField] float lifeLostShakeAmt = 0.1f;
	private AudioSource ap;

	[SerializeField] AudioClip deathSound;


	// Update is called once per frame
	void Update()
	{
		var lives = GameManager.LevelManager.Player.Lives;
		healthBar.Health = lives;

		ap = GetComponent<AudioSource>();
	}

	public IEnumerator PlayerLostLifeAnimation()
	{
		ap.PlayOneShot(deathSound);
		yield return GameManager.LevelManager.CameraShake.Shake(lifeLostShakeDuration, lifeLostShakeAmt);
	}
}
