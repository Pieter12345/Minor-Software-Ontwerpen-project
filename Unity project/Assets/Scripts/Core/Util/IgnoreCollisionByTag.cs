using UnityEngine;
using System.Collections;

public class IgnoreCollisionByTag : MonoBehaviour {

	public string[] toIgnore;

	// Use this for initialization
	void OnEnable () {
		if (collider == null)
			return;

		if(toIgnore.Length > 0){
			foreach(string s in toIgnore){
				GameObject[] gos = GameObject.FindGameObjectsWithTag(s);
				if(gos.Length>0){
					foreach(GameObject o in gos){
						Collider col = o.collider;
						if(col != null)
							Physics.IgnoreCollision(col, collider);
					}
				}
			}
		}
	}

}
