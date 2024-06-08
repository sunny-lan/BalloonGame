using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnHit : MonoBehaviour
{
	public float damage = 100;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject == GameManager.LevelManager.Player.gameObject)
		{
			GameManager.LevelManager.Player.DoDamage(damage);
		}
	}
}
