using UnityEngine;
using UnityEngine.UI;

public class RoomCanvas: MonoBehaviour {


	[HideInInspector]
	public string selectedHero;
	private PhotonView PV;
	private bool PlayerReady = false;
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();

	private bool AllPlayersReady
	{
		get
		{
			foreach (var photonPlayer in PhotonNetwork.playerList)
			{
				if(photonPlayer.CustomProperties["PlayerReady"] == null){
					Debug.Log("All Players are not Ready!");
					return false;
				} 
			}

			return true;
		}
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

	public GameObject PlayerLayoutGroup;

	private void Awake() {
		PV = GetComponent < PhotonView > ();
	}


	public void OnStartMatch() {
		if (PhotonNetwork.isMasterClient) {
			if(!AllPlayersReady)
			{
				return;
			}
			
				Debug.Log("All Players are not Ready!");
				PhotonNetwork.room.IsOpen = true;
				PhotonNetwork.room.IsVisible = false;
				PhotonNetwork.LoadLevel(2);

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
			}
		}
		return true;
	}

     private void Update()
     {
        //  Debug.Log("Player Ready = " + _playerCustomProperties["PlayerReady"]);
     }

     private void Ready()
     {
        PlayerReady = true;    
        selectedHero = "PlayerTest";
        _playerCustomProperties["PlayerReady"] = PlayerReady;
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
		// PlayerLayoutGroup.transform.Find(PhotonNetwork.player.ID.ToString()).transform.Find("PlayerNameText").GetComponent<Text>().text = "Ready";
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
	
	[PunRPC]
	private void RPC_Ready(int PlayerID) {
		PlayerLayoutGroup.transform.Find(PlayerID.ToString()).transform.Find("Pointer").gameObject.SetActive(false);
	}
}