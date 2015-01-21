using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour {

	public float maxHP = 10;
	protected float hp;
	public float HP { get { return hp; } }
	public bool IsDead { get { return hp <= 0; } }
	private bool Headshot = false;
	private bool CoRoutineRunning = false;

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
		if(hp < 0f) { hp = 0f; }
		if(hp > maxHP) { hp = maxHP; }

		OnDamage();
		OnDamage(amount);
		
		Headshot = isHeadshot;
	}

	public void Heal(float amount){
		hp += amount;
		if(hp > maxHP) { hp = maxHP; }

		OnHeal();
		OnHeal(amount);
	}
	
	// OnDeath Endgame Highscore Upload //consider moving to other c# file if referencing allows
	IEnumerator PostStats (string url) {
	
		CoRoutineRunning = true;
	
		var post = new WWWForm();
		post.AddField("PlayerID",CurrentUser.CurrentUserID);
		HighScoreKeeper.BonusPoints();
		post.AddField("Highscore",HighScoreKeeper.Score+HighScoreKeeper.AccuracyBonus);
		post.AddField("ShotsFired",(HighScoreKeeper.ShotsHit+HighScoreKeeper.ShotsMissed));
		post.AddField("Accuracy", ((int) Math.Round (100*HighScoreKeeper.Accuracy)));
		post.AddField("Headshots",HighScoreKeeper.HeadshotsTotal);
		post.AddField("BlocksPlaced",HighScoreKeeper.BlocksPlaced);
		post.AddField("BlocksDestroyed",(HighScoreKeeper.BlocksDestroyedPlayer+HighScoreKeeper.BlocksDestroyedEnemy));
		post.AddField("WaveHighscore",HighScoreKeeper.TotalWave);
		
		Debug.Log("posting");
		
		var get = new WWW(url,post);
		yield return get;
		
		if (get.error!=null) {
			Debug.Log(get.error);
		}
		else {
			Debug.Log(get.text);
		}
		CoRoutineRunning = false;
		Application.LoadLevel("GameOver");
	}
	
	protected void EndGame() {
		if (CoRoutineRunning!=true) {
			StartCoroutine(PostStats("http://drproject.twi.tudelft.nl:8083/SQL"));
		}	
		Screen.lockCursor = false;
		
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
