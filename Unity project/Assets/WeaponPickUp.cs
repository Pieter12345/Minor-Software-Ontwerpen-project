using UnityEngine;
using System.Collections;

public class WeaponPickUp : MonoBehaviour {

	public WeaponStats weaponType;
	public int amountOfAmmo = 10;

	void OnTriggerEnter (Collider other) {
		if(other.tag.Equals("Player")){
			Debug.Log("Picked up " + weaponType.ToString());
			PickUpHandler p = other.GetComponent(typeof(PickUpHandler)) as PickUpHandler;
			if(p!=null){
				p.PickUp(weaponType);
				p.PickUp(weaponType.AmmoType(), amountOfAmmo);
				Destroy(gameObject);
			} else {
				Debug.LogWarning("No PickUpHandler found on playerMesh");
			}
		}
	}
}
