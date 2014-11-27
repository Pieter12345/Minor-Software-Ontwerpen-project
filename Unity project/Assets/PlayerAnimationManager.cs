using UnityEngine;
using System.Collections;

public class PlayerAnimationManager : MonoBehaviour {

	public Transform playerMesh;

	private Animation anim;

	// Use this for initialization
	void Start () {
		anim = playerMesh.GetComponent<Animation>();
		anim["Run"].speed = 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (anim!=null){
			if(Input.GetKeyDown(KeyCode.Keypad9)){
				anim.Play("Idle");
			}
			if(Input.GetKeyDown(KeyCode.Keypad8))
				anim.Play("Run");
		}
	}
}
