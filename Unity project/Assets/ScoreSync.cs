using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSync : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Text txt = GetComponent<Text>();
		txt.text = "Score: " + (HighScoreKeeper.Score+HighScoreKeeper.AccuracyBonus);
	}

}
