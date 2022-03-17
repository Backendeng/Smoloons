using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyCanvas : MonoBehaviour {

    public InputField PlayerName;
    //public string playerName;

    //GameObject go;
    //KillsIncrementer ki;
   // string sceneName;
        
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
       // sceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        //if (sceneName == "Game") {
        //    go = GameObject.FindGameObjectWithTag("Kills");
        //    ki = go.GetComponent<KillsIncrementer>();
        //}
        
    }

    public void Update()
    {
        PhotonNetwork.playerName = PlayerName.text.ToString();
      //  if(sceneName == "Game")
       // PlayerNetwork.Instance.eachPlayerName[(PhotonNetwork.player.ID - 1) % 5] = PhotonNetwork.playerName;
    }

}
