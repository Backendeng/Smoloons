using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlayerMovement: Photon.MonoBehaviour {

	public static PlayerMovement Instance;
	// [SerializeField]
	public Text _playerHealth,
	playerKills,
	playerDeaths;
	public Image healthFG;
	public Transform selfSpawnTransform;
	private PhotonView PhotonView;
	private Vector3 TargetPosition;
	private Quaternion TargetRotation;
	public GameObject cam;
	public GameObject playerGameObject,
	target;
	private Camera c;
	public PlayerMovement pm;

	public int deaths;

	GameObject globalKillInc;
	KillsIncrementer globalKi;

	Vector3 d = new Vector3(Screen.width / 2, Screen.width / 2, 0);

	public float max_health,
	curr_health,
	health;

	private void Awake() {

		globalKillInc = GameObject.FindGameObjectWithTag("Kills");
		globalKi = globalKillInc.GetComponent < KillsIncrementer > ();
		Instance = this;
		PhotonView = GetComponent < PhotonView > ();
		curr_health = max_health = 100;
		_playerHealth = GetComponentInChildren < Text > ();
		c = cam.GetComponent < Camera > ();
		health = 10;
		target = GameObject.FindGameObjectWithTag("target");

	}

	private void Start() {

		if (PhotonView.isMine) {
			cam.SetActive(true);
		}
		else {
			cam.SetActive(false);
		}

		Invoke("changeName", 2f);

	}

	void Update() {

		PhotonView.RPC("healthSet", PhotonTargets.All);

		//Vector3 screenPos = Camera.main.WorldToScreenPoint(d);
		// PhotonView.RPC("setKills", PhotonTargets.All);

		//    target.transform.position = d;
		//canvas.transform.rotation = Quaternion.LookRotation(target.transform.forward);

		//canvas.transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
		// Debug.DrawRay(canvas.transform.position, canvas.transform.forward * 1000);

		if (globalKi.winLose[PhotonNetwork.player.ID - 1].Equals("Winner ! ! !")) {
			globalKi.WinLoseText.text = "Winner ! ! !";
		}
		else globalKi.WinLoseText.text = "Loser";

		//playerDeaths.text = deaths.ToString();
		//playerKills.text = globalKi.eachPlayerKills[gameObject.GetPhotonView().ownerId].ToString();

		if (PhotonView.isMine && PhotonNetwork.connectionState == ConnectionState.Connected) {
			if (globalKi.timer > 0 && globalKi.timer < 121) CheckInput();
		}
		else SmoothMovement();

		if (curr_health <= 0) {

			gameObject.SetActive(false);
			Invoke("FurtherRespawn", 2f);
			// setDeaths();
			//StartCoroutine(FurtherRespawn(selfSpawnTransform));
		}

		//if (!PhotonView.isMine)
		//{
		//    _playerHealth.text = curr_health.ToString();

		//}

		if (PhotonView.isMine) {
			GameUI.Instance.playerHealth.text = curr_health.ToString();
			setKills();
			setDeaths();
			// GameUI.Instance.playerKills.text = 
			//  PhotonView.RPC("RPC_PlayerUICameraFollow", PhotonTargets.OthersBuffered);

		}

	}

	public void RPC_SpawnPlayer(Transform spawnPoint, string shape) {

		PhotonNetwork.Instantiate(Path.Combine("Prefabs", shape), spawnPoint.position, Quaternion.identity, 0);

	}

	[PunRPC]
	private void RPC_PlayerUICameraFollow() {

		//canvas.transform.LookAt(this.cam.transform);

	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

		if (stream.isWriting) {
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(curr_health);

			stream.SendNext(deaths);
		}
		else {
			TargetPosition = (Vector3) stream.ReceiveNext();
			TargetRotation = (Quaternion) stream.ReceiveNext();
			curr_health = (float) stream.ReceiveNext();

			deaths = (int) stream.ReceiveNext();
		}

	}

	private void SmoothMovement() {

		transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.2f);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 500 * Time.deltaTime);

	}

	private void CheckInput() {

		float moveSpeed = 25f;
		float rotateSpeed = 250f;

		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		transform.position += transform.forward * vertical * moveSpeed * Time.deltaTime;
		transform.Rotate(new Vector3(0, horizontal * rotateSpeed * Time.deltaTime, 0));

	}
	//private void OnCollisionEnter(Collision collision)
	//{

	//    //if (!PhotonNetwork.isMasterClient)
	//    //    return;

	//    // PhotonView photonView = collider.GetComponent<PhotonView>();

	//}

	public void OnTriggerEnter(Collider collision) {

		if (collision.gameObject.tag == "weapon") {
			Debug.Log("Touched");
			// Health -= 10;
			//  Debug.Log(collider.gameObject.GetPhotonView());
			if (PhotonView != null && PhotonView.isMine) {
				curr_health -= health;

				PlayerManagement.Instance.ModifyHealth(PhotonView.owner, curr_health);
			}
			//healthFG.fillAmount = curr_health / max_health;
			PhotonView pv;

			if (collision.gameObject.GetPhotonView() != null && PhotonView.isMine) {
				pv = collision.gameObject.GetPhotonView();
				Debug.Log(pv.viewID);
			}

			if (curr_health <= 0) {
				PhotonView.RPC("increaseKills", PhotonTargets.All, collision.gameObject.GetComponentInParent < PhotonView > ().ownerId);
				PhotonView.RPC("setDeaths", PhotonTargets.All, PhotonNetwork.player.ID);
			}

		}

	}

	private void FurtherRespawn() {

		//healthFG.fillAmount = 1;
		curr_health = 100;
		gameObject.SetActive(true);
		gameObject.transform.position = selfSpawnTransform.position;

	}

	[PunRPC]
	private void increaseKills(int playerUID) {

		GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer ki = KillsInc.GetComponent < KillsIncrementer > ();
		switch (playerUID % 5) {
		case 1:
			ki.eachPlayerKills[0]++;
			ki.eachPlayerScore[0] = ki.eachPlayerScore[0] + 25;
			break;
		case 2:
			ki.eachPlayerKills[1]++;
			ki.eachPlayerScore[1] = ki.eachPlayerScore[1] + 25;
			break;
		case 3:
			ki.eachPlayerKills[2]++;
			ki.eachPlayerScore[2] = ki.eachPlayerScore[2] + 25;
			break;
		case 4:
			ki.eachPlayerKills[3]++;
			ki.eachPlayerScore[3] = ki.eachPlayerScore[3] + 25;
			break;
		case 0:
			ki.eachPlayerKills[4]++;
			ki.eachPlayerScore[4] = ki.eachPlayerScore[4] + 25;
			break;
		default:
			break;
		}
	}

	private void setKills() {
		GameObject go = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer k = go.GetComponent < KillsIncrementer > ();

		GameUI.Instance.playerKills.text = k.eachPlayerKills[(PhotonNetwork.player.ID - 1) % 5].ToString();
		GameUI.Instance.playerScore.text = k.eachPlayerScore[(PhotonNetwork.player.ID - 1) % 5].ToString();
	}

	[PunRPC]
	private void setDeaths(int id) {

		GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer ki = KillsInc.GetComponent < KillsIncrementer > ();
		switch (id % 5) {
		case 1:
			ki.eachPlayerDeaths[0]++;
			break;
		case 2:
			ki.eachPlayerDeaths[1]++;
			break;
		case 3:
			ki.eachPlayerDeaths[2]++;
			break;
		case 4:
			ki.eachPlayerDeaths[3]++;
			break;
		case 0:
			ki.eachPlayerDeaths[4]++;
			break;
		default:
			break;
		}
	}

	private void setDeaths() {
		GameObject go = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer k = go.GetComponent < KillsIncrementer > ();

		GameUI.Instance.playerDeaths.text = k.eachPlayerDeaths[(PhotonNetwork.player.ID - 1) % 5].ToString();

	}

	private void changeName() {
		PhotonView.RPC("setName", PhotonTargets.AllBuffered, PhotonNetwork.player.ID);
	}

	[PunRPC]
	private void setName(int id) {

		GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer ki = KillsInc.GetComponent < KillsIncrementer > ();
		if (PhotonView.isMine) switch (id % 5) {
		case 1:
			ki.eachPlayerName[0] = PhotonNetwork.playerList[0].ID + " " + PhotonNetwork.playerList[0].NickName;
			break;
		case 2:
			ki.eachPlayerName[1] = PhotonNetwork.playerList[1].ID + " " + PhotonNetwork.playerList[1].NickName;
			break;
		case 3:
			ki.eachPlayerName[2] = PhotonNetwork.playerList[2].ID + " " + PhotonNetwork.playerList[2].NickName;
			break;
		case 4:
			ki.eachPlayerName[3] = PhotonNetwork.playerList[3].ID + " " + PhotonNetwork.playerList[3].NickName;
			break;
		case 0:
			ki.eachPlayerName[4] = PhotonNetwork.playerList[4].ID + " " + PhotonNetwork.playerList[4].NickName;
			break;
		default:
			break;
		}

	}

	[PunRPC]
	private void healthSet() {

		GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer ki = KillsInc.GetComponent < KillsIncrementer > ();
		if (GetComponent < PhotonView > ().ownerId == 1) {
			ki.eachPlayerHealth[0] = curr_health;
		}
		if (GetComponent < PhotonView > ().ownerId == 2) {
			ki.eachPlayerHealth[1] = curr_health;
		}
		if (GetComponent < PhotonView > ().ownerId == 3) {
			ki.eachPlayerHealth[2] = curr_health;
		}
		if (GetComponent < PhotonView > ().ownerId == 4) {
			ki.eachPlayerHealth[3] = curr_health;
		}
		if (GetComponent < PhotonView > ().ownerId == 5) {
			ki.eachPlayerHealth[4] = curr_health;
		}
	}

	//[PunRPC]
	//private void setHealth(int id)
	//{

	//    GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
	//    KillsIncrementer ki = KillsInc.GetComponent<KillsIncrementer>();
	//    if (PhotonView.isMine)
	//        switch (id % 5)
	//        {
	//            case 1:
	//                ki.eachPlayerHealth[0] = PhotonNetwork.playerList[0].ID + " " + PhotonNetwork.playerList[0].NickName;
	//                break;
	//            case 2:
	//                ki.eachPlayerHealth[1] = PhotonNetwork.playerList[1].ID + " " + PhotonNetwork.playerList[1].NickName;
	//                break;
	//            case 3:
	//                ki.eachPlayerHealth[2] = PhotonNetwork.playerList[2].ID + " " + PhotonNetwork.playerList[2].NickName;
	//                break;
	//            case 4:
	//                ki.eachPlayerHealth[3] = PhotonNetwork.playerList[3].ID + " " + PhotonNetwork.playerList[3].NickName;
	//                break;
	//            case 0:
	//                ki.eachPlayerHealth[4] = PhotonNetwork.playerList[4].ID + " " + PhotonNetwork.playerList[4].NickName;
	//                break;
	//            default:
	//                break;
	//        }

	// Array.Reverse(ki.eachPlayerName);

	//    if(PhotonNetwork.player.ID == 1)
	//        ki.eachPlayerName[0] = PhotonNetwork.player.NickName;
	//    else if (PhotonNetwork.player.ID == 2)
	//        ki.eachPlayerName[1] = PhotonNetwork.player.NickName;
	//    else if (PhotonNetwork.player.ID == 3)
	//        ki.eachPlayerName[2] = PhotonNetwork.player.NickName;
	//    else if (PhotonNetwork.player.ID == 4)
	//        ki.eachPlayerName[3] = PhotonNetwork.player.NickName;
	//    else if (PhotonNetwork.player.ID == 5)
	//        ki.eachPlayerName[4] = PhotonNetwork.player.NickName;

	//}

}