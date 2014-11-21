// EnemyController script.
// This script spawns enemies and determines what they choose to do (AI).
// TODO - Script not finished.

using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// Variables & Constants.
	public Transform parentObject;
	private static Transform parent;

	private GameObject[] enemyObjects;
	private int enemyObjectSize = 0;
	public int maxAmountOfEnemies = 1000;
	private static int[] coords;
	private static GameObject enemy;

	private pathFinding pathFind;

	public GameObject player;

	// Use this for initialization.
	void Start () {
		parent = this.parentObject;
		enemyObjects = new GameObject[maxAmountOfEnemies];


		// Test code.
		coords = new int[3];
		coords[0] = 4;
		coords[1] = 0;
		coords[2] = 4;
		spawnEnemy(coords[0],coords[1],coords[2]);
		pathFind = new pathFinding(8, 0, 8); // <-- Goal coords.
		WorldBlockManagement.setBlockAt(8,3,8,3);
		WorldBlockManagement.setBlockAt(coords[0],coords[1]+3,coords[2],3);

	}
	
	// Update is called once per frame.
	float timeVar = 0;
	float timeFromStart = 0;
	void Update () {
		timeFromStart += Time.deltaTime;
		// Run this every 1 sec.
		if(timeFromStart - timeVar > 1) {
			timeVar = timeFromStart;


			// Get player location here (just every second for now...)
			Vector3 playerCoords = player.transform.position;
			int Xplayer = (int) Mathf.Round(playerCoords[0]);
			int Yplayer = (int) Mathf.Round(playerCoords[1]);
			int Zplayer = (int) Mathf.Round(playerCoords[2]);

			Debug.Log ("xp=" + Xplayer + " yp=" + Yplayer + " zp=" + Zplayer);

			pathFind.updateGoalLocation(Xplayer, Yplayer, Zplayer);

			// Stop if the thing is close to its goal.
			if(Mathf.Sqrt(Mathf.Pow(Xplayer - coords[0], 2) + Mathf.Pow(Yplayer - coords[1], 2) + Mathf.Pow(Zplayer - coords[2], 2)) <= 1) {
				Debug.Log("Mr Capsule is very close to his goal (1 meter), not moving.");
				return;
			}

//			Debug.Log("coords[0] = " + coords[0]);
//			Debug.Log("coords[1] = " + coords[1]);
//			Debug.Log("coords[2] = " + coords[2]);
			string nextMove = pathFind.getNextMove(coords[0], coords[1], coords[2]); // <-- Current pos.
			Debug.Log(nextMove);

//			Vector3 pos1 = enemy.transform.position;
//			Debug.Log("pos[0] = " + pos1[0]);
//			Debug.Log("pos[1] = " + pos1[1]);
//			Debug.Log("pos[2] = " + pos1[2]);

			// Perform the move.
			switch(nextMove) {
			case "x+": { enemy.transform.Translate(1f,0f,0f); break; }
			case "x-": { enemy.transform.Translate(-1f,0f,0f); break; }
			case "z+": { enemy.transform.Translate(0f,0f,1f); break; }
			case "z-": { enemy.transform.Translate(0f,0f,-1f); break; }

			case "x+z+": { enemy.transform.Translate(1f,0f,1f); break; }
			case "x-z+": { enemy.transform.Translate(-1f,0f,1f); break; }
			case "x+z-": { enemy.transform.Translate(1f,0f,-1f); break; }
			case "x-z-": { enemy.transform.Translate(-1f,0f,-1f); break; }
			case "AlreadyAtGoalPosition": { break; }
			default: { Debug.Log("Unknown direction returned."); break; }
			}

			// Save the new grid position.
			Vector3 pos = enemy.transform.position;
			coords[0] = (int) Mathf.Round(pos[0]-0.5f);
			coords[1] = (int) Mathf.Round(pos[1]-1f);
			coords[2] = (int) Mathf.Round(pos[2]-0.5f);

			// Go one up if the enemy is standing in a block (To simulate a jump).
			if(!WorldBlockManagement.canStandHere(coords[0], coords[1], coords[2])) {
				if(WorldBlockManagement.canStandHere(coords[0], coords[1]+1, coords[2])) {
					coords[1] += 1;
					enemy.transform.Translate(0f, 1f, 0f);
				}
				else if(WorldBlockManagement.canStandHere(coords[0], coords[1]-1, coords[2])) {
					coords[1] -= 1;
					enemy.transform.Translate(0f, -1f, 0f);
				}
				else if(WorldBlockManagement.canStandHere(coords[0], coords[1]-2, coords[2])) {
					coords[1] -= 2;
					enemy.transform.Translate(0f, -2f, 0f);
				}
			}

		}
	}



	// spawnEnemy method.
	// Spawns an enemy.
	public void spawnEnemy(int x, int y, int z) {

		// Create the capsule and store a reference to it.
		enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Create a new capsule object.
		//	GameObject enemy = (GameObject) Instantiate(PREFAB-HERE); // Create a new enemy object.
		enemy.transform.position = new Vector3(x+0.5f, y+1f, z+0.5f); // Put the enemy in position.
		enemy.transform.parent = parent; // Puts the enemy object in a tab in the hierarchy window.
//		enemy.renderer.material.mainTexture = texture;
//		enemy.renderer.material.shader = shader;
		enemyObjects[enemyObjectSize] = enemy;
		enemyObjectSize++;
	}
}
