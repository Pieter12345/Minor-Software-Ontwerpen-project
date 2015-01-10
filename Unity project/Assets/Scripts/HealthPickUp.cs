using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour {

	public float amountToHeal = 10f;

	void OnTriggerEnter (Collider other) {
		if(other.tag.Equals("Player")){
			PlayerHealth ph = other.transform.parent.GetComponent<PlayerHealth>();
			if(ph.HP < ph.maxHP){
				ph.Heal(amountToHeal);
				Destroy(gameObject);
			}
		}
	}
}
