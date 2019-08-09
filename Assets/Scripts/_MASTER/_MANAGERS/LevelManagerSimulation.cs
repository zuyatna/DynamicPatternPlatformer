using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon;
using Photon.Pun;

public class LevelManagerSimulation : MonoBehaviourPunCallbacks {

	#region Public Variables

	[Tooltip("The prefab to use for representating the player")]
    public GameObject playerPrefab;

	[Header("Leave Room")]
	public Button leaveButton;

	[Header("Item Loot")]
	public Button burnItemButton;
	public GameObject burnItemLoot;

	[Header("Special Weapon Loot")]
	public Button specialWeaponButton;
	public GameObject specialWeaponLoot;

	[Header("Weapon Loot")]
	public Button weaponButton;
	public GameObject weaponLoot;

	private int tempPlayer;
	public Text playerInRoom;
		
	#endregion

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
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

		burnItemButton.onClick.AddListener(SpawnBurnItem);
		specialWeaponButton.onClick.AddListener(SpawnSpecialWeapons);
		weaponButton.onClick.AddListener(SpawnWeapons);
		leaveButton.onClick.AddListener(LeaveRoom);		
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{

		if(photonView.IsMine)
		{
            Physics2D.IgnoreLayerCollision(9, 10);
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

	#region Item Loot and Weapon Function
	
	private void SpawnBurnItem()
	{		
		if(GameObject.FindGameObjectWithTag("ItemGrab") == null)
		{
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("RPCInstantiateBurnItem", RpcTarget.All);		
		}
		else
		{
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("RPCSpawnBurnItem", RpcTarget.All);	
		}		
	}

	private void SpawnSpecialWeapons()
	{
		if(GameObject.FindGameObjectWithTag("WeaponSpecial") == null)
		{
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("RPCInstantiateSpecialWeapons", RpcTarget.All);
		}
		else
		{
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("RPCSpawnSpecialWeapons", RpcTarget.All);	
		}	
	}

	private void SpawnWeapons()
	{
		if(GameObject.FindGameObjectWithTag("Weapon") == null)
		{
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("RPCInstantiateWeapons", RpcTarget.All);		
		}
		else
		{
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("RPCSpawnWeapons", RpcTarget.All);	
		}
	}

	#endregion

    #region RPC
	
	[PunRPC]
	private void RPCSpawnBurnItem()
	{		
		transform.position = new Vector3(0, 5, transform.position.z);
		
		var tempItemLoot = GameObject.FindGameObjectWithTag("ItemGrab");
		tempItemLoot.transform.position = transform.position;		

		tempItemLoot.SetActive(true);
	}

	[PunRPC]
	private void RPCInstantiateBurnItem()
	{
		Instantiate(burnItemLoot, new Vector3(0, 5, 90), Quaternion.identity);
	}

	[PunRPC]
	private void RPCSpawnSpecialWeapons()
	{
		transform.position = new Vector3(0, 5, transform.position.z);
		
		var tempItemLoot = GameObject.FindGameObjectWithTag("WeaponSpecial");
		tempItemLoot.transform.position = transform.position;		

		tempItemLoot.SetActive(true);
	}

	[PunRPC]
	private void RPCInstantiateSpecialWeapons()
	{
		Instantiate(specialWeaponLoot, new Vector3(0, 5, 90), Quaternion.identity);
	}

	[PunRPC]
	private void RPCSpawnWeapons()
	{
		transform.position = new Vector3(0, 5, transform.position.z);
		
		var tempItemLoot = GameObject.FindGameObjectWithTag("Weapon");
		tempItemLoot.transform.position = transform.position;		

		tempItemLoot.SetActive(true);
	}

	[PunRPC]
	private void RPCInstantiateWeapons()
	{
		Instantiate(weaponLoot, new Vector3(0, 5, 90), Quaternion.identity);
	}

    #endregion
}
