using UnityEngine;
using System.Collections;

public class PlayerHealth : Health {

	public bool playerCanRegen = true;
	[Range(0.0f, 1.0f)]
	public float regeneratingFraction = 0.2f;
	public float regenSpeed = 1.0f;
	public float regenCooldown = 2f;
	public GameObject hitOverlay;
	public GameObject soundSource;

	private AudioSource audioS;
	private HitOverlay ho;
	private float regenTo = 0f;
	private float timeLastHit;	
	private float epsilon = 0.1f;

	void Start(){
		regenTo = maxHP;
		if(soundSource!=null)
			audioS = soundSource.GetComponent<AudioSource>();
		if(hitOverlay != null)
			ho = hitOverlay.GetComponent(typeof(HitOverlay)) as HitOverlay;
	}

	protected override void OnDeath(bool isHeadshot){
		Debug.Log("You have died, my friend.");
		EndGame();
	}

	protected override void OnDamage(float amount){
		regenTo = (hp/maxHP + regeneratingFraction < 1f) ? (hp + regeneratingFraction*maxHP) : maxHP;
		timeLastHit = Time.time;
		if(ho!=null)
			ho.Hit(amount);
	}

	protected override void OnDamage(){
		regenTo = (hp/maxHP + regeneratingFraction < 1f) ? (hp + regeneratingFraction*maxHP) : maxHP;
		timeLastHit = Time.time;
		if(audioS != null)
			audioS.PlayOneShot(audioS.clip);
	}

	protected override void OnHeal(float amount){
		regenTo = ((hp+amount)/maxHP + regeneratingFraction < 1f) ? (hp + amount + regeneratingFraction*maxHP) : maxHP;
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
