using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
	[SerializeField] GameObject credits;
    [SerializeField] Toggle optimizationEnabled;

	private void Start()
	{
        StartCoroutine(playBGM()
        );

        optimizationEnabled.isOn = GameManager.EnableOptimization;
	}

	private IEnumerator playBGM()
	{
		{
			yield return null;
			yield return GameManager.BGMManager.SetMuffle(true);

		}
	}

	public void Play()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void Tutorial()
    {
        tutorial.SetActive(true);
    }

    public void Credits()
    {
        credits.SetActive(true);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
    }

    public void CloseTutorial()
    {
        tutorial.SetActive(false);
    }

    public void OptimizationEnabledChanged()
    {
        GameManager.EnableOptimization = optimizationEnabled.isOn;
    }
}
