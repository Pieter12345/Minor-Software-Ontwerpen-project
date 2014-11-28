// EnemyController script.
// This script spawns enemies and determines what they choose to do (AI).
// TODO - Script not finished.

using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// Variables & Constants.
	public Transform parentObject;
	private static Transform parent;

	private static GameObject[] enemyObjects;
	private static int enemyObjectSize = 0; // Array size of enemyObjects.
	public int maxAmountOfEnemies = 1000; // Max array size of enemyObjects.

	private static pathFinding pathToPlayer;
	private static pathFinding pathToFlag;


	private static int[] coords;
	private static GameObject enemy;

	private pathFinding pathFind;

	public GameObject player;
	private Vector3 playerCoordsOld = new Vector3(0f, 0f, 0f); // Used to check if the player has moved.

	public Object EnemyPrefab; // The prefab of the enemy.

	private int wave = 0;
	private int amountOfEnemiesOnThisWave = 0; // The initial spawned amount of enemies on the current wave.
	

	// Use this for initialization.
	void Start () {
		parent = this.parentObject;
		enemyObjects = new GameObject[maxAmountOfEnemies];

		// Initialize pathFinding.
		Vector3 playerCoords = player.transform.position;
		pathToPlayer = new pathFinding((int) Mathf.Round(playerCoords.x), (int) Mathf.Round(playerCoords.y), (int) Mathf.Round(playerCoords.z));
		pathToFlag = new pathFinding(0, 0, 0); // <-- Initial goal coords. TODO - Add a flag somewhere.

		// Start a new wave.
		startNextWave();

//		// Test code.
//		spawnEnemy((int) 4, (int) 0, (int) 4);

	}
	
	// Update is called once per frame.
//	float timeVar = 0;
//	float timeFromStart = 0;
	void Update () {

		// Update the pathfinding to the player if the player moves.
		if((playerCoordsOld - player.transform.position).sqrMagnitude > 0.5f*0.5f) {
			destroyAllEnemies = player.transform.position;
			this.updatePlayerPathfinding();
		}



//		timeFromStart += Time.deltaTime;
//		// Run this every 1 sec.
//		if(timeFromStart - timeVar > 1) {
//			timeVar = timeFromStart;
//
//
//			// Get player location here (just every second for now...)
//			Vector3 playerCoords = player.transform.position;
//			int Xplayer = (int) Mathf.Round(playerCoords[0]);
//			int Yplayer = (int) Mathf.Round(playerCoords[1]);
//			int Zplayer = (int) Mathf.Round(playerCoords[2]);
//
//			Debug.Log ("xp=" + Xplayer + " yp=" + Yplayer + " zp=" + Zplayer);
//
//			pathToPlayer.updateGoalLocation(Xplayer, Yplayer, Zplayer);
//
//		}
	}



	// spawnEnemy method.
	// Spawns an enemy.
	public void spawnEnemy(int x, int y, int z) {

		// Create the capsule and store a reference to it.
//		enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Create a new capsule object.
		enemy = (GameObject) Instantiate(EnemyPrefab); // Create a new enemy object.
		enemy.transform.position = new Vector3(x, y, z); // Put the enemy in position.
		enemy.transform.parent = parent; // Puts the enemy object in a tab in the hierarchy window.
//		enemy.renderer.material.mainTexture = texture;
//		enemy.renderer.material.shader = shader;
		enemyObjects[enemyObjectSize] = enemy;
		enemyObjectSize++;
	}

	// updatePlayerPathfinding method.
	// Updates the pathFinding to the player.
	public void updatePlayerPathfinding() {
		Vector3 playerCoords = player.transform.position;
		pathToPlayer.updateGoalLocation((int) Mathf.Round(playerCoords.x), (int) Mathf.Round(playerCoords.y), (int) Mathf.Round(playerCoords.z));
	}

	// destroyAllEnemies method.
	// Removes all enemies from the game.
	public void destroyAllEnemies() {

		// For all enemies.
		foreach(GameObject enemy in enemyObjects) {
			if(enemy != null) {
				GameObject.Destroy(enemy); // Destroy the enemy.
			}
		}

		// Clear the variables.
		for(int i=0; i < enemyObjectSize; i++) {
			enemyObjects[i] = null;
		}
		enemyObjectSize = 0;
	}

	// getPathToPlayer method.
	// Returns the pathFinding object used to get to the player.
	public static pathFinding getPathToPlayer() {
		return pathToPlayer;
	}

	// getPathToFlag method.
	// Returns the pathFinding object used to get to the flag.
	public static pathFinding getPathToFlag() {
		return pathToFlag;
	}


	// ----------------------------------
	// WaveSystem.
	// ----------------------------------

	// startNextWave method.
	// Destroys all current enemies and spawns new enemies.
	public void startNextWave() {
		destroyAllEnemies(); // Just to make sure the next wave starts with an empty list.
		wave++;
		float a = 0f;
		float b = 2f;
		int c = 3;
		amountOfEnemiesOnThisWave = (int) (a * (wave * wave) + b * wave + c);

		// Spawn the amountOfEnemiesOnThisWave enemies on random positions on the sides of the grid.
		for(int i=0; i < amountOfEnemiesOnThisWave; i++) {

			// Stick to x or z border (50% chance). Pick the other value randomly.
			int x;
			int z;
			if(Random.Range(0f, 1f) > 0.5) {
				x = (Random.Range(0f, 1f) > 0.5)? 0 : WorldBlockManagement.getLevelSize-1;
				z = Random.Range(0f, WorldBlockManagement.getLevelSize-1);
			} else {
				z = (Random.Range(0f, 1f) > 0.5)? 0 : WorldBlockManagement.getLevelSize-1;
				x = Random.Range(0f, WorldBlockManagement.getLevelSize-1);
			}

			// Spawn the enemy.
			spawnEnemy(x, WorldBlockManagement.getHighestBlockAt(x, z), z);
		}

	}





}
