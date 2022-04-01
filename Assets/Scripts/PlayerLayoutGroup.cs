using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup: MonoBehaviour {

	[SerializeField]
	private GameObject _playerListingPrefab;
	private GameObject PlayerListingPrefab {
		get {
			return _playerListingPrefab;
		}
	}

	private List < PlayerListing > _playerListings = new List < PlayerListing > ();
	private List < PlayerListing > PlayerListings {
		get {
			return _playerListings;
		}
	}

	void Start () {
		PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
		for (int i = 0; i < photonPlayers.Length; i++) {
			PlayerJoinedRoom(photonPlayers[i]);
		}
	}

	public void OnJoinedRoom() {

		foreach(Transform child in transform) {
			Destroy(child.gameObject);
		}

		MainCanvasManager.Instance.RoomCanvas.transform.SetAsLastSibling();

		PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
		for (int i = 0; i < photonPlayers.Length; i++) {
			PlayerJoinedRoom(photonPlayers[i]);
		}

	}

	private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer) {

		PlayerJoinedRoom(photonPlayer);

	}

	private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer) {

		PlayerLeftRoom(photonPlayer);

	}

	private void PlayerJoinedRoom(PhotonPlayer photonPlayer) {

		if (photonPlayer == null) return;

		PlayerLeftRoom(photonPlayer);

		GameObject playerListingObject = Instantiate(PlayerListingPrefab);
		playerListingObject.transform.SetParent(transform, false);

		PlayerListing playerListing = playerListingObject.GetComponent < PlayerListing > ();
		playerListing.ApplyPhotonPlayer(photonPlayer);

		PlayerListings.Add(playerListing);

	}

	private void PlayerLeftRoom(PhotonPlayer photonPlayer) {

		int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);

		if (index != -1) {
			Destroy(PlayerListings[index].gameObject);
			PlayerListings.RemoveAt(index);
		}

	}

	public void OnRoomState() {

		if (!PhotonNetwork.isMasterClient) return;

		PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
		PhotonNetwork.room.IsVisible = PhotonNetwork.room.IsOpen;

	}

	public void OnLeaveRoom() {

		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(1);

	}

}