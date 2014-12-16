using UnityEngine;
using System.Collections;

public class FlagHealth : Health {	
	
	protected override void OnDeath(){
		Debug.Log("Flag got killed");
		Debug.Break();
	}

}
