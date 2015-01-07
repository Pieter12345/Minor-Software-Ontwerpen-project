using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class ReloadText : MonoBehaviour {

	public GameObject weapons;

	private Text txt;
	private WeaponController wContr;

	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
		txt.enabled = false;
		wContr = weapons.GetComponent<WeaponController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(wContr == null || weapons == null) { return; }
		txt.enabled = (wContr.HasWeapon && wContr.SelectedWeaponInClip < 1 && wContr.SelectedWeaponInPool > 0);

	}
}
