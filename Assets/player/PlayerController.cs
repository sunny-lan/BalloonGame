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

	Rigidbody2D rb;
	HingeJoint2D handJoint;

	int ropeMask;

	void Awake()
	{
		handJoint = GetComponent<HingeJoint2D>();
		rb = GetComponent<Rigidbody2D>();
		ropeMask = LayerMask.GetMask("Rope");
	}

	private void Start()
	{
		handJoint.anchor = transform.worldToLocalMatrix * handHitbox.bounds.center;
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

	Rigidbody2D grabbed = null;

	private void Grab()
	{
		if (grabbed != null) { return; }
		var collision = Physics2D.OverlapArea(handHitbox.bounds.min, handHitbox.bounds.max, ropeMask);
		var grabbedRB = collision?.GetComponent<Rigidbody2D>();

		Debug.Log(grabbedRB);

		if (grabbedRB != null)
		{

			grabbed = grabbedRB;
			handJoint.connectedBody = grabbedRB;
			handJoint.enabled = true;
		}
	}

	private void UnGrab()
	{
		handJoint.connectedBody = null;
		grabbed = null;
		handJoint.enabled = false;
	}
}
