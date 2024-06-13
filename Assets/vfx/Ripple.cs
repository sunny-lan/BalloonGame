using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ripple : MonoBehaviour
{
    public float startSize = 0.2f;
    public float finalSize = 5f;
    public float animTime = 2f;

    [SerializeField] AnimationCurve alphaCurve;
    [SerializeField] AnimationCurve sizeCurve;

	private void Awake()
	{
		sr=GetComponent<SpriteRenderer>();
	}

	float t = 0;
	private SpriteRenderer sr;

	// Update is called once per frame
	void Update()
    {
        if (t > animTime)
        {
            SetProg(1);
            Destroy(gameObject);
            return;
        }

        SetProg(t / animTime);

        t += Time.deltaTime;
    }

    void SetProg(float animProg)
    {
        sr.color=sr.color.WithAlpha(1-alphaCurve.Evaluate(animProg));
        transform.localScale = Vector3.one * Mathf.Lerp(startSize, finalSize, sizeCurve.Evaluate(animProg));

    }
}
