// Class for block management in the world.
// For loading, a binary file should be in format: {levelSize, levelHeight, levelSize * levelSize * levelHeight blockData}.

using UnityEngine;
using System.Collections;
using System.IO;

public class WorldBlockManagement : MonoBehaviour {

	// Variables & Constants.
	private static int levelSize; // Length and width of the buildable floor plane.
	private static int levelHeight; // Height in which can be built.
	private static byte[] blockData; // The array of blockData.
	private static GameObject[] blockObjects; // Same size as blockData, contains the block objects.
	private static byte[] heightMap; // Heights for every (x,z).
	private static bool[] canWalkThrough; // Same size as blockData, tells if the player can walk in this block.
	private static bool[] canWalkThroughOnlyId255; // Only checks for blockID 255. This is the blockifyWorld invisible block.

	public Transform parentObject; // The (empty) parent GameObject to create new blocks in.
	private static Transform parent; // Block objects will be created as childs of this GameObject. (Default: World/Blocks)

	// Runs on creation of the object its bound to (should be the level floor plane or so).
	void Awake () {

		// Load the default level to variables.
		generateNewLevel(100, 30);
//		loadLevelFromFile("32x32x16"); // Available empty testlevels: testLevel, 100x100x20.

		// Make the parentObject static.
		parent = this.parentObject;

		// Create a floor plane with the size of the loaded level.
		createGroundPlane();

		// Blockify all already-in-scene objects with invisible blocks.
		blockifyWorld();

		// Load the level to the scene (Does not clear an old loaded level).
		int byteArrayIndex = 0;
		for(int y=0; y < levelHeight; y++) {
			for(int z=0; z < levelSize; z++) {
				for(int x=0; x < levelSize; x++) {

					// If a block should be placed at the current position.
					if(blockData[byteArrayIndex] != 0) {
						setBlockAt(x, y, z, blockData[byteArrayIndex]);
					}
					byteArrayIndex++;
				}
			}
		}

		// Set some blocks to test the script.
		Debug.Log("[INFO]: [WorldBlockManagement] Test blocks are being placed.");
//		setBlockAt(1,0,0,1);
//		setBlockAt(1,0,1,1);
//		setBlockAt(0,0,1,1);
//		setBlockAt(0,0,0,1);
//		setBlockAt(1,1,1,2);
//		setBlockAt(2,2,2,3);
//		setBlockAt(3,3,3,4);
//		setBlockAt(3,2,3,5);

//		setBlocksInRegion(0,0,0, 99,0,99, 1);

		setBlockAt(0,           0, 0,           1);
		setBlockAt(levelSize-1, 0, 0,           1);
		setBlockAt(0,           0, levelSize-1, 1);
		setBlockAt(levelSize-1, 0, levelSize-1, 1);

		setBlockAt(0,           levelHeight-2, 0,           1);
		setBlockAt(levelSize-1, levelHeight-2, 0,           1);
		setBlockAt(0,           levelHeight-2, levelSize-1, 1);
		setBlockAt(levelSize-1, levelHeight-2, levelSize-1, 1);

		Debug.Log("[INFO]: [WorldBlockManagement] Test: Saving level as custom level.");
		saveLevelToFile("save1");

	}

	// generateNewLevel method.
	// Generates a new empty level with the given properties.
	private static void generateNewLevel(int levelSize1, int levelHeight1) {

		// Initialize empty level.
		levelSize = levelSize1;
		levelHeight = levelHeight1;
		int arraySize = levelSize * levelSize * levelHeight;
		blockData = new byte[arraySize];
		for(int i=0; i < arraySize; i++) {
			blockData[i] = 0;
		}
		blockObjects = new GameObject[arraySize];
		
		// Create an empty heightmap.
		heightMap = new byte[levelSize * levelSize];
		for(int i=0; i < heightMap.Length; i++) {
			heightMap[i] = 0;
		}
		
		// Initialize canWalkThrough property.
		canWalkThrough = new bool[levelSize * levelSize * levelHeight];
		canWalkThroughOnlyId255 = new bool[levelSize * levelSize * levelHeight];
		for(int i=0; i < canWalkThrough.Length; i++) {
			canWalkThrough[i] = true;
			canWalkThroughOnlyId255[i] = true;
		}
	}
	
