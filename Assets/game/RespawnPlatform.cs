using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlatform : MonoBehaviour
{
	[SerializeField] Transform respawnPos;
	public float InvulnDuration = 3f;

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject == GameManager.LevelManager.Player.gameObject)
		{
			Close();
		}
	}

	private void Close()
	{
		gameObject.SetActive(false);
		GameManager.LevelManager.Player.Invuln = false;
	}

	Action cancelLastInvulnTimeout;

	public void Respawn(PlayerController player)
	{
		if (cancelLastInvulnTimeout != null) { cancelLastInvulnTimeout(); }

		player.Invuln = true;
		gameObject.SetActive(true);
		player.transform.position = respawnPos.position;
		player.transform.rotation = Quaternion.identity;
		bool isCancelled = false;
		cancelLastInvulnTimeout = () =>
		{
			isCancelled = true;
			cancelLastInvulnTimeout = null;
		};
		IEnumerator CloseInSeconds()
		{
			yield return new WaitForSeconds(InvulnDuration);
			if (!isCancelled)
				Close();
		}
		StartCoroutine(CloseInSeconds());
	}
}
