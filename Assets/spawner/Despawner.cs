using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<Balloon>() != null)
		{
			Destroy(collision.gameObject);
		}
	}
}
