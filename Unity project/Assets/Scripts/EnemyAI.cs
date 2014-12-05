// EnemyAI script.
// This script will be instanciated per enemy when it spawns.
// All enemy behaviour should be handled by this.

using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	// Variables & Constants.
	public GameObject enemyObject; // The object that represents this enemy.

	private pathFinding pathToPlayer;
	private pathFinding pathToFlag;

	private string status = "ready";
	private Vector3 singleMoveGoalCoords; // Goal coords of each step.

	private float acceptableErrorDistance = 0.1f; // Distance from singleMoveGoal which is acceptable to stop at.
	private float speed = 5f/3.6f; // Speed in m/s.

	// Jumping variables.
	private float gravitation = 9.81f; // [m/(s*s)] Gravitation constant (average 9.81 on earth).
	private float ySpeed; // Up is positive.
	private float yInitialSpeed = 6f; // [m/s] This represents the jumping force.

	// Runs when this enemy spawns.
	void Start () {
		// Get pathFinding object references.
		pathToPlayer = EnemyController.getPathToPlayer();
		pathToFlag = EnemyController.getPathToFlag();

		// TODO - Load random enemy texture here.
		// TODO - Initialize any random selected AI variables here.
	}
	
	// Update is called once per frame.
	void Update () {

		// Get the current position.
		Vector3 pos = enemyObject.transform.position;

		// Select actions based on what the enemy is doing.
		// Status activation variables / state indicators. TODO - Calculate each of the values of the variables.
		bool canHitPlayer = false;
		bool canHitFlag = false;
		bool standingUnderPlayer = false;
		bool canSeePlayer = false;
		bool playerInRadius = false;


//		Debug.Log("DEBUG: status = " + status);
		switch(status) {

		// If ready for a new task.
		case "ready": {
			// TODO - Implement AI choices here.
			// If close to the player, hit him.
			// elseif close to the flag, hit it.
			// elseif standing under player, use some ability to break the block above the enemy to make the player fall.
			// elseif enemy can see the player OR enemy is within a radius of the player, set path and move to player (choose between blockbreaking or normal pathfinding?).
			//     if path to player could not be found, set ignoreBlockPath and move to player.
			// else set path and move to flag (chooce between blockbreaking or normal pathfinding?).
			//     if path to flag could not be found, set ignoreBlockPath and move to flag.
			// TODO - End of todo.



			// Find a new goal position to walk to (one square each time).
			singleMoveGoalCoords = pathToPlayer.getNextMoveCoords((int) Mathf.Round(pos[0]), (int) Mathf.Round(pos[1]), (int) Mathf.Round(pos[2]));
			bool isJumpingMove = (singleMoveGoalCoords[1] - (int) Mathf.Round(pos[1])) == 1; // True if the move from current to singleMoveGoal has to be achieved with a jump.
			if(isJumpingMove) {
//				Debug.Log("DEBUG: Jump detected!");
				ySpeed = yInitialSpeed;
			}
			status = "walking";
			break;
		}

		// If busy walking (has a goal).
		case "walking": {
			// Change status and return if the singleMoveGoal is (nearly) reached and the enemy is on a fixed height level.
//			if((singleMoveGoalCoords - pos).sqrMagnitude < acceptableErrorDistance * acceptableErrorDistance && pos[1] % 1 == 0) {
			if((Mathf.Pow(singleMoveGoalCoords[0]-pos[0], 2) + Mathf.Pow(singleMoveGoalCoords[2]-pos[2], 2)) < acceptableErrorDistance * acceptableErrorDistance && pos[1] % 1 == 0) {
				status = "ready";
				break;
			}

			// Calculate the x-z direction to move in (x-z speed will not depend on y).
			float dx = singleMoveGoalCoords[0] - pos[0];
			float dz = singleMoveGoalCoords[2] - pos[2];
			Vector3 dx0dzUnit = new Vector3(dx, 0f, dz);
			dx0dzUnit.Normalize(); // Normalize the vector for the x-z direction.

			// Determine the new y position.
			float yPosNew;
			// If y is an int value AND the enemy can stand here, set the acceleration and speed to 0 if they were negative.
			if(pos[1] % 1 == 0 && WorldBlockManagement.canStandHere(Mathf.RoundToInt(pos[0]), Mathf.RoundToInt(pos[1]), Mathf.RoundToInt(pos[2]))) {
				if(ySpeed < 0) { ySpeed = 0; }
			}

			// Update y physics.
			yPosNew = pos[1] + ySpeed * Time.deltaTime;
			ySpeed -= gravitation * Time.deltaTime;

			// Snap the new y position to a block if its very close to it if the enemy is falling.
			if(yPosNew % 1f < 0.2f && ySpeed <= 0 && WorldBlockManagement.canStandHere(Mathf.RoundToInt(pos[0]), Mathf.RoundToInt(pos[1]), Mathf.RoundToInt(pos[2]))) { yPosNew = Mathf.Round(yPosNew); }

			// Bugfix: Teleport enemies to the highest block at their x-z position if they fall out of the scene.
			if(yPosNew < -0.5f) { yPosNew = WorldBlockManagement.getHighestBlockAt(Mathf.RoundToInt(pos[0]), Mathf.RoundToInt(pos[2])); }

			// Move the enemy (y already depends on time).
//			Debug.Log("Enemy move called with dx/dy/dx: " + (((dx0dzUnit * speed) * Time.deltaTime) + new Vector3(0f, yPosNew-pos[1], 0f)));
			Vector3 x0z = new Vector3(pos[0], 0f, pos[2]);
			enemyObject.transform.position = x0z + ((dx0dzUnit * speed) * Time.deltaTime) + yPosNew * transform.up;

			break;
		}
		}

	}
	
}
