using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasManager: MonoBehaviour {

	public static MainCanvasManager Instance;
	PlayerLayoutGroup plg = new PlayerLayoutGroup();

	[SerializeField]
	private LobbyCanvas _lobbyCanvas;
	public LobbyCanvas LobbyCanvas {
		get {
			return _lobbyCanvas;
		}
	}

	[SerializeField]
	private RoomCanvas _roomCanvas;
	public RoomCanvas RoomCanvas {
		get {
			return _roomCanvas;
		}
	}

	[SerializeField]
	private CreateRoom _createRoom;
	public CreateRoom CreateRoom {
		get {
			return _createRoom;
		}
	}

	[SerializeField]
	private JoinRoom _joinRoom;
	public JoinRoom JoinRoom {
		get {
			return _joinRoom;
		}
	}



	private void Awake() {

		Instance = this;

	}

	// public void Start() {
	// 	Debug.Log("sdfas");
	// 	plg.OnJoinedRoom();
	// }
}