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

	[SerializeField] Rigidbody2D ropeTopSegment;
	[SerializeField] LayerMask popMask;
	[SerializeField] Sprite pop;
	public float popDuration = 0.1f;
	public float popForce = 5;
	public bool Grabbed = false;
	public float balanceVelocityRange = 0.5f;


	Rigidbody2D rb;
	private SpriteRenderer sr;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		balanceVelocity += Random.value * balanceVelocityRange;
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
