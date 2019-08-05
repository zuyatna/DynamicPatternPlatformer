using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class LevelManager : MonoBehaviourPunCallbacks, IPunObservable
{

	public static LevelManager Instance;

	#region Public Variables

	[Tooltip("The prefab to use for representation the player")]
    public GameObject playerPrefab;

	[Header("Leave Room")]
	public Button leaveButton;

	public List<Transform> itemsDropPosition;
	public List<GameObject> itemsDrop;

	[Tooltip("Time for drop")]
	public float tempTimerDrop;
	private int _tempRandomItemDrop = 0;
	private int _tempRandomDropPosition = 1;

	[HideInInspector] public bool activeCameraMoving;
	[HideInInspector] public bool activeTimer;

	[Tooltip("Main Camera")]
	public GameObject movingCamera;
	public float time; //second
    private float _minutes;
	private float _seconds;

	public float countdown;
	private float _countdownSecond;
	public Text countdownText;

	#endregion

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		Instance = this;

		PhotonNetwork.AutomaticallySyncScene = true;
		
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
		Application.targetFrameRate = 70;
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{

		_countdownSecond = Mathf.RoundToInt(countdown % 60);
		countdown -= Time.deltaTime;
		countdownText.text = "" +_countdownSecond.ToString("0");

		if (countdown <= 0)
		{
			countdownText.text = "";
			activeCameraMoving = true;	
		}

		if(activeCameraMoving)
		{
			photonView.RPC("CameraMoving", RpcTarget.All);
		}

		if(activeTimer)
		{
			_minutes = Mathf.Floor(time / 60);
			_seconds = Mathf.RoundToInt(time % 60);
			time -= Time.deltaTime;
				
			if (time <= 0)
			{                
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MovingCamera>().enabled = false;               			
				photonView.RPC("RpcLeaveRoom", RpcTarget.All);					
			}
		}
		
		if (PhotonNetwork.CountOfPlayersInRooms == 1)
		{
			PhotonNetwork.LoadLevel("MenuGame 1");
		}
	}

	#region Photon Messages

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {

		Debug.Log("OnPhotonPlayerConnected() " +newPlayer.NickName);

		if(PhotonNetwork.IsMasterClient) {

			Debug.Log("OnPhotonPlayerConnected isMasterClient " +PhotonNetwork.IsMasterClient);
		}
	}

	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {

		Debug.Log("OnPhotonPlayerDisconnected isMasterClient " +PhotonNetwork.IsMasterClient);
	}

	/// <summary>
	/// Called when the local player left the room. We need to load the launcher scene.
	/// </summary>
	public override void OnLeftRoom() {

		SceneManager.LoadScene("MenuGame 1");
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
		PhotonNetwork.LoadLevel("Gameplay-Online");
	}

	#endregion

	#region RPC			

	[PunRPC]
	private void RpcItemsDrop()
	{		
		var randomDrop = Random.Range(0, itemsDrop.Count);
		Debug.LogWarning("item drop: " +randomDrop);

		var randomPosition = Random.Range(0, itemsDropPosition.Count);
		Debug.LogWarning("drop position: " +randomPosition);

		if(_tempRandomItemDrop == randomDrop)
		{
			randomDrop = Random.Range(0, itemsDrop.Count);
			Debug.LogWarning("item drop [again]: " +randomDrop);			
		}
		else
		{
			_tempRandomItemDrop = randomDrop;
		}
		
		if(_tempRandomDropPosition == randomPosition)
		{
			randomPosition = Random.Range(0, itemsDropPosition.Count);
			Debug.LogWarning("drop position [again]: " +randomPosition);			
		}
		else
		{
			_tempRandomDropPosition = randomPosition;
		}

		Instantiate(itemsDrop[_tempRandomItemDrop], new Vector3(itemsDropPosition[_tempRandomDropPosition].position.x, itemsDropPosition[_tempRandomDropPosition].position.y, 90), Quaternion.identity);
	}

	[PunRPC]
	private void CameraMoving()
	{		
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MovingCamera>().enabled = true;
		
		activeCameraMoving = false;
		activeTimer = true;
	}

	[PunRPC]
	private void RpcLeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	#endregion

	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // throw new System.NotImplementedException();
    }
}