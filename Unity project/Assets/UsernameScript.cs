using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsernameScript : MonoBehaviour {

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
		post.AddField("Username", Username);
		post.AddField("Pass", Password);
		
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
		post.AddField("Username", (Username == null ? "Anonymous" : Username));
		post.AddField("Pass", (Password == null ? "" : Password));
		
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
				CurrentUser.CurrentUserID = (int.Parse(get.text));
				mainMenuPanel.SetActive(true);
				loginPanel.SetActive(false);
				CurrentUser.LoggedIn = true;
			}
		}
		
		CoRoutineRunning = false;		
	}

	IEnumerator CurrentUserName (string url) {
	
		CoRoutineRunning = true;
	
		var post = new WWWForm();
		post.AddField("UserID",CurrentUser.CurrentUserID);
		
		var get = new WWW(url,post);
		yield return get;
		
		if (get.error!=null) {
			Debug.Log(get.error);
		}
		else {
			CurrentUser.CurrentUsername = get.text;
			Debug.Log(get.text);
		}
		CoRoutineRunning = false;
		
	}
	
	public void GetUsername() {
		if (CoRoutineRunning!=true) {
			StartCoroutine(CurrentUserName("http://drproject.twi.tudelft.nl:8083/UserID"));
		}			
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
		if(Username == "") { Debug.Log("[INFO] [UsernameScript] Please enter a username."); return; }
		if(Password == "") { Debug.Log("[INFO] [UsernameScript] Please enter a password."); return; }
		if(CoRoutineRunning) { Debug.Log("[INFO] [UsernameScript] Your previous request is still being processed. Please wait a moment."); return; }
		StartCoroutine(AddNewUser("http://drproject.twi.tudelft.nl:8083/newuser"));
	}

	public void SkipLogin(){
		mainMenuPanel.SetActive(true);
		loginPanel.SetActive(false);
	}
}
