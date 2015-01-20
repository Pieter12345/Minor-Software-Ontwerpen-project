using UnityEngine;
using System.Collections;

public class SkipLogInScreen : MonoBehaviour {

	public UsernameScript us;

	// Use this for initialization
	void Start () {
		if(CurrentUser.LoggedIn)
			us.SkipLogin();
	}

}
