using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rope : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        for(int i=0;i+1<transform.childCount;i++)
        {
            var a = transform.GetChild(i).GetComponent<Rigidbody2D>();
            var b = transform.GetChild(i+1);
            b.GetComponent<HingeJoint2D>().connectedBody = a;
            //b.GetComponent<DistanceJoint2D>().connectedBody = a;
		}

        List<Transform> lsit=new List<Transform>();
        foreach(Transform a in transform)
        {
            lsit.Add(a);
        }
        lsit.ForEach(x=>x.parent= null);
    }
}
