using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerAnimationManager : MonoBehaviour {

	public Transform playerMesh;
	public float epsilon = 0.01f;

	private Animation anim;
	private CharacterController controller;

	// Use this for initialization
	void Start () {
		anim = playerMesh.GetComponent<Animation>();
		anim["Run"].speed = 3;
		controller = GetComponent(typeof(CharacterController)) as CharacterController;
		anim.Play("IdleGun");
	}
	
	// Update is called once per frame
	void Update () {
		if(controller.velocity.y > epsilon){
			anim.CrossFade("Jump");
		} else if(controller.velocity.magnitude > 1){
			anim.CrossFade("Run");
		} else {
			anim.CrossFade("IdleGun");
		}
	}



}
