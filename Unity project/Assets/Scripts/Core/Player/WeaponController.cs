using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public int defaultWeapon = 0;
	public Transform[] weapons;

	private int selectedWeapon;
	public int SelectedWeapon { 
		get {
			return selectedWeapon;
		}
		set {
			selectedWeapon = value;
		}
	}

	// Use this for initialization
	void Start () {
		selectedWeapon = defaultWeapon;
	}
	
	// Update is called once per frame
	void Update () {
		if (selectedWeapon < 0)
			selectedWeapon = weapons.Length - 1;
		else if (selectedWeapon >= weapons.Length)
			selectedWeapon = 0;
		foreach (Transform w in weapons){
			w.gameObject.SetActive(w.Equals(weapons[selectedWeapon]));
		}
	}

	public void Fire(Health hp) {
		if(weapons[selectedWeapon] != null){
			Weapon weaponScript = (Weapon) weapons[selectedWeapon].GetComponent(typeof(Weapon));
			weaponScript.Fire(hp);
		}
	}
}
