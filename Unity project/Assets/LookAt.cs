using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	public Transform toLookAt;
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(toLookAt);
	}
}
