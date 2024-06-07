using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

	public IEnumerator Shake(float duration, float amount)
    {
        var originalPos = transform.position;
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.position = originalPos + (amount * Random.insideUnitCircle).WithZ(0);

            yield return new WaitForEndOfFrame();
        }
    }
}
