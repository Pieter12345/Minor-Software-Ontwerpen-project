using UnityEngine;
using System.Collections;

public class LootSpawner : MonoBehaviour {

	public Vector3 spawnOffset;

	public void Spawn (GameObject g, Vector3 pos, Quaternion rot) {

		GameObject o = (GameObject) Instantiate(g, pos + spawnOffset, rot);

		o.transform.parent = transform;
	}
}
