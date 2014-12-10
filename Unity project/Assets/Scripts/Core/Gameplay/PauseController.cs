using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	
		// on press pausekey
		if (Input.GetButtonDown("Pause")) {
			if (Time.timeScale == 1f) {
				Time.timeScale = 0f; // set time to zero if game is running
			}
			else {
				Time.timeScale = 1f; // resume game if game is not running
			}
		}
	}
}
