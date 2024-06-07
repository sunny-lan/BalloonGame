using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
	[Serializable]
	public struct Stats<T>
	{
		public T ground;
		public T air;
		public T grab;

		public Stats(T v)
		{
			ground = air = grab = v;
		}

		public static implicit operator Stats<T>(T v) => new Stats<T>(v);

		public readonly T Get(Status status)
		{
			return status switch
			{
				Status.Ground => ground,
				Status.Grab => grab,
				Status.Air => air,
				_ => throw new NotImplementedException(),
			};
		}
	}

	public Stats<float> speed = 3;
	public float jumpForce = 1;
	public float jumpForcePerChargeTime = 2;
	public float maxChargedJumpForce = 5;
	public int airJumpsAllowed = 1;
	public float balloonGrabbedVelocity = -0.3f;

	[SerializeField] SpriteRenderer handHitbox;
	[SerializeField] Transform hand;

	Collider2D collider;
	Rigidbody2D rb;
	HingeJoint2D handJoint;

	Rigidbody2D grabbed = null;
	Balloon grabbedBalloon = null;

	[SerializeField] LayerMask ropeMask;
	[SerializeField] LayerMask groundMask;

	void Awake()
	{

		handJoint = GetComponent<HingeJoint2D>();
		rb = GetComponent<Rigidbody2D>();
		collider = GetComponent<Collider2D>();

	}

	private void Start()
	{
		handJoint.anchor = hand.localPosition;

	}

	private bool GetIsGrounded()
	{
		return rb.IsTouchingLayers(groundMask);
	}

	float jumpChargeBegin;
	bool bufferedJump = false;
	public int availJumps = 0;


	private void Update()
	{
		if (GetIsGrounded() || grabbed != null)
			availJumps = airJumpsAllowed;

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			rb.AddForce(Vector2.left * speed.Get(CurrentStatus));
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			rb.AddForce(Vector2.right * speed.Get(CurrentStatus));
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (grabbed != null)
				jumpChargeBegin = Time.time;
			else
			{
				if (availJumps > 0)
				{
					Jump();
					availJumps--;
				}
			}

		}



		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Grab();
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			if (grabbed != null && jumpChargeBegin >= 0)
			{
				JumpFromGrab();
			}
			UnGrab();
		}
	}

	public enum Status
	{
		Ground,
		Grab,
		Air
	}

	Status CurrentStatus
	{
		get
		{
			if (grabbed != null) return Status.Grab;
			if (GetIsGrounded()) return Status.Ground;
			else return Status.Air;
		}
	}

	public void SetFreeze(bool freeze)
	{
		if (freeze)
		{
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		else
		{
			rb.constraints = RigidbodyConstraints2D.None;
		}
	}

	private void Jump()
	{
		rb.velocity = new(rb.velocity.x, 0);
		rb.AddForce(GetJumpDirection() * jumpForce, ForceMode2D.Impulse);
	}

	private void JumpFromGrab()
	{
		if (grabbed == null) return;
		if (jumpChargeBegin < 0) return;

		var jumpChargeDuration = Time.time - jumpChargeBegin;
		var jumpForce = Mathf.Min(maxChargedJumpForce, jumpChargeDuration * jumpForcePerChargeTime);
		Vector2 basis = GetJumpDirection();

		rb.AddForceAtPosition(jumpForce * basis, handHitbox.transform.position, ForceMode2D.Impulse);
		grabbed.AddForce(-jumpForce * basis, ForceMode2D.Impulse);
		jumpChargeBegin = -1;
	}

	private Vector2 GetJumpDirection()
	{
		var basis = Vector2.up * jumpForce;

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			basis += Vector2.left * speed.Get(CurrentStatus);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			basis += Vector2.right * speed.Get(CurrentStatus);
		}

		basis.Normalize();
		return basis;
	}
	float oldVelocity;

	private void Grab()
	{
		var collisions = Physics2D.OverlapAreaAll(handHitbox.bounds.min, handHitbox.bounds.max, ropeMask);
		Collider2D collision = null;
		float best = float.PositiveInfinity;
		foreach (var c in collisions)
		{
			var dist = (c.transform.position - hand.position).sqrMagnitude;
			if (dist < best)
			{
				collision = c;
				best = dist;
			}
		}

		var grabbedRB = collision?.GetComponent<Rigidbody2D>();

		Debug.Log(grabbedRB);

		if (grabbedRB != null)
		{

			grabbed = grabbedRB;
			var balloon = grabbedRB.GetComponent<RopeSegment>().oldParent.GetComponentInParent<Balloon>();
			Debug.Assert(balloon is not null);

			{
				balloon.Grabbed = true;
				grabbedBalloon = balloon;
			}

			collider.excludeLayers |= ropeMask;

			handJoint.anchor = transform.worldToLocalMatrix * collision.transform.position.WithW(1);
			handJoint.connectedBody = grabbedRB;
			handJoint.enabled = true;
		}
	}

	private void UnGrab()
	{
		if (grabbed == null) return;

		grabbedBalloon.Grabbed = false;
		collider.excludeLayers &= ~ropeMask;
		handJoint.connectedBody = null;
		grabbed = null;
		handJoint.enabled = false;
		grabbedBalloon = null;
	}

	[SerializeField] float MaxHealth;
	[SerializeField] private int lives = 5;

	public float Health { get; set; }
	public int Lives
	{
		get => lives; set
		{
			lives = value;
			LivesChanged?.Invoke();
		}
	}

	public event Action LivesChanged;


	public bool Invuln { get; set; }
	public void DoDamage(float damage)
	{
		if (Invuln) return;
		Health -= damage;
		if (Health < 0)
		{
			Lives--;
			Health = MaxHealth;
			//lastInvuln = Time.time;
		}
	}

	//float lastInvuln;

	//[SerializeField]
	//private float invulnTime = 3f;
}
