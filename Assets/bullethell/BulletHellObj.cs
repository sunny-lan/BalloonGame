using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BulletHellObj : MonoBehaviour
{
	[SerializeField] protected BulletHellObj[] children;
	public bool fireOnStart = false;

	protected BulletHellObj child => children.FirstOrDefault();

	protected virtual void Awake()
	{
		if (children == null || children.Length==0)
			children = (transform).Cast<Transform>().Select(x => x.GetComponent<BulletHellObj>()).ToArray();
	}

	protected virtual void Start()
	{
		if (fireOnStart) StartCoroutine(Fire());
	}
	public abstract IEnumerator Fire();
}
