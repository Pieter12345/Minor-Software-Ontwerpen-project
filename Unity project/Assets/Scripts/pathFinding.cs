// pathFinding class.
// Used for pathFinding between 2 x,y,z coordinates in the loaded level.

using UnityEngine;

public class pathFinding {

	// Variables & Constants.
	private byte levelSize;
	private byte levelHeight;
	private int[] worldOverlay;
	private int[] goalCoords; // Format: {x,y,z}

	// Constructor.
	public pathFinding(byte xGoal, byte yGoal, byte zGoal) {
		this.levelSize = WorldBlockManagement.getLevelSize();
		this.levelHeight = WorldBlockManagement.getLevelHeight();
		this.worldOverlay = new int[levelSize * levelSize * levelHeight];
	
		updateGoalLocation(xGoal, yGoal, zGoal);

	}

	// updateGoalLocation method.
	// If the player is the goal and the enemy wants to get to the goal: Call this when the player has moved, supplying new player coordinates.
	public void updateGoalLocation(int xGoal, int yGoal, int zGoal) {
		this.goalCoords = new int[3];
		this.goalCoords[0] = xGoal;
		this.goalCoords[1] = yGoal;
		this.goalCoords[2] = zGoal;

		// Update the overlay.
		// Initialize the overlay to int.MaxValue.
		for(int i=0; i < worldOverlay.Length; i++) {
			worldOverlay[i] = int.MaxValue;
		}

		// Return if the goal is outOfBounds.
		if(xGoal < 0 || yGoal < 0 || zGoal < 0 || xGoal >= levelSize || yGoal > levelHeight || zGoal > levelSize) {
			return;
		}

		worldOverlay[xGoal + levelSize*zGoal + levelSize*levelSize*yGoal] = 0; // Player position.

		int currentDistance = 0;
		bool hasChanged = true;
		while(hasChanged) {
			hasChanged = false;
			int worldOverlayIndex = 0;
			for(int y=0; y < levelHeight; y++) { // TODO - Use heightmap max value to stop iterating over all blocks above the max block+1?
				for(int z=0; z < levelSize; z++) {
					for(int x=0; x < levelSize; x++) {

						if(worldOverlay[worldOverlayIndex] == currentDistance) {
//							Debug.Log("CurrDistance: " + currentDistance);
							hasChanged = true; // Not always true, might run one useless iteration over all x,y,z positions.

							// For all directions, set the overlayValue of where the entity can move.
// Covered below.			// All directions, no jumping/falling (y = constant).
//							if(WorldBlockManagement.canStandHere(x+1, y, z  )) { setSingleWorldOverlay(x+1, y, z  , currentDistance+1); }
//							if(WorldBlockManagement.canStandHere(x-1, y, z  )) { setSingleWorldOverlay(x-1, y, z  , currentDistance+1); }
//							if(WorldBlockManagement.canStandHere(x  , y, z+1)) { setSingleWorldOverlay(x  , y, z+1, currentDistance+1); }
//							if(WorldBlockManagement.canStandHere(x  , y, z-1)) { setSingleWorldOverlay(x  , y, z-1, currentDistance+1); }

							// All directions, jumping (jump required to get here).
							if(WorldBlockManagement.canStandHere(x+1, y-1, z  ) && WorldBlockManagement.canJumpAt(x+1, y-1, z  )) { setSingleWorldOverlay(x+1, y-1, z  , currentDistance+1); }
							if(WorldBlockManagement.canStandHere(x-1, y-1, z  ) && WorldBlockManagement.canJumpAt(x-1, y-1, z  )) { setSingleWorldOverlay(x-1, y-1, z  , currentDistance+1); }
							if(WorldBlockManagement.canStandHere(x  , y-1, z+1) && WorldBlockManagement.canJumpAt(x  , y-1, z+1)) { setSingleWorldOverlay(x  , y-1, z+1, currentDistance+1); }
							if(WorldBlockManagement.canStandHere(x  , y-1, z-1) && WorldBlockManagement.canJumpAt(x  , y-1, z-1)) { setSingleWorldOverlay(x  , y-1, z-1, currentDistance+1); }
///*
							// All directions, y constant and falling (There can be more than one way to fall here per direction).
							int[] xValues = {1, -1, 0,  0};
							int[] zValues = {0,  0, 1, -1};
							for(byte i=0; i < 4; i++) {
								int x2 = xValues[i];
								int z2 = zValues[i];
									
								for(int y2 = y; y2 <= WorldBlockManagement.getHighestBlockAt(x+x2,z+z2)+1; y2++) { // For current y to the highest y an enemy can stand at.
									
									// Break if entities can not fall on the desired location anymore.
									if(!WorldBlockManagement.canWalkHere(x,y2,z)) { break; }
									
									setSingleWorldOverlay(x, y2, z, currentDistance); // Create a pilar of the same values in the air so players don't have to simulate falling later.
									
									// Set distance in worldOverlay if the player can fall here from there.
									if(WorldBlockManagement.canStandHere(x+x2, y2, z+z2)) { setSingleWorldOverlay(x+x2, y2, z+z2, currentDistance+1); }
								}

								
							}
//*/


						}
						worldOverlayIndex++;
					}
				}
			}
			currentDistance++;
		} // End of while(hasChanged).


//		// DEBUG CODE -> WRITE TO FILE.
//		string str = "";
//		int index = 0;
//		for(int y=0; y < levelHeight; y++) {
//			for(int z=levelSize-1; z >= 0 ; z--) {
//				for(int x=0; x < levelSize; x++) {
//					int nextInt;
//					if(worldOverlay[index] == int.MaxValue) { nextInt = -1; } else { nextInt = worldOverlay[index]; }
//					str += nextInt + "\t";
//					index++;
//				}
//				str += ";\r\n";
//			}
//			str += "\r\n\r\n";
//		}
//		System.IO.File.WriteAllText("debugfile.txt", str);



	}

