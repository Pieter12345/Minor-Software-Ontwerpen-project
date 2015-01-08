using UnityEngine;
using System.Collections;

public class LandMinePlacer : Weapon {

	public GameObject landMine;

	public override void Fire(Vector3 from, Vector3 to){
		if(AmmoInClip>0){
			TakeFromClip();
			Vector3 dir = to - from;
			dir.Normalize();
			Ray ray = new Ray(from, dir);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100)) {
					GameObject.Instantiate(landMine, new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z), Quaternion.identity);

			}
		}
	}
}
