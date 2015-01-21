using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FiredSync : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Text txt = GetComponent<Text>();
		txt.text = "Shots Fired: " + (HighScoreKeeper.ShotsHit+HighScoreKeeper.ShotsMissed);
	}

}
