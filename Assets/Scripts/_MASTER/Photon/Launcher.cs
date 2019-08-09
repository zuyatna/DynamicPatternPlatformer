using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {

	private static Launcher _instance;

	#region Private Variables

	/// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    private bool _isConnecting;	
		
	#endregion

	#region Public Variables

	/// <summary>
	/// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
	/// </summary>
	public byte maxPlayersPerRoom;

	#endregion

	#region Private Variables

	/// <summary>
	/// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
	/// </summary>
	private string _gameversion = "0.5beta";

	#endregion

	#region MonoBehaviour Callback

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake() {

		_instance = this;
		
		PhotonNetwork.NickName = PlayerPrefs.GetString("playerName", "No Name");
		Debug.Log("playerName: " +PhotonNetwork.NickName);
		
		// #Critical
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.AutomaticallySyncScene = true;

		// #Critical, we must first and foremost connect to Photon Online Server.
		PhotonNetwork.GameVersion = _gameversion;
		PhotonNetwork.ConnectUsingSettings();

		Application.targetFrameRate = 70;
	}

	#endregion	

	#region Photon.PunBehaviour CallBacks

	public override void OnJoinedLobby() {

		
		Debug.Log("On Joined Lobby");
	}

	public override void OnConnectedToMaster() {

		Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");

		// we don't want to do anything if we are not attempting to join a room.
		// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
		// we don't want to do anything.
		if (_isConnecting) {

			// #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJoinFailed()
			PhotonNetwork.JoinRandomRoom();
		}
		else {

			PhotonNetwork.JoinLobby(TypedLobby.Default);			
		}		
		
	}

	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {

		Debug.LogWarning("Launcher: OnDisconnectedFromPhoton() was called by PUN");
	}

	public override void OnJoinRandomFailed(short returnCode, string message) {

		Debug.Log("Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		
		// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
	}


	public override void OnJoinedRoom() {

		// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
    	_isConnecting = true;

		Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");        
        
            // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {

                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("Gameplay-Online");
            }        
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully...");
    }

    public void Connect() {

		if(PhotonNetwork.IsConnected) {

			Debug.Log("Clicked!!");
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{

			PhotonNetwork.GameVersion = _gameversion;
			PhotonNetwork.ConnectUsingSettings();
		}		
	}

	#endregion
}