using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class UnderAttackMessage : MonoBehaviour {

	public float fadeDelayTime = 2f;
	public float fadeSpeed = 5f;
	
	private Text txt;
	private float timeLastHit;

	public float Alpha { 
		get {
			if(txt != null)
				return txt.color.a;
			else
				return 0f;
		}
		set {
			if(txt != null)
				txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, value);
		}
	}

	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
		Alpha = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Time.time - timeLastHit > fadeDelayTime){
			Alpha -= fadeSpeed;
		}
		if(Alpha < 0f)
			Alpha = 0f;
	}

	public void Show(){
		timeLastHit = Time.time;
		Alpha = 1f;
	}

}
