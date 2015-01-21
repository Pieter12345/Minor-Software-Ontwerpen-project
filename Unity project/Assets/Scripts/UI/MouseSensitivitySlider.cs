using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseSensitivitySlider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.HasKey("mouseSensitivity"))
			GetComponent<Slider>().value = PlayerPrefs.GetFloat("mouseSensitivity");
	}

}
