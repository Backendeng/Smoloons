using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner: MonoBehaviour {

	Player_Controller pm = new Player_Controller();
	public Transform[] spawnPoint = new Transform[6];
	public Transform[] spawnPoint_17 = new Transform[6];
	public Transform[] spawnPoint_13 = new Transform[4];
	public GameObject Grand_17;
	public GameObject Grand_13;
	public int playerID = 0;
	public string character;

	// public bool summaryOpen;

	private void Awake() {
		// summaryOpen
		playerID = PhotonNetwork.player.ID;
		character = "Monkey";
	}

	private void Start() {

		Debug.Log(PhotonNetwork.player.ID);

		Invoke("CreatePlayer", 2f);
		if (PhotonNetwork.room.PlayerCount < 5)
		{
			spawnPoint = spawnPoint_13;
			Grand_13.SetActive(true);
			Grand_17.SetActive(false);
		} else {
			spawnPoint = spawnPoint_17;
			Grand_17.SetActive(true);
			Grand_13.SetActive(false);
			
		}
	}

	public void OnDis() {
		PhotonNetwork.Disconnect();
	}

	public void CreatePlayer() {
		switch (playerID % 6) {
		case 1:
			Debug.Log(character);
			pm.RPC_SpawnPlayer(spawnPoint[0], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[0];
			break;
		case 2:
			Debug.Log(character);
			pm.RPC_SpawnPlayer(spawnPoint[1], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[1];
			break;
		case 3:
			Debug.Log(character);
			pm.RPC_SpawnPlayer(spawnPoint[2], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[2];
			break;
		case 4:
			Debug.Log(character);
			pm.RPC_SpawnPlayer(spawnPoint[3], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[3];
			break;
		case 5:
			Debug.Log(character);
			pm.RPC_SpawnPlayer(spawnPoint[4], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[4];
			break;
		case 0:
			Debug.Log(character);
			pm.RPC_SpawnPlayer(spawnPoint[5], character);
			// Player_Controller.Instance.selfSpawnTransform = spawnPoint[4];
			break;
		default:
			break;
		}
	}

}