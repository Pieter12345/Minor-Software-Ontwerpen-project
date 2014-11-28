using UnityEngine;
using System.Collections;

public class RuntimeParenting : MonoBehaviour {

	public Transform newParent;

	// Use this for initialization
	void Start () {
		transform.parent = newParent;
	}

}
