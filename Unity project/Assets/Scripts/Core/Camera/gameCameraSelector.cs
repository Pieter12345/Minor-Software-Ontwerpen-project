﻿using UnityEngine;
using System.Collections;

public class gameCameraSelector : MonoBehaviour {

	// Variables & Constants.
	private ShooterGameCamera thirdPersonCam;
	private FirstPersonShooterGameCamera firstPersonCam;
	public bool firstPerson = false;
	public float mouseSensitivity = 100f;
	private bool isTempFirstPerson = false; // Used to save when a player is playing in third person, but is placing blocks in first.

	public Transform player;
	public Transform aimTarget;
	public Transform camTransform;
	public Transform weapon;
	public Transform modelLeftHand;
	
	public Texture crosshair;

	public bool isReloading { get{ return false; } } // TODO - Implement this.
	public bool isSwitchingWeapons { get{ return (this.firstPerson ? this.firstPersonCam.getIsSwitchingWeapons() : false); } } // TODO - Implement weapon switching in third person.
	//	public bool isSwitchingWeapons { get{ return (this.firstPerson ? this.firstPersonCam.getIsSwitchingWeapons() : this.thirdPersonCam.getIsSwitchingWeapons()); } };


	// ---------------------------------------------------------------------------------------------
	// Use this for initialization.
	// Initializes and runs the Start() method in the first or third person camera script.
	// ---------------------------------------------------------------------------------------------
	void Start () {

		// Create camera controlling objects.
		this.thirdPersonCam = new ShooterGameCamera(player, aimTarget, transform, weapon, modelLeftHand);
		this.firstPersonCam = new FirstPersonShooterGameCamera(player, aimTarget, transform, weapon);

		// Load the proper camera.
		if(firstPerson) {
			this.firstPersonCam.Start();
		} else {
			this.thirdPersonCam.Start();
		}
	}


	// ---------------------------------------------------------------------------------------------
	// LateUpdate is called once after every frame.
	// ---------------------------------------------------------------------------------------------
	void LateUpdate () {

		// Return if the game hasnt started yet or if it has been paused.
		if (Time.deltaTime == 0 || Time.timeScale == 0) { return; }
		
		// Toggle between first and third person if F5 is pressed.
		bool fKeyDown = Input.GetKeyDown(KeyCode.F);
		bool fKeyUp   = Input.GetKeyUp(KeyCode.F);
		if(Input.GetKeyDown(KeyCode.F5) || (fKeyDown && !firstPerson) || (fKeyUp && isTempFirstPerson)) {
			isTempFirstPerson = fKeyDown && !firstPerson;
			firstPerson = !firstPerson;

			// Start the right camera.
			this.startCamera(firstPerson);
		}

		// Update the proper camera.
		if(firstPerson) {
			this.firstPersonCam.Update();
		} else {
			this.thirdPersonCam.Update();
		}

		// Keep mouse sensitivity in sync with the public variable.
		if(firstPerson) {
			this.firstPersonCam.setMouseSensitivity(this.mouseSensitivity);
		} else {
			this.thirdPersonCam.setMouseSensitivity(this.mouseSensitivity);
		}
	}


	// ---------------------------------------------------------------------------------------------
	// startCamera method.
	// Runs the Start() method in the first or third person camera script.
	// ---------------------------------------------------------------------------------------------
	private void startCamera(bool firstPerson) {
		if(firstPerson) {
			this.firstPersonCam.Start();
		} else {
			this.thirdPersonCam.Start();
		}
	}


	// ---------------------------------------------------------------------------------------------
	// Redirect the setFired method to the right script.
	// ---------------------------------------------------------------------------------------------
	public void setFired(bool state = true) {
		if(firstPerson) {
			this.firstPersonCam.setFired(state);
		} else {
			this.thirdPersonCam.setFired(state);
		}
	}


	// ---------------------------------------------------------------------------------------------
	// Redirect weapon switching to the right script. Used to add weapon switching animations.
	// ---------------------------------------------------------------------------------------------
	public void requestWeaponChange(int weaponID) {
		if(firstPerson) {
			this.firstPersonCam.requestWeaponChange(weaponID);
		} else {
			this.thirdPersonCam.requestWeaponChange(weaponID);
		}
	}


	// ---------------------------------------------------------------------------------------------
	// Draw the crosshair.
	// ---------------------------------------------------------------------------------------------
	void OnGUI () {
		if (Time.time != 0 && Time.timeScale != 0) {
			GUI.DrawTexture(new Rect(Screen.width/2f-(crosshair.width*0.5f), Screen.height/2f-(crosshair.height*0.5f), crosshair.width, crosshair.height), crosshair);
		}
	}
}
