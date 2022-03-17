using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner: MonoBehaviour {

	Player_Controller pm = new Player_Controller();

	public Transform[] spawnPoint = new Transform[5];
	public int playerID = 0;
	public string character;

	// public bool summaryOpen;

	private void Awake() {
		// summaryOpen
		playerID = PhotonNetwork.player.ID;
		character = PlayerNetwork.Instance.cha;
	}

	private void Start() {

		Debug.Log(PhotonNetwork.player.ID);

		Invoke("CreatePlayer", 2f);

	}

	public void OnDis() {
		PhotonNetwork.Disconnect();
	}

	public void CreatePlayer() {
		switch (playerID % 5) {
		case 1:
			pm.RPC_SpawnPlayer(spawnPoint[0], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[0];
			break;
		case 2:
			Debug.Log(character);
			pm.RPC_SpawnPlayer(spawnPoint[1], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[1];
			break;
		case 3:
			pm.RPC_SpawnPlayer(spawnPoint[2], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[2];
			break;
		case 4:
			pm.RPC_SpawnPlayer(spawnPoint[3], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[3];
			break;
		case 0:
			pm.RPC_SpawnPlayer(spawnPoint[4], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[4];
			break;
		default:
			break;
		}
	}

}