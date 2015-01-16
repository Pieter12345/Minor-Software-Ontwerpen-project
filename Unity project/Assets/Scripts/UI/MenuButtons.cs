using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour
{
    public UnityEngine.UI.RawImage Fade;

    //Fade into menu on load//
    void Start()
    {
		if(Fade != null)
        	Fade.CrossFadeAlpha(0f, 1f, true);
        //Invoke("FadeDisable", 1f);
    }
    
    //(dep)
    void FadeDisable()
    {
        Fade.gameObject.SetActive(false);
    }
    //////////////////////////

    //Functions corresponding to Menu Buttons//
    public void HowTo()
    {
        LoadScene("LoadHowTo");
    }


    public void ToMainMenu()
    {
		Time.timeScale = 1f;
        LoadScene("LoadMainMenu");
    }

    public void RetryLevel()
    {
        LoadScene("LoadRetryLevel");

    }

    public void QuitGame()
    {
        LoadScene("LoadQuitGame");

    }

	public void Resume() {
		Time.timeScale = 1f; // resume game.
	}
    ///////////////////////////////////////////

    //Code corresponding to Menu Buttons//
    void LoadHowTo()
    {
        Debug.Log("Showing How To");
        Application.LoadLevel("HowTo");
    }

    void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu");
        Application.LoadLevel("MainMenu");
    }

    void LoadRetryLevel()
    {
        Debug.Log("Loading Level");
        Screen.lockCursor = true;
        Application.LoadLevel("Level");
    }

    void LoadQuitGame()
    {
        Debug.Log("Exiting game");
        Application.Quit();
    }
    //////////////////////////////////////

    //Fades screen to black in 1 second, then calls the function
    void LoadScene(string function)
    {
		if(Fade != null)
        	Fade.CrossFadeAlpha(1f, 1f, true);
        Invoke(function, 1f);
    }
}
