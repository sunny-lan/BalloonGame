using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
	[SerializeField] HealthBar healthBar;

	[SerializeField] float lifeLostShakeDuration = 0.5f;
	[SerializeField] float lifeLostShakeAmt = 0.1f;
	private AudioSource ap;

	[SerializeField] AudioClip deathSound;

	[SerializeField] JumpMeter jumpMeter;
	[SerializeField] RectTransform canvas;

	[SerializeField] RectTransform loseScreen;

	public void GetWorldCorners(Vector3[] worldSpaceArray)
	{
		 canvas.GetWorldCorners(worldSpaceArray);
	}

	public void ShowLoseScreen()
	{
		StartCoroutine(GameManager.BGMManager.SetMuffle(true));
		loseScreen.gameObject.SetActive(true);
	}

	public void ReturnMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1;
	}

	// Update is called once per frame
	void Update()
	{
		var player = GameManager.LevelManager.Player;
		var lives = player.Lives;
		healthBar.Health = lives;
		if (player.ChargedJumpForce >= 0)
		{
			jumpMeter.gameObject.SetActive(true);
			jumpMeter.Amount = player.ChargedJumpForce / player.maxChargedJumpForce;
		}
		else
		{
			jumpMeter.gameObject.SetActive(false);
		}

		ap = GetComponent<AudioSource>();
	}

	public IEnumerator PlayerLostLifeAnimation()
	{
		ap.PlayOneShot(deathSound);
		yield return GameManager.LevelManager.CameraShake.Shake(lifeLostShakeDuration, lifeLostShakeAmt);
	}
}
