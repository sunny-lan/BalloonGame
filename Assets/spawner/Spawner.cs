using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject[] objectsToSpawn;
	[SerializeField] Rect spawnArea;

	public float rate = 2;//object per second
	public float density = 2; //object per meter
	public float individualVariation = 0.1f;
	public float baseVariation = 0.3f;

	// Start is called before the first frame update
	void Start()
	{
		if(objectsToSpawn==null || objectsToSpawn.Length == 0) { 
			objectsToSpawn=((IEnumerable<Transform>)transform).Select(x=>x.gameObject).ToArray();	
			
		}

		StartCoroutine(Run());
	}
	void OnDrawGizmos()
	{
		// Green
		Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
		DrawRect(spawnArea);
	}

	void DrawRect(Rect rect)
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(
			 new Vector4(rect.center.x, rect.center.y, 0.01f, 1), new Vector3(rect.size.x, rect.size.y, 0.01f));
	}
	
	

	// Update is called once per frame
	IEnumerator Run()
	{
		float[] ordering = Enumerable.Range(0, (int)Mathf.Floor(spawnArea.width * density))
			.Select(x=>x/density)
			.ToArray();

		while (true)
		{
			ordering.Shuffle();

			var baseOffset = Random.value * baseVariation;
			foreach (var x in ordering)
			{
				yield return new WaitForSeconds(1 / rate);
				Spawn(x + baseOffset + Random.value * individualVariation);
			}
		}
	}

	private void Spawn(float x)
	{
		int idx = Random.Range(0, objectsToSpawn.Length);	
		float y = Random.Range(spawnArea.yMin, spawnArea.yMax);

		var obj = Instantiate(objectsToSpawn[idx], transform.localToWorldMatrix * new Vector4(x, y, 0, 1), Quaternion.identity);
		obj.SetActive(true);
	}
}
