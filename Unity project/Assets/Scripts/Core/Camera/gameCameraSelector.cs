using UnityEngine;
using System.Collections;

public class gameCameraSelector : MonoBehaviour {

	// Variables & Constants.
	private ShooterGameCamera thirdPersonCam;
	private FirstPersonShooterGameCamera firstPersonCam;
	public bool firstPerson = false;

	public Transform player;
	public Transform aimTarget;
	public Transform camTransform;
	public Transform weapon;
	
	private Texture crosshair;

	// Use this for initialization.
	void Start () {
		this.thirdPersonCam = new ShooterGameCamera(player, aimTarget, transform, crosshair, weapon);
		this.firstPersonCam = new FirstPersonShooterGameCamera(player, aimTarget, transform, crosshair, weapon);
	}
	
	// Update is called once per frame.
	void LateUpdate () {

		// Toggle between first and third person if F5 is pressed.
		if(Input.GetKeyDown(KeyCode.F5)) {
			firstPerson = !firstPerson;

			// Load the proper camera.
			if(firstPerson) {
				this.firstPersonCam.Start();
			} else {
				this.thirdPersonCam.Start();
			}
		}

		// Update the proper camera.
		if(firstPerson) {
			this.firstPersonCam.Update();
		} else {
			this.thirdPersonCam.Update();
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
}
