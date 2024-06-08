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

	[SerializeField] RespawnPlatform respawnPlatform;

	private void Awake()
	{
		GameManager.LevelManager = this;
	}

	private void Start()
	{
		StartCoroutine(GameManager.BGMManager.SetMuffle(false));
		Respawn();
	}


	private void Respawn()
	{
		respawnPlatform.Respawn(Player);
	}

	public bool Stopped { get; private set; } = false;

	private void StopGameLogic()
	{
		Player.Invuln = true;
		Player.SetFreeze(true);
		Stopped=true;
	}

	private void ReturnGameLogic()
	{
		Stopped = false;
		Player.SetFreeze(false);
	}

	int lastLives = -1;
	private void Update()
	{
		var lives = GameManager.LevelManager.Player.Lives;
		if (lives < lastLives)
		{
			StartCoroutine(OnPlayerLostLife());
		}
		lastLives = lives;
	}

	private void Lost()
	{
		Stopped=true;
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
