using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateRoom : MonoBehaviour
{
    [SerializeField] private Text setRoomName;
    private Text getRoomName
    {
        get { return setRoomName; }
    }
    [SerializeField] private Text setRoomInfo;
    [SerializeField] private Text setHostInfo;

    [SerializeField] private List<Text> playersInfo;


    [SerializeField] private Button exitRoomPanel;
    [SerializeField] private Button readyBtn;

    [SerializeField] private GameObject roomPanel;
    [SerializeField] private GameObject menusPanel;
    [SerializeField] private GameObject readyTxt;

    bool isPlayerReady = false;
    


    private void Start()
    {
        exitRoomPanel.onClick.AddListener(OnLeftRoom);
        readyBtn.onClick.AddListener(ReadyPlay);
    }

    private void FixedUpdate()
    {
        setRoomInfo.text = "Room: " + setRoomName.text;
        setHostInfo.text = PhotonNetwork.NickName.ToString();
    }

    public void OnClickCreateRoom()
    {
        if (PhotonNetwork.CreateRoom(setRoomName.text))
        {
            print("Create room '" + setRoomName.text +"' successfully sent...");

            menusPanel.SetActive(false);
            roomPanel.SetActive(true);
        }
        else
        {
            print("Create room '" + setRoomName.text + "' failed to send...");
        }
    }   

    private void OnPhotonCreatedRoomFailed(object[] codeAndMessage)
    {
        print("Create room '" + setRoomName.text + "' failed: " + codeAndMessage[1]);
    }

    private void OnJoinedRoom()
    {
        Debug.Log("Count Players in Room: " + PhotonNetwork.CountOfPlayersInRooms);

        for (int i = 0; i <= PhotonNetwork.CountOfPlayersInRooms; i++)
        {
            playersInfo[i].text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

    private void OnCreatedRoom()
    {
        print("Room '" + setRoomName.text + "' created successfully...");
    }

    private void OnLeftRoom()
    {
        print("Player was left the Room '" + setRoomName.text + "'...");

        PhotonNetwork.LeaveRoom();
        isPlayerReady = false;

        readyTxt.SetActive(isPlayerReady);
        menusPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    private void ReadyPlay()
    {
        if (!isPlayerReady)
        {
            isPlayerReady = true;            
        }
        else
        {
            isPlayerReady = false;
        }

        readyTxt.SetActive(isPlayerReady);

    }
}
