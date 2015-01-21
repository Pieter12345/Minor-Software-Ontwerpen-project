using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {

	public GameObject pauseMenu;
	public GameObject PlaySound;
	public GameObject PauseSound;

	// Update is called once per frame
	void Update () {
		// on press pausekey
		if (Input.GetButtonDown("Pause") || Input.GetKeyUp(KeyCode.Escape)) {
			TogglePause();
		}

		Screen.lockCursor = Time.timeScale != 0f;
		if(pauseMenu == null){ return; }
		pauseMenu.SetActive(Time.timeScale == 0f);

	}

	public void TogglePause(){
		if (Time.timeScale != 0f) {
			Time.timeScale = 0f; // set time to zero if game is running
			PlaySound.GetComponent<AudioSource>().Pause();
			PauseSound.GetComponent<AudioSource>().Play();
		}
		else {
			Time.timeScale = 1f; // resume game if game is not running
			PlaySound.GetComponent<AudioSource>().Play();
			PauseSound.GetComponent<AudioSource>().Stop();
		}
	}

}
