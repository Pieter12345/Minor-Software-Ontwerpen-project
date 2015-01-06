using UnityEngine;
using System.Collections;

[System.Serializable]
public class LootAble {
	
	public GameObject pickUp;
	[Range(0.0f, 1.0f)]
	public float probability = 1f;

}
