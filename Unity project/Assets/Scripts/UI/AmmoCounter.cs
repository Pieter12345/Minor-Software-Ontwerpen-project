using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoCounter : MonoBehaviour {

	public Transform weaponController;
	private WeaponController wContr;

	private Text ammoText;

	void Start(){
		wContr = weaponController.GetComponent<WeaponController>() as WeaponController;
		ammoText = GetComponent<Text>() as Text;
	}

	// Update is called once per frame
	void Update () {
		if(wContr != null){
			string toShow = "";
			if(wContr.HasWeapon)
				toShow =  "Ammo: " + wContr.SelectedWeaponInClip + "/" + wContr.SelectedWeaponInPool;
			ammoText.text = toShow;
		}
	}
}
