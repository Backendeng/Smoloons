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
	public void OnRestart() {
		// _playerCustomProperties["PlayerReady"] = false;
        // PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
		
		if (PhotonNetwork.isMasterClient) {
			GameObject [] blocks = GameObject.FindGameObjectsWithTag("Breakable");
			foreach (GameObject block in blocks) {
				PhotonNetwork.Destroy(block);
			}
			GameObject [] powerups = GameObject.FindGameObjectsWithTag("powerup");
			foreach (GameObject powerup in powerups) {
				PhotonNetwork.Destroy(powerup);
			}
			GameObject [] players = GameObject.FindGameObjectsWithTag("Player");
			foreach (GameObject player in players) {
				PhotonNetwork.Destroy(player);
			}
			GameObject [] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
			foreach (GameObject ghost in ghosts) {
				PhotonNetwork.Destroy(ghost);
			}

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