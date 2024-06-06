using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	[SerializeField]
	private int health = 5;

	private void UpdateMaxHealth()
	{
		if (health <= transform.childCount) return;

		var template = transform.GetChild(0);
		for (int i = transform.childCount; i < health; i++)
		{
			Instantiate(template, transform);
		}
	}

	public int Health
	{
		get => health; set
		{
			health = value;
			UpdateHealth();
		}
	}

	private void UpdateHealth()
	{
		
		UpdateMaxHealth();
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(i < health);
		}
	}

	private void Start()
	{
		UpdateHealth();
	}
}
