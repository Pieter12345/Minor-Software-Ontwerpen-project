using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float timeTillDisable = 4f;

	private float timeEnabled;

	void OnEnable(){
		timeEnabled = Time.time;
	}

	// Update is called once per frame
	void Update () {
		if(timeEnabled - Time.time > timeTillDisable)
			Destroy(gameObject);
	}
}
