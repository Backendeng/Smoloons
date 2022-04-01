using UnityEngine;
using UnityEngine.UI;

public class RoomCanvas: MonoBehaviour {

	public string character = "";

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

}