	// setSingleWorldOverlay method.
	// Used for updateGoalLocation method.
	private void setSingleWorldOverlay(int x, int y, int z, int distanceToSet) {
		int index = x + levelSize*z + levelSize*levelSize*y;
		if(worldOverlay[index] == int.MaxValue) {
			worldOverlay[index] = distanceToSet;
//			Debug.Log("setSingleWorldOverlay: x=" + x + " y=" + y + " z=" + z);
//			WorldBlockManagement.setBlockAt(x,y+2,z,2); // DEBUG line.
		}
	}

	// getNextMove method.
	// x, y, z are the current coordinates of the entity that has to find a path.
	// Will return which way to move in. Format: 
	public string getNextMove(int x, int y, int z) {
		int currentOverlay = getSingleWorldOverlay(x, y, z);
//		Debug.Log (currentOverlay);

		for(int y2 = y; y2 <= y+1; y2++) {
			// Diagonal moves.
			if(getSingleWorldOverlay(x+1, y2, z+1) < currentOverlay && (getSingleWorldOverlay(x, y2, z+1) != int.MaxValue || getSingleWorldOverlay(x+1, y2, z) != int.MaxValue)) { return "x+z+"; }
			if(getSingleWorldOverlay(x-1, y2, z+1) < currentOverlay && (getSingleWorldOverlay(x, y2, z+1) != int.MaxValue || getSingleWorldOverlay(x-1, y2, z) != int.MaxValue)) { return "x-z+"; }
			if(getSingleWorldOverlay(x+1, y2, z-1) < currentOverlay && (getSingleWorldOverlay(x, y2, z-1) != int.MaxValue || getSingleWorldOverlay(x+1, y2, z) != int.MaxValue)) { return "x+z-"; }
			if(getSingleWorldOverlay(x-1, y2, z-1) < currentOverlay && (getSingleWorldOverlay(x, y2, z-1) != int.MaxValue || getSingleWorldOverlay(x-1, y2, z) != int.MaxValue)) { return "x-z-"; }
		}
		for(int y2 = y; y2 <= y+1; y2++) {
			// Left, right, up and down at y (includes falling) and y+1 (jumping).
			if(getSingleWorldOverlay(x+1, y2, z) < currentOverlay) { return "x+"; }
			if(getSingleWorldOverlay(x-1, y2, z) < currentOverlay) { return "x-"; }
			if(getSingleWorldOverlay(x, y2, z+1) < currentOverlay) { return "z+"; }
			if(getSingleWorldOverlay(x, y2, z-1) < currentOverlay) { return "z-"; }
		}

		if(x == this.goalCoords[0] && y == this.goalCoords[1] && z == this.goalCoords[2]) {
			Debug.Log("At goal pos: x=" + x + " y=" + y + " z=" + z);
			return "AlreadyAtGoalPosition";
		}

		return "noPathFoundError";
	}

	// getSingleWorldOverlay method.
	// Used for getNextMove method. Returns the distance value at the given location.
	private int getSingleWorldOverlay(int x, int y, int z) {
		int index = x + levelSize*z + levelSize*levelSize*y;
		if(index >= worldOverlay.Length || x < 0 || y < 0 || z < 0 || x > levelSize || y > levelHeight || z > levelSize) {
			return int.MaxValue;
		}
		return worldOverlay[index];
	}

//	// wait method. - CRASHES UNITY.
//	// Sleeps for the given amount of seconds.
//	private void wait(int sec) {
//		float currTime = Time.timeSinceLevelLoad;
//		while(true) {
//			if(Time.timeSinceLevelLoad - currTime > sec) {
//				return;
//			}
//		}
//	}


}
