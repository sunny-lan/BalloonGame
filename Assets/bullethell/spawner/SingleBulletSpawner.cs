using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBulletSpawner : BulletHellObj
{

	public bool waitForChild = false;
	public bool destroyOnFinish = false;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		child.gameObject.SetActive(false);
	}

	public override IEnumerator Fire()
	{
		var bho = Instantiate(child, transform.position, transform.rotation);
		bho.gameObject.SetActive(true);

		IEnumerator fire()
		{
			yield return bho.Fire();
			if (destroyOnFinish)
			{
				Destroy(bho);
			}
		}

		if (waitForChild)
		{
			yield return fire();
		}
		else
			StartCoroutine(fire());

	}
}
