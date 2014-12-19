using UnityEngine;
using System.Collections;

public class PlayerHealth : Health {

	public bool playerCanRegen = true;
	[Range(0.0f, 1.0f)]
	public float regeneratingFraction = 0.2f;
	public float regenSpeed = 1.0f;
	public float regenCooldown = 2f;

	private float regenTo = 0f;
	private float timeLastHit;	
	private float epsilon = 0.1f;

	void Start(){
		regenTo = maxHP;
	}

	protected override void OnDeath(bool isHeadshot){
		Debug.Log("YOU JUST DIED MADAFAKA!!!");
		Screen.lockCursor = false;
		Application.LoadLevel("GameOver");
	}

	protected override void OnDamage(float amount){
		regenTo = (hp/maxHP + regeneratingFraction < 1f) ? (hp + regeneratingFraction*maxHP) : maxHP;
		timeLastHit = Time.time;
	}

	protected override void OnDamage(){
		regenTo = (hp/maxHP + regeneratingFraction < 1f) ? (hp + regeneratingFraction*maxHP) : maxHP;
		timeLastHit = Time.time;
	}

	protected override void OnRegen(){
		if(playerCanRegen){
			if(Mathf.Abs(hp-regenTo) < epsilon)
				hp = regenTo;
			if(Time.time - timeLastHit > regenCooldown)
				hp = Mathf.Lerp(hp, regenTo, regenSpeed*Time.deltaTime);
		}
	}

}
