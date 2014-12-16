using UnityEngine;
using System.Collections;

public class PickUpHandler : MonoBehaviour {

	public GameObject weapons;
	private WeaponController weaponController;
	private AmmoManager ammoManager;

	void Start(){
		weaponController = weapons.GetComponent(typeof(WeaponController)) as WeaponController;
		ammoManager = weapons.GetComponent(typeof(AmmoManager)) as AmmoManager;
	}

	public void PickUp(WeaponStats w){

		weaponController.PickUpWeapon(w);
	}

	public void PickUp(AmmoTypes a, int amount){
		ammoManager.AddAmmo(a, amount);
	}

	void Update(){
		//cheat gun
		if(Input.GetKeyUp(KeyCode.F10)){
			PickUp(WeaponStats.PISTOL);
		}
	}

}
