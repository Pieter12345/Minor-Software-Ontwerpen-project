using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float InitialHP = 10;
	public bool DestroyOnDeath = false;
	private float hp;
	public float HP{ get {return hp;} }

	void Awake(){
		hp = InitialHP;
	}

	void Update(){
		if (hp <= 0){
			if(DestroyOnDeath)
				Destroy(gameObject);
		}
	}

	public void Damage(float amount){
		hp -= amount;
	}

	public void Heal(float amount){
		Damage(-amount);
	}
}
