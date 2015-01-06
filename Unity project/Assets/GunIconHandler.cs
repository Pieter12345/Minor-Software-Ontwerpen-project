using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class GunIconHandler : MonoBehaviour {

	public Transform weaponController;
	private WeaponController wContr;

	private Image icon;
	public Sprite[] icons;

	void Start(){
		wContr = weaponController.GetComponent<WeaponController>() as WeaponController;
		icon = GetComponent<Image>();
		icon.enabled = wContr.HasWeapon;
	}
	
	// Update is called once per frame
	void Update () {
		if(icons.Length < wContr.weapons.Length){
			Debug.LogWarning("Not enough sprites to show icon in GunIconHandler");
			return;
		}


		if(wContr != null){
			icon.enabled = wContr.HasWeapon;
			icon.sprite = icons[wContr.SelectedWeapon];
		}
	}
}
