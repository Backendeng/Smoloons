using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class Player_Controller: Photon.MonoBehaviour {

	private Rigidbody rigidBody;
	private Transform myTransform;
	private Animator animator;
	private Player player;
	private PhotonView PhotonView;
	private Vector3 TargetPosition;
	private Quaternion TargetRotation;
	private bool mobile;

	public static Player_Controller Instance;
	public KillsIncrementer globalKi;
	public Material ghost_material;
	public Sprite player_image;
	public GameObject bombPrefab;
	public GameObject playerGameObject, target;
	public GameObject globalKillInc;
	public GameObject Map_parent;
	public GameObject floor_prefab;
	public GameObject wall_prefab;
	public GameObject DeletePlayerObject;

	public float holding_time, hand_time, delete_time, end_time;
	public bool canDropBombs, movement_status, holding_status, delete_status, animation_status, Target_animation_status, hand_status, dance_status, cry_status, clap_status, death_status;
	public string playername, shape;
	public int count_kill, count_hit, count_bomb, count_break, count_step, PlayerKillID, PlayerViewID;
	public int order = 1;
	public int [] orderList;
	public Vector3 prevPosition;

	private void Awake() {
		globalKillInc = GameObject.FindGameObjectWithTag("Kills");
		globalKi = globalKillInc.GetComponent < KillsIncrementer > ();
		Instance = this;
		PhotonView = GetComponent < PhotonView > ();
		target = GameObject.FindGameObjectWithTag("target");
	}

	// Use this for initialization
	void Start() {

		if (Application.CanStreamedLevelBeLoaded("Game")) {
			mobile = false;
		} else {
			mobile = true;
		}
		movement_status = holding_status = animation_status = delete_status = false;
		canDropBombs = true;
		holding_time = hand_time = delete_time = end_time = 0.0f;
		count_kill = count_hit = count_bomb = count_break = count_step = 0;
		PlayerViewID = PlayerKillID = 0;
		playername = shape = "";
		player = GetComponent < Player > ();
		rigidBody = GetComponent < Rigidbody > ();
		myTransform = transform.Find("model").transform;
		animator = transform.Find("model").GetComponent < Animator > ();

		if (!globalKi.EndGame_status){
			Invoke("changeName", 2f);
			Invoke("RPC_sendName", 2f);
			dance_status = cry_status = clap_status = death_status = false;
			hand_status = true;
		}
	}

	// Update is called once per frame
	void Update() {

		if (!globalKi.EndGame_status){
			if (PhotonView.isMine && PhotonNetwork.connectionState == ConnectionState.Connected) {
				UpdateMovement();
			} else {
				SmoothMovement();
			}

			if (PhotonView.isMine) {
				setKills();
				setDeaths();
			}

			if (holding_status){
				holding_time += Time.deltaTime;
		
				if(holding_time > 4)
				{
					transform.Find("bubble").gameObject.SetActive(false);
					animator.SetBool("holding", false);
					animator.SetBool("hitup", false);
				}
				if(holding_time > 5)
				{
					if (player.bombs != 0)
						canDropBombs = true;
					holding_status = false;
					movement_status = true;
					holding_time = 0.0f;
				}
			}

			for (int i = 0; i < globalKi.allPlayers.Length; i++) {
				if (PhotonView.isMine) {
					if ( gameObject.tag == "Ghost"){
						globalKi.allPlayers[i].transform.Find("model").gameObject.SetActive(true);
						globalKi.allPlayers[i].transform.Find("name").gameObject.SetActive(true);
						if (!globalKi.EndGame_status){
							GameObject.FindGameObjectWithTag("light").transform.GetChild(0).gameObject.SetActive(false);
							GameObject.FindGameObjectWithTag("light").transform.GetChild(1).gameObject.SetActive(true);
						}
					} else {
						if (globalKi.allPlayers[i].tag == "Ghost") {
							globalKi.allPlayers[i].transform.Find("model").gameObject.SetActive(false);
							globalKi.allPlayers[i].transform.Find("name").gameObject.SetActive(false);
						}
					}
				}
			}

		} else {
			movement_status = false;
			holding_status = false;
			animation_status = false;
			delete_status = false;
			end_time += Time.deltaTime;
			if (!death_status && gameObject.tag == "Player" && end_time > 1 && gameObject.name == "Monkey"){
				for ( int i = 0 ; i < 6 ; i++ ){
				if (globalKi.eachPlayerKillOrder[i] == "" || globalKi.eachPlayerKillOrder[i] == transform.GetChild(1).GetComponent<TextMeshPro>().text){
					// if (globalKi.eachPlayerKillOrder[i] == transform.GetChild(1).GetComponent<TextMeshPro>().text) {
						// break;
					// } else {
						// globalKi.eachPlayerKillOrder[i] = transform.GetChild(1).GetComponent<TextMeshPro>().text;
						// globalKi.eachPlayerOrderName[i] = transform.GetChild(3).GetComponent<TextMesh>().text;
						// globalKi.eachPlayerKills[i] = count_kill;
						// globalKi.eachPlayerBomb[i] = count_bomb;
						// globalKi.eachPlayerBreak[i] = count_break;
						// globalKi.eachPlayerStep[i] = count_step;
						// PhotonView.RPC("DeathMonkeyInfo", PhotonTargets.All, i, transform.GetChild(1).GetComponent<TextMeshPro>().text, transform.GetChild(3).GetComponent<TextMesh>().text, transform.GetChild(4).GetComponent<TextMesh>().text, transform.GetChild(5).GetComponent<TextMesh>().text, transform.GetChild(6).GetComponent<TextMesh>().text, transform.GetChild(7).GetComponent<TextMesh>().text);
						PhotonView.RPC("DeathMonkeyInfo", PhotonTargets.All, i, transform.GetChild(1).GetComponent<TextMeshPro>().text);
						PhotonView.RPC("DeathMonkeyInfo1", PhotonTargets.All, i, transform.GetChild(3).GetComponent<TextMesh>().text);
						PhotonView.RPC("DeathMonkeyInfo2", PhotonTargets.All, i, transform.GetChild(4).GetComponent<TextMesh>().text);
						PhotonView.RPC("DeathMonkeyInfo3", PhotonTargets.All, i, transform.GetChild(5).GetComponent<TextMesh>().text);
						PhotonView.RPC("DeathMonkeyInfo4", PhotonTargets.All, i, transform.GetChild(6).GetComponent<TextMesh>().text);
						PhotonView.RPC("DeathMonkeyInfo5", PhotonTargets.All, i, transform.GetChild(7).GetComponent<TextMesh>().text);
						death_status = true;
						break;
					// }
				}
			}
				// PhotonView.Find(PhotonView.viewID).transform.GetComponent<Player_Controller>().count_kill = count_kill;
				// PhotonView.Find(PhotonView.viewID).transform.GetComponent<Player_Controller>().count_bomb = count_bomb;
				// PhotonView.Find(PhotonView.viewID).transform.GetComponent<Player_Controller>().count_break = count_break;
				// PhotonView.Find(PhotonView.viewID).transform.GetComponent<Player_Controller>().count_step = count_step;
				// PhotonView.RPC("MonkeyInfo", PhotonTargets.All, PhotonView.viewID, count_bomb, count_break, count_step);
				// death_status = true;
			}

			transform.Find("bubble").gameObject.SetActive(false);
			animator.SetBool("holding", false);
			animator.SetBool("hitup", false);
		}
		
		if (dance_status) {
			animator.SetBool("Dance", true );
		}
		if (cry_status) {
			animator.SetBool("crying", true );
		}
		if (clap_status) {
			animator.SetBool("clap", true );
		}

		hand_time += Time.deltaTime;
		if (hand_time < 2 && hand_status) {
			animator.SetBool("Hand", true );
		}
		if (hand_time > 2) {
			animator.SetBool("Hand", false );
		}
		if (hand_time > 3) {
			Camera_animation.zoom = true;
		}
		if (hand_time > 5 && hand_time < 6) {
			movement_status = true;
		}

		if (PlayerKillID != 0) {
			GameObject ghostMonkey = PhotonView.Find(PlayerViewID).gameObject;

			for ( int i = 0 ; i < 6 ; i++ ){
				if (globalKi.eachPlayerKillOrder[i] == "" || globalKi.eachPlayerKillOrder[i] == ghostMonkey.transform.GetChild(1).GetComponent<TextMeshPro>().text){
					if (globalKi.eachPlayerKillOrder[i] == ghostMonkey.transform.GetChild(1).GetComponent<TextMeshPro>().text) {
						break;
					} else {
						if (PlayerKillID != PlayerViewID){
							PhotonView.Find(PlayerKillID).transform.GetComponent<Player_Controller>().count_kill += 1;
							PhotonView.Find(PlayerKillID).transform.GetChild(4).GetComponent<TextMesh>().text = PhotonView.Find(PlayerKillID).transform.GetComponent<Player_Controller>().count_kill.ToString();
						// } else {
						// 	globalKi.eachPlayerKillOrder[i] = transform.GetChild(1).GetComponent<TextMeshPro>().text;
						// 	globalKi.eachPlayerOrderName[i] = transform.GetChild(3).GetComponent<TextMesh>().text;
						// 	globalKi.eachPlayerKills[i] = count_kill;
						// 	globalKi.eachPlayerBomb[i] = count_bomb;
						// 	globalKi.eachPlayerBreak[i] = count_break;
						// 	globalKi.eachPlayerStep[i] = count_step;
						}
						if (ghostMonkey.name == "Monkey") {
							PhotonView.RPC("DeathMonkeyInfo", PhotonTargets.All, i, ghostMonkey.transform.GetChild(1).GetComponent<TextMeshPro>().text);
							PhotonView.RPC("DeathMonkeyInfo1", PhotonTargets.All, i, ghostMonkey.transform.GetChild(3).GetComponent<TextMesh>().text);
							PhotonView.RPC("DeathMonkeyInfo2", PhotonTargets.All, i, ghostMonkey.transform.GetChild(4).GetComponent<TextMesh>().text);
							PhotonView.RPC("DeathMonkeyInfo3", PhotonTargets.All, i, ghostMonkey.transform.GetChild(5).GetComponent<TextMesh>().text);
							PhotonView.RPC("DeathMonkeyInfo4", PhotonTargets.All, i, ghostMonkey.transform.GetChild(6).GetComponent<TextMesh>().text);
							PhotonView.RPC("DeathMonkeyInfo5", PhotonTargets.All, i, ghostMonkey.transform.GetChild(7).GetComponent<TextMesh>().text);
						}
						// globalKi.eachPlayerKillOrder[i] = ghostMonkey.transform.GetChild(1).GetComponent<TextMeshPro>().text;
						// globalKi.eachPlayerOrderName[i] = ghostMonkey.transform.GetChild(3).GetComponent<TextMesh>().text;
						// globalKi.eachPlayerKills[i] = ghostMonkey.GetComponent<Player_Controller>().count_kill;
						// globalKi.eachPlayerBomb[i] = ghostMonkey.GetComponent<Player_Controller>().count_bomb;
						// globalKi.eachPlayerBreak[i] = ghostMonkey.GetComponent<Player_Controller>().count_break;
						// globalKi.eachPlayerStep[i] = ghostMonkey.GetComponent<Player_Controller>().count_step;
						PlayerKillID = 0;
						PlayerViewID = 0;
						break;
					}
				}
			}

			
		}

		if (delete_status) {
			delete_time += Time.deltaTime;
			
			if (delete_time > 2) {
				DeletePlayerObject.transform.position = new Vector3 (50, 50, 50);
				delete_time = .0f;
				delete_status = false;
			}
		}
		
	}

	private void UpdateMovement() {
		animator.SetBool("Walking", false);
		animation_status = false;
		//Depending on the player number, use different input for moving
		UpdatePlayer2Movement();

	}

	/// <summary>
	/// Updates Player 2's movement and facing rotation using the arrow keys and drops bombs using Enter or Return
	/// </summary>
	private void UpdatePlayer2Movement() {

		if (movement_status)
		{
			if (Input.GetButton("Up") || Input.GetKey(KeyCode.W))
			{ //Up movement

				rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, player.moveSpeed);
				myTransform.rotation = Quaternion.Euler(0, -90, -30);
				animator.SetBool("Walking", true);
				animation_status = true;
			}

			if (Input.GetButton("Left") || Input.GetKey(KeyCode.A)) { //Left movement
				rigidBody.velocity = new Vector3( - player.moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
				myTransform.rotation = Quaternion.Euler(-30, 180, 0);
				animator.SetBool("Walking", true);
				animation_status = true;
			}

			if (Input.GetButton("Down") || Input.GetKey(KeyCode.S)) { //Down movement
				rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -player.moveSpeed);
				myTransform.rotation = Quaternion.Euler(0, 90, 30);
				animator.SetBool("Walking", true);
				animation_status = true;
			}

			if (Input.GetButton("Right") || Input.GetKey(KeyCode.D)) { //Right movement
				rigidBody.velocity = new Vector3(player.moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
				myTransform.rotation = Quaternion.Euler(30, 0, 0);
				animator.SetBool("Walking", true);
				animation_status = true;
			}

			if (mobile) {
				Vector3 vel = new Vector3(Input.GetAxis("Horizontal") * player.moveSpeed, rigidBody.velocity.y, Input.GetAxis("Vertical") * player.moveSpeed);
				if (vel != rigidBody.velocity) {
					rigidBody.velocity = vel;
					myTransform.rotation = Quaternion.Euler(0, FindDegree(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 0);
					animator.SetBool("Walking", true);
					animation_status = true;
				}
			}

			if (canDropBombs && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))) { //Drop Bomb. For Player 2's bombs, allow both the numeric enter as the return key or players 
				//without a numpad will be unable to drop bombs

				DropBomb();

			}

			// Count step taken
			if (Mathf.Abs(transform.position.x - prevPosition.x) > 1 ){
				prevPosition = new Vector3 (Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
				count_step++;
				transform.GetChild(7).GetComponent<TextMesh>().text = count_step.ToString();
			}
			if (Mathf.Abs(transform.position.z - prevPosition.z) > 1 ){
				prevPosition = new Vector3 (Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
				count_step++;
				transform.GetChild(7).GetComponent<TextMesh>().text = count_step.ToString();
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
				count_bomb++;
				transform.GetChild(5).GetComponent<TextMesh>().text = count_bomb.ToString();
				PhotonView.RPC("setBomb", PhotonTargets.All, go.GetComponent < PhotonView > ().viewID, PhotonView.viewID, player.explosion_power);
				// go.name = PhotonView.viewID.ToString();
				// go.GetComponent < Bomb > ().explode_size = player.explosion_power;
				go.GetComponent < Bomb > ().player = player;
				if (player.canKick) {
					go.GetComponent < Rigidbody > ().isKinematic = false; // make bomb kickable
				}
				canDropBombs = false;
			}
		}
	}

	[PunRPC]
	private void setBomb(int bombViewID, int playerViewID, int power) {
		GameObject bombObject = PhotonView.Find(bombViewID).gameObject;
		bombObject.name = playerViewID.ToString();
		bombObject.GetComponent < Bomb > ().explode_size = power;
	}

	public void RPC_SpawnPlayer(Transform spawnPoint, Transform cameraPoint, string shape, string name) {
		
		Camera_animation.zoom = false;
		Camera_animation.current_monkey_postion = cameraPoint;
		GameObject playerObject = PhotonNetwork.Instantiate(Path.Combine("Prefabs", shape), spawnPoint.position, Quaternion.identity, 0);
		prevPosition = spawnPoint.position;
		playerObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 90, 30);
		playerObject.name = "Monkey";
		playerObject.transform.GetChild(3).GetComponent<TextMesh>().text = shape;
		playerObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = name;
	
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

		if (!globalKi.EndGame_status) {
			if (stream.isWriting ) {
				stream.SendNext(transform.position);
				stream.SendNext(myTransform.rotation);
				stream.SendNext(animation_status);
			}
			else {
				TargetPosition = (Vector3) stream.ReceiveNext();
				TargetRotation = (Quaternion) stream.ReceiveNext();
				Target_animation_status = (bool) stream.ReceiveNext();
			}
		}

	}

	private void SmoothMovement() {

		transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.2f);
		transform.Find("model").transform.rotation = Quaternion.RotateTowards(transform.Find("model").transform.rotation, TargetRotation, 500 * Time.deltaTime);
		if (Target_animation_status) 
			transform.Find("model").GetComponent < Animator > ().SetBool("Walking", true);
		else
			transform.Find("model").GetComponent < Animator > ().SetBool("Walking", false);
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
				if (collision.gameObject.name != "Explosion(Clone)")
					PhotonView.RPC("GhostMonkey", PhotonTargets.All, viewID, collision.gameObject.name);
			}
		}
		if (collision.collider.CompareTag("LastStone")) {
			int viewID =  PhotonView.viewID;
			PhotonView.RPC("DeletePlayer", PhotonTargets.All, viewID);
		}
		if (collision.collider.CompareTag("Player")) {
			if (holding_time > 0.4 && holding_status) {
				int viewID =  PhotonView.viewID;
				PhotonView.RPC("GhostMonkey", PhotonTargets.All, viewID, collision.gameObject.transform.GetComponent<PhotonView>().viewID.ToString()+"(clone)");
			}
		}
		if (collision.collider.CompareTag("step")) {
			count_step++;
		}

	}

	public void OnTriggerEnter(Collider collision) {

		if (collision.gameObject.tag == "weapon") {
			Debug.Log("Touched");
			// Health -= 10;
			if (PhotonView != null && PhotonView.isMine) {

			}
			PhotonView pv;

			if (collision.gameObject.GetPhotonView() != null && PhotonView.isMine) {
				pv = collision.gameObject.GetPhotonView();
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
	private void GhostMonkey(int viewID, string killID) {
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
		// if (killID != viewID.ToString()){
			PlayerViewID = viewID;
			int.TryParse(killID.Split(char.Parse("("))[0], out PlayerKillID);
		// }
		// if (PhotonView.isMine) {
		// 	PhotonView.RPC("killIncrease", PhotonTargets.All, viewID, killID);
		// }

		// GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
		// KillsIncrementer ki = KillsInc.GetComponent < KillsIncrementer > ();
		// for ( int i = 0 ; i < 6 ; i++ ){
		// 	if (ki.eachPlayerKillOrder[i] == ""){
		// 		ki.eachPlayerKillOrder[i] = ghostMonkey.transform.GetChild(1).GetComponent<TextMeshPro>().text;
		// 		ki.eachPlayerOrderName[i] = ghostMonkey.transform.GetChild(3).GetComponent<TextMesh>().text;
		// 		ki.eachPlayerKills[i] = ghostMonkey.GetComponent<Player_Controller>().count_kill;
		// 		break;
		// 	}
		// }
		
		
	}

	[PunRPC]
	private void DeletePlayer(int viewID) {
		DeathAnimation(PhotonView.Find(viewID).gameObject);
	}

	private void setKills() {
		GameObject go = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer k = go.GetComponent < KillsIncrementer > ();

		GameUI.Instance.playerKills.text = k.eachPlayerKills[(PhotonNetwork.player.ID - 1) % 5].ToString();
		GameUI.Instance.playerScore.text = k.eachPlayerScore[(PhotonNetwork.player.ID - 1) % 5].ToString();
	}

	private void DeathAnimation(GameObject DeathPlayer)
	{
		if (DeathPlayer.tag == "Player") {
			PhotonView.RPC("GhostMonkey", PhotonTargets.All, DeathPlayer.transform.GetComponent<PhotonView>().viewID, DeathPlayer.transform.GetComponent<PhotonView>().viewID.ToString());
		}
		DeathPlayer.transform.Find("model").transform.rotation = Quaternion.Euler(0, 90, 30);
		DeathPlayer.transform.Find("model").GetComponent < Animator > ().SetBool("die", true);
		DeathPlayer.transform.GetComponent<CapsuleCollider>().enabled = false;
		DeathPlayer.transform.GetComponent<Player_Controller>().movement_status = false;
		DeathPlayer.transform.position = new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
		DeletePlayerObject = DeathPlayer;
		delete_status = true;
		// Destroy(DeathPlayer, 2);
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
			string shape = transform.GetChild(3).GetComponent<TextMesh>().text;
			string player_name = transform.GetComponent<Player_Controller>().playername;
			PhotonView.RPC("sendName", PhotonTargets.All, viewID, name, shape, player_name);
		}
	}

	[PunRPC]
	private void sendName (int viewID, string name, string shape, string player_name){
		PhotonView.Find(viewID).gameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = name;
		PhotonView.Find(viewID).gameObject.transform.GetChild(3).GetComponent<TextMesh>().text = shape;
		PhotonView.Find(viewID).gameObject.transform.GetComponent<Player_Controller>().playername = player_name;
	}

	[PunRPC]
	private void setName(int id) {

		orderList = new int[PhotonNetwork.playerList.Length];
		for (int i = 0; i < PhotonNetwork.playerList.Length ; i++) {
			orderList[i] = PhotonNetwork.playerList[i].ID;
		}
		for (int i = 0; i < orderList.Length ; i++) {
			if (id > orderList[i] ){
				order++;
			}
		}

		GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer ki = KillsInc.GetComponent < KillsIncrementer > ();
		if (PhotonView.isMine) switch (order % 6) {
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

	public void RPC_DeleteBlock (int viewID) {
		PhotonView.RPC("DeleteBlock", PhotonTargets.All, viewID);
	}

	[PunRPC]
    private void DeleteBlock(int viewID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
    }

	[PunRPC]
    private void DeathMonkeyInfo(int i, string RPC_KillOrder)
    {
		Debug.Log(RPC_KillOrder);
        globalKi.eachPlayerKillOrder[i] = RPC_KillOrder;
    }
	[PunRPC]
    private void DeathMonkeyInfo1(int i, string RPC_OrderName)
    {
		Debug.Log(RPC_OrderName);
		globalKi.eachPlayerOrderName[i] = RPC_OrderName;
    }
	[PunRPC]
    private void DeathMonkeyInfo2(int i, string RPC_count_kill)
    {
		Debug.Log(RPC_count_kill);
		globalKi.eachPlayerKills[i] = RPC_count_kill;
    }
	[PunRPC]
    private void DeathMonkeyInfo3(int i, string RPC_count_bomb)
    {
		Debug.Log(RPC_count_bomb);
		globalKi.eachPlayerBomb[i] = RPC_count_bomb;
    }
	[PunRPC]
    private void DeathMonkeyInfo4(int i, string RPC_count_break)
    {
		Debug.Log(RPC_count_break);
		globalKi.eachPlayerBreak[i] = RPC_count_break;
    }
	[PunRPC]
    private void DeathMonkeyInfo5(int i, string RPC_count_step)
    {
		Debug.Log(RPC_count_step);
		globalKi.eachPlayerStep[i] = RPC_count_step;
    }

	[PunRPC]
    private void MonkeyInfo(int viewID, int RPC_count_bomb, int RPC_count_break, int RPC_count_step)
    {
		
		Transform WinnerMonkey = PhotonView.Find(viewID).transform;
		WinnerMonkey.GetComponent<Player_Controller>().count_bomb = RPC_count_bomb;
		WinnerMonkey.GetComponent<Player_Controller>().count_break = RPC_count_break;
		WinnerMonkey.GetComponent<Player_Controller>().count_step = RPC_count_step;
    }
}