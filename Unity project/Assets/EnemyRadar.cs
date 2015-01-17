using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyRadar : MonoBehaviour {

	public Sprite enemyRadarDotSprite;
	public Sprite radarSprite;

	public int radarSize = 100;

	public GameObject radarPanel;

	private GameObject[] enemyDots;

//	private Sprite enemyRadarSprite;

	// Use this for initialization.
	void Start () {
//		this.enemyRadarSprite = Sprite.Create(this.radarTexture, new Rect(0f, 0f, this.radarSize >> 1, this.radarSize >> 1), new Vector2(this.radarSize >> 1, this.radarSize >> 1));

		// Create 1000 dots for the minimap. This should be enough.
		this.enemyDots = new GameObject[1000];
		for(int i = 0; i < enemyDots.Length; i++) {
			this.enemyDots[i].AddComponent("Image");
			this.enemyDots[i].GetComponent<Image>().sprite = this.enemyRadarDotSprite;
			this.enemyDots[i].transform.SetParent(this.radarPanel.transform);
			this.enemyDots[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f);
			this.enemyDots[i].SetActive(false);
		}

		// Create and show the radar.
		this.createRadar();

//		GameObject radarObject = new GameObject();
//		radarObject.AddComponent("Image");
//		radarObject.GetComponent<Image>().sprite = this.enemyRadarDotSprite;
//		radarObject.transform.SetParent(this.radarPanel.transform);
//		radarObject.transform.localPosition = new Vector3(50f, 50f, 0f);
//		radarObject.transform.localScale = new Vector3(0.1f, 0.1f, 1f);

	}
	
	// Update is called once per frame.
	void Update () {

		// Get all enemy objects.
		GameObject[] enemyObjects = EnemyController.getEnemyObjects();

		// Set position and rotation of enemyDots.
		for(int i = 0; i < enemyObjects.Length; i++) {
//			this.enemyDots[i].transform.localPosition = ... // TODO - set position and rotation here.
			this.enemyDots[i].SetActive(true);
		}

		// Disable any remaining enemyDots.
		int j = enemyObjects.Length;
		while(j < enemyDots.Length && this.enemyDots[j].activeInHierarchy) {
			this.enemyDots[j].SetActive(false);
		}


//		GUI.DrawTexture(new Rect(0f, 0f, this.radarSize >> 1, this.radarSize >> 1), this.radarTexture);
	}

	// createRadar method.
	// Draws the radar to the screen.
	public void createRadar() {
		GameObject radarObject = new GameObject();
		radarObject.AddComponent("Image");
		radarObject.GetComponent<Image>().sprite = this.radarSprite;
		radarObject.transform.SetParent(this.radarPanel.transform);
		radarObject.transform.localPosition = new Vector3(this.radarSize >> 1, this.radarSize >> 1, 0f); // Set radar to the middle of the panel.
		// TODO - Scale image to desired radarSize based on sprite size?

	}

	// addEnemyDot method.
	// Draws enemy dots to the radar.
	public void addEnemyDot(Vector3 pos) {
		GameObject enemyDotObject = new GameObject();
		enemyDotObject.AddComponent("Image");
		enemyDotObject.GetComponent<Image>().sprite = this.enemyRadarDotSprite;
		enemyDotObject.transform.SetParent(this.radarPanel.transform);
		enemyDotObject.transform.localScale = new Vector3(0.1f, 0.1f, 1f); // TODO - Resize based on sprite size?
		enemyDotObject.transform.localPosition = pos;
	}

}
