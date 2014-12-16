// EnemyAI script.
// This script will be instanciated per enemy when it spawns.
// All enemy behaviour should be handled by this.

using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	private Transform player;
	private Transform flag;

	// Variables & Constants.
	public GameObject enemyObject; // The object that represents this enemy.

	private string status = "ready";
	private Vector3 singleMoveGoalCoords; // Goal coords of each step.
	
	private float acceptableErrorDistance = 0.1f; // Distance from singleMoveGoal which is acceptable to stop at.
	private float speedMax = 10f/3.6f; // Speed in m/s.
	private float speedMin = 5f/3.6f; // Speed in m/s.
	private float speed;
	
	private float enemyRadius = 0.4f; // The supportive radius of the enemy. This is used to determine wether an enemy can stand at a position or not.

	// PathFinding variables.
	private pathFinding pathToPlayer;
	private pathFinding pathToFlag;
	private pathFinding pathToPlayerIgnoringBlocks;
	private pathFinding pathToFlagIgnoringBlocks;
	private pathFinding currentPathfinding; // The current active pathfinding.
	private float pathfindingLastChanged = Time.time;
	private float pathfindingChangeCooldown = 5f; // [sec]. The amount of time before the enemy can switch between the player and flag pathFindings.

	// Jumping variables.
	private float gravitation = 9.81f; // [m/(s*s)] Gravitation constant (average 9.81 on earth).
	private float ySpeed; // Up is positive.
	private float yInitialSpeed = 6f; // [m/s] This represents the jumping force.
	
	// Attacking player & flag variables.
	private float hittTimer = Time.time; // [sec] Time since start of this frame (updated later).
	public float hittCooldown = 0.5f; // [sec] The enemy will attack with this interval.
	public float damagePerHitt = 1f; // The damage dealt per hitt.

	// Block breaking variables.
	private bool isBreakingBlock = false;
	private float blockBreakTimer = Time.time; // [sec] Time since start of this frame (updated later).
	private float blockBreakCooldown = 2f; // [sec] The enemy can destroy one block in this amount of time.

	// Runs when this enemy spawns.
	void Start () {
		// Get pathFinding object references.
		pathToPlayer = EnemyController.getPathToPlayer();
		pathToFlag = EnemyController.getPathToFlag();
		pathToPlayerIgnoringBlocks = EnemyController.getPathToPlayerIgnoringBlocks();
		pathToFlagIgnoringBlocks = EnemyController.getPathToFlagIgnoringBlocks();

		// Initial pathFinding.
		currentPathfinding = pathToPlayer;

		player = GameObject.FindGameObjectWithTag("Player").transform;
		flag = GameObject.FindGameObjectWithTag("Flag").transform;

		speed = Random.Range(speedMin, speedMax);
		// TODO - Load random enemy texture here.
		// TODO - Initialize any random selected AI variables here.
	}
	
	// Update is called once per frame.
	void Update () {

		// Get the current position.
		Vector3 pos = enemyObject.transform.position;

		// Get the player & flag position.
		Vector3 playerPos = EnemyController.playerStatic.transform.position;
		Vector3 flagPos = new Vector3(EnemyController.flagPos[0], EnemyController.flagPos[1], EnemyController.flagPos[2]);

		// Get the player & flag health.
		// TODO - Add references to player flag and health here, just to send damage to them.

		// Select actions based on what the enemy is doing.
		// Status activation variables / state indicators. TODO - Calculate each of the values of the variables.
		bool canHitPlayer = (pos - playerPos).sqrMagnitude <= Mathf.Pow(1.5f, 2f); // True if enemy to player distance <= 1.5.
		bool canHitFlag = (pos - flagPos).sqrMagnitude <= Mathf.Pow(1.5f, 2f); // True if enemy to flag distance <= 1.5.
		bool standingUnderPlayer = (new Vector3(playerPos[0], 0f, playerPos[2]) - new Vector3(pos[0], 0f, pos[2])).sqrMagnitude <= Mathf.Pow(1f, 2f); // True if enemy to player in x-z plane distance <= 1.
		bool playerInRadius = (pos - playerPos).sqrMagnitude <= Mathf.Pow(15f, 2f); // True if enemy to player distance <= 15.

		// Send a raycast to determine wether the enemy can see the player.
		RaycastHit hitInfo;
		bool canSeePlayer = false;
		if(Physics.Raycast(pos, playerPos-pos + transform.up, out hitInfo, Mathf.Infinity)) { // Mathf.Infinity can be changed to the max distance checked by the raycast.
//			Debug.Log ("Enemy can see player: " + (hitInfo.collider == EnemyController.playerStatic.collider));
			canSeePlayer = hitInfo.collider == EnemyController.playerStatic.collider;
		}
		

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


			// Attack the player and flag if possible. Prefers player over flag if both in range.
			if(canHitPlayer || canHitFlag) {
				if(Time.time - hittTimer >= hittCooldown) {
					hittTimer = Time.time;
					if(canHitPlayer) {
						// TODO - Damage the player here + animation?
						Debug.Log("Damaging player!");
						(player.parent.GetComponent(typeof(Health)) as Health).Damage(damagePerHitt);
					} else {
						// TODO - Damage the flag here + animation?
						Debug.Log("Damaging flag!");
						(flag.GetComponent(typeof(Health)) as Health).Damage(damagePerHitt);
					}
					break;
				}
			}

			// Break block above enemy if standing under player.
			if(standingUnderPlayer && !isBreakingBlock) {
				isBreakingBlock = true;
				blockBreakTimer = Time.time;
				Debug.Log("Blockbreak above head started. Time.time = " + blockBreakTimer);
			}
			if(Time.time - blockBreakTimer >= blockBreakCooldown && isBreakingBlock) {

				int yAboveEnemy = Mathf.RoundToInt(pos[1]+1);
				do {
					yAboveEnemy++;
					int blockID = WorldBlockManagement.getBlockAt(Mathf.RoundToInt(pos[0]), yAboveEnemy, Mathf.RoundToInt(pos[2]));
					if(blockID != 0 && blockID != 255) {
						Debug.Log("Blockbreak above head finished. Removing block at: " + Mathf.RoundToInt(pos[0]) + " " +  yAboveEnemy + " " + Mathf.RoundToInt(pos[2]));
						// TODO - Add block break animation here.
						WorldBlockManagement.breakBlockAt(Mathf.RoundToInt(pos[0]), yAboveEnemy, Mathf.RoundToInt(pos[2]));
						break;
					}
				} while(yAboveEnemy < WorldBlockManagement.getLevelHeight());

				isBreakingBlock = false;
				blockBreakTimer = Time.time;
			}
			if(isBreakingBlock) { break; } // Dont continue moving while breaking a block.
			

			// If enemy can see player or is within a radius of the player, set path to player. If no path can be found, use the block-breaking pathFinding.
			// Else, set path to flag. If no path can be found, use the block-breaking pathFinding.
			if(canSeePlayer || playerInRadius) {
				if(!(currentPathfinding.Equals(pathToPlayer) || currentPathfinding.Equals(pathToPlayerIgnoringBlocks))) {
					if(mayChangePathfinding()) {
						currentPathfinding = pathToPlayer;
						if(currentPathfinding.getNextMoveText(Mathf.RoundToInt(pos[0]), Mathf.RoundToInt(pos[1]), Mathf.RoundToInt(pos[2])) == "noPathFoundError") { currentPathfinding = pathToPlayerIgnoringBlocks; }
					}
				} else {
					currentPathfinding = pathToPlayer;
					if(currentPathfinding.getNextMoveText(Mathf.RoundToInt(pos[0]), Mathf.RoundToInt(pos[1]), Mathf.RoundToInt(pos[2])) == "noPathFoundError") { currentPathfinding = pathToPlayerIgnoringBlocks; }
				}
			}
			else {
				if(!(currentPathfinding.Equals(pathToFlag) || currentPathfinding.Equals(pathToFlagIgnoringBlocks))) {
					if(mayChangePathfinding()) {
						currentPathfinding = pathToFlag;
						if(currentPathfinding.getNextMoveText(Mathf.RoundToInt(pos[0]), Mathf.RoundToInt(pos[1]), Mathf.RoundToInt(pos[2])) == "noPathFoundError") { currentPathfinding = pathToFlagIgnoringBlocks; }
					}
				} else {
					currentPathfinding = pathToFlag;
					if(currentPathfinding.getNextMoveText(Mathf.RoundToInt(pos[0]), Mathf.RoundToInt(pos[1]), Mathf.RoundToInt(pos[2])) == "noPathFoundError") { currentPathfinding = pathToFlagIgnoringBlocks; }
				}
			}

			// Find a new goal position to walk to (one square each time).
			singleMoveGoalCoords = currentPathfinding.getNextMoveCoords(Mathf.RoundToInt(pos[0]), Mathf.RoundToInt(pos[1]), Mathf.RoundToInt(pos[2]));
			bool isJumpingMove = (singleMoveGoalCoords[1] - Mathf.RoundToInt(pos[1])) == 1; // True if the move from current to singleMoveGoal has to be achieved with a jump.
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

			// Check if a block has to be broken to move to the desired position.
			if(WorldBlockManagement.getBlockAt((int) singleMoveGoalCoords[0], (int) singleMoveGoalCoords[1], (int) singleMoveGoalCoords[2]) != 0) {
				if(!isBreakingBlock) {
					isBreakingBlock = true;
					blockBreakTimer = Time.time;
					Debug.Log("Blockbreak started. Time.time = " + blockBreakTimer);
				}
				else if(Time.time - blockBreakTimer >= blockBreakCooldown) {
					Debug.Log("Blockbreak finished. Removing block.");
					// TODO - Add block break animation here.
					WorldBlockManagement.breakBlockAt((int) singleMoveGoalCoords[0], (int) singleMoveGoalCoords[1], (int) singleMoveGoalCoords[2]);
					isBreakingBlock = false;
					blockBreakTimer = Time.time;
				}
			}
			else if(WorldBlockManagement.getBlockAt((int) singleMoveGoalCoords[0], (int) singleMoveGoalCoords[1]+1, (int) singleMoveGoalCoords[2]) != 0) {
				if(!isBreakingBlock) {
					isBreakingBlock = true;
					blockBreakTimer = Time.time;
					Debug.Log("Blockbreak started. Time.time = " + blockBreakTimer);
				}
				else if(Time.time - blockBreakTimer >= blockBreakCooldown) {
					Debug.Log("Blockbreak finished. Removing block.");
					// TODO - Add block break animation here.
					WorldBlockManagement.breakBlockAt((int) singleMoveGoalCoords[0], (int) singleMoveGoalCoords[1]+1, (int) singleMoveGoalCoords[2]);
					isBreakingBlock = false;
					blockBreakTimer = Time.time;
				}
			}
			if(isBreakingBlock) { break; } // Dont continue moving while breaking a block.

			// Calculate the x-z direction to move in (x-z speed will not depend on y).
			float dx = singleMoveGoalCoords[0] - pos[0];
			float dz = singleMoveGoalCoords[2] - pos[2];
			Vector3 dx0dzUnit = new Vector3(dx, 0f, dz);
			dx0dzUnit.Normalize(); // Normalize the vector for the x-z direction.
			this.turnTo(dx0dzUnit, Time.deltaTime); // Turn to the goal direction.

			// Determine the new y position.
			float yPosNew;
			// If y is an int value AND the enemy can stand here, set the acceleration and speed to 0 if they were negative.
			if(pos[1] % 1f == 0f && WorldBlockManagement.isSupportedAt(pos, enemyRadius)) {
				if(ySpeed < 0f) { ySpeed = 0f; }
			}

			// Update y physics.
			yPosNew = pos[1] + ySpeed * Time.deltaTime;
			ySpeed -= gravitation * Time.deltaTime;

			// Snap the new y position to a block if its very close to it if the enemy is falling.
			if(yPosNew % 1f < 0.2f && ySpeed <= 0 && WorldBlockManagement.isSupportedAt(new Vector3(pos.x, Mathf.Round(pos.y), pos.z), enemyRadius)) { yPosNew = Mathf.Round(yPosNew); }

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

	// turnTo method.
	// (Slowly) turns this enemy to a given direction. This method should be called in an update loop to reach the desired rotation.
	private void turnTo(Vector3 directionVector, float deltaTime) {
		Transform enemyModel = enemyObject.transform.FindChild("EnemyModel").transform;
		Vector3 stepTowardsDesiredDirection = Vector3.MoveTowards(enemyModel.forward, directionVector, 1f * deltaTime); // 1f is the rotation speed.
		enemyModel.LookAt(stepTowardsDesiredDirection + enemyModel.position);

	}

	// mayChangePathfinding method.
	// Returns true if the pathfinding has not changed for more than pathfindingChangeCooldown seconds.
	private bool mayChangePathfinding() {
		if(Time.time - pathfindingLastChanged >= pathfindingChangeCooldown) {
			pathfindingLastChanged = Time.time;
			return true;
		}
		return false;
	}
}