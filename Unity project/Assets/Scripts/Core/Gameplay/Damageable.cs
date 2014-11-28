using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour {

	public Transform HPManager;
	public float defense = 1.0f;
	
	private Health hp;
	public Health master { get { return hp; } }

	// Use this for initialization
	void Start () {
		hp = HPManager.GetComponent<Health> ();
	}
	
	public void Damage(float baseAmount){
		Debug.Log ("Damaged for: " + baseAmount/defense + " points");
		hp.Damage(baseAmount/defense);
	}

}
