using UnityEngine;
using System.Collections;

public class FlagHealth : Health {	

	public GameObject underAttackMessage;
	public float regenSpeed = 2;

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
	protected override void OnRegen(){
		Heal(regenSpeed*Time.deltaTime);
	}
}
