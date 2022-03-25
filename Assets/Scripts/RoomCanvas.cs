using UnityEngine;
using UnityEngine.UI;

public class RoomCanvas: MonoBehaviour {

	public string character = "";
	//public Button[] characterButton = new Button[5];

	public void OnStartMatch() {
		PlayerNetwork.Instance.cha = "Monkey";
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.room.IsOpen = true;
			PhotonNetwork.room.IsVisible = false;
			Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
			Debug.Log(PhotonNetwork.PlayerList.Length);
			// PhotonNetwork.LoadLevel(2);
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

}