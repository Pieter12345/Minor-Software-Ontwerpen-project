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
	public float mouseSensitivity = 150f;
	private float minRotationX = 10f; // 0 = down.
	private float maxRotationX = 170f; // 180 = up.
	private Transform lastSelectedWeapon; // Used to update the position of the weapon model on change.

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
	private float aimDownSightFovSpeed = 5f; // Speed of changing the aimDownSight FOV.
	private float aimDownSightTranslationSpeed = 0.3f;
	private float aimDownSightRotationSpeed = 0.3f;
	private float aimDownSightMouseSensitivityMultiplier = 0.4f; // Multiplied by the mouseSensitivity to be able to aim more precise.

	// Weapon reloading & switching variables.
	private float lastWeaponsRotationSpeed = 0; // Used to store the speed to rotate back with.

	// Weapon switching variables.
	private bool isSwitchingWeapons = false;
	private int weaponToSwitchTo;
	private float weaponsSwitchRotationSpeed = 0.5f;
	
	// Weapon reloading variables.
	private bool isReloadingWeapon = false;
	private float weaponsReloadRotationSpeed = 0.5f;

	// Weapon rotation when sprinting.
	private float weaponsSprintRotationSpeed = 0.3f;


	// ---------------------------------------------------------------------------------------------
	// Constructor.
	// ---------------------------------------------------------------------------------------------
	public FirstPersonShooterGameCamera(Transform player, Transform aimTarget, Transform cameraTransform, Transform weapon) {
		this.player = player;
		this.aimTarget = aimTarget;
		this.camTransform = cameraTransform;
		this.weapon = weapon;
	}


	// ---------------------------------------------------------------------------------------------
	// Start is called once.
	// ---------------------------------------------------------------------------------------------
	public void Start() {
//		camTransform.rotation = player.transform.rotation; // Initialize on player rotation so we can set the start angle by rotating the player in the editor.
		this.cameraRotation = camTransform.rotation.eulerAngles;
		this.recoilTimer = Time.time;

		// Disable the player model for first person.
		Transform playerModel = player.FindChild("TeddyANIMATED full");
		Renderer[] renderers = playerModel.GetComponentsInChildren<Renderer>();
		foreach(Renderer singleRenderer in renderers) {
			singleRenderer.enabled = false;
		}

		// Enable the currently used weapon model and attach it to the camera instead of the playermodel.
		this.lastSelectedWeapon = null; // Force weapon model position update.
		this.updateSelectedWeapon(true); // Update the weapon model position.
		
	}


	// ---------------------------------------------------------------------------------------------
	// Update is called every frame.
	// ---------------------------------------------------------------------------------------------
	public void Update() {

		// Get the mouse input.
		float horizontal = Input.GetAxis("Mouse X");
		float vertical = Input.GetAxis("Mouse Y");
		if(vertical >  10f) { vertical =  10f; } // Limit y so it cant glitch by looking up or down too fast.
		if(vertical < -10f) { vertical = -10f; } // Limit y so it cant glitch by looking up or down too fast.

		// Set the camera position and rotation.
		camTransform.position = player.transform.position + (Vector3.up * 1.6f); // Set camera to player position at eye height.
		this.cameraRotation += new Vector3(-vertical, horizontal, 0f) * Time.deltaTime * mouseSensitivity * (this.isAimingDownSight ? this.aimDownSightMouseSensitivityMultiplier : 1f);
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

		// Enable the currently used weapon model and attach it to the camera instead of the playermodel.
		this.updateSelectedWeapon();

		// Update weapon switching and reloading animation. These are both in one method because they change the same object rotation.
		this.updateWeaponChangeAndReload();

	
	}


	// ---------------------------------------------------------------------------------------------
	// updateAimTargetPos method.
	// Does a new raycast to find and set the new position of the aimTarget.
	// ---------------------------------------------------------------------------------------------
	private void updateAimTargetPos() {
		// Set the position of the aimPoint to the position we are looking at.
		RaycastHit hit;
		if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 1000f, int.MaxValue - LayerMask.GetMask("Ignore Aimpoint Raycast"))) { // Ignore layer 8 (player collider).
			aimTarget.position = camTransform.position + camTransform.forward * hit.distance;
		}
	}


	// ---------------------------------------------------------------------------------------------
	// updateRecoil method.
	// Checks if the player has fired and adds recoil per shot.
	// ---------------------------------------------------------------------------------------------
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


	// ---------------------------------------------------------------------------------------------
	// updateAimDownSight method.
	// Checks for user input and handles the aim down sight.
	// ---------------------------------------------------------------------------------------------
	private void updateAimDownSight() {

		// Get if the player is in block placing mode or is he is sprinting. Stop aiming down sight if he is.
		bool isBlockPlacingMode = player.parent.GetComponent<FireController>().BlockPlacingMode;
		bool isSprinting = player.GetComponent<NormalCharacterMotor>().getIsSprinting;
		if((isBlockPlacingMode || isSprinting || this.isReloadingWeapon || this.isSwitchingWeapons) && this.isAimingDownSight) {
			this.aimDownSightDesiredFieldOfView = 60f;
			this.isAimingDownSight = false;
		}

		// If the player is not in block placing mode and had clicked the right mouse button, aim down sight.
		if(Input.GetMouseButtonDown(1) && !isBlockPlacingMode) { // 1 = right mouse button.
			this.isAimingDownSight = !this.isAimingDownSight;
			if(this.isAimingDownSight) {
				this.aimDownSightDesiredFieldOfView = 30f;
			} else {
				this.aimDownSightDesiredFieldOfView = 60f;
			}
		}

		// Move the fieldOfView towards the desired fieldOfView.
		if(this.aimDownSightDesiredFieldOfView != this.camTransform.camera.fieldOfView) {
			this.camTransform.camera.fieldOfView += (this.aimDownSightDesiredFieldOfView - this.camTransform.camera.fieldOfView) * Time.deltaTime * this.aimDownSightFovSpeed;
		}

		// Move weapon model from current location to the middle of the screen (or back).
		WeaponController wcont = weapon.GetComponent<WeaponController>();
		Transform selectedWeaponTransform = wcont.SelectedWeaponTransform;
		if(this.isAimingDownSight) {

			// Move towards position.
			if(!selectedWeaponTransform.localPosition.Equals(wcont.SelectedWeaponType.firstPersonAimDownSightModelPosition())) {
				selectedWeaponTransform.localPosition += (wcont.SelectedWeaponType.firstPersonAimDownSightModelPosition() - selectedWeaponTransform.localPosition) * this.aimDownSightTranslationSpeed;
			}

			// Move towards rotation.
			if(!selectedWeaponTransform.localRotation.eulerAngles.Equals(wcont.SelectedWeaponType.firstPersonAimDownSightModelRotation())) {
				selectedWeaponTransform.localRotation = Quaternion.Euler(selectedWeaponTransform.localRotation.eulerAngles + (wcont.SelectedWeaponType.firstPersonAimDownSightModelRotation() - selectedWeaponTransform.localRotation.eulerAngles) * this.aimDownSightRotationSpeed);
			}

		} else {

			// Move towards position.
			if(!selectedWeaponTransform.localPosition.Equals(wcont.SelectedWeaponType.firstPersonModelPosition())) {
				selectedWeaponTransform.localPosition += (wcont.SelectedWeaponType.firstPersonModelPosition() - selectedWeaponTransform.localPosition) * this.aimDownSightTranslationSpeed;
			}
			
			// Move towards rotation.
			if(!selectedWeaponTransform.localRotation.eulerAngles.Equals(wcont.SelectedWeaponType.firstPersonModelRotation())) {
				selectedWeaponTransform.localRotation = Quaternion.Euler(selectedWeaponTransform.localRotation.eulerAngles + (wcont.SelectedWeaponType.firstPersonModelRotation() - selectedWeaponTransform.localRotation.eulerAngles) * this.aimDownSightRotationSpeed);
			}

		}
	}


	// ---------------------------------------------------------------------------------------------
	// updateSelectedWeapon method.
	// Checks if the right weapon is still shown and attaches it to the camera instead of the playermodel.
	// ---------------------------------------------------------------------------------------------
	private void updateSelectedWeapon(bool initialRun = false) {
		WeaponController wcont = weapon.GetComponent<WeaponController>();
		Transform selectedWeapon =  wcont.SelectedWeaponTransform;
		if(lastSelectedWeapon == null || !lastSelectedWeapon.Equals(selectedWeapon)) { // Only update on change.
			lastSelectedWeapon = selectedWeapon;

			// Enable renderer (also in children is available).
			foreach(Renderer renderer in selectedWeapon.GetComponentsInChildren<Renderer>()) {
				renderer.enabled = true;
			}
			if(selectedWeapon.renderer != null) {
				selectedWeapon.renderer.enabled = true; // Show weapon model.
			}

			// Set attachment properties.
			if(initialRun) {
				selectedWeapon.parent.SetParent(camTransform); // Attach to camera.
				selectedWeapon.parent.transform.localPosition = new Vector3(0f, 0f, 0f); // Reset Weapons position (parent).
				selectedWeapon.parent.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // Reset Weapons rotation (parent).
			}
			selectedWeapon.localPosition = wcont.SelectedWeaponType.firstPersonModelPosition(); // Set position.
			selectedWeapon.localRotation = Quaternion.Euler(wcont.SelectedWeaponType.firstPersonModelRotation()); // Set rotation.
		}
	}


	// ---------------------------------------------------------------------------------------------
	// setFired method.
	// Should be set to true when the player fired. Used to add recoil.
	// ---------------------------------------------------------------------------------------------
	public void setFired(bool state = true) {
		this.Fired = state;
	}


	// ---------------------------------------------------------------------------------------------
	// requestWeaponChange method.
	// Used to start the weapon switching animation.
	// ---------------------------------------------------------------------------------------------
	public void requestWeaponChange(int weaponID) {
		if(!this.isReloadingWeapon) {
			this.weaponToSwitchTo = weaponID; // Set the new weapon ID to switch to later.
			this.isSwitchingWeapons = true; // Start the animation on next update.
			this.isReloadingWeapon = false; // Cancel reloading if active.
		}
	}


