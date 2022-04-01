﻿using UnityEngine;
using UnityEngine.UI;

public class RoomCanvas: MonoBehaviour {


	[HideInInspector]
	public string selectedHero;
	private PhotonView PV;
	private bool PlayerReady = false;
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();

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

	public GameObject PlayerLayoutGroup;

	public void OnStartMatch() {
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.room.IsOpen = true;
			PhotonNetwork.room.IsVisible = false;
			PhotonNetwork.LoadLevel(2);
		}

	}

	public void Dragon() {
		PlayerNetwork.Instance.cha = "Player";
	}
	public void Condor() {
		PlayerNetwork.Instance.cha = "CondorM";
	}
	public void Chicken() {
		PlayerNetwork.Instance.cha = "ChickenM";
	}

	

     private void Update()
     {
        //  Debug.Log("Player Ready = " + _playerCustomProperties["PlayerReady"]);
     }

     private void Ready()
     {
         PlayerReady = true;    
         selectedHero = "PlayerTest";
         PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
         _playerCustomProperties["PlayerReady"] = PlayerReady;
		PlayerLayoutGroup.transform.Find(PhotonNetwork.player.ID.ToString()).transform.Find("PlayerNameText").GetComponent<Text>().text = "Ready";
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

}