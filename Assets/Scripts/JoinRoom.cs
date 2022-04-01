using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour
{
    public Text RoomNameInput;
    public GameObject JoinDialog;
	// Use this for initialization
	void Start () {

	}

    public void OnJoin() {
        PhotonNetwork.JoinRoom(RoomNameInput.text);
    }

    public void OnExit() {
		MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
	}

	public void OnAppear() {
		MainCanvasManager.Instance.JoinRoom.transform.SetAsLastSibling();
	}
}
