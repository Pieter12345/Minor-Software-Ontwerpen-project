// Class for block management in the world.
// For loading, a binary file should be in format: {levelSize, levelHeight, levelSize * levelSize * levelHeight blockData}.

using UnityEngine;
using System.Collections;
using System.IO;

public class WorldBlockManagement : MonoBehaviour {

	// Variables & Constants.
	private static byte levelSize; // Length and width of the buildable floor plane.
	private static byte levelHeight; // Height in which can be built.
	private static byte[] blockData; // The array of blockData.
	private static GameObject[] blockObjects; // Same size as blockData, contains the block objects.
	
	public Object blockPrefab; // Drag the block prefab to this field. (Default: Block)
	private static Object block;

	public Transform parentObject; // The (empty) parent GameObject to create new blocks in.
	private static Transform parent; // Block objects will be created as childs of this GameObject. (Default: World/Blocks)

	// Runs on creation of the object its bound to (should be the level floor plane or so).
	void Start () {

		// Load the default level to variables.
		loadFromFile("Assets/Levels/testLevel.bytes");

		// Make the blockPrefab and parentObject static.
		block = this.blockPrefab;
		parent = this.parentObject;

		// Load the level to the scene.
		int byteArrayIndex = 0;
		for(int y=0; y < levelHeight; y++) {
			for(int z=0; z < levelSize; z++) {
				for(int x=0; x < levelSize; x++) {

					// If a block should be placed at the current position.
					if(blockData[byteArrayIndex] != 0) {

						GameObject b = (GameObject) Instantiate(block); // Create a new block object.
						b.transform.position = new Vector3(x, y, z); // Put the block in position.
						b.transform.SetParent(parent); // Puts the block object in a tab in the hierarchy window.
						blockObjects[byteArrayIndex] = b;
					}
					byteArrayIndex++;
				}
			}
		}

		// Set some blocks to test the script.
		Debug.Log("[INFO]: [WorldBlockManagement] Test blocks are being placed.");
		setBlockAt(0,0,0,1);
		setBlockAt(1,1,1,1);
		setBlockAt(2,2,2,1);

	}

	// loadFromFile method.
	// Loads the levelSize, levelHeight and blockData from file.
	private static void loadFromFile(string filePath) {

		// Check if the file exists.
		if(System.IO.File.Exists(filePath)) {

			// Get the data from the file.
			BinaryReader binData = new BinaryReader(File.Open(filePath, FileMode.Open));
			levelSize = (byte) binData.BaseStream.ReadByte();
			levelHeight = (byte) binData.BaseStream.ReadByte();
			blockData = binData.ReadBytes((int) (binData.BaseStream.Length-2));
			blockObjects = new GameObject[(int) (binData.BaseStream.Length-2)];
		}
		else {
			Debug.Log("[SEVERE]: [WorldBlockManagement] The given levelfile does not exist.");
		}
	}

	// setBlockAt method.
	// Sets a block with blockID at position (x,y,z).
	public static void setBlockAt(int x, int y, int z, byte blockID) {

		// Get the index of the position in the blockData array.
		int byteArrayIndex = x + levelSize*z + levelSize*levelSize*y;

		// Check if the position exists.
		if(byteArrayIndex >= blockData.Length || x >= levelSize || z >= levelSize || y >= levelHeight) {
			Debug.Log("[INFO]: [WorldBlockManagement] The setBlockAt method has been called with out of bounds arguments. Not creating block.");
			return;
		}

		// Set the block in the array and create the object in the scene.
		blockData[byteArrayIndex] = blockID;

		// Destroy the old block object if it exists.
		if(blockObjects[byteArrayIndex] != null) {
			Destroy(blockObjects[byteArrayIndex]);
		}
		if(blockID == 0) {
			blockObjects[byteArrayIndex] = null;
		}
		else {
			GameObject b = (GameObject) Instantiate(block); // Create a new block object.
			b.transform.position = new Vector3(x, y, z); // Put the block in position.
			b.transform.SetParent(parent); // Puts the block object in a tab in the hierarchy window.
			blockObjects[byteArrayIndex] = b;
		}
	}

	// getBlockAt method.
	// Returns the blockID at position (x,y,z).
	public static byte getBlockAt(int x, int y, int z) {

		// Get the index of the position in the blockData array.
		int byteArrayIndex = x + levelSize*z + levelSize*levelSize*y;

		// Get the blockID at the given location.
		return(blockData[byteArrayIndex]);
	}


	// Quote to copy:   """""""



//	// Update is called once per frame.
//	void Update () {
//		
//	}
}
