using UnityEngine;
using System.Collections;

public class RPGGrenade : MonoBehaviour {

	private bool fired = false;

	public void Fire(Vector3 to){
		fired = true;
		Debug.Log("fired = " + fired);
	}

	public void Reset(){
		fired = false;
		transform.position = transform.parent.position;
	}

	void OnCollisionEnter(Collision col){
		Debug.Log("BOOM");
		if(fired)
			gameObject.SetActive(false);
	}
}