	// loadLevelFromFile method.
	// Loads the levelSize, levelHeight and blockData from file.
	private static void loadLevelFromFile(string fileName) {
		string filePath = "Assets/Levels/" + fileName + ".bytes";

		// Check if the file exists.
		if(System.IO.File.Exists(filePath)) {

			// Get the data from the file.
			BinaryReader binData = new BinaryReader(File.Open(filePath, FileMode.Open));
			levelSize = (byte) binData.BaseStream.ReadByte();
			levelHeight = (byte) binData.BaseStream.ReadByte();
			blockData = binData.ReadBytes((int) (binData.BaseStream.Length-2));
			blockObjects = new GameObject[(int) (binData.BaseStream.Length-2)];
			binData.Close();

			// Create an empty heightmap.
			heightMap = new byte[levelSize * levelSize];
			for(int i=0; i < heightMap.Length; i++) {
				heightMap[i] = 0;
			}

			// Initialize canWalkThrough property.
			canWalkThrough = new bool[levelSize * levelSize * levelHeight];
			canWalkThroughOnlyId255 = new bool[levelSize * levelSize * levelHeight];
			for(int i=0; i < canWalkThrough.Length; i++) {
				canWalkThrough[i] = true;
				canWalkThroughOnlyId255[i] = true;
			}
		}
		else {
			Debug.Log("[SEVERE]: [WorldBlockManagement] The given levelfile does not exist. Filepath: " + filePath  + ".");
		}
	}

	// saveLevelToFile
	// Saves the current state of the level.
	// Argument fileName can be a name without extension.
	public void saveLevelToFile(string fileName) {
		string filePath = "Assets/Levels/customSaves/" + fileName + ".bytes";
		
		FileStream outStream = new FileStream(filePath, FileMode.Create);
		BinaryWriter writer = new BinaryWriter(outStream);

		writer.Write(levelSize);
		writer.Write(levelHeight);
		for(int i=0; i < blockData.Length; i++) {
			writer.Write(blockData[i]);
		}
		writer.Close();

	}

