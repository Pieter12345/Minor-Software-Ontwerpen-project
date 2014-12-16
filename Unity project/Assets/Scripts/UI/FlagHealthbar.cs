using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlagHealthbar : MonoBehaviour {

	public Transform flag;
	private Health flagHP;
	
	private Slider slider;
	
	// Use this for initialization
	void Start () {
		if(flag!=null)
			flagHP = flag.GetComponent(typeof(Health)) as Health;
		slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(flagHP != null){
			slider.value = flagHP.HP/flagHP.maxHP;
		}
	}
}
