using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlatform : MonoBehaviour
{
	[SerializeField] Transform respawnPos;
	public float InvulnDuration = 3f;
	public float followingInvulnDuration = 2f;

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject == GameManager.LevelManager.Player.gameObject)
		{
			Close();
		}
	}

	private void Close()
	{
		if (cancelInvuln2 != null) { cancelInvuln2(); cancelInvuln2 = null; }
		gameObject.SetActive(false);
		bool isCancelled = false;
		cancelInvuln2 = () => isCancelled = true;
		IEnumerator setInvuln()
		{
			yield return new WaitForSeconds(followingInvulnDuration);
			if (!isCancelled)
				GameManager.LevelManager.Player.Invuln = false;
		}
		StartCoroutine(setInvuln());
	}

	Action cancelLastInvulnTimeout;
	Action cancelInvuln2;

	public void Respawn(PlayerController player)
	{
		if (cancelLastInvulnTimeout != null) { cancelLastInvulnTimeout(); cancelLastInvulnTimeout = null; }
		if (cancelInvuln2 != null) { cancelInvuln2(); cancelInvuln2 = null; }

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
