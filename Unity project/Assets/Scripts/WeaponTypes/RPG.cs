using UnityEngine;
using System.Collections;

public class RPG : Weapon {

	public Transform grenade;

	public override void Fire(Vector3 from, Vector3 to){
		if(grenade == null){
			Debug.LogError("RPG has no grenade!");
			return;
		}
		grenade.GetComponent<RPGGrenade>().Fire(to);

	}
}
