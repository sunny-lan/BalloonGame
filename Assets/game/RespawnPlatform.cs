using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlatform : MonoBehaviour
{
	[SerializeField] Transform respawnPos;

	private void OnCollisionExit2D(Collision2D collision)
	{
		if(collision.gameObject == GameManager.LevelManager.Player.gameObject)
		{
			gameObject.SetActive(false);
			GameManager.LevelManager.Player.Invuln = false;
		}
	}

	public void Respawn(PlayerController player)
	{
		player.Invuln = true;
		gameObject.SetActive(true);
		player.transform.position = respawnPos.position;
		player.transform.rotation = Quaternion.identity;
	}
}
