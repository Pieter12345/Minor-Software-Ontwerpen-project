using UnityEngine;
using System.Collections;

public class FlagHealth : Health {	

	public GameObject underAttackMessage;

	private UnderAttackMessage uam;

	void Start() {
		if(underAttackMessage != null)
			uam = underAttackMessage.GetComponent<UnderAttackMessage> ();
	}

	protected override void OnDamage() {
		if(uam!= null)
			uam.Show();
	}

	protected override void OnDeath(bool isHeadshot){
		Debug.Log("Flag got killed");
		EndGame();
	}

}
