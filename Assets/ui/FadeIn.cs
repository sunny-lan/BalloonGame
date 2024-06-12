using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FadeIn : MonoBehaviour
{
	public float duration = 0.5f;
	private CanvasGroup g;

	private void Awake()
	{
		g = GetComponent<CanvasGroup>();
	}

	private void OnEnable()
	{
		StartCoroutine(Go());
	}

	IEnumerator Go()
	{
		for (float t = 0; t <= duration; t += Time.unscaledDeltaTime)
		{
			float prog = t / duration;
			g.alpha = prog;
			yield return null;
		}
		g.alpha = 1.0f;
	}
}
