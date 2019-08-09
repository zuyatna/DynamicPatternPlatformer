using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

	public static LoginManager Instance;

	#region Login Field

	[SerializeField] private InputField loginUsername;
	[SerializeField] private InputField loginPassword;

	#endregion

	#region Register Field

	[SerializeField] private InputField registerUsername;
	[SerializeField] private InputField registerEmail;
	[SerializeField] private InputField registerPassword;
	[SerializeField] private InputField registerConfirmPassword;
	[SerializeField] private InputField registerDisplayName;

	#endregion

	[SerializeField] private GameObject loginPanel;
	[SerializeField] private GameObject registerPanel;
	[SerializeField] private GameObject confirmRegisterPanel;
	[SerializeField] private Text confirmText;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		Instance = this;
	}

	public void Login() {

		AccountInfo.Login(loginUsername.text, loginPassword.text);
	}

	public void Register() {

		if(registerConfirmPassword.text == registerPassword.text) {
			AccountInfo.Register(registerUsername.text, registerEmail.text, registerPassword.text, registerDisplayName.text);
		}
		else
		{
			Debug.LogError("Password don't match!");
		}
	}

	public void PlayAsGuest()
	{
		AccountInfo.AuthenticateWithPlayFab();
	}

	public void DisablePanel() {

		if(loginPanel.activeInHierarchy) {
			loginPanel.SetActive(false);
			confirmRegisterPanel.SetActive(false);
			registerPanel.SetActive(true);
		}
		else
		{
			loginPanel.SetActive(true);
			confirmRegisterPanel.SetActive(false);
			registerPanel.SetActive(false);
		}		
	}

	public void ConfirmRegister(string _result) {
		
		confirmRegisterPanel.SetActive(true);
		registerPanel.SetActive(false);
		loginPanel.SetActive(false);
		
		confirmText.text = _result;
	}
}
