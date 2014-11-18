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
	public pathFinding(byte xStart, byte yStart, byte zStart, byte xGoal, byte yGoal, byte zGoal) {
		this.levelSize = WorldBlockManagement.getLevelSize();
		this.levelHeight = WorldBlockManagement.getLevelHeight();
		this.worldOverlay = new int[levelSize * levelSize * levelHeight];

	}

	// updateGoalLocation method.
	// If the player is the goal and the enemy wants to get to the goal: Call this when the player has moved, supplying new player coordinates.
	public void updateGoalLocation(byte xGoal, byte yGoal, byte zGoal) {
		this.goalCoords = new int[3];
		this.goalCoords[0] = xGoal;
		this.goalCoords[1] = yGoal;
		this.goalCoords[2] = zGoal;

		// Update the overlay.
		int[] worldOverLayOld = new int[worldOverlay.Length];
		for(int i=0; i < worldOverlay.Length; i++) {
			worldOverlay[i] = -1;
			worldOverLayOld[i] = worldOverlay[i];
		}
		worldOverlay[xGoal + levelSize*zGoal + levelSize*levelSize*yGoal] = 0; // Player position.
		int currentDistance = 0;
		bool hasChanged = true;
		while(hasChanged) {
			hasChanged = false;
			int worldOverlayIndex = 0;
			for(byte x=0; x < levelSize; x++) {
				for(byte z=0; z < levelSize; z++) {
					for(byte y=0; y < levelHeight; y++) {
						if(worldOverlay[worldOverlayIndex] == currentDistance) {
							hasChanged = true; // Not always true, might run one useless iteration over all x,y,z positions.

							// For all directions, set the overlayValue of where the entity can move.
// Covered below.			// All directions, no jumping/falling (y = constant).
//							if(WorldBlockManagement.canStandHere(x+1,y,z)) { setSingleWorldOverlay(x+1, y, z, currentDistance+1); }
//							if(WorldBlockManagement.canStandHere(x-1,y,z)) { setSingleWorldOverlay(x-1, y, z, currentDistance+1); }
//							if(WorldBlockManagement.canStandHere(x,y,z+1)) { setSingleWorldOverlay(x, y, z+1, currentDistance+1); }
//							if(WorldBlockManagement.canStandHere(x,y,z-1)) { setSingleWorldOverlay(x, y, z-1, currentDistance+1); }

							// All directions, jumping (jump required to get here).
							if(WorldBlockManagement.canStandHere(x+1,y-1,z) && WorldBlockManagement.canJumpAt(x+1,y-1,z)) { setSingleWorldOverlay(x+1, y-1, z, currentDistance+1); }
							if(WorldBlockManagement.canStandHere(x-1,y-1,z) && WorldBlockManagement.canJumpAt(x-1,y-1,z)) { setSingleWorldOverlay(x-1, y-1, z, currentDistance+1); }
							if(WorldBlockManagement.canStandHere(x,y-1,z+1) && WorldBlockManagement.canJumpAt(x,y-1,z+1)) { setSingleWorldOverlay(x, y-1, z+1, currentDistance+1); }
							if(WorldBlockManagement.canStandHere(x,y-1,z-1) && WorldBlockManagement.canJumpAt(x,y-1,z-1)) { setSingleWorldOverlay(x, y-1, z-1, currentDistance+1); }

							// All directions, y constant and falling (There can be more than one way to fall here per direction).
							for(int y2 = y; y2 <= WorldBlockManagement.getHighestBlockAt(x+1,z); y2++) {

								// Break if entities can not fall on the desired location anymore.
								if(!WorldBlockManagement.canWalkHere(x,y2,z)) {
									break;
								}
								setSingleWorldOverlay(x, y2, z, currentDistance); // Create a pilar of the same values in the air so players don't have to simulate falling later.

								// Set distance in worldOverlay if the player can fall here from there.
								if(WorldBlockManagement.canStandHere(x+1, y2, z)) { setSingleWorldOverlay(x+1, y2, z, currentDistance+1); }
								if(WorldBlockManagement.canStandHere(x-1, y2, z)) { setSingleWorldOverlay(x-1, y2, z, currentDistance+1); }
								if(WorldBlockManagement.canStandHere(x, y2, z+1)) { setSingleWorldOverlay(x, y2, z+1, currentDistance+1); }
								if(WorldBlockManagement.canStandHere(x, y2, z-1)) { setSingleWorldOverlay(x, y2, z-1, currentDistance+1); }
							}
						}
						worldOverlayIndex++;
					}
				}
			}
			currentDistance++;
		} // End of while(hasChanged).
	}

	// setSingleWorldOverlay method.
	// Used for updateGoalLocation method.
	private void setSingleWorldOverlay(int x, int y, int z, int distanceToSet) {
		int index = x + levelSize*z + levelSize*levelSize*y;
		if(worldOverlay[index] == -1) {
			worldOverlay[index] = distanceToSet;
		}
	}

	// getNextMove method.
	// x, y, z are the current coordinates of the entity that has to find a path.
	// Will return which way to move in. Format: 
	public string getNextMove(int x, int y, int z) {
		int currentOverlay = getSingleWorldOverlay(x, y, z);
		if(getSingleWorldOverlay(x+1, y, z) < currentOverlay) { return "x+"; }
		if(getSingleWorldOverlay(x-1, y, z) < currentOverlay) { return "x-"; }
		if(getSingleWorldOverlay(x, y, z+1) < currentOverlay) { return "z+"; }
		if(getSingleWorldOverlay(x, y, z-1) < currentOverlay) { return "z-"; }
		return "noPathFoundError";
	}

	// getSingleWorldOverlay method.
	// Used for getNextMove method. Returns the distance value at the given location.
	private int getSingleWorldOverlay(int x, int y, int z) {
		int index = x + levelSize*z + levelSize*levelSize*y;
		return worldOverlay[index];
	}


}
