using UnityEngine;
using System.Collections;

public class FireController : MonoBehaviour {

	public Transform aimTarget;
	public Transform camera;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
			OnFire();
	}

	void OnFire () {
		Screen.lockCursor = true;
		Vector3 dir = aimTarget.position-camera.position;
		dir.Normalize();
		Ray ray = new Ray(camera.position, dir);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 1000)) {

			Debug.Log("Shot hit " + hit.transform.name);

			Health hp = hit.transform.GetComponent<Health>();
			if(hp!=null)
				hp.Damage(1);
		}
	}
}
