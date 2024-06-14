using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
	public float baseJumpForce = 2;
	public int airJumpsAllowed = 1;
	public float balloonGrabbedVelocity = -0.3f;

	[SerializeField] SpriteRenderer handHitbox;
	[SerializeField] Transform hand;
	[SerializeField] SpriteRenderer body;

	[SerializeField] Sprite normalSprite;
	[SerializeField] Sprite grabbedSprite;
	[SerializeField] Sprite jumpingSprite;

	Collider2D myCollider;
	private AudioSource audioSource;
	[SerializeField] private CameraShake shake;
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
		myCollider = GetComponent<Collider2D>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		handJoint.anchor = hand.localPosition;

	}

	private bool GetIsGrounded()
	{
		return rb.IsTouchingLayers(groundMask);
	}

	float jumpChargeBegin = -1;
	bool bufferedJump = false;
	public int availJumps = 0;


	public float maxAirVelocity = 2.5f;

	[SerializeField] GameObject ripple;


	bool lastInvuln = false;

	public InputAction jumpInput;
	public InputAction moveInput;
	public InputAction grabInput;

	bool lastJumpInput = false;
	bool lastGrabInput = false;

	private void Update()
	{
		if (lastInvuln != Invuln)
		{
			if (Invuln)
			{
				body.color = body.color.WithAlpha(0.5f);
			}
			else
			{

				body.color = body.color.WithAlpha(1);
			}
			lastInvuln = Invuln;
		}

		if (GetIsGrounded() || grabbed != null)
		{
			availJumps = airJumpsAllowed;
		}

		var movement = moveInput.ReadValue<float>();
		if (movement<0)
		{
			if (!(CurrentStatus is Status.Air && rb.velocity.x < -maxAirVelocity))
				rb.AddForce(Vector2.left * speed.Get(CurrentStatus) * -movement);
		}
		if (movement>0)
		{
			if (!(CurrentStatus is Status.Air && rb.velocity.x > maxAirVelocity))
				rb.AddForce(Vector2.right * speed.Get(CurrentStatus) * movement);
		}

		var curJumpInput= jumpInput.ReadValue<bool>();

		if (curJumpInput && !lastJumpInput)
		{
			if (grabbed != null)
				jumpChargeBegin = Time.time;
			else
			{
				if (GetIsGrounded())
				{
					Debug.Log("jump grounded");
					Jump();
					availJumps = airJumpsAllowed;
				}
				else
				{
					Debug.Log("air jump cnt=" + availJumps);
					if (availJumps > 0)
					{
						Jump(true);
						availJumps--;
					}
				}
			}
		}


		var curGrabInput = grabInput.ReadValue<bool>();
		if (curGrabInput && !lastGrabInput)
		{
			Grab();
		}
		if ((lastGrabInput && !curGrabInput) || (lastJumpInput && !curJumpInput))
		{
			if (grabbed != null && jumpChargeBegin >= 0)
			{
				JumpFromGrab();
			}
			UnGrab();
		}

		lastJumpInput = curJumpInput;
		lastGrabInput = curGrabInput;

	}

	[SerializeField] AudioClip jump;

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

	private void Jump(bool air = false)
	{
		if (air)
		{
			rb.velocity = new(rb.velocity.x, 0);
			Instantiate(ripple, transform);
		}

		var jForce = GetJumpDirection() * jumpForce;
		Debug.Log("jforce=" + jForce + " velocity=" + rb.velocity);
		rb.AddForce(jForce, ForceMode2D.Impulse);
		audioSource.PlayOneShot(jump);
		StartCoroutine(PlayJumpAnim());
	}

	[SerializeField] float jumpAnimTime = 0.2f;

	public float ChargedJumpForce
	{
		get
		{
			if (jumpChargeBegin < 0 || grabbed == null)
			{
				return -1;
			}
			var jumpChargeDuration = Time.time - jumpChargeBegin;
			var jumpForce = Mathf.Min(maxChargedJumpForce, jumpChargeDuration * jumpForcePerChargeTime + baseJumpForce);
			return jumpForce;
		}
	}

	IEnumerator PlayJumpAnim()
	{
		body.sprite = jumpingSprite;
		yield return new WaitForSeconds(jumpAnimTime);
		body.sprite = normalSprite;
	}

	private void JumpFromGrab()
	{
		if (grabbed == null) return;
		if (jumpChargeBegin < 0) return;

		var jumpChargeDuration = Time.time - jumpChargeBegin;
		var jumpForce = Mathf.Min(maxChargedJumpForce, jumpChargeDuration * jumpForcePerChargeTime + baseJumpForce);
		Vector2 basis = GetJumpDirection();

		rb.AddForceAtPosition(jumpForce * basis, handHitbox.transform.position, ForceMode2D.Impulse);
		grabbed.AddForce(-jumpForce * basis, ForceMode2D.Impulse);
		jumpChargeBegin = -1;
		availJumps = airJumpsAllowed;

	}

	public float jumpRatio = 2;

	private Vector2 GetJumpDirection()
	{
		var basis = jumpRatio * Vector2.up;

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			basis += Vector2.left;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			basis += Vector2.right;
		}

		basis.Normalize();
		return basis;
	}
	float oldVelocity;

	private void Grab()
	{
		// Find point closest to hand but also inside box
		var collisions = Physics2D.OverlapAreaAll(handHitbox.bounds.min, handHitbox.bounds.max, ropeMask);
		Collider2D collision = null;
		(int isInside, float distFromHand) best = (2, float.PositiveInfinity);
		Vector2 closestPtOn = default;
		foreach (var c in collisions)
		{
			Vector2 pos = c.transform.position;
			var closestPt = myCollider.ClosestPoint(pos);
			var dist = (c.transform.position - hand.position).sqrMagnitude;
			var score = (isInside: (closestPt - pos).magnitude < 0.01f ? 0 : 1, distFromHand: dist);
			if (score.isInside < best.isInside || (score.isInside == best.isInside && score.distFromHand < best.distFromHand))
			{
				collision = c;
				best = score;
				closestPtOn = closestPt;
			}
		}

		// If grabbed rope then try to attach hinge from player to rope
		var grabbedRB = collision?.GetComponent<Rigidbody2D>();
		Debug.Log(grabbedRB);

		if (grabbedRB != null)
		{
			//snap to box
			transform.position += collision.transform.position - closestPtOn.WithZ(0);

			// grab hinge
			handJoint.anchor = transform.worldToLocalMatrix * collision.transform.position.WithW(1);
			handJoint.connectedBody = grabbedRB;
			handJoint.enabled = true;

			grabbed = grabbedRB;
			var balloon = grabbedRB.GetComponent<RopeSegment>().oldParent.GetComponentInParent<Balloon>();
			Debug.Assert(balloon is not null);

			{
				balloon.Grabbed = true;
				grabbedBalloon = balloon;
			}

			myCollider.excludeLayers |= ropeMask;

			body.sprite = grabbedSprite;
		}
	}

	public void UnGrab()
	{
		if (grabbed == null) return;

		grabbedBalloon.Grabbed = false;
		myCollider.excludeLayers &= ~ropeMask;
		handJoint.connectedBody = null;
		grabbed = null;
		handJoint.enabled = false;
		grabbedBalloon = null;
		body.sprite = normalSprite;
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

	public float deathShakeDuration = 0.2f;
	public float deathShakeAmt = 0.1f;
	public bool Invuln { get; set; }
	public void DoDamage(float damage)
	{
		if (Invuln) return;
		Health -= damage;
		if (Health < 0)
		{
			Lives--;
			Health = MaxHealth;
			StartCoroutine(shake.Shake(deathShakeDuration, deathShakeAmt));
			//lastInvuln = Time.time;
		}
	}

	//float lastInvuln;

	//[SerializeField]
	//private float invulnTime = 3f;
}
