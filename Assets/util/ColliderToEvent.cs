using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderToEvent : MonoBehaviour
{
	public event Action<Collision2D> CollisionEnter2D;
	public event Action<Collision2D> CollisionExit2D;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		CollisionEnter2D?.Invoke(collision);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		CollisionExit2D?.Invoke(collision);
	}
}
