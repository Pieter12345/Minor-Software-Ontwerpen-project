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
	
	public void takeAmmo(AmmoTypes type, int amount){
		addAmmo(type, amount);
	}

	public void addAmmo(AmmoTypes type, int amount){
		pools[(int) type].HeldAmount += amount;
	}

	public int GetAmmoCount(AmmoTypes type){
		return pools[(int) type].HeldAmount;
	}
}
