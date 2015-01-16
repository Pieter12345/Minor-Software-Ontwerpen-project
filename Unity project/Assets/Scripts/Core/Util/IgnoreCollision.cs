using UnityEngine;
using System.Collections;

public class IgnoreCollision : MonoBehaviour {

	public GameObject[] toIgnore;

	void OnEnable(){
		foreach(GameObject o in toIgnore){
			Physics.IgnoreCollision(collider, o.collider);
		}
	}

}
