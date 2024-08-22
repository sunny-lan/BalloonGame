using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
	[SerializeField] TMP_Text level;
	[SerializeField] HealthBar healthBar;

	[SerializeField] float lifeLostShakeDuration = 0.5f;
	[SerializeField] float lifeLostShakeAmt = 0.1f;
	private AudioSource ap;

	[SerializeField] AudioClip deathSound;
	[SerializeField] AudioClip levelupSound;

	[SerializeField] JumpMeter jumpMeter;
	[SerializeField] RectTransform canvas;

	[SerializeField] RectTransform loseScreen;
	[SerializeField] RectTransform winScreen;

	[SerializeField] InterstitialAdG interstitialAd;

	private void Awake()
	{
		ap = GetComponent<AudioSource>();
		interstitialAd.OnAdClosed.AddListener(() =>
		{
			SceneManager.LoadScene("MainGame");
			Time.timeScale = 1;
		});
	}

	public void GetWorldCorners(Vector3[] worldSpaceArray)
	{
		canvas.GetWorldCorners(worldSpaceArray);
	}

	public void ShowLoseScreen()
	{
		StartCoroutine(GameManager.BGMManager.SetMuffle(true));
		loseScreen.gameObject.SetActive(true);
	}

	public void ShowWinScreen()
	{
		StartCoroutine(GameManager.BGMManager.SetMuffle(true));
		winScreen.gameObject.SetActive(true);
	}

	public void ReturnMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1;
	}

	public void Retry()
	{
		interstitialAd.gameObject.SetActive(true);
		
	}

	

	int lastLevel = 999;

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

		var lvl = GameManager.LevelManager.Level.currentIndex;
		if (lastLevel != lvl)
		{
			if (lvl > lastLevel)
			{
				StartCoroutine(BlipLevelCounter());
				ap.PlayOneShot(levelupSound);
			}
			lastLevel = lvl;
			level.text = "Level " + lastLevel;
		}
	}

	private IEnumerator BlipLevelCounter()
	{
		var baseSz = level.fontSize;
		for (float t = 0; t < 0.2f; t += Time.deltaTime)
		{
			var prog = t / 0.2f;
			var sz = baseSz + 10 * Mathf.Sin(prog * Mathf.PI);
			level.fontSize = sz;
			yield return null;
		}
		level.fontSize = baseSz;
	}

	public IEnumerator PlayerLostLifeAnimation()
	{
		ap.PlayOneShot(deathSound);
		yield return GameManager.LevelManager.CameraShake.Shake(lifeLostShakeDuration, lifeLostShakeAmt);
	}

}
