using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyRadar : MonoBehaviour {

	// Variables & Constants.
	public Sprite enemyRadarDotSprite;
	public Sprite radarSprite;

//	public int radarSize = 100;
	public float radarScale = 5f; // 1 block = 1 * radarScale pixels.
	public float maxRadarViewRange = 100f; // [Meters] - The max distance in which enemies will appear on the radar.

	public GameObject radarObject;
	public GameObject radarPanel;
	public Transform playerTransform;
	public Transform camTransform;

	private GameObject[] enemyDots;

	// Use this for initialization.
	void Start () {
//		this.enemyRadarSprite = Sprite.Create(this.radarTexture, new Rect(0f, 0f, this.radarSize >> 1, this.radarSize >> 1), new Vector2(this.radarSize >> 1, this.radarSize >> 1));
//		this.radarMidPoint = new Vector3(this.radarSize >> 1, this.radarSize >> 1, 0f);

		// Create 1000 dots for the minimap. This should be enough.
		this.enemyDots = new GameObject[1000];
		for(int i = 0; i < enemyDots.Length; i++) {
			this.enemyDots[i] = new GameObject();
			this.enemyDots[i].name = "enemyRadarDotSprite [ready for use]";
			this.enemyDots[i].AddComponent("Image");
			this.enemyDots[i].GetComponent<Image>().sprite = this.enemyRadarDotSprite;
			this.enemyDots[i].transform.SetParent(this.radarPanel.transform);
			this.enemyDots[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
			this.enemyDots[i].SetActive(false);
		}

		// Create and show the playerDot on the radar.
		GameObject playerDot = new GameObject();
		playerDot.AddComponent("Image");
		playerDot.name = "playerDotSprite";
		playerDot.GetComponent<Image>().sprite = this.enemyRadarDotSprite; // TODO - Change player dot color?
		playerDot.transform.SetParent(this.radarPanel.transform);
		playerDot.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
		playerDot.transform.localPosition = new Vector3(0f, 0f, 0f);
		playerDot.GetComponent<Image>().color=Color.red;

		// Create and show the radar sprite.
		this.createRadar();

	}
	
	// Update is called once per frame.
	void Update () {
		
		// Get all enemy objects.
		GameObject[] enemyObjects = EnemyController.getEnemyObjects();

		// Set position and rotation of enemyDots.
		Vector3 playerPos2D = new Vector3(playerTransform.position.x, playerTransform.position.z, 0f);
		for(int i = 0; i < enemyObjects.Length; i++) {
			if(enemyObjects[i] != null) {
				Vector3 enemyPos2D = new Vector3(enemyObjects[i].transform.position.x, enemyObjects[i].transform.position.z, 0f);
				Vector3 playerToEnemyVector = enemyPos2D - playerPos2D;
				if(playerToEnemyVector.magnitude <= this.maxRadarViewRange) {
					this.enemyDots[i].transform.localPosition = (playerToEnemyVector * this.radarScale);
					this.enemyDots[i].name = "enemyRadarDotSprite [active on radar]";
					this.enemyDots[i].SetActive(true);
				} else {
					this.enemyDots[i].name = "enemyRadarDotSprite [ready for use]";
					this.enemyDots[i].SetActive(false); // Enemy is too far away.
				}
			} else {
				this.enemyDots[i].name = "enemyRadarDotSprite [ready for use]";
				this.enemyDots[i].SetActive(false); // Enemy has died.
			}
		}

		// Disable any remaining enemyDots.
		int j = enemyObjects.Length;
		while(j < enemyDots.Length && this.enemyDots[j].activeInHierarchy) {
			this.enemyDots[j].SetActive(false);
		}

 		// Rotate the radarPanel to point it in the direction the player is looking in.
		this.radarPanel.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, this.camTransform.rotation.eulerAngles.y));
	}

	// createRadar method.
	// Draws the radar to the screen.
	public void createRadar() {
		GameObject radarObject = new GameObject();	//same as playerDot?
		radarObject.name = "radarSprite";
		radarObject.AddComponent("Image");
		radarObject.GetComponent<Image>().sprite = this.radarSprite;
		radarObject.transform.SetParent(this.radarPanel.transform);
		radarObject.transform.localPosition = new Vector3(0f, 0f, 0f); // Set localPosition to the middle of the radarPanel.
		radarObject.transform.localScale = new Vector3(0.1f, 0.1f, 1f); // TODO - Scale image to desired radarSize based on sprite size?
		radarObject.GetComponent<Image>().color=Color.red;
	}

//	// addEnemyDot method.
//	// Draws enemy dots to the radar.
//	public void addEnemyDot(Vector3 pos) {
//		GameObject enemyDotObject = new GameObject();
//		enemyDotObject.AddComponent("Image");
//		enemyDotObject.GetComponent<Image>().sprite = this.enemyRadarDotSprite;
//		enemyDotObject.transform.SetParent(this.radarPanel.transform);
//		enemyDotObject.transform.localScale = new Vector3(0.1f, 0.1f, 1f); // TODO - Resize based on sprite size?
//		enemyDotObject.transform.localPosition = pos;
//	}

}
