using UnityEngine;
using System.Collections;

public class SimpleGun : Weapon {

	private float timeLastShot;
	
	public Transform camera;

	public override void Fire(Vector3 from, Vector3 to){
		if(Time.time - timeLastShot > FireInterval){
			timeLastShot = Time.time;
			if (AmmoInClip > 0){
				// Add Recoil
				camera.GetComponent<ShooterGameCamera>().Fired = true;
				ShotEffects();
				TakeFromClip();
				Vector3 dir = to - from;
				dir.Normalize();
				Ray ray = new Ray(from, dir);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {
					
					Debug.Log("Shot hit " + hit.transform.name);
									
					
					Damageable hp = hit.transform.GetComponent<Damageable>();
					if(hp!= null)
						hp.Damage(BaseDamage);
				}
			}
		}

	}

	void Awake(){
		timeLastShot = 0f;
	}
}
