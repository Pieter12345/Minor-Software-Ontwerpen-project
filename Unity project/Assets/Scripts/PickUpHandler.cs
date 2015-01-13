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
		// If it is the first weapon pickup of this type.
		if(weaponController.PickUpWeapon(w)) {
			PickUp(w.AmmoType(), w.InitialAmmoPickupAmount()); // Add initial ammo boost.
		}
	}

	public void PickUp(AmmoTypes a, int amount){
		ammoManager.AddAmmo(a, amount);
	}

	void Update(){
		//cheat gun
		if(Input.GetKeyDown(KeyCode.F10)){
			PickUp(WeaponStats.PISTOL);
			PickUp(WeaponStats.RPG);
			PickUp(WeaponStats.LANDMINE);
			PickUp(WeaponStats.RIFLE);
			PickUp(WeaponStats.SHOTGUN);
		}
	}

}
