using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public PlayerController Player;
	public CameraShake CameraShake;
	public PlayerUI PlayerUI;
	public Camera Camera;
	public PlaylistSpawner Level;

	[SerializeField] RespawnPlatform respawnPlatform;

	private void Awake()
	{
		GameManager.LevelManager = this;
	}

	private void Start()
	{
		GameManager.OnBGMManagerInit(() =>
			StartCoroutine(PlayMusicAfterOneFRame()));
		Respawn();
	}

	IEnumerator PlayMusicAfterOneFRame()
	{
		yield return null;
		yield return GameManager.BGMManager.SetMuffle(false);
	}


	private void Respawn()
	{
		respawnPlatform.Respawn(Player);
	}

	public bool Stopped { get; private set; } = false;

	private void StopGameLogic()
	{
		Player.Invuln = true;
		Player.UnGrab();
		Player.SetFreeze(true);
		Stopped = true;
	}

	private void ReturnGameLogic()
	{
		Stopped = false;
		Player.SetFreeze(false);
	}

	int lastLives = -1;
	private void Update()
	{
		var lives = Player.Lives;
		if (lives < lastLives)
		{
			StartCoroutine(OnPlayerLostLife());
		}
		lastLives = lives;
		if(Level.totalIndex == Level.currentIndex)
		{
			Won();
		}
	}

	private void Won()
	{
		Stopped = true;
		PlayerUI.ShowWinScreen();
		Time.timeScale = 0;
	}

	private void Lost()
	{
		Stopped = true;
		PlayerUI.ShowLoseScreen();
		Time.timeScale = 0;
	}

	private IEnumerator OnPlayerLostLife()
	{
		var lives = GameManager.LevelManager.Player.Lives;
		StopGameLogic();
		yield return PlayerUI.PlayerLostLifeAnimation();
		if (lives == 0)
		{
			Lost();
			yield break;
		}
		Respawn();
		ReturnGameLogic();
	}

}
