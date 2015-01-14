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

	public GameObject loginPanel;
	public GameObject mainMenuPanel;

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
			if(get.text.Equals("Wrong Pass")) {
				// DEnied
				Debug.LogWarning("Wrong pass");
			} else if(get.text.Equals("Username Not Found")) {
				//
				Debug.LogWarning("Wrong user");
			} else { //login succes
				Debug.Log("Login successful");
				Debug.Log(get.text);
				SetCurrentUser(int.Parse(get.text));
				mainMenuPanel.SetActive(true);
				loginPanel.SetActive(false);
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
	public void SkipLogin(){
		mainMenuPanel.SetActive(true);
		loginPanel.SetActive(false);
	}
}
