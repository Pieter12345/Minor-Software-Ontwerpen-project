using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class HitOverlay : MonoBehaviour {
	
	private Image im;

	public float damageToAlphaScaling = 1f;
	public float fadeSpeed = 100f;
	public float epsilon = 0.01f;

	public float Alpha{ 
		get {
			if(im!=null)
				return im.color.a*255;
			else
				return 0f;
		}
		set{
			if(im!=null)
				im.color = new Color(im.color.r, im.color.g, im.color.b, value/255);
		}
	}

	// Use this for initialization
	void Start () {
		im = GetComponent<Image>();
		Alpha = 0f;
	}

	// Update is called once per frame
	void Update () {
		if(Alpha < epsilon){
			Alpha = 0f;
			return;
		}
		Alpha -= Time.deltaTime*fadeSpeed;
	}

	public void Hit(float damage){
		Alpha += damage*damageToAlphaScaling;
	}
}
