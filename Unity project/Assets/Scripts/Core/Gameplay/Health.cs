using UnityEngine;
using System.Collections;

public abstract class Health : MonoBehaviour {

	public float maxHP = 10;
	protected float hp;
	public float HP { get { return hp; } }
	public bool IsDead { get { return hp <= 0; } }

	void Awake(){
		hp = maxHP;
	}

	void Update(){
		if (IsDead)
			OnDeath();
	}

	public void Damage(float amount){
		OnDamaged();
		OnDamaged(amount);
		hp -= amount;
	}

	public void Heal(float amount){
		OnHeal();
		hp += amount;
		if(hp > maxHP)
			hp = maxHP;
	}

	protected virtual void OnDeath(){
		Destroy(gameObject);
	}
	
	protected virtual void OnDamaged(){}
	protected virtual void OnDamaged(float amount){}

	protected virtual void OnHeal(){}
}