//	// ---------------------------------------------------------------------------------------------
//	// updateWeaponChange method.
//	// Used to perform the weapon switching animation.
//	// ---------------------------------------------------------------------------------------------
//	public void updateWeaponChange() {
//		float desiredWeaponsRotation = this.isSwitchingWeapons ? 30f : 0f;
//		if(Mathf.Abs(weapon.localEulerAngles.x - desiredWeaponsRotation) > 0.05f) {
//			weapon.localEulerAngles += new Vector3((desiredWeaponsRotation - weapon.localEulerAngles.x) * this.weaponsSwitchRotationSpeed, 0f, 0f);
//		} else {
//			this.isSwitchingWeapons = false;
//			weapon.localEulerAngles = new Vector3(desiredWeaponsRotation, 0f, 0f);
//
//			// Switch the weapon model if it is at its max rotation (off the camera).
//			if(desiredWeaponsRotation != 0) {
//				WeaponController wcont = weapon.GetComponent<WeaponController>();
//				wcont.SelectedWeapon = this.weaponToSwitchTo;
//			}
//		}
//	}


	// ---------------------------------------------------------------------------------------------
	// ReloadWeapon method.
	// Used to add weapon reloading animations.
	// ---------------------------------------------------------------------------------------------
	public void ReloadWeapon() {
		WeaponController wcont = weapon.GetComponent<WeaponController>();
		Weapon selectedWeapon =  wcont.SelectedWeaponTransform.GetComponent(typeof(Weapon)) as Weapon;
		if(weapon.localEulerAngles.x == 0f && !this.isSwitchingWeapons && selectedWeapon.AmmoInClip != selectedWeapon.ClipSize) {
			this.isReloadingWeapon = true;
		}
	}


