using UnityEngine;
using System.Collections;



public class SplashScript : MonoBehaviour {

    public UnityEngine.UI.RawImage Fade;

	// Use this for initialization
	void Start () {
        Fade.CrossFadeAlpha(0, 1, false);
        
        Invoke("loadMainMenu", 5.0f);
        
	}

    void loadMainMenu()
    {
        Fade.CrossFadeAlpha(1, 5, false);
        Application.LoadLevel("MainMenu");
    }
}