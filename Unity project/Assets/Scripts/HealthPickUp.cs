using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour {

	public float amountToHeal = 10f;

	void OnTriggerEnter (Collider other) {
		if(other.tag.Equals("Player")){
			other.transform.parent.GetComponent<PlayerHealth>().Heal(amountToHeal);
		}
	}
}
