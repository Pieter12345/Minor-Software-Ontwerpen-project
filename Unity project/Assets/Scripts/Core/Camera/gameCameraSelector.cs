﻿using UnityEngine;
using System.Collections;

public class gameCameraSelector : MonoBehaviour {

	// Variables & Constants.
	private ShooterGameCamera thirdPersonCam;
	private FirstPersonShooterGameCamera firstPersonCam;
	public bool firstPerson = false;
	private bool isTempFirstPerson = false; // Used to save when a player is playing in third person, but is placing blocks in first.

	public Transform player;
	public Transform aimTarget;
	public Transform camTransform;
	public Transform weapon;
	
	public Texture crosshair;

	// Use this for initialization.
	void Start () {
		this.thirdPersonCam = new ShooterGameCamera(player, aimTarget, transform, weapon);
		this.firstPersonCam = new FirstPersonShooterGameCamera(player, aimTarget, transform, weapon);

		// Load the proper camera.
		if(firstPerson) {
			this.firstPersonCam.Start();
		} else {
			this.thirdPersonCam.Start();
		}
	}
	
	// Update is called once per frame.
	void LateUpdate () {
		
		// Toggle between first and third person if F5 is pressed.
		bool fKeyDown = Input.GetKeyDown(KeyCode.F);
		bool fKeyUp   = Input.GetKeyUp(KeyCode.F);
		if(Input.GetKeyDown(KeyCode.F5) || (fKeyDown && !firstPerson) || (fKeyUp && isTempFirstPerson)) {
			isTempFirstPerson = fKeyDown && !firstPerson;
			firstPerson = !firstPerson;

			// Start the right camera.
			this.startCamera(firstPerson);
		}

		// 

		// Update the proper camera.
		if(firstPerson) {
			this.firstPersonCam.Update();
		} else {
			this.thirdPersonCam.Update();
		}
	}

	// startCamera method.
	// Runs the Start() method in the first or third person camera script.
	private void startCamera(bool firstPerson) {
		if(firstPerson) {
			this.firstPersonCam.Start();
		} else {
			this.thirdPersonCam.Start();
		}
	}

	// Redirect the setFired method to the right script.
	public void setFired(bool state = true) {
		if(firstPerson) {
			this.firstPersonCam.setFired(state);
		} else {
			this.thirdPersonCam.setFired(state);
		}
	}

	// Draw the crosshair.
	void OnGUI () {
		if (Time.time != 0 && Time.timeScale != 0) {
			GUI.DrawTexture(new Rect(Screen.width/2f-(crosshair.width*0.5f), Screen.height/2f-(crosshair.height*0.5f), crosshair.width, crosshair.height), crosshair);
		}
	}
}
