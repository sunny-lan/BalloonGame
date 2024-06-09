using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	[SerializeField] float rotationSpeed = 5;


	// Update is called once per frame
	void Update()
	{
		transform.localEulerAngles = new(0, 0, transform.localEulerAngles.z + rotationSpeed * Time.deltaTime * Mathf.PI * 2);
	}

	public void SetSpeed(float s)
	{
		rotationSpeed = s;	
	}
}
