using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoomCanvas1: MonoBehaviour {


	[HideInInspector]
	public string selectedHero;
	private PhotonView PV;
	private bool PlayerReady = false;
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
	private bool ready_status;
	public static bool master_status = true;

	private bool AllPlayersReady ()
	{
			foreach (var photonPlayer in PhotonNetwork.playerList)
			{
				if(photonPlayer.CustomProperties["PlayerReady"] == null){
					Debug.Log("All Players are not Ready!");
					ready_status = false;
					return false;
				} 
			}

			return false;
	}
	// private bool AllPlayersReady
	// {
	// 	get
	// 	{
	// 		foreach (var photonPlayer in PhotonNetwork.playerList)
	// 		{
	// 			if(photonPlayer.CustomProperties["Ping"] == false) return false;
	// 		}

	// 		return true;
	// 	}
	// }
	public string character = "";

	public GameObject PlayerLayoutGroup1;

	private void Awake() {
		PV = GetComponent < PhotonView > ();
	}

	private void Start(){
		PV.RPC("RPC_UnReady", PhotonTargets.All);
	}

	public void OnStartMatch() {
		if (PhotonNetwork.isMasterClient) {
			ready_status = true;
			
			// StartCoroutine (WaitAllReady ());  
			// StartCoroutine (WaitAllReady ());  

			for (int i =0; i < PlayerLayoutGroup1.transform.childCount; i++ ) {
				// Debug.Log(PlayerLayoutGroup1.transform.GetChild(i).transform.GetChild(3).gameObject.activeSelf);
				if (PlayerLayoutGroup1.transform.GetChild(i).transform.GetChild(3).gameObject.activeSelf)
				{
					// Debug.Log(false);
					ready_status = false;
				}
					
			}
			// StartCoroutine (WaitAllReady ());
			// Debug.Log(ready_status);
			if(ready_status)
			{
				
				Debug.Log("All Players are Ready!");
				PhotonNetwork.room.IsOpen = true;
				PhotonNetwork.room.IsVisible = false;
				PhotonNetwork.LoadLevel(2);
			}

			// foreach (var photonPlayer in PhotonNetwork.playerList)
			// {
			// 	if(photonPlayer.CustomProperties["PlayerReady"] == null) {
			// 		Debug.Log("All Players are not Ready!");
			// 		return;
			// 	}
			// }
			
		}

	}


	public void Dragon() {
		PlayerNetwork.Instance.cha = "Player";
	}

	private bool AllReady(){
		foreach (var photonPlayer in PhotonNetwork.playerList)
		{
			if(photonPlayer.CustomProperties["PlayerReady"] == null) {
				Debug.Log("All Players are not Ready!");
				return false;
			} else if ((bool) photonPlayer.CustomProperties["PlayerReady"] == false) {
				Debug.Log("All Players are not Ready!");
				return false;
			} 
			
		}
		return true;
	}

	private IEnumerator WaitAllReady()
	{
		// yield return new WaitUntil (() => AllPlayersReady ());
		Debug.Log("Started Coroutine at timestamp : " + Time.time);
		yield return new WaitForSeconds(5);
	}

     private void Update()
     {
        //  Debug.Log("Player Ready = " + _playerCustomProperties["PlayerReady"]);
		OnIsMaster();
     }

     private void Ready()
     {
        PlayerReady = true;    
        selectedHero = "PlayerTest";
        _playerCustomProperties["PlayerReady"] = PlayerReady;
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
		// PlayerLayoutGroup1.transform.Find(PhotonNetwork.player.ID.ToString()).transform.Find("PlayerNameText").GetComponent<Text>().text = "Ready";
		// Debug.Log("Player Ready = " + _playerCustomProperties["PlayerReady"]);
		PV.RPC("RPC_Ready", PhotonTargets.All, PhotonNetwork.player.ID);
     }

    public void OnReady()
     {
         Ready();

        //  if (PhotonNetwork.IsMasterClient)
        //  {
        //      foreach (var photonPlayer in PhotonNetwork.PlayerList)
        //      {
        //          photonPlayer.CustomProperties["PlayerReady"] = true;
        //          PhotonNetwork.LoadLevel(3);
        //      }
        //  }

     }
	

	 public void OnIsMaster() {
		 if (PhotonNetwork.isMasterClient) {
			// _playerCustomProperties["isMaster"] = true;
        	// PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
			// crown.SetActive(true);
			if (master_status){
				_playerCustomProperties["isMaster"] = true;
        		PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
				PV.RPC("RPC_Crown1", PhotonTargets.All, PhotonNetwork.player.ID, true);
				master_status = false;
			}
		
		 } else {
			if (!master_status){
				_playerCustomProperties["isMaster"] = false;
        		PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
				PV.RPC("RPC_Crown1", PhotonTargets.All, PhotonNetwork.player.ID, false);
				master_status = true;
			}
			// _playerCustomProperties["isMaster"] = false;
        	// PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
			// crown.SetActive(false);
		 }
	 }

	[PunRPC]
	private void RPC_Ready(int PlayerID) {
		PlayerLayoutGroup1.transform.Find(PlayerID.ToString()).transform.Find("Pointer").gameObject.SetActive(false);
	}

	[PunRPC]
	private void RPC_UnReady() {
		_playerCustomProperties["PlayerReady"] = false;
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
		foreach (Transform child in PlayerLayoutGroup1.transform)
		{
			child.Find("Pointer").gameObject.SetActive(true);
		}
	}

	[PunRPC]
	private void RPC_Crown1(int PlayerID, bool flag) {
		PlayerLayoutGroup1.transform.Find(PlayerID.ToString()).transform.Find("Crown").gameObject.SetActive(flag);
	}

}