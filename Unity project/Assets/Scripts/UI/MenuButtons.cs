using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour {

	public void HowTo(){
		Debug.Log ("Showing How To");
		Application.LoadLevel("HowTo");
	}

	public void ToMainMenu(){
		Debug.Log ("Loading Main Menu");
		Application.LoadLevel("MainMenu");
	}

	public void RetryLevel(){
		Debug.Log ("Loading Level");
		Screen.lockCursor = true;
		Application.LoadLevel("Level");
	}

	public void QuitGame(){
		Debug.Log ("Exiting game");
		Application.Quit();
	}
}
