using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerAnimationManager : MonoBehaviour {

	public Transform playerMesh;
	public float epsilon = 0.01f;

	public GameObject weapons;
	private WeaponController wContr;

	private Animation anim;
	private CharacterController controller;

	// Use this for initialization
	void Start () {
		anim = playerMesh.GetComponent<Animation>();
		anim["ColtLopen"].speed = 3;
		anim["ShotgunLopen"].speed = 3;
		controller = GetComponent(typeof(CharacterController)) as CharacterController;
		wContr = weapons.GetComponent(typeof(WeaponController)) as WeaponController;
		anim.Play("ColtIdle");
	}
	
	// Update is called once per frame
	void Update () {
		if(controller == null){
			controller = GetComponent(typeof(CharacterController)) as CharacterController;
			return;
		}
		if(controller.velocity.y > epsilon){
			anim.CrossFade("Springen");
		} else if(controller.velocity.magnitude > 1){
			if(wContr.HasWeapon)
				if(wContr.SelectedWeaponType != WeaponStats.RIFLE && wContr.SelectedWeaponType != WeaponStats.SHOTGUN)
					anim.CrossFade("ColtLopen");
				else
					anim.CrossFade("ShotgunLopen");
			else
				anim.CrossFade("Rennen");
		} else {
			if(wContr.HasWeapon)
				anim.CrossFade("ColtIdle");
			else
				anim.CrossFade("Wachten");
		}
	}



}
