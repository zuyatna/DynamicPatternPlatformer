using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using Photon.Realtime;

public class AccountInfo : MonoBehaviour {

	private static AccountInfo instance;
    public static AccountInfo Instance {

        get { return instance;}
        set { instance = value;}
    }
    
    [SerializeField] private GetPlayerCombinedInfoResultPayload info;
    public GetPlayerCombinedInfoResultPayload Info {

        get { return info;}
        set { info = value;}
    }    

    private static string _playFabPlayerIdCache;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        if(instance != this)
        {
            instance = this;            
        }        
        DontDestroyOnLoad(gameObject);        
    }

    public static void Register(string _username, string _email, string _password, string _displayName) {

        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest() {

            TitleId = PlayFabSettings.TitleId, Email = _email, Username = _username, Password = _password, DisplayName = _displayName
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegister, OnAPIError);
    }

    public static void Login(string _username, string _password) {

        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest() {

            TitleId = PlayFabSettings.TitleId, Username = _username, Password = _password
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLogin, OnAPIError);        
    }

    public static void GetAccountInfo(string _playfabId) {

        GetPlayerCombinedInfoRequestParams paramInfo = new GetPlayerCombinedInfoRequestParams() {

            GetTitleData = true,
            GetUserInventory = true,
            GetUserAccountInfo = true,
            GetUserVirtualCurrency = true,
            GetPlayerProfile = true,
            GetPlayerStatistics = true,
            GetUserData = true,
            GetUserReadOnlyData = true
        };

        GetPlayerCombinedInfoRequest request = new GetPlayerCombinedInfoRequest() {

            PlayFabId = _playfabId,
            InfoRequestParameters = paramInfo
        };        

        PlayFabClientAPI.GetPlayerCombinedInfo(request, OnGotAccountInfo, OnAPIError);

        // Build the request, in this case, there are no parameters to set
        GetAccountInfoRequest req = new GetAccountInfoRequest();
    
        // Send the request, and provide 2 callbacks for when the request succeeds or fails
        PlayFabClientAPI.GetAccountInfo(req, OnGetAccountInfoSuccess, OnAPIError);
    }

    static void OnRegister(RegisterPlayFabUserResult _result) {

//        LoginManager.Instance.ConfirmRegister("Registered with: " +_result.PlayFabId);
        Debug.Log("Registered with: " +_result.PlayFabId);
    }

    static void OnLogin(LoginResult _result) {

        Debug.Log("Login with: " +_result.PlayFabId);
        PlayerPrefs.DeleteAll();

        // Here: load scenes
        GetAccountInfo(_result.PlayFabId);           
        SceneManager.LoadScene(1);

        //We can player PlayFabId. This will come in handy during next step
        _playFabPlayerIdCache = _result.PlayFabId;        

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest() {

			PhotonApplicationId = PhotonNetwork.PhotonServerSettings.name
		}, OnPhotonAuthSuccess, OnAPIError);
    }

    public static void AuthenticateWithPlayFab() {

        Debug.Log("PlayFab authenticating using custom ID...");

		PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest() {

			CreateAccount = true,
			CustomId = PlayFabSettings.DeviceUniqueIdentifier			
		}, RequestPhotonToken, OnPlayFabError);

        PlayerPrefs.DeleteAll();      

        var _playerName = "Player " + Random.Range(1, 100);

        // Set DisplayName in PlayerPrefs
        PlayerPrefs.SetString("DisplayName", _playerName);
        PhotonNetwork.NickName = _playerName;

        Debug.Log(PlayerPrefs.GetString("DisplayName"));           
    }

    static private void RequestPhotonToken(LoginResult _obj) {

		Debug.Log("PlayFab authenticated. Requesting photon token...");        
        
        // Here: load scenes
        SceneManager.LoadScene(1);
        
		// We can player PlayFabId. This will come in handy during next step
		_playFabPlayerIdCache = _obj.PlayFabId;

		PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest() {

			PhotonApplicationId = PhotonNetwork.PhotonServerSettings.name
		}, AuthenticationWithPhoton, OnPlayFabError);
	}

    static private void AuthenticationWithPhoton(GetPhotonAuthenticationTokenResult _obj) {

		Debug.Log("Photon token acuired: " +_obj.PhotonCustomAuthenticationToken +" Authentication complete.");        

        PhotonNetwork.NickName = PlayerPrefs.GetString("DisplayName");

		// We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
		var _customAuth = new AuthenticationValues {

			AuthType = CustomAuthenticationType.Custom
		};

		// We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID(!) and not username.
		_customAuth.AddAuthParameter("username", _playFabPlayerIdCache); // Expected by PlayFab custom auth service

		// We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issue to your during previous step.
		_customAuth.AddAuthParameter("token", _obj.PhotonCustomAuthenticationToken);

		// We finally tell Photon to use this authentication parameters throughout the entire application.
		PhotonNetwork.AuthValues = _customAuth;
	}

    static private void OnLoginSuccess(LoginResult result) {

        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.DeleteAll();

        GetAccountInfo(result.PlayFabId);
        SceneManager.LoadScene(1);
    }

    static private void OnLoginFailure(PlayFabError error) {

        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

	static private void OnPlayFabError(PlayFabError _obj) {

		Debug.Log(_obj.GenerateErrorReport());
	}


    static void OnGotAccountInfo(GetPlayerCombinedInfoResult _result) {

        Debug.Log("Updated account info...");
        Instance.Info = _result.InfoResultPayload;        
    }

    public static void OnAPIError(PlayFabError _error) {

//        LoginManager.Instance.ConfirmRegister(_error.ToString());
		Debug.LogError(_error);
	}

    /// <summary>
    /// Callback for GetAccountInfo Success
    /// </summary>
    /// <param name="Result"> Result - from the API Call</param>
    static void OnGetAccountInfoSuccess(GetAccountInfoResult result) {        
        
        Debug.Log(result.AccountInfo.TitleInfo.DisplayName);        

        // Set DisplayName in PlayerPrefs
        PlayerPrefs.SetString("DisplayName", result.AccountInfo.TitleInfo.DisplayName);        

        PhotonNetwork.NickName = result.AccountInfo.TitleInfo.DisplayName;
    }

    static void OnPhotonAuthSuccess(GetPhotonAuthenticationTokenResult _result) {

        Debug.Log("Photon token acquired: " +_result.PhotonCustomAuthenticationToken +" Authentication complete.");        

        // We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };

        // We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID (!) and not username.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);    // expected by PlayFab custom auth service
       
        // We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issues to your during previous step.
        customAuth.AddAuthParameter("token", _result.PhotonCustomAuthenticationToken);
        
        // We finally tell Photon to use this authentication parameters throughout the entire application.
        PhotonNetwork.AuthValues = customAuth;  
    }
}
