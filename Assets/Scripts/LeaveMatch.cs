using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveMatch: MonoBehaviour {

	public Text MC;
	public Text connState;

	private void Update() {
		if (PhotonNetwork.isMasterClient) {
			MC.text = "Master";
		}

		if (PhotonNetwork.connectionState == ConnectionState.Connected) connState.text = "Connected";
		else connState.text = "Disconnected";

	}

	public void OnLeaveMatch() {
		if (PhotonNetwork.isMasterClient) PlayerNetwork.Instance.PlayersInGame--;
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(1);

	}
	public void OnRestart() {
		PhotonNetwork.room.IsVisible = true;
		PhotonNetwork.LoadLevel(3);
	}

	public void ExitGame() {
		Application.Quit();
	}
}