using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Collections;

public class PlayerNetwork: Photon.MonoBehaviour {

	public static PlayerNetwork Instance;
	public string PlayerName {
		get;
		private set;
	} 
	public int PlayersInGame = 0;
	//[SerializeField]
	public bool mc,
	wasAlreadyConnected;
	public int numofplayer = 0;

	private ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
	// string a = "avdhesh";
	private PhotonView PhotonView;
	public Player_Controller CurrentPlayer;
	public string cha;
	public string map;

	private Coroutine playerPingCoroutine;

	private void Awake() {

		Instance = this;
		PhotonView = GetComponent < PhotonView > ();
		wasAlreadyConnected = false;

		PlayerName = "Player" + Random.Range(10000, 99999);

		PhotonNetwork.sendRate = 200;
		PhotonNetwork.sendRateOnSerialize = 60;

		SceneManager.sceneLoaded += OnSceneFinishedLoading;
		//Debug.Log(a.Substring(a.Length-2,1));

	}

	private void Update() {
		numofplayer = PhotonNetwork.playerList.Length;
		if (PhotonNetwork.isMasterClient) mc = true;
		else mc = false;
	}

	public void OnPhotonPlayerDisconnected() {

		PlayersInGame--;

	}

	private void OnDisconnectedFromPhoton() {
		wasAlreadyConnected = true;

		PhotonNetwork.ReconnectAndRejoin();

	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {

		if (scene.name == "Game") {

			if (PhotonNetwork.isMasterClient) {

				MasterLoadedGame();
			}

			else NonMasterLoadedGame();

		}

	}

	private void MasterLoadedGame() {

		PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
		PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
		Debug.Log("M spwaned");

	}

	private void NonMasterLoadedGame() {

		PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
		Debug.Log("C spwaned");

	}

	[PunRPC]
	private void RPC_LoadGameOthers() {

		PhotonNetwork.LoadLevel(2);

	}

	[PunRPC]
	private void RPC_LoadedGameScene(PhotonPlayer photonPlayer) {

		PlayerManagement.Instance.AddPlayerStats(photonPlayer);

		PlayersInGame++;
		if (PlayersInGame == PhotonNetwork.playerList.Length) {
			Debug.Log("All players are in scene.");
		}

	}

	//public void ChangeHealth(PhotonPlayer photonPlayer, int health)
	//{

	//    PhotonView.RPC("RPC_ChangeHealth", photonPlayer, health);

	//}

	//[PunRPC]
	//private void RPC_ChangeHealth(int health)
	//{

	//    if (CurrentPlayer == null)
	//        return;

	//    if (health <= 0)
	//        PhotonNetwork.Destroy(CurrentPlayer.gameObject);
	//    else
	//        CurrentPlayer.Health = health;
	//}

	private IEnumerator SetPlayerPing() {

		while (PhotonNetwork.connected) {
			playerCustomProperties["Ping"] = PhotonNetwork.GetPing();
			PhotonNetwork.player.SetCustomProperties(playerCustomProperties);

			yield
			return new WaitForSeconds(5f);
		}

		yield
		break;

	}

	private void OnConnectedToMaster() {

		if (playerPingCoroutine != null) StopCoroutine(playerPingCoroutine);
		playerPingCoroutine = StartCoroutine(SetPlayerPing());

	}

}