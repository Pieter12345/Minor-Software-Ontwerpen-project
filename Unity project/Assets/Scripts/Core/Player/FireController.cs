﻿using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour {

	public Transform aimTarget;
	public Transform camera;
	public Transform transBlock;
	public Transform blockManager;

	private bool blockMode = false;
	private WorldBlockManagement blocks;

	void Start(){
		blocks = blockManager.GetComponent<WorldBlockManagement>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Modifier")){ //Place block mode enabled
			blockMode = true;
		} else {
			blockMode = false;
		}

		if(Input.GetButtonDown("Fire1")){
			if (blockMode){
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

		UpdateBlockOutline();
	}

	void OnFireWeapon () {
		Screen.lockCursor = true;
		Vector3 dir = aimTarget.position-camera.position;
		dir.Normalize();
		Ray ray = new Ray(camera.position, dir);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100)) {

			Debug.Log("Shot hit " + hit.transform.name);

			Health hp = hit.transform.GetComponent<Health>();
			if(hp!=null)
				hp.Damage(1);
		}
	}

	void OnPlaceBlock(){
		blocks.setBlockAt(Mathf.FloorToInt(transBlock.position.x),
		                  Mathf.FloorToInt(transBlock.position.y),
		                  Mathf.FloorToInt(transBlock.position.z),
		                  (byte)1);
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

			blocks.setBlockAt(Mathf.FloorToInt(destroyPosition.x),
			                  Mathf.FloorToInt(destroyPosition.y),
			                  Mathf.FloorToInt(destroyPosition.z),
			                  (byte)0);
		}
	}

	void UpdateBlockOutline(){
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
			}
		}
	}
}
