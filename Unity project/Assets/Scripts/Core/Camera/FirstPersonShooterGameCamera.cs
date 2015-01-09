using UnityEngine;
using System.Collections;

public class FirstPersonShooterGameCamera {

	// Variables & Constants.
	private Transform player;
	private Transform aimTarget;
	private Transform weapon;
	private Transform camTransform;

	// Camera & control variables.
	private Vector3 cameraRotation;
	public float mouseSensitivity = 300f;
	private float minRotationX = 10f; // 0 = down.
	private float maxRotationX = 170f; // 180 = up.

	// Recoil variables.
	private bool Fired = false;
	private float recoilFixedUpdateStepSize = 0.01f; // [sec]. Should be small, this acts like Time.timescale.
	private float recoilTimer; // [sec].
	private Vector3 recoilRotation = new Vector3(0f, 0f, 0f); // Current camera rotation caused by recoil.
	private Vector3 desiredRecoilRotation = new Vector3(0f, 0f, 0f); // Desired camera rotation caused by recoil (move recoilRotation to this).
	private float recoilRotationSpeed = 0f;

	// Aim down sight variables.
	private bool isAimingDownSight = false;
	private float aimDownSightDesiredFieldOfView = 60f; // Default Field Of View.
	private float aimDownSightSpeed = 5f; // Speed of changing the aimDownSight.

	// Constructor.
	public FirstPersonShooterGameCamera(Transform player, Transform aimTarget, Transform cameraTransform, Transform weapon) {
		this.player = player;
		this.aimTarget = aimTarget;
		this.camTransform = cameraTransform;
		this.weapon = weapon;
	}

	// Start is called once.
	public void Start() {
		camTransform.rotation = player.transform.rotation; // Initialize on player rotation so we can set the start angle by rotating the player in the editor.
		this.cameraRotation = camTransform.rotation.eulerAngles;
		this.recoilTimer = Time.time;

		// Disable the player model for first person.
		Transform playerModel = player.FindChild("TeddyANIMATED full");
		Renderer[] renderers = playerModel.GetComponentsInChildren<Renderer>();
		foreach(Renderer singleRenderer in renderers) {
			singleRenderer.enabled = false;
		}

	}

	// Update is called every frame.
	public void Update() {

		// Get the mouse input.
		float horizontal = Input.GetAxis("Mouse X");
		float vertical = Input.GetAxis("Mouse Y");
		if(vertical >  10f) { vertical =  10f; } // Limit y so it cant glitch by looking up or down too fast.
		if(vertical < -10f) { vertical = -10f; } // Limit y so it cant glitch by looking up or down too fast.

		// Set the camera position and rotation.
		camTransform.position = player.transform.position + (Vector3.up * 1.6f); // Set camera to player position at eye height.
		this.cameraRotation += new Vector3(-vertical, horizontal, 0f) * Time.deltaTime * mouseSensitivity;
		this.cameraRotation.x = (this.cameraRotation.x + 360f) % 360f; // Maps [-360, inf] to [0, 360].

		if(this.cameraRotation.x < 180f && this.cameraRotation.x > 90f - this.minRotationX) { this.cameraRotation.x = 90f - this.minRotationX; }
		if(this.cameraRotation.x > 180f && this.cameraRotation.x < 270f + (180f-this.maxRotationX)) { this.cameraRotation.x = 270f + (180f-this.maxRotationX); }

		camTransform.rotation = Quaternion.Euler(this.cameraRotation + this.recoilRotation);

		// Set the position of the aimPoint to the position we are looking at. TODO - Maybe only enable this when F is pressed, and implement a random raycast for shooting?
		this.updateAimTargetPos();

		// Update the recoil.
		this.updateRecoil();

		// Aim down sight.
		this.updateAimDownSight();

	}

