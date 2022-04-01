using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing: MonoBehaviour {

	public PhotonPlayer PhotonPlayer {
		get;
		private set;
	}

	[SerializeField]
	private Text _playerName;
	private Text PlayerName {
		get {
			return _playerName;
		}
	}

	[SerializeField]
	private Text _playerPing;
	private Text PlayerPing {
		get {
			return _playerPing;
		}
	}

	public void ApplyPhotonPlayer(PhotonPlayer photonPlayer) {

		PhotonPlayer = photonPlayer;
		PlayerName.text = PhotonPlayer.NickName;
		gameObject.name = photonPlayer.ID.ToString();

		StartCoroutine(ShowPlayerPing());

	}

	private IEnumerator ShowPlayerPing() {

		while (PhotonNetwork.connected) {
			int ping = (int) PhotonPlayer.CustomProperties["Ping"];
			PlayerPing.text = ping.ToString();

			yield
			return new WaitForSeconds(1f);
		}

		yield
		break;

	}

}