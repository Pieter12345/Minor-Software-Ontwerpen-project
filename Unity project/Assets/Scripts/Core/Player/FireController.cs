using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour {

	public Transform aimTarget;
	public Transform camera;
	public Transform transBlock;
//	public Transform blockManager;
	public Transform weaponController;

	private bool blockMode = false;
//	private WorldBlockManagement blocks;
	private WeaponController weapons;
	private bool positionValid = true;
	private int selectedBlock = 2;

	void Start(){
//		blocks = blockManager.GetComponent<WorldBlockManagement>();
		weapons = weaponController.GetComponent<WeaponController>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Modifier")){ //Place block mode enabled
			blockMode = true;
			selectedBlock += Mathf.CeilToInt(Input.GetAxisRaw("Mouse ScrollWheel"));
		} else {
			blockMode = false;
			weapons.SelectedWeapon += Mathf.CeilToInt(Input.GetAxisRaw("Mouse ScrollWheel"));
		}
		if(selectedBlock < 1){
			selectedBlock = 3;
		} else if(selectedBlock > 3){
			selectedBlock = 1;
		}


		if(Input.GetButtonDown("Fire1")){
			if (blockMode && positionValid){
				OnPlaceBlock();
			} else{
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