	// setBlockAt method.
	// Sets a block with blockID at position (x,y,z).
	public static void setBlockAt(int x, int y, int z, byte blockID) {

		// Get the index of the position in the blockData array.
		int byteArrayIndex = x + levelSize*z + levelSize*levelSize*y;

		// Check if the position exists.
		if(byteArrayIndex >= blockData.Length || x >= levelSize || z >= levelSize || y >= levelHeight-1 || x < 0 || y < 0 || z < 0) { // y-1 because the pathfinding algorithm needs the upperspace to save the overlay to.
			Debug.Log("[SEVERE]: [WorldBlockManagement] The setBlockAt method has been called with out of bounds arguments: x=" + x + ", y=" + y + ", z=" + z + ". Not creating block.");
			return;
		}

		// Set the block in the array and create the object in the scene.
		blockData[byteArrayIndex] = blockID;

		// Destroy the old block object if it exists.
		if(blockObjects[byteArrayIndex] != null) {
			Destroy(blockObjects[byteArrayIndex]);
		}
		if(blockID == 0) { // Air (clear block).
			blockObjects[byteArrayIndex] = null;
			canWalkThrough[byteArrayIndex] = true;
			canWalkThroughOnlyId255[byteArrayIndex] = true;
		}
		else {
			if(blockID == 255) { // PathFinding block (Enemies cant walk here, no physical block).
				blockObjects[byteArrayIndex] = null;
				canWalkThrough[byteArrayIndex] = false;
				canWalkThroughOnlyId255[byteArrayIndex] = false;
				return;
			}

			// Update the heightmap.
			if(blockID != 0) { // Block placed.
				if(heightMap[x + levelSize*z] <= y) { // If block is placed higher than old heightMap value.
					heightMap[x + levelSize*z] = (byte) y;
				}
			}
			else { // Block removed.
				if(getBlockAt(x,y,z) != 0) { // Only edit heightmap if a block was removed.
					if(heightMap[x + levelSize*z] <= y) { // If block is removed at (or above) the old heightMap value.
						for(int newHeight=y; newHeight >= 0; newHeight--) {
							if(getBlockAt(x,y,z) != 0) {
								heightMap[x + levelSize*z] = (byte) y;
								break;
							}
						}
					}
				}
			}

			// Load the texture, shader and canWalkThrough value (Depends on blockID).
			string textureFileName = "textureNotFoundTexture.png";
			string shaderName = "Diffuse";
			string blockShape = "full"; // The shape of the block. Should be one of: {full, topHalf, bottomHalf}.
			bool canWalkThroughLocal = false; // True if enemies can walk through this block.
			bool blockInvisible = false; // To spawn invisible blocks.
			bool hasCollider = true; // Disables collider but can still stop enemies.
			switch(blockID) {
			case 1: {
				textureFileName = "stone.png";
				break;
			}
			case 2: {
				textureFileName = "brick.png";
				break;
			}
			case 3: {
				textureFileName = "leaves.png";
				shaderName = "Transparent/Diffuse";
				break;
			}
			case 4: {
				textureFileName = "brick.png";
				blockShape = "topHalf";
				break;
			}
			case 5: {
				textureFileName = "brick.png";
				blockShape = "bottomHalf";
				canWalkThroughLocal = true;
				break;
			}
//			case 255: { // Invisible block without hitbox/collider.
//				blockInvisible = true;
//				hasCollider = false;
//				break;
//			}
			default: {
				Debug.Log("[SEVERE]: [WorldBlockManagement] The setBlockAt method has been called with an unknown blockID: " + blockID + ". Setting the block with textureNotFoundTexture and Diffuse shader.");
				break;
			}
			}
			
			canWalkThrough[byteArrayIndex] = canWalkThroughLocal; // Set "canWalkThrough" block property.
// Redundant			canWalkThroughOnlyId255[byteArrayIndex] = true; // If a new block is set and it wasnt ID 255.
			
			Texture2D texture = Resources.LoadAssetAtPath<Texture2D>("Assets/Resources/Textures/BlockTextures/" + textureFileName) as Texture2D;
			if(texture == null) {
				Debug.Log("[SEVERE]: [WorldBlockManagement] Texture " + textureFileName + " could not be loaded. Using textureNotFoundTexture.png.");
				texture = Resources.LoadAssetAtPath<Texture2D>("Assets/Resources/Textures/BlockTextures/textureNotFoundTexture.png") as Texture2D;
				shaderName = "Diffuse";
				if(texture == null) {
					Debug.Log("[SEVERE]: [WorldBlockManagement] Texture textureNotFoundTexture.png could not be loaded. Failed to add a texture to a block.");
				}
			}

			Shader shader = Shader.Find(shaderName);
			if(shader == null) {
				Debug.Log("[SEVERE]: [WorldBlockManagement] Shader " + shaderName + " could not be loaded. Using the default Diffuse shader.");
				shader = Shader.Find("Diffuse");
				if(shader == null) {
					Debug.Log("[SEVERE]: [WorldBlockManagement] Shader Diffuse (default shader) could not be loaded. Failed to add a shader to a block.");
				}
			}

			// Create the block and store a reference to it.
			GameObject b = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create a new block object.
		//	GameObject b = (GameObject) Instantiate(block); // Create a new block object.
			b.transform.position = new Vector3(x+0.5f, y+0.5f, z+0.5f); // Put the block in position.
			b.transform.parent = parent; // Puts the block object in a tab in the hierarchy window.
			b.renderer.material.mainTexture = texture;
			b.renderer.material.shader = shader;
			b.transform.collider.enabled = hasCollider;
			blockObjects[byteArrayIndex] = b;

			// Adjust the shape of the block (allow half blocks).
			if(blockShape.Equals("topHalf")) {
				b.transform.localScale = new Vector3(1f, 0.5f, 1f);
//				b.renderer.material.mainTextureScale = new Vector2(1f, 0.5f); // Scale the texture (x,y) to maintain the original texture aspect ratio.
				b.transform.position += new Vector3(0f, 0.25f, 0f); // Shift the block to the top of the 1x1x1 cube.

				// Change the texture coordinates so that the sides will contain only half the texture.
				MeshFilter mf = (MeshFilter) b.GetComponent("MeshFilter");
				mf.mesh.uv = setUVmapForHalfBlock(mf.mesh.uv);
				// OneLiner -> ((MeshFilter) b.GetComponent("MeshFilter")).mesh.uv = setUVmapForHalfBlock(((MeshFilter) b.GetComponent("MeshFilter")).mesh.uv);

			}
			else if(blockShape.Equals("bottomHalf")) {
				b.transform.localScale = new Vector3(1f, 0.5f, 1f);
//				b.renderer.material.mainTextureScale = new Vector2(1f, 0.5f); // Scale the texture (x,y) to maintain the original texture aspect ratio.
				b.transform.position -= new Vector3(0f, 0.25f, 0f); // Shift the block to the bottom of the 1x1x1 cube.

				// Change the texture coordinates so that the sides will contain only half the texture.
				MeshFilter mf = (MeshFilter) b.GetComponent("MeshFilter");
				mf.mesh.uv = setUVmapForHalfBlock(mf.mesh.uv);
				// OneLiner -> ((MeshFilter) b.GetComponent("MeshFilter")).mesh.uv = setUVmapForHalfBlock(((MeshFilter) b.GetComponent("MeshFilter")).mesh.uv);
			}

			// Set the block visibility.
			if(blockInvisible) {
				b.renderer.enabled = false; // Don't render a texture.
			}
		}
	}

