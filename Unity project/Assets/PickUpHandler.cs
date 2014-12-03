using UnityEngine;
using System.Collections;

public class PickUpHandler : MonoBehaviour {

	public GameObject weapons;
	private WeaponController weaponController;

	void Start(){
		weaponController = weapons.GetComponent(typeof(WeaponController)) as WeaponController;
	}

	public void PickUp(WeaponStats w){

		weaponController.PickUpWeapon(w);
	}

}
