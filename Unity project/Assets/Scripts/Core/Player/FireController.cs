using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour {

	public Transform aimTarget;
	public Transform cameraTransform;
	public Transform transBlock;
	public Transform weaponController;
	public GameObject torch;

	public float blockPlaceTime = 0.5f;

	private bool blockPlacingMode = false;
	public bool BlockPlacingMode{ get { return blockPlacingMode; } }
	public int AmountOfBlocks{ get; set; }
	public int amountOfStartingBlocks = 20;
	private WeaponController weapons;
	private bool positionValid = true;
	private int selectedBlock = 2;
	private float timeLastBlock;

	void Start(){
		weapons = weaponController.GetComponent<WeaponController>();
		timeLastBlock = 0f;
		AmountOfBlocks = amountOfStartingBlocks;
	}

	// Update is called once per frame
	void Update () {
		if(Time.timeScale != 0f){

			gameCameraSelector gameCamera = cameraTransform.GetComponent<gameCameraSelector>();

			if(Input.GetButtonUp("Torch")){
				if(torch != null)
					torch.SetActive(!torch.activeInHierarchy);
			}

			if(Input.GetButton("Modifier")){ //Place block mode enabled
				blockPlacingMode = true;
				selectedBlock += Mathf.CeilToInt(Input.GetAxisRaw("Mouse ScrollWheel"));
				if(selectedBlock < 1){
					selectedBlock = WorldBlockManagement.blocks.Length;
				} else if(selectedBlock > WorldBlockManagement.blocks.Length){
					selectedBlock = 1;
				}
				UpdateAimBlockTex(selectedBlock);
			} else {
				blockPlacingMode = false;
				if(Input.GetAxisRaw("Mouse ScrollWheel") > 0) {
//					weapons.SelectNextWeaponUp();
					int nextWeapon = (weapons.SelectedWeapon + 1) % weapons.getWeaponArraySize();
					for(int i = 0; i <= weapons.getWeaponArraySize()-2; i++) { // Limit the amount of max loops to prevent infinite loop bugs.
						if(weapons.hasWeaponID(nextWeapon)) {
							gameCamera.requestWeaponChange(nextWeapon);
							break;
						}
						nextWeapon = (nextWeapon + 1) % weapons.getWeaponArraySize();
					}
				}
				else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0) {
//					weapons.SelectNextWeaponDown();
					int nextWeapon = (weapons.SelectedWeapon - 1 + weapons.getWeaponArraySize()) % weapons.getWeaponArraySize();
					for(int i = 0; i <= weapons.getWeaponArraySize()-2; i++) { // Limit the amount of max loops to prevent infinite loop bugs.
						if(weapons.hasWeaponID(nextWeapon)) {
							gameCamera.requestWeaponChange(nextWeapon);
							break;
						}
						nextWeapon = (nextWeapon - 1 + weapons.getWeaponArraySize()) % weapons.getWeaponArraySize();
					}
				}
			}

			if(Input.GetButtonUp("Fire1")){
				timeLastBlock = 0;
			}

			if(Input.GetButton("Fire1")){
				if (blockPlacingMode && positionValid){
					float interval = Time.time - timeLastBlock;
					if(interval > blockPlaceTime && AmountOfBlocks > 0){
						OnPlaceBlock();
						AmountOfBlocks--;
						timeLastBlock = Time.time;
					}
				} else if(!blockPlacingMode){
					OnFireWeapon();
				}
			}
			if(Input.GetButtonDown("Fire2")){
				if (blockPlacingMode){
					OnDestroyBlock();
				} else{
					//doSomething
				}
			}

			WeaponStats[] w = weapons.WeaponsInInventory;
			if(Input.GetKeyUp(KeyCode.Alpha1)){
				foreach(WeaponStats i in w){
					if((int) i == 0){
						gameCamera.requestWeaponChange(0);
//						weapons.SelectedWeapon = 0;
					}
				}
			} else if(Input.GetKeyUp(KeyCode.Alpha2)){
				foreach(WeaponStats i in w){
					if((int) i == 1){
						gameCamera.requestWeaponChange(1);
//						weapons.SelectedWeapon = 1;
					}
				}
			} else if(Input.GetKeyUp(KeyCode.Alpha3)){
				foreach(WeaponStats i in w){
					if((int) i == 2){
						gameCamera.requestWeaponChange(2);
//						weapons.SelectedWeapon = 2;
					}
				}
			} else if(Input.GetKeyUp(KeyCode.Alpha4)){
				foreach(WeaponStats i in w){
					if((int) i == 3){
						gameCamera.requestWeaponChange(3);
//						weapons.SelectedWeapon = 3;
					}
				}
			} else if(Input.GetKeyUp(KeyCode.Alpha5)){
				foreach(WeaponStats i in w){
					if((int) i == 4){
						gameCamera.requestWeaponChange(4);
//						weapons.SelectedWeapon = 4;
					}
				}
			} else if(Input.GetKeyUp(KeyCode.Alpha6)){
				foreach(WeaponStats i in w){
					if((int) i == 5){
						gameCamera.requestWeaponChange(5);
//						weapons.SelectedWeapon = 5;
					}
				}
			} else if(Input.GetKeyUp(KeyCode.Alpha7)){
				foreach(WeaponStats i in w){
					if((int) i == 6){
						gameCamera.requestWeaponChange(6);
//						weapons.SelectedWeapon = 6;
					}
				}
			} else if(Input.GetKeyUp(KeyCode.Alpha8)){
				foreach(WeaponStats i in w){
					if((int) i == 7){
						gameCamera.requestWeaponChange(7);
//						weapons.SelectedWeapon = 7;
					}
				}
			}


			if(Input.GetButtonDown("Reload"))
				Reload();
		}

		if(Input.GetKeyDown(KeyCode.F9))
			weapons.AddAmmoToCurrent(1000);
		UpdateBlockOutline();
	}

	void OnFireWeapon () {
		gameCameraSelector gameCamera = cameraTransform.GetComponent<gameCameraSelector>();
		if(!gameCamera.isSwitchingWeapons && !gameCamera.isReloading && !Input.GetKey(KeyCode.LeftShift)) {
			weapons.Fire(cameraTransform.position, aimTarget.position);
		}
	}

	void OnPlaceBlock(){
		HighScoreKeeper.BlockAction(true,true);
		WorldBlockManagement.setBlockAt(Mathf.FloorToInt(transBlock.position.x),
		                  Mathf.FloorToInt(transBlock.position.y),
		                  Mathf.FloorToInt(transBlock.position.z),
		                  (byte) selectedBlock);
		EnemyController.updatePathFinding();
	}

	void OnDestroyBlock(){
		Vector3 dir = aimTarget.position-cameraTransform.position;
		dir.Normalize();
		Ray ray = new Ray(cameraTransform.position, dir);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 10f, int.MaxValue - LayerMask.GetMask("Ignore Aimpoint Raycast"))) {
			
			Debug.DrawRay(cameraTransform.position, dir, Color.red);
			
			Vector3 destroyPosition = (new Vector3(Mathf.Floor(hit.point.x-0.5f*hit.normal.x),
			                                   Mathf.Floor(hit.point.y-0.5f*hit.normal.y),
			                                   Mathf.Floor(hit.point.z-0.5f*hit.normal.z))
			                       ) + (new Vector3(0.5f, 0.5f, 0.5f));

			WorldBlockManagement.breakBlockAt(Mathf.FloorToInt(destroyPosition.x),
			                  Mathf.FloorToInt(destroyPosition.y),
			                  Mathf.FloorToInt(destroyPosition.z));
		}
	}

	void UpdateBlockOutline(){
		positionValid = true;
		transBlock.gameObject.SetActive(blockPlacingMode);
		if(blockPlacingMode){
			Vector3 dir = aimTarget.position-cameraTransform.position;
			dir.Normalize();
			Ray ray = new Ray(cameraTransform.position, dir);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10f, int.MaxValue - LayerMask.GetMask("Ignore Aimpoint Raycast"))) {

				Debug.DrawRay(cameraTransform.position, dir, Color.red);

				transBlock.position = (new Vector3(Mathf.Floor(hit.point.x+0.5f*hit.normal.x),
				                                   Mathf.Floor(hit.point.y+0.5f*hit.normal.y),
				                                   Mathf.Floor(hit.point.z+0.5f*hit.normal.z))
				                       ) + (new Vector3(0.5f, 0.5f, 0.5f));
				if(Physics.CheckSphere(transBlock.position, 0.4f)) {
					positionValid = false;
				}
			}
		}

		if (positionValid && AmountOfBlocks > 0) {
			transBlock.renderer.material.color = Color.green;
		} else {
			transBlock.renderer.material.color = Color.red;
		}
	}

	void UpdateAimBlockTex(int BlockID) {
		Texture2D CurrentTexture = (Texture2D) WorldBlockManagement.blocks[BlockID-1].texture;
		transBlock.renderer.material.SetTexture("_MainTex",CurrentTexture);
	}
	
	public void Reload(){
		weapons.Reload();
	}
}
