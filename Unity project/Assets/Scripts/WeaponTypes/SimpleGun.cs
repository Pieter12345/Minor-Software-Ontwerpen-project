using UnityEngine;
using System.Collections;

public class SimpleGun : Weapon {

	private float timeLastShot;
	private ObjectPool shellPool;

	public Transform camera;

	public GameObject shell;
	public Vector3 ejectLocation;
	public Vector3 ejectForce;

	public override void Fire(Vector3 from, Vector3 to){
		if(Time.time - timeLastShot > FireInterval){
			timeLastShot = Time.time;
			if (AmmoInClip > 0){
				// Add Recoil
				if(camera.GetComponent<ShooterGameCamera>().enabled) {
					camera.GetComponent<ShooterGameCamera>().setFired(true);
				}
				if(camera.GetComponent<FirstPersonShooterGameCamera>().enabled){
					camera.GetComponent<FirstPersonShooterGameCamera>().setFired(true);
				}
				ShotEffects();
				TakeFromClip();
				EjectShell();
				Vector3 dir = to - from;
				dir.Normalize();
				Ray ray = new Ray(from, dir);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {
					
					Debug.Log("Shot hit " + hit.transform.name);
					
					//Stats Accuracy counter- needs revising with each new enemy
					if (hit.transform.tag == "Enemy") {
						HighScoreKeeper.ShotsFired(true);
					}
					else {
						HighScoreKeeper.ShotsFired(false);
					}
					
					
					Damageable hp = hit.transform.GetComponent<Damageable>();
					if(hp!= null)
						hp.Damage(BaseDamage);
				}
			}
		}

	}

	void Awake(){
		timeLastShot = 0f;
		if(shell != null)
			shellPool = new ObjectPool(shell);
	}

	public void EjectShell(){
		GameObject s = shellPool.GetFreeObject();
		s.SetActive(true);
		s.transform.rotation = transform.rotation;
		s.transform.position = transform.position + ejectLocation;
		s.rigidbody.AddForce(s.transform.rotation* ejectForce);
	}
}
