using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class rooms : MonoBehaviourPunCallbacks 
{
	public GameObject readygame;
	public Text roomId;
	public Text playerName;
	public Text[] playersInRoom;
	

	private void Awake()
	{
		roomId.text = PhotonNetwork.CurrentRoom.Name;
		playerName.text = PhotonNetwork.NickName;

		PhotonNetwork.AutomaticallySyncScene = true;
		
		if (PhotonNetwork.IsMasterClient)
		{
			readygame.SetActive(true);
		}
	}

	private void Update()
	{
		int i = 0;
		foreach (var player in PhotonNetwork.PlayerListOthers)
		{
			playersInRoom[i].text = player.NickName;
			i++;
		}
	}

	public void GoToAnotherScene()
	{
		PhotonNetwork.LoadLevel("Gameplay-Online");
	}
	
	public void BacktoMenu()
	{
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		SceneManager.LoadSceneAsync("MenuGame 1");
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log("Player Left Room: " +otherPlayer);
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log("Player Entered Room: " +newPlayer);
	}
}
