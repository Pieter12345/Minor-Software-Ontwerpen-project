using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NameSync : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Text txt = GetComponent<Text>();
		txt.text = "Anonymous";
		if(CurrentUser.CurrentUsername != null && CurrentUser.CurrentUsername != "")
			txt.text = CurrentUser.CurrentUsername;
	}

}
