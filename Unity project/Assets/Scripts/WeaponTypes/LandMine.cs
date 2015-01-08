using UnityEngine;
using System.Collections;

public class LandMine : MonoBehaviour {

	public float explosionRadius = 1f;
	public GameObject explosionEffect;

	void OnTriggerEnter(Collider col){
		
		Collider[] hit = Physics.OverlapSphere(transform.position, explosionRadius);
		
		if(explosionEffect !=null){
			Instantiate(explosionEffect, transform.position, Quaternion.identity);
		}
		
		foreach(Collider c in hit){
			Damageable dam = c.GetComponent(typeof(Damageable)) as Damageable;
			if(dam != null)
				(dam.HPManager.GetComponent(typeof(Health)) as Health).Damage(WeaponStats.LANDMINE.BaseDamage(),false);
		}

		Destroy (gameObject);

	}
}
