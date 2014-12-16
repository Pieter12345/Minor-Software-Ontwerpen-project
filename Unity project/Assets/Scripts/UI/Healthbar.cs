using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour {

	public Transform player;
	private Health playerHP;
	
	private Slider slider;
	
	// Use this for initialization
	void Start () {
		if(player!=null)
			playerHP = player.GetComponent(typeof(Health)) as Health;
		slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerHP != null){
			slider.value = playerHP.HP/playerHP.maxHP;
		}
	}
}
