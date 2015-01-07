using UnityEngine;
using System.Collections;



public class SplashScript : MonoBehaviour
{

    public UnityEngine.UI.RawImage Fade;

    // Use this for initialization
    void Start()
    {
        
        Fade.CrossFadeAlpha(0f, 1f, true);
        Invoke("FadeBlack", 5f);

    }

    void FadeBlack()
    {
        Fade.CrossFadeAlpha(1f, 1f, true);
        Invoke("LoadMainMenu", 1f);

    }
    void LoadMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
}