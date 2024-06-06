using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteFlash : MonoBehaviour
{
	private Image sr;
	private Material mat;



	// Start is called before the first frame update
	void Awake()
    {
		sr = GetComponent<Image>();
        mat=sr.material;
    }

    
    public IEnumerator Flash(float t)
    {
        var oldColor = mat.color;
        mat.color = Color.white;
        
        yield return new WaitForSeconds(t);
        mat.color=oldColor;
    }
}
