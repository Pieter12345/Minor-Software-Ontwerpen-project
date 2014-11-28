using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

	private AmmoManager ammoManager;

	public WeaponStats weaponType;

	public float BaseDamage{ get { return weaponType.BaseDamage(); } }

	public AmmoTypes AmmoType { get{return weaponType.AmmoType();} }

	public int ClipSize{ get{return weaponType.ClipSize();} }

	public float FireInterval{ get {return weaponType.FireInterval();} }
	public float FireRate{ get{ return (FireInterval != 0) ? 1f/FireInterval : 0; } }

	public float ReloadTime{ get{ return weaponType.ReloadTime();} }
	
	//public float Recoil = weaponType.Recoil();

	private int ammoInClip = 0;
	public int AmmoInClip { 
		get { 
			return ammoInClip; 
		} 
		set { 
			ammoInClip = value; 
		} 
	}

	void Start() {
		ammoManager = transform.parent.GetComponent<AmmoManager>();
	}

	public void RefillClip() {
		if (ClipSize - AmmoInClip <= GetAmmoCount ()){
			ammoManager.TakeAmmo(AmmoType, 
			                     ClipSize - AmmoInClip
			                     );
			ammoInClip = ClipSize;
		} else {
			AmmoInClip += GetAmmoCount ();
			ammoManager.TakeAmmo(AmmoType, GetAmmoCount());
		}
	}

	public int GetAmmoCount(){
		return ammoManager.GetAmmoCount(AmmoType);
	}

	public abstract void Fire(Vector3 from, Vector3 to);

	public void TakeFromClip(){
		TakeFromClip(1);
	}

	public void TakeFromClip(int amount){
		ammoInClip -= amount;
		if (ammoInClip < 0)
			ammoInClip = 0;
	}
}
