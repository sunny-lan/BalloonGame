using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
	[SerializeField] GameObject credits;

	private void Start()
	{
        StartCoroutine(GameManager.BGMManager.SetMuffle(true));
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
}
