using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsernameScript : MonoBehaviour {

	public static int CurrentUser = 0;

	public static string Username;
	public static string Password;
	
	public GameObject UserField;
	public GameObject PassField;
	
	private bool CoRoutineRunning = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator AddNewUser (string url) {
	
		CoRoutineRunning = true;
	
		var post = new WWWForm();
		post.AddField("Username",Username);
		post.AddField("Pass",Password);
		
		var get = new WWW(url,post);
		yield return get;
		
		if (get.error!=null) {
			Debug.Log(get.error);
		}
		else {
			Debug.Log(get.text);
			if (get.text == "Username Already Taken") {
				//Do User NAME Taken action
			}
			else {
				// Resume Game / LOG SUCCES
			}
		}
		CoRoutineRunning = false;
	}
	
	IEnumerator LoginUser (string url) {
	
		CoRoutineRunning = true;
	
		var post = new WWWForm();
		post.AddField("Username",Username);
		post.AddField("Pass",Password);
		
		var get = new WWW(url,post);
		yield return get;
		
		if (get.error!=null) {
			Debug.Log(get.error);
		}
		else {
			Debug.Log(get.text);
			if(get.text == "Wrong Pass") {
				// DEnied
			}
			if(get.text == "Username Not Found") {
				//
			}
			
			else { //login succes
				SetCurrentUser(int.Parse(get.text));
			}
		}
		CoRoutineRunning = false;
	}

	
	public void UserSetInfo() {
		Username = UserField.GetComponent<InputField>().text;
	}
	
	public void PassSetInfo() {
		Password = PassField.GetComponent<InputField>().text;
	}
	
	public void UserLogin() {
		if (CoRoutineRunning!=true) {
			StartCoroutine(LoginUser("http://drproject.twi.tudelft.nl:8083/login"));
		}			
	}
	
	public void PostUserInfo () {
		if (CoRoutineRunning!=true && Password!="" && Username!="") {
			StartCoroutine(AddNewUser("http://drproject.twi.tudelft.nl:8083/newuser"));
		}	
	}
	
	private void SetCurrentUser(int UserID) {
		CurrentUser = UserID;
	}
	
}
