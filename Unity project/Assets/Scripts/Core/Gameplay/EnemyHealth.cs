using UnityEngine;
using System.Collections;

public class EnemyHealth : Health {

	public bool DestroyOnDeath = false;

	protected override void OnDeath(){
		if(DestroyOnDeath) {
			DestroyImmediate(gameObject);
			EnemyController.refreshEnemiesLeft(); // Call this after the Destroy.
		}
	}

}
