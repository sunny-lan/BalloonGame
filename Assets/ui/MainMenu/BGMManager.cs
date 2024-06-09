using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
	private AudioLowPassFilter lowPassFilter;
	private AudioSource audioSource;
	public float muffleTime = 0.5f;

	public float muffleAmt = 2700;
	public float unmuffleAmt = 22000;

	public float muffleVol = 0.7f;
	public float unmuffleVol = 1f;


	private void Awake()
	{
		if (GameManager.BGMManager != null) { Destroy(gameObject); return; }
		GameManager.BGMManager = this;
		DontDestroyOnLoad(gameObject);
	}

	// Start is called before the first frame update
	void Start()
	{
		lowPassFilter = GetComponent<AudioLowPassFilter>();
		audioSource = GetComponent<AudioSource>();
		if (GameManager.BGMManager != this) return;
		audioSource.Play();
	}

	public IEnumerator SetMuffle(bool muffle)
	{
		var curCutoff = lowPassFilter.cutoffFrequency;
		var curVol = audioSource.volume;
		for (float t = 0; t <= muffleTime; t += Time.deltaTime)
		{
			float prog = t / muffleTime;

			if (muffle)
			{
				lowPassFilter.cutoffFrequency = Mathf.Lerp(curCutoff, muffleAmt, prog);
				audioSource.volume = Mathf.Lerp(curVol, muffleVol, prog);
			}
			else
			{
				lowPassFilter.cutoffFrequency = Mathf.Lerp(curCutoff, unmuffleAmt, prog);
				audioSource.volume = Mathf.Lerp(curVol, unmuffleVol, prog);
			}

			yield return null;
		}
	}
}
