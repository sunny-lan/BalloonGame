using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject[] objectsToSpawn;
	[SerializeField] Rect spawnArea;

	public float rate = 2;//object per second

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
		while (true)
		{
			var delay = Mathf.Log(1 - Mathf.Min(0.99f, Random.value)) / (-rate);
			yield return new WaitForSeconds(delay);
			Spawn();
		}
	}

	private void Spawn()
	{
		int idx = Random.Range(0, objectsToSpawn.Length);	
		float x = Random.Range(spawnArea.xMin, spawnArea.xMax);
		float y = Random.Range(spawnArea.yMin, spawnArea.yMax);

		var obj = Instantiate(objectsToSpawn[idx], transform.localToWorldMatrix * new Vector4(x, y, 0, 1), Quaternion.identity);
		obj.SetActive(true);
	}
}
