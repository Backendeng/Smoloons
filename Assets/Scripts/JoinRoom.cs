using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour
{
    public Text RoomNameInput;
    public GameObject JoinDialog;
    LobbyCanvas lobbyCanvas;
	// Use this for initialization
	void Start () {

        GameObject lobbyCanvasObject = MainCanvasManager.Instance.LobbyCanvas.gameObject;
        if (lobbyCanvasObject == null)
            return;

        lobbyCanvas = lobbyCanvasObject.GetComponent<LobbyCanvas>();

	}

    public void OnJoin() {
        lobbyCanvas.OnJoinRoom(RoomNameInput.text);
    }

    public void OnExit() {
		JoinDialog.SetActive(false);
	}

	public void OnAppear() {
		JoinDialog.SetActive(false);
	}
}
