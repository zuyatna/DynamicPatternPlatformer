using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;

public class LevelManager : MonoBehaviourPunCallbacks, IPunObservable
{

	public static LevelManager Instance;

	#region Public Variables

	[Tooltip("The prefab to use for representating the player")]
    public GameObject playerPrefab;

	[Header("Leave Room")]
	public Button leaveButton;

	public List<Transform> itemsDropPosition;
	public List<GameObject> itemsDrop;

	[Tooltip("Time for drop")]
	private float timerDrop;
	public float tempTimerDrop;
	private int tempRandomItemDrop = 0;
	private int tempRandomDropPosition = 1;

	private int tempPlayer = 1;
	public Text playerInRoom;

	private int players = 1;

	[HideInInspector] public bool activeCameraMoving;
	[HideInInspector] public bool activeTimer;

	public Text waitingPlayer;
	public GameObject loadingObject;
	public int maxPlayer;

	[Tooltip("Main Camera")]
	public GameObject movingCamera;
	public float time; //detik
    private float minutes;
	private float seconds;
		
	#endregion

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		Instance = this;		

		if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab References. Please set it up GameObject 'GameManager'", this);
        }
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
			{
				Debug.Log("We are Instantiating LocalPlayer from "+Application.loadedLevelName);
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

				PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(Random.Range(-5, 7), -2.30f, 0f), Quaternion.identity, 0);				
			}
			else
			{
				Debug.Log("Ignoring scene load for "+Application.loadedLevelName);
			}
        }

		leaveButton.onClick.AddListener(LeaveRoom);		

		timerDrop = tempTimerDrop;


		foreach (var player in PhotonNetwork.PlayerListOthers)
		{
			tempPlayer++;
			maxPlayer -= tempPlayer;

			photonView.RPC("RPCPlayers", RpcTarget.All, 1f);

			if(tempPlayer > 0)
			{
				playerInRoom.text = "PIR: " +tempPlayer +" RPC: " +players;
				waitingPlayer.text = "Waiting Until " +maxPlayer +" Players";
			}	
		}			

		Application.targetFrameRate = 70;	
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{

		if(photonView.IsMine)
		{
			if(players >= 2)
			{
				// activeCameraMoving = true;
				loadingObject.SetActive(false);			
			}

			if(activeCameraMoving)
			{
				photonView.RPC("CameraMoving", RpcTarget.All);
			}

			if(activeTimer)
			{
				minutes = Mathf.Floor(time / 60);
				seconds = Mathf.RoundToInt(time % 60);
				time -= Time.deltaTime;
				
				if (time <= 0)
				{                
					GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MovingCamera>().enabled = false;               			
					photonView.RPC("RPCLeaveRoom", RpcTarget.All);					
				}
			}				   
		}
		
	}

	#region Photon Messages

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {

		Debug.Log("OnPhotonPlayerConnected() " +newPlayer.NickName);

		if(PhotonNetwork.IsMasterClient) {

			Debug.Log("OnPhotonPlayerConnected isMasterClient " +PhotonNetwork.IsMasterClient);
			LoadArena();
		}
	}

	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {

		Debug.Log("OnPhotonPlayerDisconnected isMasterClient " +PhotonNetwork.IsMasterClient);
		LoadArena();
	}

	/// <summary>
	/// Called when the local player left the room. We need to load the launcher scene.
	/// </summary>
	public override void OnLeftRoom() {

		SceneManager.LoadScene(2);
	}

	#endregion

	#region Public Methods

	public void LeaveRoom() {

		PhotonNetwork.LeaveRoom();
	}

	#endregion	

	#region Private Methods

	void LoadArena() {

		if(!PhotonNetwork.IsMasterClient) {

			Debug.LogError("PhotonNetwork: Trying to Load a Level but we are not the master client");
		}

		Debug.Log("PhotonNetwork: Loading Level: " +PhotonNetwork.CurrentRoom.PlayerCount);
		PhotonNetwork.LoadLevel(5);
	}

	#endregion

	#region RPC			

	[PunRPC]
	private void RPCItemsDrop()
	{		
		var _randomDrop = Random.Range(0, itemsDrop.Count);
		Debug.LogWarning("item drop: " +_randomDrop);

		var _randomPosition = Random.Range(0, itemsDropPosition.Count);
		Debug.LogWarning("drop position: " +_randomPosition);

		if(tempRandomItemDrop == _randomDrop)
		{
			_randomDrop = Random.Range(0, itemsDrop.Count);
			Debug.LogWarning("item drop [again]: " +_randomDrop);			
		}
		else
		{
			tempRandomItemDrop = _randomDrop;
		}
		
		if(tempRandomDropPosition == _randomPosition)
		{
			_randomPosition = Random.Range(0, itemsDropPosition.Count);
			Debug.LogWarning("drop position [again]: " +_randomPosition);			
		}
		else
		{
			tempRandomDropPosition = _randomPosition;
		}

		Instantiate(itemsDrop[tempRandomItemDrop], new Vector3(itemsDropPosition[tempRandomDropPosition].position.x, itemsDropPosition[tempRandomDropPosition].position.y, 90), Quaternion.identity);
	}

	[PunRPC]
	private void CameraMoving()
	{		
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MovingCamera>().enabled = true;
		activeCameraMoving = false;
		activeTimer = true;
	}

	[PunRPC]
	private void RPCLeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	[PunRPC]
	private void RPCPlayers(int _player)
	{
		players += _player;
	}

    #endregion

	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // throw new System.NotImplementedException();
    }
}
