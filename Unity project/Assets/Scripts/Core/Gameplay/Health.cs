using UnityEngine;
using System.Collections;
using System;

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
	
	// OnDeath Endgame Highscore Upload //consider moving to other c# file if referencing allows
	IEnumerator PostStats (string url) {
		var post = new WWWForm();
		post.AddField("PlayerID",1);
		post.AddField("Highscore",HighScoreKeeper.Score);
		post.AddField("ShotsFired",(HighScoreKeeper.ShotsHit+HighScoreKeeper.ShotsMissed));
		post.AddField("Accuracy", ((int) Math.Round (HighScoreKeeper.Accuracy)));
		post.AddField("BlocksPlaced",HighScoreKeeper.BlocksPlaced);
		post.AddField("BlocksDestroyed",(HighScoreKeeper.BlocksDestroyedPlayer+HighScoreKeeper.BlocksDestroyedEnemy));
		post.AddField("WaveHighscore",HighScoreKeeper.TotalWave);
		
		
		var get = new WWW(url,post);
		yield return get;
		
		if (get.error!=null) {
			Debug.Log(get.error);
		}
		else {
			Debug.Log(get.text);
		}
	}
	
	protected void EndGame() {
		StartCoroutine(PostStats("http://drproject.twi.tudelft.nl:8083/SQL"));
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
