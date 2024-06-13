using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static BGMManager bGMManager;

	public static LevelManager LevelManager { get; internal set; }

	public static BGMManager BGMManager
	{
		get => bGMManager; internal set
		{
			bGMManager = value;
			if (bGMManager != null)
				BGMManagerInit?.Invoke();
		}
	}

	public static event Action BGMManagerInit;
	public static void OnBGMManagerInit(Action a)
	{
		if (bGMManager != null)
			a();
		else
			BGMManagerInit += a;
	}
}
