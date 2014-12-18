using UnityEngine;
using System.Collections;

public class BindPosition : MonoBehaviour {

	public Transform toBindTo;
//	Vector3 offset;
	Quaternion rotOffset;

	// Use this for initialization
	void Start () {
//		offset = toBindTo.position - transform.position;
		rotOffset = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = toBindTo.position;
		transform.rotation = toBindTo.rotation*rotOffset;
	}
}
