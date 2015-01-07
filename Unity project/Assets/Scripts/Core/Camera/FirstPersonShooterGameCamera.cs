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
			Debug.Log (singleRenderer.name);
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
		RaycastHit hit;
		if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 1000f, int.MaxValue - LayerMask.GetMask("Ignore Aimpoint Raycast"))) { // Ignore layer 8 (player collider).
			aimTarget.position = camTransform.position + camTransform.forward * hit.distance;
		}

		// Update the recoil.
		this.updateRecoil();

	}

	// updateRecoil method.
	// Checks if the player has fired and adds recoil per shot.
	private void updateRecoil() {

		// Get the weapon recoil property.
		WeaponController wcont = weapon.GetComponent<WeaponController>();
		Weapon selectedWeapon =  wcont.SelectedWeaponTransform.GetComponent(typeof(Weapon)) as Weapon;
		float weaponRecoilIntensity = selectedWeapon.Recoil;

		// If the player has fired a gun, apply random recoil to the camera.
		if(this.Fired) {
			this.desiredRecoilRotation += new Vector3(Random.Range(-20f, -5f), Random.Range(-5f, 5f), Random.Range(-10f, 10f));
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

	// setFired method.
	// Should be set to true when the player fired.
	public void setFired(bool state = true) {
		this.Fired = state;
	}
}
