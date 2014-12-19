using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {

	public GameObject pauseMenu;

	// Update is called once per frame
	void Update () {
		// on press pausekey
		if (Input.GetButtonDown("Pause")) {
			TogglePause();
		}

		Screen.lockCursor = Time.timeScale != 0f;
		if(pauseMenu == null){ return; }
		pauseMenu.SetActive(Time.timeScale == 0f);

	}

	public void TogglePause(){
		if (Time.timeScale != 0f) {
			Time.timeScale = 0f; // set time to zero if game is running
		}
		else {
			Time.timeScale = 1f; // resume game if game is not running
		}
	}

}
