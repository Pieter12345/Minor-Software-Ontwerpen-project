using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSync : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Text txt = GetComponent<Text>();
		txt.text = "Waves Completed: " + HighScoreKeeper.TotalWave;
	}

}
