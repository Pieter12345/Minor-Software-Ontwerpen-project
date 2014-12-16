// EnemyController script.
// This script spawns enemies and determines what they choose to do (AI).

using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// Variables & Constants.
	public Transform parentObject;
	private static Transform parent;

	public Transform flag;
	public static int[] flagPos = new int[] {10, 0, 10}; // x, y and z coordinates of the flag.

	private static GameObject[] enemyObjects;
	private static int enemyObjectSize = 0; // Array size of enemyObjects.
	public int maxAmountOfEnemies = 1000; // Max array size of enemyObjects.

	private static pathFinding pathToPlayer;
	private static pathFinding pathToFlag;
	private static pathFinding pathToPlayerIgnoringBlocks; // Used for block-breaking enemies.
	private static pathFinding pathToFlagIgnoringBlocks; // User for block-breaking enemies.


	private static int[] coords;
	private static GameObject enemy;

	private pathFinding pathFind;

	public GameObject player;
	public static GameObject playerStatic;
	private Vector3 playerCoordsOld = new Vector3(0f, 0f, 0f); // Used to check if the player has moved.

	public Object EnemyPrefab; // The prefab of the enemy.
	private static Object EnemyPrefabStatic;

	private static int wave = 0;
	private static int enemiesLeft = 0;
//	private int amountOfEnemiesOnThisWave = 0; // The initial spawned amount of enemies on the current wave.
//  private int enemiesKilledOnThisWave = 0;
	

	// Use this for initialization.
	void Start () {
		if (flag != null)
			flagPos = new int[] {
				Mathf.FloorToInt(flag.position.x), 
				Mathf.FloorToInt(flag.position.y), 
				Mathf.FloorToInt(flag.position.z)
			};

		EnemyPrefabStatic = EnemyPrefab; // Create static reference.
		playerStatic = player;

		parent = this.parentObject;
		enemyObjects = new GameObject[maxAmountOfEnemies];

		// Initialize pathFinding.
		Vector3 playerCoords = player.transform.position;
		pathToPlayer = new pathFinding((int) Mathf.Round(playerCoords.x), (int) Mathf.Round(playerCoords.y), (int) Mathf.Round(playerCoords.z));
		pathToFlag = new pathFinding(flagPos[0], flagPos[1], flagPos[2]); // <-- Initial goal coords.
		pathToPlayerIgnoringBlocks = new pathFinding((int) Mathf.Round(playerCoords.x), 0, (int) Mathf.Round(playerCoords.z), true);
		pathToFlagIgnoringBlocks = new pathFinding(flagPos[0], flagPos[1], flagPos[2], true); // <-- Initial goal coords.

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
			playerCoordsOld = player.transform.position;
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
	public static void spawnEnemy(int x, int y, int z) {

		// Create the capsule and store a reference to it.
//		enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Create a new capsule object.
		enemy = (GameObject) Instantiate(EnemyPrefabStatic); // Create a new enemy object.
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
		pathToPlayerIgnoringBlocks.updateGoalLocation((int) Mathf.Round(playerCoords.x), 0, (int) Mathf.Round(playerCoords.z));
	}

	// destroyAllEnemies method.
	// Removes all enemies from the game.
	public static void destroyAllEnemies() {

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

	// getPathToPlayerIgnoringBlocks method.
	// Returns the pathFinding object used to get to the player.
	public static pathFinding getPathToPlayerIgnoringBlocks() {
		return pathToPlayerIgnoringBlocks;
	}

	// getPathToFlag method.
	// Returns the pathFinding object used to get to the flag.
	public static pathFinding getPathToFlag() {
		return pathToFlag;
	}

	// getPathToFlagIgnoringBlocks method.
	// Returns the pathFinding object used to get to the player.
	public static pathFinding getPathToFlagIgnoringBlocks() {
		return pathToFlagIgnoringBlocks;
	}


	// ----------------------------------
	// WaveSystem.
	// ----------------------------------

	// startNextWave method.
	// Destroys all current enemies and spawns new enemies.
	public static void startNextWave() {
		destroyAllEnemies(); // Just to make sure the next wave starts with an empty list.
		wave++;
		float a = 0f;
		float b = 2f;
		int c = 3;
		int amountOfEnemiesOnThisWave = (int) (a * (wave * wave) + b * wave + c);
		enemiesLeft = amountOfEnemiesOnThisWave; // Initialize every wave.

		// Spawn the amountOfEnemiesOnThisWave enemies on random positions on the sides of the grid.
		for(int i=0; i < amountOfEnemiesOnThisWave; i++) {

			// Stick to x or z border (50% chance). Pick the other value randomly.
			int x;
			int z;
			if(Random.Range(0f, 1f) > 0.5) {
				x = (Random.Range(0f, 1f) > 0.5)? 1 : WorldBlockManagement.getLevelSize()-2;
				z = (int) (1f + Random.Range(0f, WorldBlockManagement.getLevelSize()-2));
			} else {
				z = (Random.Range(0f, 1f) > 0.5)? 1 : WorldBlockManagement.getLevelSize()-2;
				x = (int) (1f + Random.Range(0f, WorldBlockManagement.getLevelSize()-2));
			}

			// Spawn the enemy.
			spawnEnemy(x, WorldBlockManagement.getHighestBlockAt(x, z), z);
		}

	}
    
    // refreshEnemiesLeft method.
    // Updates the amount of enemies left. Call this when an enemy is destroyed.
    public static void refreshEnemiesLeft() {
        
        // Count the amount of enemies left.
        int enemiesLeft2 = 0;
        for(int i=0; i < enemyObjectSize; i++) {
            if(enemyObjects[i] != null) {
                enemiesLeft2++;
            }
        }
        
        // Spawn a new wave if there are no enemies left.
        if(enemiesLeft2 == 0) {
            startNextWave();
        }

		enemiesLeft = enemiesLeft2;
		Debug.Log("Enemies left: " + enemiesLeft2);
    }

	// FixedUpdate method.
	// Runs every fixed amount of time.
	void FixedUpdate() {
		pathToPlayer.UpdatePathFixed(); // Allows the pathfinding to calc the path in steps.
		pathToPlayerIgnoringBlocks.UpdatePathFixed(); // Allows the pathfinding to calc the path in steps.
		pathToFlag.UpdatePathFixed(); // Allows the pathfinding to calc the path in steps.
		pathToFlagIgnoringBlocks.UpdatePathFixed(); // Allows the pathfinding to calc the path in steps.

		// DEBUG - Spawns enemies when pressing F8.
		if(Input.GetKeyDown(KeyCode.F8)) {
			startNextWave();
		}
	}

	// updatePathFinding method.
	// Updates the pathfinding to the player and flag. This should be called when the scene changes (Block places/breaks).
	public static void updatePathFinding() {
		pathToPlayer.updatePathFinding();
		pathToFlag.updatePathFinding();
	}

	// getWave method.
	// Returns the current wave number.
	public static int getWave() {
		return wave;
	}

	// getEnemiesLeft method.
	// Returns the amount of enemies left on the current wave (and therefor in the scene).
	public static int getEnemiesLeft() {
		return enemiesLeft;
	}



}
