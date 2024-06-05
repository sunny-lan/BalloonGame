using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 3;
	public float jumpForce = 1;

	[SerializeField] SpriteRenderer handHitbox;
	[SerializeField] Transform hand;

	Collider2D collider;
	Rigidbody2D rb;
	HingeJoint2D handJoint;

	Rigidbody2D grabbed = null;

	LayerMask ropeMask;

	void Awake()
	{
		handJoint = GetComponent<HingeJoint2D>();
		rb = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();
		ropeMask = LayerMask.GetMask("Rope");
	}

	private void Start()
	{
		handJoint.anchor = hand.localPosition;

	}


	private void Update()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			rb.AddForce(Vector2.left * speed);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			rb.AddForce(Vector2.right * speed);
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Grab();
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			UnGrab();
		}
	}


	private void Grab()
	{
		if (grabbed != null) { return; }
		var collision = Physics2D.OverlapArea(handHitbox.bounds.min, handHitbox.bounds.max, ropeMask);
		//Collider2D collision = null;
		//float best = float.PositiveInfinity;
		//foreach (var c in collisions)
		//{
		//	var dist = (collision.transform.position - hand.position).sqrMagnitude;
		//	if (dist < best)
		//	{
		//		collision = c;
		//		best = dist;
		//	}
		//}

		var grabbedRB = collision?.GetComponent<Rigidbody2D>();

		Debug.Log(grabbedRB);

		if (grabbedRB != null)
		{

			grabbed = grabbedRB;

			collider.excludeLayers |= ropeMask;

			handJoint.anchor = transform.worldToLocalMatrix * collision.transform.position.WithW(1);
			handJoint.connectedBody = grabbedRB;
			handJoint.enabled = true;
		}
	}

	private void UnGrab()
	{
		collider.excludeLayers &= ~ropeMask;
		handJoint.connectedBody = null;
		grabbed = null;
		handJoint.enabled = false;
	}
}