	// setUVmapForHalfBlock method.
	// Halfs the texture size on the sides of the cube. Top and bottom remain untouched.
	private static Vector2[] setUVmapForHalfBlock(Vector2[] uvMap) {

		// Front.
		uvMap[0] = new Vector2(0f,0f);
		uvMap[1] = new Vector2(1f,0f);
		uvMap[2] = new Vector2(0f,0.5f);
		uvMap[3] = new Vector2(1f,0.5f);

		// Back.
		uvMap[10] = new Vector2(0f,0f);
		uvMap[11] = new Vector2(1f,0f);
		uvMap[6]  = new Vector2(0f,0.5f);
		uvMap[7]  = new Vector2(1f,0.5f);

		// Left.
		uvMap[16] = new Vector2(0f,0f);
		uvMap[18] = new Vector2(1f,0f);
		uvMap[19] = new Vector2(0f,0.5f);
		uvMap[17] = new Vector2(1f,0.5f);

		// Right.
		uvMap[20] = new Vector2(0f,0f);
		uvMap[22] = new Vector2(1f,0f);
		uvMap[23] = new Vector2(0f,0.5f);
		uvMap[21] = new Vector2(1f,0.5f);

		return uvMap;
	}

	// getBlockAt method.
	// Returns the blockID at position (x,y,z).
	public static byte getBlockAt(int x, int y, int z) {

		// Catch outOfBounds errors.
		if(x<0 || y<0 || z<0 || x >= levelSize || y >= levelHeight || z >= levelSize) {
			return 0;
		}

		// Get the index of the position in the blockData array.
		int byteArrayIndex = x + levelSize*z + levelSize*levelSize*y;

		// Get the blockID at the given location.
		return(blockData[byteArrayIndex]);
	}

	// getHighestBlockAt method.
	// Returns the height of the highest block at position (x,z).
	public static int getHighestBlockAt(int x, int z) {

		// Catch outOfBounds errors.
		if(x<0 || z<0 || x >= levelSize || z >= levelSize) {
			return 0;
		}
		
		// Get the index of the position in the blockData array.
		int byteArrayIndex = x + levelSize*z;
		
		// Get the height at the given location.
		return(heightMap[byteArrayIndex]);
	}
	
