using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

	[SerializeField] float damage = 10000;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController>() is PlayerController player)
		{
			player.DoDamage(damage);
		}
	}
}
