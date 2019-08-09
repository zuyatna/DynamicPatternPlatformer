using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class PlayFabAuthenticat : MonoBehaviour {

	private string playFabPlayerIdCache;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		AuthenticateWithPlayFab();
		DontDestroyOnLoad(gameObject);
	}

	/*
     * Step 1
     * We authenticate current PlayFab user normally. 
     * In this case we use LoginWithCustomID API call for simplicity.
     * You can absolutely use any Login method you want.
     * We use PlayFabSettings.DeviceUniqueIdentifier as our custom ID.
     * We pass RequestPhotonToken as a callback to be our next step, if 
     * authentication was successful.
     */
	private void AuthenticateWithPlayFab() {
		LogMessage("PlayFab authenticating using custom ID...");

		PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest() {
			CreateAccount = true,
			CustomId = PlayFabSettings.DeviceUniqueIdentifier			
		}, RequestPhotonToken, OnPlayFabError);
	}

	/*
    * Step 2
    * We request Photon authentication token from PlayFab.
    * This is a crucial step, because Photon uses different authentication tokens
    * than PlayFab. Thus, you cannot directly use PlayFab SessionTicket and
    * you need to explicitely request a token. This API call requires you to 
    * pass Photon App ID. App ID may be hardcoded, but, in this example,
    * We are accessing it using convenient static field on PhotonNetwork class
    * We pass in AuthenticateWithPhoton as a callback to be our next step, if 
    * we have acquired token succesfully
    */
	private void RequestPhotonToken(LoginResult _obj) {
		LogMessage("PlayFab authenticated. Requesting photon token...");

		// We can player PlayFabId. This will come in handy during next step
		playFabPlayerIdCache = _obj.PlayFabId;

		PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest() {
			PhotonApplicationId = PhotonNetwork.PhotonServerSettings.name
		}, AuthenticationWithPhoton, OnPlayFabError);
	}

	/*
     * Step 3
     * This is the final and the simplest step. We create new AuthenticationValues instance.
     * This class describes how to authenticate a players inside Photon environment.
     */
	private void AuthenticationWithPhoton(GetPhotonAuthenticationTokenResult _obj) {
		LogMessage("Photon token acuired: " +_obj.PhotonCustomAuthenticationToken +" Authentication complete.");

		// We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
		var _customAuth = new AuthenticationValues {
			AuthType = CustomAuthenticationType.Custom
		};

		// We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID(!) and not username.
		_customAuth.AddAuthParameter("username", playFabPlayerIdCache); // Expected by PlayFab custom auth service

		// We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issue to your during previous step.
		_customAuth.AddAuthParameter("token", _obj.PhotonCustomAuthenticationToken);

		// We finally tell Photon to use this authentication parameters throughout the entire application.
		PhotonNetwork.AuthValues = _customAuth;
	}

	private void OnPlayFabError(PlayFabError _obj) {
		LogMessage(_obj.GenerateErrorReport());
	}

	public void LogMessage(string _message) {
		Debug.Log("PlayFab + Photon Example: " +_message);
	}
}