	// canWalkHere method + overload.
	// Returns true if the given location AND the location above it are available.
	public static bool canWalkHere(int x, int y, int z) { return canWalkHere(x, y, z, false); }
	public static bool canWalkHere(int x, int y, int z, bool onlyCheckId255) {

		// Catch outOfBounds errors (not yMax).
		if(x<0 || y<0 || z<0 || x >= levelSize || z >= levelSize) {
			return false;
		}

		// Get the index of the position in the blockData array.
		int byteArrayIndex = x + levelSize*z + levelSize*levelSize*y;
		
		// Get the canWalkThrough at the given location.
		if(y > levelHeight-1) {
			return true; // Return true as we could walk on top of the highest block.
		}
		else if(y == levelHeight-1) {
			if(onlyCheckId255) { return canWalkThroughOnlyId255[byteArrayIndex]; }
			else { return canWalkThrough[byteArrayIndex]; }
		} else {
//			Debug.Log ("x=" + x + " y=" + y + " z=" + z);
			if(onlyCheckId255) { return(canWalkThroughOnlyId255[byteArrayIndex] && canWalkThroughOnlyId255[byteArrayIndex + levelSize*levelSize]); }
			else { return(canWalkThrough[byteArrayIndex] && canWalkThrough[byteArrayIndex + levelSize*levelSize]); } // Block && one block higher.
		}
	}

	// canStandHere method + overload.
	// Returns true if a player/enemy can stand here without falling.
	public static bool canStandHere(int x, int y, int z) { return canStandHere(x, y, z, false); }
	public static bool canStandHere(int x, int y, int z, bool onlyCheckId255) {

		// Catch outOfBounds errors.
		if(x<0 || y<0 || z<0 || x >= levelSize || y >= levelHeight || z >= levelSize) {
			return false;
		}

		if(!canWalkHere(x,y,z)) { return false; }
		if(y == 0) { return true; } // Entities can always walk on the bottom. There should be a ground plane here.
		int byteArrayIndex = x + levelSize*z + levelSize*levelSize*(y-1);
		if(onlyCheckId255) { return !canWalkThroughOnlyId255[byteArrayIndex]; }
		else { return !canWalkThrough[byteArrayIndex]; } // True if there is a solid block below.
	}

	// canJumpAt method + overload.
	// Returns true if the player can jump at this location.
	public static bool canJumpAt(int x, int y, int z) { return canJumpAt(x, y, z, false); }
	public static bool canJumpAt(int x, int y, int z, bool onlyCheckId255) {

		// Catch outOfBounds errors.
		if(x<0 || y<0 || z<0 || x >= levelSize || y >= levelHeight || z >= levelSize) {
			return false;
		}

		if(y+2 >= levelHeight) { return true; } // Jumping is always possible above max block height.
		int byteArrayIndex = x + levelSize*z + levelSize*levelSize*(y+2);
		
		if(onlyCheckId255) { return canWalkThroughOnlyId255[byteArrayIndex]; }
		else { return canWalkThrough[byteArrayIndex]; } // True is no block above head.
	}

	// getLevelSize method.
	// Returns the level size.
	public static int getLevelSize() {
		return levelSize;
	}

	// getLevelHeight method.
	// Returns the level size.
	public static int getLevelHeight() {
		return levelHeight;
	}

	// setBlocksInRegion method.
	// Sets blocks in a cube defined by 2 positions.
	public void setBlocksInRegion(int x1, int y1, int z1, int x2, int y2, int z2, byte blockID) {
		int Xmin = Mathf.Min(x1, x2);
		int Xmax = Mathf.Max(x1, x2);
		int Ymin = Mathf.Min(y1, y2);
		int Ymax = Mathf.Max(y1, y2);
		int Zmin = Mathf.Min(z1, z2);
		int Zmax = Mathf.Max(z1, z2);

		for(int x = Xmin; x <= Xmax; x++) {
			for(int z = Zmin; z <= Zmax; z++) {
				for(int y = Ymin; y <= Ymax; y++) {
					setBlockAt(x, y, z, blockID);
				}
			}
		}
	}

