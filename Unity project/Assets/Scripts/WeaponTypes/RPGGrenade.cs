using UnityEngine;
using System.Collections;

public class RPGGrenade : MonoBehaviour {
	
	public float fireForce = 100f;
	public float explosionRadius = 2f;
	public Transform unfiredGrenade;

	private Transform RPG;

	void Start(){
		RPG = transform.parent;
		transform.parent = null;
	}

	public void Fire(Vector3 to){
		rigidbody.velocity = Vector3.zero;
		transform.forward = -RPG.forward;
		transform.position = RPG.position;
		rigidbody.AddForce((to-transform.position).normalized*fireForce);
		rigidbody.angularVelocity = Vector3.zero;
	}

	public void Reset(){
		unfiredGrenade.gameObject.SetActive(true);
		transform.position = RPG.position;
		gameObject.SetActive(false);
	}

	void OnCollisionEnter(Collision col){

		Collider[] hit = Physics.OverlapSphere(transform.position, explosionRadius);

		foreach(Collider c in hit){
			Damageable dam = c.GetComponent(typeof(Damageable)) as Damageable;
			if(dam != null)
				(dam.HPManager.GetComponent(typeof(Health)) as Health).Damage(WeaponStats.RPG.BaseDamage());
		}

		Reset ();
	}
}
