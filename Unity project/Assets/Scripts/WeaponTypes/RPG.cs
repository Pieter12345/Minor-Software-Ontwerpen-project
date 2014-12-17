using UnityEngine;
using System.Collections;

public class RPG : Weapon {

	public Transform firedGrenade;
	public Transform unfiredGrenade;

	public override void Fire(Vector3 from, Vector3 to){
		if(firedGrenade == null || unfiredGrenade == null){
			Debug.LogError("RPG has no grenade!");
			return;
		}
		unfiredGrenade.gameObject.SetActive(false);
		firedGrenade.gameObject.SetActive(true);
		firedGrenade.GetComponent<RPGGrenade>().Fire(to);
	}
}
