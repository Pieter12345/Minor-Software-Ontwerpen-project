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
			ammoManager.takeAmmo(AmmoType, 
			                     ClipSize - AmmoInClip
			                     );
			ammoInClip = ClipSize;
		} else {
			AmmoInClip += GetAmmoCount ();
			ammoManager.takeAmmo(AmmoType, GetAmmoCount());
		}
	}

	public int GetAmmoCount(){
		return ammoManager.GetAmmoCount(AmmoType);
	}

	public abstract void Fire(Health hp);
}
