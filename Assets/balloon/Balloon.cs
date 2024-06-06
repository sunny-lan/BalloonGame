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

	[DoNotSerialize]
	public bool Grabbed = false;

	[Serialize] Joint2D topRopeJoint;

	Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
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
	}

}
