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

	public void Fire(Vector3 from, Vector3 to) {
		if(weapons[selectedWeapon] != null){
			Weapon weaponScript = (Weapon) weapons[selectedWeapon].GetComponent(typeof(Weapon));
			if (weaponScript.AmmoInClip > 0){
				weaponScript.TakeFromClip();
				Vector3 dir = to - from;
				dir.Normalize();
				Ray ray = new Ray(from, dir);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {
					
					Debug.Log("Shot hit " + hit.transform.name);
					
					Health hp = hit.transform.GetComponent<Health>();
					weaponScript.Fire(hp);
				}
			}
		}
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
