using UnityEngine;
using System.Collections;

public class PopUpButton : MonoBehaviour {

	public GameObject ToDisable;

	public void Disable(){
		if(ToDisable == null)
			ToDisable = gameObject;
		ToDisable.SetActive(false);
	}

	public void Kill(){
		if(ToDisable == null)
			ToDisable = gameObject;
		Destroy (ToDisable);
	}
}
