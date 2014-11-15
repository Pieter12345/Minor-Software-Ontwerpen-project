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
	
//	public Object blockPrefab; // Drag the block prefab to this field. (Default: Block)
//	private static Object block;

	public Transform parentObject; // The (empty) parent GameObject to create new blocks in.
	private static Transform parent; // Block objects will be created as childs of this GameObject. (Default: World/Blocks)

	// Runs on creation of the object its bound to (should be the level floor plane or so).
	void Start () {

		// Load the default level to variables.
		loadLevelFromFile("testLevel");

		// Make the blockPrefab and parentObject static.
//		block = this.blockPrefab;
		parent = this.parentObject;

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
		setBlockAt(0,0,0,1);
		setBlockAt(1,1,1,2);
		setBlockAt(2,2,2,3);

		Debug.Log("[INFO]: [WorldBlockManagement] Test: Saving level as custom level.");
		saveLevelToFile("save1");

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
		}
		else {
			Debug.Log("[SEVERE]: [WorldBlockManagement] The given levelfile does not exist.");
		}
	}

	// saveLevelToFile
	// Saves the current state of the level.
	// Argument fileName can be a name without extension.
	public void saveLevelToFile(string fileName) {
		string filePath = "Assets/Levels/customSaves/" + fileName + ".binary";
		
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

			// Load the texture.
			string textureFileName = "";
			switch(blockID) {
			case 1: {
				textureFileName = "stone.png";
				break;
			}
			case 2: {
				textureFileName = "brick.png";
				break;
			}
			}

			Texture2D texture = Resources.LoadAssetAtPath<Texture2D>("Assets/Resources/Textures/BlockTextures/" + textureFileName) as Texture2D;

			if(texture == null) {
				Debug.Log("[SEVERE]: [WorldBlockManagement] Texture " + textureFileName + " could not be loaded. Using textureNotFoundTexture.png.");
				texture = Resources.LoadAssetAtPath<Texture2D>("Assets/Resources/Textures/BlockTextures/textureNotFoundTexture.png") as Texture2D;
				if(texture == null) {
					Debug.Log("[SEVERE]: [WorldBlockManagement] Texture textureNotFoundTexture.png could not be loaded. Failed to add a texture to a block.");
				}
			}

			// Create the block and store a reference to it.
			GameObject b = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create a new block object.
		//	GameObject b = (GameObject) Instantiate(block); // Create a new block object.
			b.transform.position = new Vector3(x+0.5f, y+0.5f, z+0.5f); // Put the block in position.
			b.transform.SetParent(parent); // Puts the block object in a tab in the hierarchy window.
			b.renderer.material.mainTexture = texture;
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
