using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour {

	public Transform aimTarget;
	public Transform camera;
	public Transform transBlock;
	public Transform weaponController;

	public float blockPlaceTime = 0.5f;

	private bool blockMode = false;
	private WeaponController weapons;
	private bool positionValid = true;
	private int selectedBlock = 2;
	private float timeLastBlock;

	void Start(){
		weapons = weaponController.GetComponent<WeaponController>();
		timeLastBlock = 0;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Modifier")){ //Place block mode enabled
			blockMode = true;
			if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
				weapons.SelectNextWeaponUp();
			else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
				weapons.SelectNextWeaponDown();
		} else {
			blockMode = false;
			weapons.SelectedWeapon += Mathf.CeilToInt(Input.GetAxisRaw("Mouse ScrollWheel"));
		}
		if(selectedBlock < 1){
			selectedBlock = 3;
		} else if(selectedBlock > 3){
			selectedBlock = 1;
		}

		if(Input.GetButtonUp("Fire1")){
			timeLastBlock = 0;
		}

		if(Input.GetButton("Fire1")){
			if (blockMode && positionValid){
				float interval = Time.time - timeLastBlock;
				if(interval > blockPlaceTime){
					OnPlaceBlock();
					timeLastBlock = Time.time;
				}
			} else if(!blockMode){
				OnFireWeapon();
			}
		}
		if(Input.GetButtonDown("Fire2")){
			if (blockMode){
				OnDestroyBlock();
			} else{
				//doSomething
			}
		}

		if(Input.GetButtonDown("Reload"))
			Reload();

		if(Input.GetKeyDown(KeyCode.F9))
			weapons.AddAmmoToCurrent(10);
		UpdateBlockOutline();
	}

	void OnFireWeapon () {
		Screen.lockCursor = true;
		weapons.Fire(camera.position, aimTarget.position);
	}

	void OnPlaceBlock(){
		WorldBlockManagement.setBlockAt(Mathf.FloorToInt(transBlock.position.x),
		                  Mathf.FloorToInt(transBlock.position.y),
		                  Mathf.FloorToInt(transBlock.position.z),
		                  (byte) selectedBlock);
	}

	void OnDestroyBlock(){
		Vector3 dir = aimTarget.position-camera.position;
		dir.Normalize();
		Ray ray = new Ray(camera.position, dir);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 10)) {
			
			Debug.DrawRay(camera.position, dir, Color.red);
			
			Vector3 destroyPosition = (new Vector3(Mathf.Floor(hit.point.x-0.5f*hit.normal.x),
			                                   Mathf.Floor(hit.point.y-0.5f*hit.normal.y),
			                                   Mathf.Floor(hit.point.z-0.5f*hit.normal.z))
			                       ) + (new Vector3(0.5f, 0.5f, 0.5f));

			WorldBlockManagement.setBlockAt(Mathf.FloorToInt(destroyPosition.x),
			                  Mathf.FloorToInt(destroyPosition.y),
			                  Mathf.FloorToInt(destroyPosition.z),
			                  (byte)0);
		}
	}

	void UpdateBlockOutline(){
		positionValid = true;
		transBlock.gameObject.SetActive(blockMode);
		if(blockMode){
			Vector3 dir = aimTarget.position-camera.position;
			dir.Normalize();
			Ray ray = new Ray(camera.position, dir);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10)) {

				Debug.DrawRay(camera.position, dir, Color.red);

				transBlock.position = (new Vector3(Mathf.Floor(hit.point.x+0.5f*hit.normal.x),
				                                   Mathf.Floor(hit.point.y+0.5f*hit.normal.y),
				                                   Mathf.Floor(hit.point.z+0.5f*hit.normal.z))
				                       ) + (new Vector3(0.5f, 0.5f, 0.5f));
				if(Physics.CheckSphere(transBlock.position, 0.4f)) {
					positionValid = false;
				}
			}
		}

		if (positionValid) {
			transBlock.renderer.material.color = Color.green;
		} else {
			transBlock.renderer.material.color = Color.red;
		}
	}

	public void Reload(){
		weapons.Reload();
	}
}
