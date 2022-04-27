using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyCanvas : MonoBehaviour {

    public InputField PlayerName;
        
    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup {
        get { return _roomLayoutGroup; }
    }

    public void OnJoinRoom(string roomName) {

        if (PhotonNetwork.JoinRoom(roomName)) {
        }
        else {
            Debug.Log("Join room failed");
        }
    }

    private void Awake()
    {
    }

    private void Start()
    {
          
    }

    public void Update()
    {
        PhotonNetwork.playerName = PlayerName.text.ToString();
    }
    
    public void OnAppear() {
	}
}
