using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class AccSync : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Text txt = GetComponent<Text>();
		txt.text = "Accuracy: " + ((int) Math.Round (100*HighScoreKeeper.Accuracy));
	}

}
