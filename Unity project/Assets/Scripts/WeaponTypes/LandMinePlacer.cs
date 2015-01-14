using UnityEngine;
using System.Collections;

public class LandMinePlacer : Weapon {

	public GameObject landMine;
	public float maxDistance = 10f;

	public override void Fire(Vector3 from, Vector3 to){
		if(AmmoInClip>0){
			Vector3 dir = to - from;
			dir.Normalize();
			Ray ray = new Ray(from, dir);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, maxDistance, int.MaxValue - LayerMask.GetMask("Ignore Aimpoint Raycast"))) {
				if(!hit.transform.tag.Equals("Enemy")){
					TakeFromClip();
					GameObject.Instantiate(landMine, new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z), Quaternion.identity);
				}
			}
		}
	}
}
