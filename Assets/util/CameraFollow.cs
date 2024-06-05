using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float speed = 5;

    void Update()
    {
        var tmp = Vector3.Lerp(transform.position, playerTransform.position, Time.deltaTime * speed);
        tmp.z = transform.position.z;
		transform.position = tmp;
    }
}
