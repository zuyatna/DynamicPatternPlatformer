using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyNetwork : MonoBehaviourPunCallbacks
{
    public byte maxPlayesrInRoom = 4;
    public InputField inputFindRoom;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master..");
        PhotonNetwork.NickName = PlayerPrefs.GetString("playerName", "No Name");

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby as " +PhotonNetwork.NickName + "...");
    }
    
    public void ClickCreateRoom()
    {
        string roomName = "" + Random.Range(0, 1000);
        Debug.Log("RoomName: " +roomName);
        
        if (PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4}))
        {
            Debug.Log("Create Room Successfully Sent...");
        }
        else
        {
            Debug.Log("Create Room Failed to Send...");
        }
    }

    public void ClickFindRoom()
    {
        if (PhotonNetwork.JoinRoom(inputFindRoom.text))
        {
            Debug.Log("Join to: " +inputFindRoom);
        }
        else
        {
            Debug.Log("Failed to join:" +inputFindRoom);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed, returnCode: " +returnCode +", message: " +message);
        ClickCreateRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created Successfully...");
        SceneManager.LoadSceneAsync("Room");
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadSceneAsync("Room");
    }
}
