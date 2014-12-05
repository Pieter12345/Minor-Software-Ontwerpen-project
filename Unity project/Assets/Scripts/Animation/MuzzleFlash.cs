using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {

	public float fadeSpeed = 0.25f;
	public float epsilon = 0.05f;
	public float baseAlpha = 64f;
	public float baseIntensity = 0.35f;

	// Use this for initialization
	void Start () {
		if(fadeSpeed >= 1f)
			fadeSpeed = 0f;
		renderer.enabled = false;
		light.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Color c = renderer.material.GetColor("_TintColor");
		renderer.material.SetColor("_TintColor", new Color(c.r, c.g, c.b, c.a*fadeSpeed));
		light.intensity = light.intensity*fadeSpeed;
		if(c.a < epsilon){
			renderer.enabled = false;
			light.enabled = false;
		}
	}
	public void Flash(){
		renderer.enabled = true;
		light.enabled = true;
		Reset();
	}

	private void Reset(){
		Color c = renderer.material.GetColor("_TintColor");
		renderer.material.SetColor("_TintColor", new Color(c.r, c.g, c.b, baseAlpha));
		light.intensity = baseIntensity;
	}

}