	// updateAimTargetPos method.
	// Does a new raycast to find and set the new position of the aimTarget.
	private void updateAimTargetPos() {
		// Set the position of the aimPoint to the position we are looking at.
		RaycastHit hit;
		if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 1000f, int.MaxValue - LayerMask.GetMask("Ignore Aimpoint Raycast"))) { // Ignore layer 8 (player collider).
			aimTarget.position = camTransform.position + camTransform.forward * hit.distance;
		}
	}

	// updateRecoil method.
	// Checks if the player has fired and adds recoil per shot.
	private void updateRecoil() {
		
		// If the player has fired a gun, apply random recoil to the camera.
		if(this.Fired) {
			
			// Get the weapon recoil intensity (weapon property).
			WeaponController wcont = weapon.GetComponent<WeaponController>();
			Weapon selectedWeapon =  wcont.SelectedWeaponTransform.GetComponent(typeof(Weapon)) as Weapon;
			float weaponRecoilIntensity = (selectedWeapon != null) ? selectedWeapon.Recoil : 0f;

			// Apply the recoil.
			this.desiredRecoilRotation += new Vector3(Random.Range(-20f, -5f), Random.Range(-5f, 5f), Random.Range(-10f, 10f)) * weaponRecoilIntensity;
			this.recoilRotationSpeed += 3f;
			this.Fired = false;
		}

		// Custom FixedUpdate.
		if(Time.time - this.recoilTimer >= this.recoilFixedUpdateStepSize) {
			this.recoilTimer += this.recoilFixedUpdateStepSize;

			// Move recoilRotation towards the desiredRecoilRotation.
			Vector3 differenceVector = (this.desiredRecoilRotation - this.recoilRotation);
			if(differenceVector.sqrMagnitude < this.recoilRotationSpeed * this.recoilRotationSpeed) {
				this.recoilRotation = this.desiredRecoilRotation;
			} else {
				this.recoilRotation += differenceVector.normalized * this.recoilRotationSpeed;
			}

			// Decrease the recoilRotationSpeed and recoilRotation so it will return to 0 eventually.
			this.recoilRotationSpeed -= (this.recoilRotationSpeed <= 0 ? 0f : 0.05f);
			this.desiredRecoilRotation *= (this.desiredRecoilRotation.sqrMagnitude <= 0.1f ? 0f : 0.9f);
			this.recoilRotation *= (this.recoilRotation.sqrMagnitude <= 0.1f ? 0f : 1f);
		}

	}

	// updateAimDownSight method.
	// Checks for user input and handles the aim down sight.
	private void updateAimDownSight() {

		// Get if the player is in block placing mode or is he is sprinting. Stop aiming down sight if he is.
		bool isBlockPlacingMode = (player.parent.GetComponent<FireController>().BlockPlacingMode == null ? false : player.parent.GetComponent<FireController>().BlockPlacingMode);
		bool isSprinting = (player.GetComponent<NormalCharacterMotor>().getIsSprinting == null ? false : player.GetComponent<NormalCharacterMotor>().getIsSprinting);
		if((isBlockPlacingMode || isSprinting) && isAimingDownSight) {
			this.aimDownSightDesiredFieldOfView = 60f;
			isAimingDownSight = false;
		}

		// If the player is not in block placing mode and had clicked the right mouse button, aim down sight.
		if(Input.GetMouseButtonDown(1) && !isBlockPlacingMode) { // 1 = right mouse button.
			isAimingDownSight = !isAimingDownSight;
			if(isAimingDownSight) {
				this.aimDownSightDesiredFieldOfView = 30f;
			} else {
				this.aimDownSightDesiredFieldOfView = 60f;
			}
		}
		if(this.aimDownSightDesiredFieldOfView != this.camTransform.camera.fieldOfView) {
			this.camTransform.camera.fieldOfView += (this.aimDownSightDesiredFieldOfView - this.camTransform.camera.fieldOfView) * Time.deltaTime * this.aimDownSightSpeed;
		}
	}

	// setFired method.
	// Should be set to true when the player fired. Used to add recoil.
	public void setFired(bool state = true) {
		this.Fired = state;
	}
}
