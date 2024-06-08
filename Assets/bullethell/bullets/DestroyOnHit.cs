using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
	public LayerMask Hit;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (((Hit >> collision.gameObject.layer)&1) != 0)
		{
			Destroy(gameObject);
		}
	}
}
