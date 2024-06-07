using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

	[SerializeField] float damage = 10000;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<PlayerController>() is PlayerController player)
		{
			player.DoDamage(damage);
		}
	}
}
