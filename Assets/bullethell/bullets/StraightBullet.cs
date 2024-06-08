using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : BulletHellObj
{
    public float speed = 2f;

	public override IEnumerator Fire()
	{
		yield break;
	}

	// Update is called once per frame
	void Update()
    {
        transform.position += speed * Time.deltaTime * transform.right;
    }

	
}
