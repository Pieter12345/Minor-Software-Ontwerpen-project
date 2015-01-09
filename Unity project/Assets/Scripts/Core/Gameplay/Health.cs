using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float maxHP = 10;
	protected float hp;
	public float HP { get { return hp; } }
	public bool IsDead { get { return hp <= 0; } }
	private bool Headshot = false;

	void Awake(){
		hp = maxHP;
	}

	void Update(){
		if(IsDead) {
			OnDeath(Headshot);
		}
		OnRegen();
	}

	public void Damage(float amount, bool isHeadshot){
		hp -= amount;

		OnDamage();
		OnDamage(amount);
		
		Headshot = isHeadshot;
	}

	public void Heal(float amount){
		hp += amount;
		if(hp > maxHP)
			hp = maxHP;

		OnHeal();
		OnHeal(amount);
	}
	
	protected void EndGame() {
		HighScoreKeeper.LogHighscore();
		Screen.lockCursor = false;
		Application.LoadLevel("GameOver");
	}

	protected virtual void OnDeath(bool isHeadshot){
		Destroy(gameObject);
	}

	
	protected virtual void OnDamage(){}
	protected virtual void OnDamage(float amount){}

	protected virtual void OnHeal(){}
	protected virtual void OnHeal(float amount){}

	protected virtual void OnRegen(){}
}
