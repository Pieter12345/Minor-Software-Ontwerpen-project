using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BloackPlaceSync : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Text txt = GetComponent<Text>();
		txt.text = "Blocks Placed: " + HighScoreKeeper.BlocksPlaced;
	}

}
