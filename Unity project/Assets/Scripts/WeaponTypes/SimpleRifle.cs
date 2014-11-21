using UnityEngine;
using System.Collections;

public class SimpleRifle : Weapon {

	public float baseDamage = 1.0f;
	
	public override int ClipSize{ get { return 12; } }
	
	public override AmmoTypes AmmoType { get 
		{
			return AmmoTypes.RIFLE_BULLET;
		} 
	}
	
	public override void Fire(Health hp){
		if(hp!= null)
			hp.Damage(baseDamage);
	}

}
