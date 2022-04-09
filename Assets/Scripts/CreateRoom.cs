using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;


public class CreateRoom: MonoBehaviour {

	public string arenaCreationStatus;
	public GameObject LobNet;
	public GameObject CreateDialog;
	
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

	public void OnCopy() {
		// GUIUtility.systemCopyBuffer = RoomName.text;
		// passCopyToBrowser(GUIUtility.systemCopyBuffer);
		#if UNITY_WEBGL        
            WebGLCopyAndPasteAPI.GetCopyClipboard(RoomName.text);
    	#endif        
	}

	public void OnExit() {
		MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
	}

	public void OnAppear() {
		MainCanvasManager.Instance.CreateRoom.transform.SetAsLastSibling();
	}

	public void OnCreateRoom() {

		RoomOptions roomOptions = new RoomOptions() {
			IsVisible = true,
			IsOpen = true,
			MaxPlayers = 6
		};

		roomOptions.PlayerTtl = 3000;
		roomOptions.EmptyRoomTtl = 3000;

		if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default)) {
			arenaCreationStatus = "Arena creation request sent successfully.";
			Debug.Log("Request for room creation sent successfully.");
		}
		else {
			arenaCreationStatus = "Arena creation request failed";
			Debug.Log("Request for room creation failed.");
		}

		//  con = true;
		//  if (con == true)
		//     idf();
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

	//private void idf() {
	//    PlayerNetwork.Instance.eachPlayerName[((PhotonNetwork.player.ID) - 1) % 5] = PhotonNetwork.playerName;
	//}

}