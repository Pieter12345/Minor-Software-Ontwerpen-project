using UnityEngine;
using System.Collections;

public class FirstPersonShooterGameCamera : MonoBehaviour {

	public Transform player;
	public Transform aimTarget;
	
	public float smoothingTime = 0.5f;
	public Vector3 pivotOffset = new Vector3(1.3f, 0.4f,  0.0f);
	public Vector3 camOffset   = new Vector3(0.0f, 0.7f, -2.4f);
	public Vector3 closeOffset = new Vector3(0.35f, 1.7f, 0.0f);
	
	public float horizontalAimingSpeed = 270f;
	public float verticalAimingSpeed = 270f;
	public float maxVerticalAngle = 80f;
	public float minVerticalAngle = -80f;
	
//	public float mouseSensitivity = 0.1f;
	
	public Texture reticle;
	
	private float angleH = 0;
	private float angleV = 0;
	private float maxCamDist = 3;
	private LayerMask mask;
	private Vector3 smoothPlayerPos;
	
	//Recoil parameters
	private float maxRecoil;
	private float CameraRecoil;
	private bool RecoilActive = false;
//	public bool Fired = false;






	// Variables & Constants.
	public float mouseSensitivity = 300f;
	
	public Transform weapon;
	private bool Fired = false;
//	private int amountOfActiveRecoilShots = 0;
	private float recoilFixedUpdateStepSize = 0.01f; // [sec]. Should be small, this acts like Time.timescale.
	private float recoilTimer; // [sec].
	private Vector3 recoilRotation = new Vector3(0f, 0f, 0f); // Current camera rotation caused by recoil.
	private Vector3 desiredRecoilRotation = new Vector3(0f, 0f, 0f); // Desired camera rotation caused by recoil (move recoilRotation to this).
	private float recoilRotationSpeed = 0f;
	private Vector3 cameraRotation;

	private float minRotationX = 10f; // 0 = down.
	private float maxRotationX = 170f; // 180 = up.

	// Start is called once.
	void Start() {
		transform.rotation = player.transform.rotation; // Initialize on player rotation so we can set the start angle by rotating the player in the editor.
		this.cameraRotation = transform.rotation.eulerAngles;
		this.recoilTimer = Time.time;

		/* TODO - Use this code to disable the player model and add a weapon + arm somewhere. Maybe just disable his head...
		// Disable the model for first person.
		Transform playerModel = player.FindChild("teddybeerRennen2");
		Renderer[] renderers = playerModel.GetComponentsInChildren<Renderer>();
		foreach(Renderer singleRenderer in renderers) {
			singleRenderer.enabled = false;
		}
		*/

	}

	// Update is called every frame.
	void Update() {

		// Get the mouse input.
		float horizontal = Input.GetAxis("Mouse X");
		float vertical = Input.GetAxis("Mouse Y");
		if(vertical >  10f) { vertical =  10f; } // Limit y so it cant glitch by looking up or down too fast.
		if(vertical < -10f) { vertical = -10f; } // Limit y so it cant glitch by looking up or down too fast.

		// Set the camera position and rotation.
		transform.position = player.transform.position + (Vector3.up * 1.6f); // Set camera to player position at eye height.
		this.cameraRotation += new Vector3(-vertical, horizontal, 0f) * Time.deltaTime * mouseSensitivity;
		this.cameraRotation.x = (this.cameraRotation.x + 360f) % 360f; // Maps [-360, inf] to [0, 360].

		if(this.cameraRotation.x < 180f && this.cameraRotation.x > 90f - this.minRotationX) { this.cameraRotation.x = 90f - this.minRotationX; }
		if(this.cameraRotation.x > 180f && this.cameraRotation.x < 270f + (180f-this.maxRotationX)) { this.cameraRotation.x = 270f + (180f-this.maxRotationX); }

		transform.rotation = Quaternion.Euler(this.cameraRotation + this.recoilRotation);

		// Set the position of the aimPoint to the position we are looking at. TODO - Maybe only enable this when F is pressed, and implement a random raycast for shooting?
		float aimTargetDist;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f)) {
			aimTarget.position = transform.position + transform.forward * hit.distance;
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
//			this.desiredRecoilRotation += transform.rotation.eulerAngles + new Vector3(Random.Range(0f, 5f), Random.Range(-1f, 1f), Random.Range(-2f, 2f));
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

	// setFired method. TODO - Change the reference to this in SimpleGun.cs. This now looks for 2 scripts. (Merge scripts first).
	// Should be set to true when the player fired.
	public void setFired(bool state = true) {
		this.Fired = state;
	}


	/*
	// Use this for initialization
	void Start () {
		smoothPlayerPos = player.position;
	}
	
	
	// Update is called once per frame
	void LateUpdate () {
		if (Time.deltaTime == 0 || Time.timeScale == 0 || player == null) 
			return;
		
		// Set Recoil Degrees
		
		// weapon property
		WeaponController wcont = weapon.GetComponent<WeaponController>();
		Weapon selected =  wcont.SelectedWeaponTransform.GetComponent(typeof(Weapon)) as Weapon;
		float weaponRecoilIntensity = selected.Recoil; 
		
		if (Fired == true) {
			CameraRecoil = weaponRecoilIntensity * 5.0f;		
			maxRecoil = CameraRecoil;
			RecoilActive = true;
			Fired = false;
		}
		
		if (CameraRecoil > -maxRecoil && RecoilActive == true) {
			if (true) {
				CameraRecoil -= weaponRecoilIntensity * 1.0f;
			}
			if (CameraRecoil <= -maxRecoil) {
				RecoilActive = false;
				CameraRecoil = 0.0f;
			}
		}
		
		
		angleH += Mathf.Clamp(Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal2"), -1, 1) * horizontalAimingSpeed * Time.deltaTime ;
		
		angleV += Mathf.Clamp(Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical2"), -1, 1) * verticalAimingSpeed * Time.deltaTime + CameraRecoil;
		// limit vertical angle
		angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
		
		// Before changing camera, store the prev aiming distance.
		// If we're aiming at nothing (the sky), we'll keep this distance.
		float prevDist = (aimTarget.position - transform.position).magnitude;
		
		// Set aim rotation
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
		transform.rotation = aimRotation;
		
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
		if (Physics.Raycast(closeCamPoint, closeToFarDir, out hit, maxCamDist + padding, mask)) {
			maxCamDist = hit.distance - padding;
		}
		transform.position = closeCamPoint + closeToFarDir * maxCamDist;
		
		// Do a raycast from the camera to find the distance to the point we're aiming at.
		float aimTargetDist;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 1000)) {
			aimTargetDist = hit.distance + 0.05f;
		}
		else {
			// If we're aiming at nothing, keep prev dist but make it at least 5.
			aimTargetDist = Mathf.Max(5, prevDist);
		}
		
		// Set the aimTarget position according to the distance we found.
		// Make the movement slightly smooth.
		aimTarget.position = transform.position + transform.forward * aimTargetDist;
	}

*/
	
	void OnGUI () {
		if (Time.time != 0 && Time.timeScale != 0)
			GUI.DrawTexture(new Rect(Screen.width/2-(reticle.width*0.5f), Screen.height/2-(reticle.height*0.5f), reticle.width, reticle.height), reticle);
	}
}
