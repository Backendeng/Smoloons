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
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
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
		

		if ((bool) PhotonNetwork.room.CustomProperties["PrivateRoom"])
			MainCanvasManager.Instance.RoomCanvas.transform.SetAsLastSibling();
		if ((bool) PhotonNetwork.room.CustomProperties["PrivateRoom"] == false)
			MainCanvasManager.Instance.RoomCanvas1.transform.SetAsLastSibling();

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
		
		if (photonPlayer.CustomProperties["PlayerReady"] == null){

		} else if ((bool) photonPlayer.CustomProperties["PlayerReady"] == false) {

		} else {
			playerListingObject.transform.Find("Pointer").gameObject.SetActive(false);
		} 

		if (photonPlayer.CustomProperties["isMaster"] == null){

		} else if ((bool) photonPlayer.CustomProperties["isMaster"] == false) {

		} else {
			playerListingObject.transform.Find("Crown").gameObject.SetActive(true);
		} 

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
		
		_playerCustomProperties["PlayerReady"] = false;
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel(1);

	}

}