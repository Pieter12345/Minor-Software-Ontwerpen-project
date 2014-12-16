using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthDisplay : MonoBehaviour {

	public Transform player;
	private Health playerHP;

	private Text displayText;

	// Use this for initialization
	void Start () {
		if(player!=null)
			playerHP = player.GetComponent(typeof(Health)) as Health;
		displayText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerHP != null){
			displayText.text = "HP: " + playerHP.HP + "/" + playerHP.maxHP;
		}
	}
}
