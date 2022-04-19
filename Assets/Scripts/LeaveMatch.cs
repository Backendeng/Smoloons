using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveMatch: MonoBehaviour {

	public Text MC;
	public Text connState;
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();

	private void Update() {
		if (PhotonNetwork.isMasterClient) {
			MC.text = "Master";
		}

		if (PhotonNetwork.connectionState == ConnectionState.Connected) connState.text = "Connected";
		else connState.text = "Disconnected";

	}

	public void OnLeaveMatch() {
		// if (PhotonNetwork.isMasterClient) PlayerNetwork.Instance.PlayersInGame--;
		_playerCustomProperties["PlayerReady"] = false;
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(1);

	}
	public void OnRestart() {
		_playerCustomProperties["PlayerReady"] = false;
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
		PhotonNetwork.room.IsVisible = true;
		PhotonNetwork.LoadLevel(3);
	}

	public void ExitGame() {
		Application.Quit();
	}
}