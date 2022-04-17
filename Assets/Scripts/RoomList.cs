using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomList : MonoBehaviour {

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
        PlayerName.text = PhotonNetwork.playerName;
    }

    public void changeName() {
        if (PlayerName.text.ToString() != null && PlayerName.text.ToString() != "") {
            PhotonNetwork.playerName = PlayerName.text.ToString();
        } else {
            PhotonNetwork.playerName = "P" + Random.Range(1000000, 9999999);
        }
    }

    public void Update()
    {
        // PhotonNetwork.playerName = PlayerName.text.ToString();
      //  if(sceneName == "Game")
       // PlayerNetwork.Instance.eachPlayerName[(PhotonNetwork.player.ID - 1) % 5] = PhotonNetwork.playerName;
    }
    
    public void OnAppear() {
		MainCanvasManager.Instance.RoomList.transform.SetAsLastSibling();
	}
}
