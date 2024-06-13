using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rope : MonoBehaviour
{
	private List<Transform> children;

	// Start is called before the first frame update
	void Awake()
	{
		for (int i = 0; i + 1 < transform.childCount; i++)
		{
			var a = transform.GetChild(i).GetComponent<Rigidbody2D>();
			var b = transform.GetChild(i + 1);
			b.GetComponent<HingeJoint2D>().connectedBody = a;
			//b.GetComponent<DistanceJoint2D>().connectedBody = a;
		}

		List<Transform> lsit = new List<Transform>();
		foreach (Transform a in transform)
		{
			lsit.Add(a);
			a.GetComponent<RopeSegment>().oldParent = this;
		}
		lsit.ForEach(x => x.parent = null);
		this.children = lsit;
	}

	public void Freeze(Transform parent)
	{

		foreach (Transform a in children)
		{
			a.GetComponent<HingeJoint2D>().enabled = false;
			a.GetComponent<Rigidbody2D>().simulated = false;
		}

		foreach (Transform a in children)
		{

			a.parent = parent;
		}
	}

	public void Unfreeze()
	{
		foreach (Transform a in children)
			a.parent = null;

		foreach (Transform a in children)
		{
			a.GetComponent<HingeJoint2D>().enabled = true;
			a.GetComponent<Rigidbody2D>().simulated = true;
		}
	}

	public void OnDestroy()
	{
		foreach (Transform a in children) Destroy(a.gameObject);
	}
}