//	// ---------------------------------------------------------------------------------------------
//	// updateWeaponReload method.
//	// Used to perform the weapon reloading animation.
//	// ---------------------------------------------------------------------------------------------
//	public void updateWeaponReload() {
//		float desiredWeaponsRotation = this.isReloadingWeapon ? 30f : 0f;
//		if(Mathf.Abs(weapon.localEulerAngles.x - desiredWeaponsRotation) > 0.05f) {
//			weapon.localEulerAngles += new Vector3((desiredWeaponsRotation - weapon.localEulerAngles.x) * this.weaponsReloadRotationSpeed, 0f, 0f);
//		} else {
//			this.isReloadingWeapon = false;
//			weapon.localEulerAngles = new Vector3(desiredWeaponsRotation, 0f, 0f);
//			
//			// Reload the weapon if it is at its max rotation (off the camera).
//			if(desiredWeaponsRotation != 0) {
//				WeaponController wcont = weapon.GetComponent<WeaponController>();
//				wcont.Reload();
//			}
//		}
//	}


	// ---------------------------------------------------------------------------------------------
	// updateWeaponChangeAndReload method.
	// Used to perform the weapon reloading and switching animation.
	// ---------------------------------------------------------------------------------------------
	public void updateWeaponChangeAndReload() {
		float desiredWeaponsRotation = this.isReloadingWeapon || this.isSwitchingWeapons ? 30f : 0f;

		// Cancel weapon change and reload in sprint. Add sprint animation instead.
		if(Input.GetKey(KeyCode.LeftShift)) {
			desiredWeaponsRotation = 10f;
			this.isReloadingWeapon = false;
			this.isSwitchingWeapons = false;
			this.lastWeaponsRotationSpeed = this.weaponsSprintRotationSpeed;
		}

		if(this.isReloadingWeapon && this.isSwitchingWeapons) {
			this.lastWeaponsRotationSpeed = Mathf.Min (this.weaponsReloadRotationSpeed, this.weaponsSwitchRotationSpeed);
		} else if(this.isReloadingWeapon) {
			this.lastWeaponsRotationSpeed = this.weaponsReloadRotationSpeed;
		} else if(this.isSwitchingWeapons) {
			this.lastWeaponsRotationSpeed = this.weaponsSwitchRotationSpeed;
		}
		if(Mathf.Abs(weapon.localEulerAngles.x - desiredWeaponsRotation) > 0.05f) {
			weapon.localEulerAngles += new Vector3((desiredWeaponsRotation - weapon.localEulerAngles.x) * this.lastWeaponsRotationSpeed, 0f, 0f);
		} else {
			weapon.localEulerAngles = new Vector3(desiredWeaponsRotation, 0f, 0f);
			
			// Reload or switch the weapon if it is at its max rotation (off the camera).
			if(desiredWeaponsRotation != 0) {
				WeaponController wcont = weapon.GetComponent<WeaponController>();
				if(this.isReloadingWeapon) {
					this.isReloadingWeapon = false;
					wcont.Reload();
				}
				if(this.isSwitchingWeapons) {
					this.isSwitchingWeapons = false;
					wcont.SelectedWeapon = this.weaponToSwitchTo;
				}
			}
		}
	}


	// ---------------------------------------------------------------------------------------------
	// setMouseSensitivity method.
	// ---------------------------------------------------------------------------------------------
	public void setMouseSensitivity(float sensitivity) {
		this.mouseSensitivity = sensitivity;
	}


	// ---------------------------------------------------------------------------------------------
	// getIsSwitchingWeapons method.
	// Returns true if weapons are being switched.
	// ---------------------------------------------------------------------------------------------
	public bool getIsSwitchingWeapons() {
		return this.isSwitchingWeapons || weapon.localEulerAngles.x != 0; // isSwitchingWeapons turns false after 50% of the switch.
	}


	// ---------------------------------------------------------------------------------------------
	// getIsAimingDownSight method.
	// Returns true if the player is aiming down sight or moving there.
	// ---------------------------------------------------------------------------------------------
	public bool getIsAimingDownSight() {
		return this.isAimingDownSight;
	}


	// ---------------------------------------------------------------------------------------------
	// getIsReloading method.
	// Returns true if the player is aiming down sight or moving there.
	// ---------------------------------------------------------------------------------------------
	public bool getIsReloading() {
		return this.isReloadingWeapon || weapon.localEulerAngles.x != 0;
	}

}
