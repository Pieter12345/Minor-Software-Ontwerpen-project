using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour {

	public GameObject loot;
	[Range(0.0f, 1.0f)]
	public float dropProbability = 0.5f;

	private bool isQuitting = false;

	void OnApplicationQuit() {
		isQuitting = true;
	}

	void OnDisable(){
		if (Random.Range(0f, 1f) < dropProbability && !isQuitting)
			Instantiate(loot, transform.position, Quaternion.identity);
	}

}