using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class iCTG_NetworkManager : MonoBehaviourPunCallbacks
{
    // Instance
    public static iCTG_NetworkManager Instance;

    public string room;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            gameObject.SetActive(false);
        else
        {
            // Set the instance
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.LocalPlayer.NickName = iCTG_PlayerNetwork.Instance.PlayerName;
        CreateRoom(room);
    }

    // Attempts to create a room
    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room " + PhotonNetwork.CurrentRoom.Name);
    }

    // Attempts to Join a room
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to Room " + PhotonNetwork.CurrentRoom.Name);
    }

    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
