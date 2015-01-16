using UnityEngine;
using System.Collections;

public class SimpleShotgun : Weapon {

	private float timeLastShot;
	private int countHits;

	public Transform cameraTransform;

	Vector3 from, to;

	void Update(){
		Vector3 centerDir = to - from;
		centerDir.Normalize();
		Vector3[] directions = CalculateRayDirections(centerDir);
		foreach(Vector3 dir in directions){
			Debug.DrawRay(from, dir, Color.red);
		}
	}

	public override void Fire(Vector3 from, Vector3 to){
		this.from = from;
		this.to = to;
		if(Time.time - timeLastShot > FireInterval){
			timeLastShot = Time.time;
			if (AmmoInClip > 0){
				// Add Recoil
				cameraTransform.GetComponent<gameCameraSelector>().setFired(true);
				// Reset #Hits
				countHits = 0;
				
				ShotEffects();
				TakeFromClip();
				Vector3 centerDir = to - from;
				centerDir.Normalize();
				Vector3[] directions = CalculateRayDirections(centerDir);
				foreach(Vector3 dir in directions){
					Ray ray = new Ray(from, dir);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 100, int.MaxValue - LayerMask.GetMask("Ignore Aimpoint Raycast"))) {
						ImpactEffects(hit.point);
						Debug.Log("Shotgun hit " + hit.transform.name);
						
						if (hit.transform.tag == "Enemy") {
							countHits+=1;
						}
						
						Damageable hp = hit.transform.GetComponent<Damageable>();
						if(hp!= null)
							hp.Damage(BaseDamage/directions.Length);
					}
				}
				if (countHits>0) {
					HighScoreKeeper.ShotsFired(true);
				}
			}
		}
		
	}
	
	void Awake(){
		timeLastShot = 0f;
	}

	private Vector3[] CalculateRayDirections(Vector3 centerRay){
		Vector3[] dir = new Vector3[7];
		float angle = Random.Range(6f,10f); 
		dir[0] = centerRay;
		dir[1] = Quaternion.Euler ( 0f, angle, 0f) * centerRay;
		dir[2] = Quaternion.Euler ( 0f,-angle, 0f) * centerRay;
		dir[3] = Quaternion.Euler ( angle, 0f, 0f) * centerRay;
		dir[4] = Quaternion.Euler (-angle, 0f, 0f) * centerRay;
		dir[5] = Quaternion.Euler ( 0f, 0f, angle) * centerRay;
		dir[6] = Quaternion.Euler ( 0f, 0f,-angle) * centerRay;
		return dir;
	}
}
