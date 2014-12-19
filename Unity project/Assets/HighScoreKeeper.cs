﻿using UnityEngine;
using System;


public class HighScoreKeeper : MonoBehaviour {

	// public parameters
	public static int BaseWaveScore = 5;			//Base score granted on every wave
	public static int WaveScoreProgression = 1;		//Points Increasing per wave
	public static int WaveTimeScore = 10;			//Bonus for ending wave quickly
	
	public static int EnemyKillPoints = 1;			//Points per enemy Kill
	public static int HeadShotMultiplier = 2;		//Bonus per Headshot
	
	// Private variables
	private static int Score = 0;					//The HighScore
	
	// Wave parameters
	private static int TotalWave = 1;				//Amount of Waves
	private static float WaveTime = 0;				//Time passed in the current wave
	private static float MaxWaveTime;				//Criterium for the WaveTimeBonus
	
	// Block Stats
	private static int BlocksPlaced = 0;			// # of blocks placed
	private static int BlocksDestroyedPlayer = 0;	// # of blocks destroyed by the player
	private static int BlocksDestroyedEnemy = 0;	// # of blocks destroyed by the enemy
	
	// Firing Stats
	private static int ShotsHit = 0;				//Shots that hit an enemy
	private static int ShotsMissed = 0;				//Shots that miss an enemy
	private static float Accuracy;					//Ratio between shots hit and shots fired
	private static int AccuracyBonus;				//Bonus at end game for a high accuracy
	private static int AccuracyBonusParameter = 100;//Parameter for the accuracy bonus

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		WaveTime += Time.deltaTime;
	}
	
	// Return the higshcore
	static int getHighscore() {
		return Score;
	}
	
	// Block Stats - Called on block placement/Destruction
	public static void BlockAction(bool Placed, bool byPlayer) {
		if (Placed) {
			BlocksPlaced += 1;
		}
		
		if (Placed != true) {
			if (byPlayer) {
				BlocksDestroyedPlayer+= 1;
			}
			else {
				BlocksDestroyedEnemy += 1;
			}
		}
	}
	
	// Wave Progression - Called at end of wave
	static void PointsNextWave(int WaveNumber) {
	
		int Points = BaseWaveScore + WaveScoreProgression * WaveNumber;
		Score += Points;
		TotalWave += 1;
		
		MaxWaveTime = (WaveNumber * 5) + 30;
		
		if (WaveTime < MaxWaveTime) {
			Score += WaveTimeScore;
		}
		WaveTime = 0;
	}
	
	// Kill Points - Called at enemy Kill
	static void PointsOnKill(bool Headshot) {
		if (Headshot) {
			Score += EnemyKillPoints * HeadShotMultiplier;
		}
		else {
			Score += EnemyKillPoints;
		}
		Score += 1;
	}
	
	// Shot Fired accuracy - Called at FireWeapon
	static void ShotsFired(bool HitEnemy) {
		if (HitEnemy) {
			ShotsHit += 1;
		}
		else {
			ShotsMissed += 1;
		}
		Accuracy = ShotsHit / ((float)(ShotsHit + ShotsMissed));
	}
	
	// Accuracy Bonus
	static void BonusPoints() {
		
		//Accuracy Bonus
		if (Accuracy > 0.50f) {
			if (Accuracy > 0.70f) {
				if (Accuracy > 0.95) {
					AccuracyBonus = AccuracyBonusParameter;
				}
				else {
					AccuracyBonus = (int)Math.Round(0.7f * AccuracyBonusParameter);
				}
			}
			else {
				AccuracyBonus = (int)Math.Round(0.2f * AccuracyBonusParameter);
			}
		}
	}
	
}