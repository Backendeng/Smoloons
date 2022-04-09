using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyNetwork: MonoBehaviour {

	public string status;
	public Text lobbyStatusText,
	arenaStatusText;
	// Use this for initialization
	private void Start() {

		if (!PhotonNetwork.connected) {

			status = "Connecting to server . . .";

			Debug.Log("Connecting to server . . .");
			PhotonNetwork.ConnectUsingSettings("0.0.0.0");

		}
	}

	private void OnConnectedToMaster() {

		status = "Connected to master.";

		Debug.Log("Connected to master.");
		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

		PhotonNetwork.JoinLobby(TypedLobby.Default);

	}

	private void OnJoinedLobby() {

		status = "Lobby Joined";

		Debug.Log("Lobby Joined");

		if (!PhotonNetwork.inRoom) MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();

	}

	private void Update() {
		lobbyStatusText.text = status;
	}

}