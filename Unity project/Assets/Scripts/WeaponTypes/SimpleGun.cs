using UnityEngine;
using System.Collections;

public class SimpleGun : Weapon {

	private float timeLastShot;

	public override void Fire(Vector3 from, Vector3 to){
		if(Time.time - timeLastShot > FireInterval){
			timeLastShot = Time.time;
			if (AmmoInClip > 0){
				// Add Recoil
				//weaponScript.weaponType.Recoil();
				GameObject.Find("Main Camera").GetComponent<ShooterGameCamera>().Fired = true;
				
				TakeFromClip();
				Vector3 dir = to - from;
				dir.Normalize();
				Ray ray = new Ray(from, dir);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {
					
					Debug.Log("Shot hit " + hit.transform.name);
									
					
					Health hp = hit.transform.GetComponent<Health>();
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