	private void createGroundPlane() {
		GameObject groundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane); // Create a new plane object.
		groundPlane.transform.Translate(new Vector3(levelSize/2, -0.01f, levelSize/2));
		groundPlane.transform.localScale = new Vector3(levelSize/10f, 0f, levelSize/10f);
	}

	// blockifyWorld method.
	// Blockifies all objects which have a collider. This is used to indicate where objects are for pathFinding.
	private void blockifyWorld() {
		for(int x = 0; x < levelSize; x++) {
			for(int z = 0; z < levelSize; z++) {
				for(int y = 0; y < levelHeight-1; y++) {
					if(Physics.CheckSphere(new Vector3(x+0.5f, y+0.5f, z+0.5f), 0.49f)) {
						if(getBlockAt(x, y, z) == 0) {
							setBlockAt(x, y, z, 255); // Invisible block.
						}
					}
				}
			}
		}
	}

	// isSupportedAt method.
	// Returns wether a player/enemy is being supported at a certain position. If true, the enemy/player should not fall.
	// capsuleRadius is the radius of your support, this should not exceed 0.5.
	public static bool isSupportedAt(Vector3 pos, float capsuleRadius) {
		pos += new Vector3(0.5f, 0f, 0.5f); // Scale the entitypos with the level.
		// Give warning on wrong input.
		if(capsuleRadius > 0.5f) { Debug.Log("[SEVERE] [WorldBlockManagement] isSupportedAt method is called with too large capsuleRadius."); }
		if(pos.x<0 || pos.y<0 || pos.z<0 || pos.x >= levelSize || pos.y >= levelHeight || pos.z >= levelSize) {
			Debug.Log("[SEVERE] [WorldBlockManagement] isSupportedAt method is called with an out of bounds position.");
			return false;
		}

		// Check if the entity is an integer y value.
		if(Mathf.Abs(pos.y % 1f) > 0f) { return false; } // TODO - Maybe increase this value a little to prevent glitching?

		// Get the on-grid position.
		int x = Mathf.FloorToInt(pos.x);
		int y = Mathf.RoundToInt(pos.y);
		int z = Mathf.FloorToInt(pos.z);

		// Return true if the current position is supportive.
		if(canStandHere(x, y, z)) { return true; }

		// Check blocks around the entity and return true if the entity has support on them.
		// TODO - Enemies glitch when going in the z- x+ direction. Fix this.
		float capsuleRadiusSqr = capsuleRadius * capsuleRadius;
		if(canStandHere(x+1, y, z  ) && x+1 - pos.x <= capsuleRadius) { return true; }
		if(canStandHere(x-1, y, z  ) && pos.x - x   <= capsuleRadius) { return true; }
		if(canStandHere(x  , y, z+1) && z+1 - pos.z <= capsuleRadius) { return true; }
		if(canStandHere(x  , y, z-1) && pos.z - z   <= capsuleRadius) { return true; }

		if(canStandHere(x+1, y, z+1) && (pos - new Vector3(x+1, pos.y, z+1)).sqrMagnitude <= capsuleRadiusSqr) { return true; }
		if(canStandHere(x-1, y, z+1) && (pos - new Vector3(x  , pos.y, z+1)).sqrMagnitude <= capsuleRadiusSqr) { return true; }
		if(canStandHere(x+1, y, z-1) && (pos - new Vector3(x+1, pos.y, z  )).sqrMagnitude <= capsuleRadiusSqr) { return true; } // Fails?
		if(canStandHere(x-1, y, z-1) && (pos - new Vector3(x  , pos.y, z  )).sqrMagnitude <= capsuleRadiusSqr) { return true; }

		// Return false if no support was found.
		return false;
	}

	// Quote to copy:   """""""



//	// Update is called once per frame.
//	void Update () {
//		
//	}
}
