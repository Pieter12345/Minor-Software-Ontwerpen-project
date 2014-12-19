using UnityEngine;
using System.Collections;

public class EnemyHealth : Health {

	public bool DestroyOnDeath = true;

	protected override void OnDeath(bool isHeadshot){
	
		HighScoreKeeper.PointsOnKill(isHeadshot); //Adds Highscore Points

		if(DestroyOnDeath) {
			DestroyImmediate(gameObject);
			EnemyController.refreshEnemiesLeft(); // Call this after the DestroyImmediate.
		}
	}

}
