using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

	private AmmoManager ammoManager;

	public abstract AmmoTypes AmmoType { get; }

	public abstract int ClipSize{ get; }

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

	public abstract void Fire(Health hp);

	public void TakeFromClip(){
		TakeFromClip(1);
	}

	public void TakeFromClip(int amount){
		ammoInClip -= amount;
		if (ammoInClip < 0)
			ammoInClip = 0;
	}
}
