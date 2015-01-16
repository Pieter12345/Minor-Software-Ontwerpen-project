using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyRadar : MonoBehaviour {

	public Texture2D enemyRadarDotTexture;
	public Texture2D radarTexture;
	public int radarSize = 100;

	public GameObject radarPanel;

	public Sprite enemyRadarSprite;

//	private Sprite enemyRadarSprite;

	// Use this for initialization.
	void Start () {
//		this.enemyRadarSprite = Sprite.Create(this.radarTexture, new Rect(0f, 0f, this.radarSize >> 1, this.radarSize >> 1), new Vector2(this.radarSize >> 1, this.radarSize >> 1));

		GameObject radarObject = new GameObject();
		radarObject.AddComponent("Image");
		radarObject.GetComponent<Image>().sprite = this.enemyRadarSprite;
		radarObject.transform.SetParent(this.radarPanel.transform);
		radarObject.transform.localPosition = new Vector3(50f, 50f, 0f);
		radarObject.transform.localScale = new Vector3(0.1f, 0.1f, 1f);

	}
	
	// Update is called once per frame.
	void Update () {
//		GUI.DrawTexture(new Rect(0f, 0f, this.radarSize >> 1, this.radarSize >> 1), this.radarTexture);
	}
}
