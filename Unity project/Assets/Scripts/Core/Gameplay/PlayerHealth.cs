using UnityEngine;
using System.Collections;

public class PlayerHealth : Health {

	public bool playerCanRegen = true;
	[Range(0.0f, 1.0f)]
	public float regeneratingFraction = 0.2f;
	public float regenSpeed = 1.0f;
	public float regenCooldown = 10f;

	private float regenTo = 0f;
	private float timeLastHit;	

	

	protected override void OnDeath(){
		Debug.Log("YOU JUST DIED MADAFAKA!!!");
		Debug.Break();
	}

	protected override void OnDamage(float amount){
		regenTo = (hp/maxHP + regeneratingFraction > 1f) ? hp + regeneratingFraction*maxHP : maxHP;
		timeLastHit = Time.time;
	}

	protected override void OnDamage(){
		regenTo = (hp/maxHP + regeneratingFraction > 1f) ? hp + regeneratingFraction*maxHP : maxHP;
		timeLastHit = Time.time;
	}

	protected override void OnRegen(){
		if(playerCanRegen){
			if(Time.time - timeLastHit > regenCooldown)
				Mathf.Lerp(hp, regenTo, regenSpeed*Time.deltaTime);
		}
	}

}
