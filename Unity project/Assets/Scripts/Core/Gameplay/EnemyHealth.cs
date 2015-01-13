using UnityEngine;
using System.Collections;

public class EnemyHealth : Health {

	public bool DestroyOnDeath = true;

	protected override void OnDeath(bool isHeadshot){
	
		HighScoreKeeper.PointsOnKill(isHeadshot); //Adds Highscore Points
		FireController fc = GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponent<FireController> ();
		if(fc!=null)
			fc.AmountOfBlocks++;
		if(DestroyOnDeath) {
			DestroyImmediate(gameObject);
			EnemyController.refreshEnemiesLeft(); // Call this after the DestroyImmediate.
		}
	}

}
