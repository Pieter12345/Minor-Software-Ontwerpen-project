using UnityEngine;
using System.Collections;

public class FlagHealth : Health {	

	protected override void OnDeath(bool isHeadshot){
		Debug.Log("Flag got killed");
		Screen.lockCursor = false;
		Application.LoadLevel("GameOver");
	}

}
