using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	public float bouyancyForce;
	public bool autoBalance = true;
	public float autoBalanceAmt = 0.005f;
	public float balanceVelocity = 0.1f;
	public float grabbedVelocity = -0.2f;

	[SerializeField] Rope rope;
	[SerializeField] Rigidbody2D ropeTopSegment;
	[SerializeField] LayerMask popMask;
	[SerializeField] Sprite pop;
	public float popDuration = 0.1f;
	public float popForce = 5;
	private bool grabbed = false;
	public float balanceVelocityRange = 0.5f;
	public float spawnSimDuration = 3f;


	Rigidbody2D rb;
	private SpriteRenderer sr;

	public bool Grabbed
	{
		get => grabbed; set
		{
			grabbed = value;
			if (grabbed)
				rope.Unfreeze();
			else
			{
				if (GameManager.EnableOptimization)
					StartCoroutine(WaitAndDisable());
			}
		}
	}

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		balanceVelocity += Random.value * balanceVelocityRange;
		GameManager.EnableOptimizationChanged += GameManager_EnableOptimizationChanged;
	}

	private void GameManager_EnableOptimizationChanged()
	{
        if (!GameManager.EnableOptimization)
			rope.Unfreeze();
		else
			StartCoroutine(WaitAndDisable());

	}

	private void Start()
	{

		if (GameManager.EnableOptimization)
			StartCoroutine(WaitAndDisable());
	}

	private IEnumerator WaitAndDisable()
	{
		yield return new WaitForSeconds(spawnSimDuration);
		if (!Grabbed && GameManager.EnableOptimization)
			rope.Freeze(transform);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (autoBalance)
		{
			float balanceVelocity = !Grabbed ? this.balanceVelocity : grabbedVelocity;
			if (rb.velocity.y > balanceVelocity)
			{
				bouyancyForce = Mathf.Max(0, bouyancyForce - autoBalanceAmt * Time.fixedDeltaTime);
			}
			else if (rb.velocity.y < balanceVelocity)
			{
				bouyancyForce += autoBalanceAmt * Time.fixedDeltaTime;
			}
		}

		rb.AddForce(Vector2.up * bouyancyForce, ForceMode2D.Force);
		if (rb.IsTouchingLayers(popMask))
		{
			StartCoroutine(Pop());
		}
	}



	IEnumerator Pop()
	{
		ropeTopSegment.AddForce(Vector2.down * popForce, ForceMode2D.Impulse);
		sr.sprite = pop;
		yield return new WaitForSeconds(popDuration);
		Destroy(gameObject);
	}
}
