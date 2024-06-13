using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleWall : MonoBehaviour
{
	[SerializeField] private GameObject ripple;
    [SerializeField] LayerMask hits;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (((hits >> collision.gameObject.layer) & 1) != 0)
		{

			Instantiate(ripple, collision.transform.position, Quaternion.identity);
		}

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		OnTriggerEnter2D(collision.collider);
	}
}
