using UnityEngine;
using System.Collections;

public class SimpleGun : Weapon {

	public float baseDamage = 1.0f;

	public override void Fire(Health hp){
		if(hp!= null)
			hp.Damage(baseDamage);
	}
}
