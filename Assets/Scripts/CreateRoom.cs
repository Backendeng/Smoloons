using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;


public class CreateRoom: MonoBehaviour {

	public string arenaCreationStatus; // photon network status commet
	public GameObject LobNet;
	public GameObject CreateDialog;
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
	LobbyNetwork LN;

	[SerializeField]
	private Text _roomName;
	private Text RoomName {
		get {
			return _roomName;
		}
	}

	//public bool con=false;
	private void Awake() {
		LobNet = GameObject.FindGameObjectWithTag("LN");
	}
	private void Start() {
		LN = LobNet.GetComponent < LobbyNetwork > ();
		OnGenerateRoomName();
	}

	public void OnGenerateRoomName() {
		RoomName.text = "Smoloon" + Random.Range(10000000, 99999999);
	}

	// room name copy event for inviting player
	public void OnCopy() {
		#if UNITY_WEBGL        
            WebGLCopyAndPasteAPI.GetCopyClipboard(RoomName.text);
		#else
			GUIUtility.systemCopyBuffer = RoomName.text;
    	#endif        
	}

	public void OnExit() {
		MainCanvasManager.Instance.RoomCanvas.transform.SetAsLastSibling();
	}

	public void OnCreateExit() {
		MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
	}

	public void OnAppear() {
		MainCanvasManager.Instance.CreateRoom.transform.SetAsLastSibling();
	}

	// Create Room for public game play
	public void OnCreateRoom() {

		RoomOptions roomOptions = new RoomOptions() {
			IsVisible = true,
			IsOpen = true,
			MaxPlayers = 6
		};
		
		_playerCustomProperties["PrivateRoom"] = false; // custom private room status.
		roomOptions.CustomRoomProperties = _playerCustomProperties;

		roomOptions.PlayerTtl = 3000;
		roomOptions.EmptyRoomTtl = 3000;
		
		RoomCanvas1.master_status = true; // isMaster? yes
		RoomCanvas.master_status = true; // isMaster? yes

		_playerCustomProperties["isMaster"] = true; // custom isMaster status.
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
		
		// Create room.
		if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default)) {
			arenaCreationStatus = "Arena creation request sent successfully.";
			Debug.Log("Request for room creation sent successfully.");
		}
		else {
			arenaCreationStatus = "Arena creation request failed";
			Debug.Log("Request for room creation failed.");
		}

	}

	// private room
	public void OnPrivateCreateRoom() {

		RoomOptions roomOptions = new RoomOptions() {
			IsVisible = false,
			IsOpen = true,
			MaxPlayers = 6
		};

		_playerCustomProperties["PrivateRoom"] = true;
		roomOptions.CustomRoomProperties = _playerCustomProperties;

		roomOptions.PlayerTtl = 3000;
		roomOptions.EmptyRoomTtl = 3000;

		RoomCanvas.master_status = true;
		RoomCanvas1.master_status = true;

		_playerCustomProperties["isMaster"] = true;
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
		
		if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default)) {
			arenaCreationStatus = "Arena creation request sent successfully.";
			Debug.Log("Request for room creation sent successfully.");
		}
		else {
			arenaCreationStatus = "Arena creation request failed";
			Debug.Log("Request for room creation failed.");
		}

	}

	private void OnPhotonCreateRoomFailed(object[] codeAndMessage) {
		arenaCreationStatus = "Arena creation failed";
		Debug.Log("Room creation failed : " + codeAndMessage);

	}

	private void OnCreatedRoom() {
		arenaCreationStatus = "Arena created successfully";
		Debug.Log("Room created successfully.");

	}

	private void Update() {
		LN.arenaStatusText.text = arenaCreationStatus;
	}

}