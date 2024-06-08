using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletHellObj : MonoBehaviour
{
    [SerializeField] protected BulletHellObj child;
    public bool fireOnStart = false;

    protected virtual void Awake()
    {
        if(child==null && transform.childCount>0)
            child = transform.GetChild(0).GetComponent<BulletHellObj>();
    }

	protected virtual void Start()
    {
        if(fireOnStart)StartCoroutine(Fire());  
    }
    public abstract IEnumerator Fire();
}
