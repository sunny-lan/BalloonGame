using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	public float bouyancyForce;
	public bool autoBalance = true;
	public float autoBalanceAmt = 0.005f;

	Rigidbody2D rb;//aa
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (autoBalance)
		{
			if (rb.velocity.y > 0)
			{
				bouyancyForce -= autoBalanceAmt;
			}
			else if(rb.velocity.y < 0) 
			{
				bouyancyForce += autoBalanceAmt;
			}
		}

		rb.AddForce(Vector2.up * bouyancyForce, ForceMode2D.Force);
	}
}
