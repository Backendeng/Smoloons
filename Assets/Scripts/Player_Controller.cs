using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class Player_Controller: Photon.MonoBehaviour {

	private string FireAxis = "Fire 1";

	public bool canDropBombs = true;
	//Can the player drop bombs?
	public bool canMove = true;
	//Can the player move?
	//Prefabs
	public GameObject bombPrefab;
	//Cached components
	private Rigidbody rigidBody;
	private Transform myTransform;
	private Animator animator;
	private Player player;
	private bool mobile;

	public static Player_Controller Instance;
	// [SerializeField]
	public Text _playerHealth,
	playerKills,
	playerDeaths;
	public Image healthFG;
	public Transform selfSpawnTransform;
	private PhotonView PhotonView;
	private Vector3 TargetPosition;
	private Quaternion TargetRotation;
	// public GameObject cam;
	public GameObject playerGameObject,
	target;
	private Camera c;
	public Player_Controller pm;

	public int deaths;

	GameObject globalKillInc;
	KillsIncrementer globalKi;

	Vector3 d = new Vector3(Screen.width / 2, Screen.width / 2, 0);

	public float max_health,
	curr_health,
	health;

	private Blocks[, ] array_representation;
	private int start_poses = 10;
	public float holding_time;

	public bool movement_status, holding_status;
	public GameObject Map_parent;
	public GameObject floor_prefab;
	public GameObject wall_prefab;
	public Material ghost_material;
	public Sprite player_image;
	public string playername;
	private void Awake() {
		globalKillInc = GameObject.FindGameObjectWithTag("Kills");
		globalKi = globalKillInc.GetComponent < KillsIncrementer > ();
		Instance = this;
		PhotonView = GetComponent < PhotonView > ();
		curr_health = max_health = 1;
		_playerHealth = GetComponentInChildren < Text > ();
		// c = cam.GetComponent<Camera>();
		health = 10;
		target = GameObject.FindGameObjectWithTag("target");
	}

	// Use this for initialization
	void Start() {

		if (Application.CanStreamedLevelBeLoaded("Game")) {
			mobile = false;
		} else {
			mobile = true;
		}
		movement_status = true;
		holding_status = false;
		holding_time = 0.0f;
		player = GetComponent < Player > ();
		//Cache the attached components for better performance and less typing
		rigidBody = GetComponent < Rigidbody > ();
		myTransform = transform.Find("model").transform;
		animator = transform.Find("model").GetComponent < Animator > ();
		Invoke("changeName", 2f);
		Invoke("RPC_sendName", 2f);
	}

	// Update is called once per frame
	void Update() {

		// PhotonView.RPC("healthSet", PhotonTargets.All);

		// if (globalKi.winLose[PhotonNetwork.player.ID - 1].Equals("Winner ! ! !"))
		// {
		//     globalKi.WinLoseText.text = "Winner ! ! !";
		// }
		// else
		//     globalKi.WinLoseText.text = "Loser";

		if (PhotonView.isMine && PhotonNetwork.connectionState == ConnectionState.Connected) {
			UpdateMovement();
		} else {
			SmoothMovement();
		}
		if (PhotonView.isMine) {
			GameUI.Instance.playerHealth.text = curr_health.ToString();
			setKills();
			setDeaths();
			// GameUI.Instance.playerKills.text = 
			//  PhotonView.RPC("RPC_PlayerUICameraFollow", PhotonTargets.OthersBuffered);
		}
		if (holding_status){
			holding_time += Time.deltaTime;
	
			if(holding_time > 5)
			{
				transform.Find("bubble").gameObject.SetActive(false);
				animator.SetBool("holding", false);
				animator.SetBool("hitup", false);
			}
			if(holding_time > 6)
			{
				holding_time = 0.0f;
				holding_status = false;
				movement_status = true;
			}
		}
		for (int i = 0; i < globalKi.allPlayers.Length; i++) {
			if (PhotonView.isMine) {
				if ( gameObject.tag == "Ghost"){
					globalKi.allPlayers[i].transform.Find("model").gameObject.SetActive(true);
					globalKi.allPlayers[i].transform.Find("name").gameObject.SetActive(true);
					GameObject.FindGameObjectWithTag("light").transform.GetChild(0).gameObject.SetActive(false);
					GameObject.FindGameObjectWithTag("light").transform.GetChild(1).gameObject.SetActive(true);
				} else {
					if (globalKi.allPlayers[i].tag == "Ghost") {
						globalKi.allPlayers[i].transform.Find("model").gameObject.SetActive(false);
						globalKi.allPlayers[i].transform.Find("name").gameObject.SetActive(false);
					}
				}
			}
		}
	}

	private void UpdateMovement() {
		animator.SetBool("Walking", false);

		//Depending on the player number, use different input for moving
		UpdatePlayer2Movement();

	}

	/// <summary>
	/// Updates Player 2's movement and facing rotation using the arrow keys and drops bombs using Enter or Return
	/// </summary>
	private void UpdatePlayer2Movement() {

		if (movement_status)
		{
			if (Input.GetButton("Up"))
			{ //Up movement

				rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, player.moveSpeed);
				myTransform.rotation = Quaternion.Euler(0, -90, -30);
				animator.SetBool("Walking", true);
			}

			if (Input.GetButton("Left")) { //Left movement
				rigidBody.velocity = new Vector3( - player.moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
				myTransform.rotation = Quaternion.Euler(-30, 180, 0);
				animator.SetBool("Walking", true);
			}

			if (Input.GetButton("Down")) { //Down movement
				rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -player.moveSpeed);
				myTransform.rotation = Quaternion.Euler(0, 90, 30);
				animator.SetBool("Walking", true);
			}

			if (Input.GetButton("Right")) { //Right movement
				rigidBody.velocity = new Vector3(player.moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
				myTransform.rotation = Quaternion.Euler(30, 0, 0);
				animator.SetBool("Walking", true);
			}

			if (mobile) {
				Vector3 vel = new Vector3(Input.GetAxis("Horizontal") * player.moveSpeed, rigidBody.velocity.y, Input.GetAxis("Vertical") * player.moveSpeed);
				if (vel != rigidBody.velocity) {
					rigidBody.velocity = vel;
					myTransform.rotation = Quaternion.Euler(0, FindDegree(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 0);
					animator.SetBool("Walking", true);
				}
			}

			if (canDropBombs && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))) { //Drop Bomb. For Player 2's bombs, allow both the numeric enter as the return key or players 
				//without a numpad will be unable to drop bombs

				DropBomb();

			}
		}
	}

	public static float FindDegree(float x, float y) {
		float value = (float)((Mathf.Atan2(x, y) / Math.PI) * 180f);
		if (value < 0) value += 360f;

		return value;
	}

	/// <summary>
	/// Drops a bomb beneath the player
	/// </summary>
	public void DropBomb() {
		if (player.bombs != 0) {

			player.bombs--;

			if (bombPrefab) { //Check if bomb prefab is assigned first
				GameObject go = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Bomb"), new Vector3(Mathf.RoundToInt(myTransform.position.x), bombPrefab.transform.position.y, Mathf.RoundToInt(myTransform.position.z)), bombPrefab.transform.rotation, 0);

				go.GetComponent < Bomb > ().explode_size = player.explosion_power;
				go.GetComponent < Bomb > ().player = player;
				if (player.canKick) {
					go.GetComponent < Rigidbody > ().isKinematic = false; // make bomb kickable
				}
				canDropBombs = false;
			}
		}
	}

	public void RPC_SpawnPlayer(Transform spawnPoint, string shape, string name) {

		GameObject playerObject = PhotonNetwork.Instantiate(Path.Combine("Prefabs", shape), spawnPoint.position, Quaternion.identity, 0);
		playerObject.name = "Monkey";
		playerObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = name;
		playername = name;
	}

	[PunRPC]
	private void RPC_PlayerUICameraFollow() {

		//canvas.transform.LookAt(this.cam.transform);

	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

		if (stream.isWriting) {
			stream.SendNext(transform.position);
			stream.SendNext(myTransform.rotation);
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
		transform.Find("model").transform.rotation = Quaternion.RotateTowards(transform.Find("model").transform.rotation, TargetRotation, 500 * Time.deltaTime);

	}

	private void CheckInput() {

		float moveSpeed = 25f;
		float rotateSpeed = 250f;

		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		transform.position += transform.forward * vertical * moveSpeed * Time.deltaTime;
		transform.Rotate(new Vector3(0, horizontal * rotateSpeed * Time.deltaTime, 0));

	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Explosion")) {
			if (!holding_status){
				int viewID =  PhotonView.viewID;
				PhotonView.RPC("BubbleMonkey", PhotonTargets.All, viewID);
				// movement_status = false;
				// holding_time  = .0f;
				// holding_status = true;
				// myTransform.rotation = Quaternion.Euler(0, 90, 30);
				// animator.SetBool("hitup", true);
				// animator.SetBool("holding", true);
				// transform.Find("bubble").gameObject.SetActive(true);
			} else if (holding_time > 0.4) {
				int viewID =  PhotonView.viewID;
				// PhotonView.RPC("DeletePlayer", PhotonTargets.All, viewID);
				// GhostMonkey();

				PhotonView.RPC("GhostMonkey", PhotonTargets.All, viewID, collision.gameObject.name);
			}
		}
		if (collision.collider.CompareTag("LastStone")) {
			Debug.Log("laststone");
			int viewID =  PhotonView.viewID;
			PhotonView.RPC("DeletePlayer", PhotonTargets.All, viewID);
		}
		if (collision.collider.CompareTag("Player")) {
			if (holding_time > 0.4 && holding_status) {
				int viewID =  PhotonView.viewID;
				PhotonView.RPC("GhostMonkey", PhotonTargets.All, viewID, collision.gameObject.name);
			}
		}

	}

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

	[PunRPC]
	private void BubbleMonkey(int viewID) {
		GameObject bubbleMonkey = PhotonView.Find(viewID).gameObject;
		bubbleMonkey.transform.GetComponent<Player_Controller>().movement_status = false;
		bubbleMonkey.transform.GetComponent<Player_Controller>().holding_time  = .0f;
		bubbleMonkey.transform.GetComponent<Player_Controller>().holding_status = true;
		bubbleMonkey.transform.Find("model").transform.rotation = Quaternion.Euler(0, 90, 30);
		bubbleMonkey.transform.Find("model").GetComponent < Animator > ().SetBool("hitup", true);
		bubbleMonkey.transform.Find("model").GetComponent < Animator > ().SetBool("holding", true);
		bubbleMonkey.transform.Find("bubble").gameObject.SetActive(true);
	}
	[PunRPC]
	private void GhostMonkey(int viewID, string name) {
		// canDropBombs = false;
		GameObject ghostMonkey = PhotonView.Find(viewID).gameObject;
		ghostMonkey.transform.GetComponent<Player_Controller>().canDropBombs = false;
		// transform.GetComponent<CapsuleCollider>().enabled = false;
		ghostMonkey.transform.GetComponent<CapsuleCollider>().center = new Vector3(0f, 2.5f, 0f);
		ghostMonkey.gameObject.tag = "Ghost";
		for (int i = 1 ; i < 7; i++ ){
			ghostMonkey.transform.Find("model").GetChild(i).GetComponent<SkinnedMeshRenderer> ().material = ghost_material;
		}
		ghostMonkey.transform.Find("bubble").gameObject.SetActive(false);
		ghostMonkey.transform.GetComponent<Player_Controller>().holding_time = 10;
		ghostMonkey.transform.GetComponent<AudioSource>().enabled = true;
		
		GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer ki = KillsInc.GetComponent < KillsIncrementer > ();
		for ( int i = 0 ; i < 6 ; i++ ){
			if (ki.eachPlayerKillOrder[i] == ""){
				ki.eachPlayerKillOrder[i] = ghostMonkey.transform.GetChild(1).GetComponent<TextMeshPro>().text;
				break;
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
	private void DeletePlayer(int viewID) {
		DeathAnimation(PhotonView.Find(viewID).gameObject);
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
		case 5:
			ki.eachPlayerKills[4]++;
			ki.eachPlayerScore[4] = ki.eachPlayerScore[4] + 25;
			break;
		case 0:
			ki.eachPlayerKills[5]++;
			ki.eachPlayerScore[5] = ki.eachPlayerScore[5] + 25;
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

	private void DeathAnimation(GameObject DeathPlayer)
	{
		DeathPlayer.transform.Find("model").transform.rotation = Quaternion.Euler(0, 90, 30);
		DeathPlayer.transform.Find("model").GetComponent < Animator > ().SetBool("die", true);
		DeathPlayer.transform.GetComponent<CapsuleCollider>().enabled = false;
		DeathPlayer.transform.position = new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
		Destroy(DeathPlayer, 2);
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
		case 5:
			ki.eachPlayerDeaths[4]++;
			break;
		case 0:
			ki.eachPlayerDeaths[5]++;
			break;
		default:
			break;
		}
	}

	private void setDeaths() {
		GameObject go = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer k = go.GetComponent < KillsIncrementer > ();

		GameUI.Instance.playerDeaths.text = k.eachPlayerDeaths[(PhotonNetwork.player.ID - 1) % 6].ToString();

	}

	private void changeName() {
		PhotonView.RPC("setName", PhotonTargets.AllBuffered, PhotonNetwork.player.ID);
	}

	private void RPC_sendName() {
		if (PhotonView.isMine) {
			int viewID =  PhotonView.viewID;
			string name = transform.GetChild(1).GetComponent<TextMeshPro>().text;
			PhotonView.RPC("sendName", PhotonTargets.All, viewID, name);
		}
	}

	[PunRPC]
	private void sendName (int viewID, string name){
		PhotonView.Find(viewID).gameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = name;
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
		case 5:
			ki.eachPlayerName[4] = PhotonNetwork.playerList[4].ID + " " + PhotonNetwork.playerList[4].NickName;
			break;
		case 0:
			ki.eachPlayerName[5] = PhotonNetwork.playerList[5].ID + " " + PhotonNetwork.playerList[5].NickName;
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

	public void RPC_DeleteBlock (int viewID) {
		Debug.Log(viewID);
		PhotonView.RPC("DeleteBlock", PhotonTargets.All, viewID);
	}

	[PunRPC]
    private void DeleteBlock(int viewID)
    {
		Debug.Log(viewID);
        PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
    }

}