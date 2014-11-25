using UnityEngine;
using System.Collections;


[RequireComponent(typeof(WeaponController))]
public class AmmoManager : MonoBehaviour {

	WeaponController contr;
	AmmoPool[] pools;

	void Awake(){
		pools = new AmmoPool[AmmoTypes.GetValues(typeof(AmmoTypes)).Length];
		for(int i = 0; i < pools.Length; i++){
			pools[i].HeldAmount = 0;
			pools[i].type = (AmmoTypes) i;
			pools[i].MaxAmount = pools[i].type.GetMaxAmmo();
		}
	}

	// Use this for initialization
	void Start () {
		contr = GetComponent<WeaponController>();
	}
	
	public void TakeAmmo(AmmoTypes type, int amount){
		AddAmmo(type, -amount);
	}

	public void AddAmmo(AmmoTypes type, int amount){
		pools[(int) type].HeldAmount += amount;
	}

	public int GetAmmoCount(AmmoTypes type){
		return pools[(int) type].HeldAmount;
	}

	public bool HasAmmo(AmmoTypes type){
		return GetAmmoCount(type) > 0;
	}
}
