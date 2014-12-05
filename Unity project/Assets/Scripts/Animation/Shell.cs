using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

	public float destroyTime = 5f;

	void OnEnable(){
		Invoke("Kill", destroyTime);
	}

	void OnDisable(){
		CancelInvoke("Kill");
	}

	void Kill(){
		gameObject.SetActive(false);
	}

}
