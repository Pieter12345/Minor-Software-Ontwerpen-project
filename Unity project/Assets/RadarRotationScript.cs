using UnityEngine;
using System.Collections;

public class RadarRotationScript : MonoBehaviour {
    public GameObject RadarPointer;
	
	// Update is called once per frame
	void Update () {
        this.RadarPointer.transform.Rotate(new Vector3(0f, 0f, -10f));
	}
}
