using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlockCounter : MonoBehaviour {

	public Transform player;
	private Text text;
	private FireController fc;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		if(player != null)
			fc = player.GetComponent<FireController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(fc!=null)
			text.text = "Blocks: " + fc.AmountOfBlocks.ToString();
	}
}
