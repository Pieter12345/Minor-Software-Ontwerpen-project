using UnityEngine;
using System.Collections;

public class RPG : Weapon {

	public Transform firedGrenade;
	public Transform unfiredGrenade;

	private float timeLastShot;

	void Update(){
		unfiredGrenade.gameObject.SetActive(AmmoInClip > 0);
	}

	public override void Fire(Vector3 from, Vector3 to){
		if(Time.time - timeLastShot > FireInterval){
			timeLastShot = Time.time;
			if (AmmoInClip > 0){
				TakeFromClip();
				if(firedGrenade == null || unfiredGrenade == null){
					Debug.LogError("RPG has no grenade!");
					return;
				}
				ShootSound();
				unfiredGrenade.gameObject.SetActive(false);
				firedGrenade.gameObject.SetActive(true);
				firedGrenade.GetComponent<RPGGrenade>().Fire(to);
			}
		}
	}
}
