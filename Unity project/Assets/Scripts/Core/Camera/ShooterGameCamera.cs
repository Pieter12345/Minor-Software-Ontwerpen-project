using UnityEngine;
using System.Collections;

public class ShooterGameCamera {
	
	private Transform player;
	private Transform aimTarget;
	private Transform camTransform;
	private Transform weapon;
	
	public float smoothingTime = 0.5f;
	public Vector3 pivotOffset = new Vector3(1.3f, 0.4f,  0.0f);
	public Vector3 camOffset   = new Vector3(0.0f, 0.7f, -2.4f);
	public Vector3 closeOffset = new Vector3(0.35f, 1.7f, 0.0f);
	
	public float horizontalAimingSpeed = 270f;
	public float verticalAimingSpeed = 270f;
	public float maxVerticalAngle = 80f;
	public float minVerticalAngle = -80f;
	
	public float mouseSensitivity = 0.1f;

	private float angleH = 0;
	private float angleV = 0;
	private float maxCamDist = 3;
	private Vector3 smoothPlayerPos;

	// Recoil variables.
	private bool Fired = false;
	private float recoilFixedUpdateStepSize = 0.01f; // [sec]. Should be small, this acts like Time.timescale.
	private float recoilTimer; // [sec].
	private Vector3 recoilRotation = new Vector3(0f, 0f, 0f); // Current camera rotation caused by recoil.
	private Vector3 desiredRecoilRotation = new Vector3(0f, 0f, 0f); // Desired camera rotation caused by recoil (move recoilRotation to this).
	private float recoilRotationSpeed = 0f;

	// Constructor.
	public ShooterGameCamera(Transform player, Transform aimTarget, Transform cameraTransform, Transform weapon) {
		this.player = player;
		this.aimTarget = aimTarget;
		this.camTransform = cameraTransform;
		this.weapon = weapon;
	}
	
	// Use this for initialization.
	public void Start () {
		smoothPlayerPos = player.position;

		// Enable the player model for third person.
		Transform playerModel = player.FindChild("TeddyANIMATED full");
		Renderer[] renderers = playerModel.GetComponentsInChildren<Renderer>();
		foreach(Renderer singleRenderer in renderers) {
			singleRenderer.enabled = true;
		}
	}
		
	
	// Update is called once per frame.
	public void Update () {
		if (Time.deltaTime == 0 || Time.timeScale == 0 || player == null) 
			return;
		
		// Calculate Recoil rotation.
		
		// Get the weapon recoil property.
		WeaponController wcont = weapon.GetComponent<WeaponController>();
		Weapon selectedWeapon =  wcont.SelectedWeaponTransform.GetComponent(typeof(Weapon)) as Weapon;
		float weaponRecoilIntensity = (selectedWeapon != null) ? selectedWeapon.Recoil : 0f;

		// If the player has fired a gun, apply random recoil to the camera.
		if(this.Fired) {
			this.desiredRecoilRotation += new Vector3(Random.Range(-20f, -5f), Random.Range(-5f, 5f), Random.Range(-10f, 10f) * weaponRecoilIntensity);
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

		
		angleH += Mathf.Clamp(Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal2"), -1, 1) * horizontalAimingSpeed * Time.deltaTime ;
		
		angleV += Mathf.Clamp(Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical2"), -1, 1) * verticalAimingSpeed * Time.deltaTime;
		// limit vertical angle
		angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
		
		// Before changing camera, store the prev aiming distance.
		// If we're aiming at nothing (the sky), we'll keep this distance.
		float prevDist = (aimTarget.position - camTransform.position).magnitude;
		
		// Set camera rotation
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
		camTransform.rotation = Quaternion.Euler(aimRotation.eulerAngles + this.recoilRotation);
		
		// Find far and close position for the camera
		smoothPlayerPos = Vector3.Lerp(smoothPlayerPos, player.position, smoothingTime * Time.deltaTime);
		smoothPlayerPos.x = player.position.x;
		smoothPlayerPos.z = player.position.z;
		Vector3 farCamPoint = smoothPlayerPos + camYRotation * pivotOffset + aimRotation * camOffset;
		Vector3 closeCamPoint = player.position + camYRotation * closeOffset;
		float farDist = Vector3.Distance(farCamPoint, closeCamPoint);
		
		// Smoothly increase maxCamDist up to the distance of farDist
		maxCamDist = Mathf.Lerp(maxCamDist, farDist, 5 * Time.deltaTime);
		
		// Make sure camera doesn't intersect geometry
		// Move camera towards closeOffset if ray back towards camera position intersects something 
		RaycastHit hit;
		Vector3 closeToFarDir = (farCamPoint - closeCamPoint) / farDist;
		float padding = 0.3f;
		if (Physics.Raycast(closeCamPoint, closeToFarDir, out hit, maxCamDist + padding)) {
			maxCamDist = hit.distance - padding;
		}
		camTransform.position = closeCamPoint + closeToFarDir * maxCamDist;

		// Set the position of the aimPoint to the position we are looking at. TODO - Maybe only enable this when F is pressed, and implement a random raycast for shooting?
		this.updateAimTargetPos();
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

	// setFired method.
	// Should be set to true when the player fired.
	public void setFired(bool state = true) {
		this.Fired = state;
	}
}
