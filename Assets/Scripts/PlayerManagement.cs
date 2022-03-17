using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement: MonoBehaviour {

	public static PlayerManagement Instance;
	private PhotonView PhotonView;

	private List < PlayerStats > PlayerStats = new List < PlayerStats > ();
	[SerializeField]
	private float h;

	private void Awake() {

		Instance = this;
		PhotonView = GetComponent < PhotonView > ();

	}

	private void Update() {
		// h = PlayerMovement.Instance.curr_health;
	}

	public void AddPlayerStats(PhotonPlayer photonPlayer) {

		int index = PlayerStats.FindIndex(x => x.PhotonPlayer == photonPlayer);
		if (index == -1) {
			PlayerStats.Add(new PlayerStats(photonPlayer, h));
		}

	}

	public void ModifyHealth(PhotonPlayer photonPlayer, float healthValue) {

		int index = PlayerStats.FindIndex(x => x.PhotonPlayer == photonPlayer);

		if (index != -1) {
			PlayerStats playerStats = PlayerStats[index];
			playerStats.Health = healthValue;
			Debug.Log(playerStats.Health);
			//PlayerNetwork.Instance.ChangeHealth(photonPlayer, playerStats.Health);
		}

	}

}

public class PlayerStats {

	public PlayerStats(PhotonPlayer photonPlayer, float health) {

		PhotonPlayer = photonPlayer;
		Health = health;

	}

	public readonly PhotonPlayer PhotonPlayer;
	public float Health;

}