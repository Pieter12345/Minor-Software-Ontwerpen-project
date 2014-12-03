using UnityEngine;
using System.Collections;

public class WeaponPickUp : MonoBehaviour {

	public WeaponStats weaponType;
	
	void OnTriggerEnter (Collider other) {
		if(other.tag.Equals("Player")){
			PickUpHandler p = other.GetComponent(typeof(PickUpHandler)) as PickUpHandler;
			p.PickUp(weaponType);
		}
	}
}
