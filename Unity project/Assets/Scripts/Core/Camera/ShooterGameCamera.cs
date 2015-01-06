using UnityEngine;
using System.Collections;

public class ShooterGameCamera {
	
	private Transform player;
	private Transform aimTarget;
	private Transform camTransform;

	private Texture reticle;
	
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
	
	// Recoil parameters.
	private float maxRecoil;
	private float CameraRecoil;
	private bool RecoilActive = false;
	public bool Fired = false;
	private Transform weapon;

	// Constructor.
	public ShooterGameCamera(Transform player, Transform aimTarget, Transform cameraTransform, Texture crosshair, Transform weapon) {
		this.player = player;
		this.aimTarget = aimTarget;
		this.camTransform = cameraTransform;
		this.reticle = crosshair;
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
		
		// Set Recoil Degrees.
		
		/*weapon property*/
		WeaponController wcont = weapon.GetComponent<WeaponController>();
		Weapon selected =  wcont.SelectedWeaponTransform.GetComponent(typeof(Weapon)) as Weapon;
		float weaponRecoilIntensity = (selected != null) ? selected.Recoil : 0f; 
		
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
		float prevDist = (aimTarget.position - camTransform.position).magnitude;
		
		// Set aim rotation
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
		camTransform.rotation = aimRotation;
		
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
		
		// Do a raycast from the camera to find the distance to the point we're aiming at.
		float aimTargetDist;
		if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 1000)) {
			aimTargetDist = hit.distance + 0.05f;
		}
		else {
			// If we're aiming at nothing, keep prev dist but make it at least 5.
			aimTargetDist = Mathf.Max(5, prevDist);
		}
		
		// Set the aimTarget position according to the distance we found.
		// Make the movement slightly smooth.
		aimTarget.position = camTransform.position + camTransform.forward * aimTargetDist;
	}

	// setFired method.
	// Should be set to true when the player fired.
	public void setFired(bool state = true) {
		this.Fired = state;
	}

//	void OnGUI () {
//		if (Time.time != 0 && Time.timeScale != 0)
//			GUI.DrawTexture(new Rect(Screen.width/2-(reticle.width*0.5f), Screen.height/2-(reticle.height*0.5f), reticle.width, reticle.height), reticle);
//	}
}
