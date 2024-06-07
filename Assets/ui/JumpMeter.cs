using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{
	[SerializeField] RectTransform bar;

	public float Amount
	{
		get => bar.localScale.y;
		set => bar.localScale = new(1, value, 1);
	}
}
