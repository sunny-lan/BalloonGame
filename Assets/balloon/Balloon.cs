using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	public float bouyancyForce;
	public bool autoBalance = true;
	public float autoBalanceAmt = 0.005f;
	public float balanceVelocity = 0.1f;

	Rigidbody2D rb;//aa
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (autoBalance)
		{
			if (rb.velocity.y > balanceVelocity)
			{
				bouyancyForce = Mathf.Max(0, bouyancyForce - autoBalanceAmt * Time.fixedDeltaTime);
			}
			else if(rb.velocity.y < balanceVelocity) 
			{
				bouyancyForce += autoBalanceAmt * Time.fixedDeltaTime;
			}
		}

		rb.AddForce(Vector2.up * bouyancyForce, ForceMode2D.Force);
	}
}
