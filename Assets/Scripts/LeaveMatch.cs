using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveMatch: MonoBehaviour {

	public Text MC;
	public Text connState;
	private PhotonView PV;
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
	public GameObject PlayerLayoutGroup;

	private void Awake() {
		PV = GetComponent < PhotonView > ();
	}

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

	// when game is over, master player can click restart.
	public void OnRestart() {
		
		if (PhotonNetwork.isMasterClient) {

			PhotonNetwork.DestroyAll();

			PhotonNetwork.automaticallySyncScene = true;
			if ((bool) PhotonNetwork.room.CustomProperties["PrivateRoom"] == false){
				PhotonNetwork.room.IsVisible = true;
				PhotonNetwork.LoadLevel(3);
			} else {
				PhotonNetwork.LoadLevel(4);
			}
			
		}
	}

	public void ExitGame() {
		Application.Quit();
	}
}