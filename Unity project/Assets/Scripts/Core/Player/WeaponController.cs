using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour {

	public int defaultWeapon = 0;
	public Transform[] weapons;

	private bool hasWeapon = false;
	public bool HasWeapon{ get{ return hasWeapon; } }

	private bool[] weaponsInInventory;
	public WeaponStats[] WeaponsInInventory{
		get {
			List<WeaponStats> w = new List<WeaponStats>();
			for(int i = 0; i < weaponsInInventory.Length; i++) {
				if(weaponsInInventory[i])
					w.Add((WeaponStats) i);
			}
			return w.ToArray();
		}
	}

	public GameObject camera;

	private int selectedWeapon;
	public int SelectedWeapon { 
		get {
			return selectedWeapon;
		}
		set {
			selectedWeapon = value;
		}
	}
	public Transform SelectedWeaponTransform { 
		get {
			return weapons[selectedWeapon];
		}
	}
	public WeaponStats SelectedWeaponType { 
		get {
			return (SelectedWeaponTransform.GetComponent(typeof(Weapon)) as Weapon).weaponType;
		}
	}

	// Use this for initialization
	void Start () {
		selectedWeapon = defaultWeapon;
		WeaponStats[] v = (WeaponStats[]) WeaponStats.GetValues(typeof(WeaponStats));
		weaponsInInventory = new bool[v.Length];
	}

	// Update is called once per frame
	void Update () {
		wrapSelectedWeaponIndex();
		foreach (Transform w in weapons){
			w.gameObject.SetActive(w.Equals(weapons[selectedWeapon]));
		}
	}

	public void SelectNextWeaponUp(){
		while (!weaponsInInventory[(int) SelectedWeaponType]){
			selectedWeapon++;
			wrapSelectedWeaponIndex();
		}
	}

	public void SelectNextWeaponDown(){
		while (!weaponsInInventory[(int) SelectedWeaponType]){
			selectedWeapon--;
			wrapSelectedWeaponIndex();
		}
	}

	private void wrapSelectedWeaponIndex(){
		if (selectedWeapon < 0)
			selectedWeapon = weaponsInInventory.Length - 1;
		else if (selectedWeapon >= weapons.Length)
			selectedWeapon = 0;
	}

	public void Fire(Vector3 from, Vector3 to) {
		if(weapons[selectedWeapon] != null && hasWeapon){
			Weapon weaponScript = (Weapon) weapons[selectedWeapon].GetComponent(typeof(Weapon));
			weaponScript.Fire(from, to);
		}

	}

	public void PickUpWeapon(WeaponStats w){
		hasWeapon = true;
		weaponsInInventory[(int) w] = true;
	}

	public void DropWeapon(WeaponStats w){
		bool weaponFound = false;
		foreach(bool b in weaponsInInventory){
			if (b == true)
				weaponFound = true;
		}
		if(!weaponFound)
			hasWeapon = false;
		weaponsInInventory[(int) w] = false;
	}

	public void Reload(){
		Weapon weaponScript = (Weapon) weapons[selectedWeapon].GetComponent(typeof(Weapon));
		weaponScript.RefillClip();
	}

	public void AddAmmoToCurrent(int amount){
		Weapon weaponScript = (Weapon) weapons[selectedWeapon].GetComponent(typeof(Weapon));
		AmmoManager am = GetComponent<AmmoManager>();
		am.AddAmmo(weaponScript.AmmoType, amount);
	}
}
