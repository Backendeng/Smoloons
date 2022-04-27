using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillsIncrementer: MonoBehaviour {

	public static KillsIncrementer Instance;

	public string[] eachPlayerKillOrder = new string[6]; // death player order - player name
	public string[] eachPlayerKills = new string[6]; // player kill count
	public string[] eachPlayerBreak = new string[6]; // player break block count
	public string[] eachPlayerBomb = new string[6]; // player bomb creation count
	public string[] eachPlayerStep = new string[6]; // player taken step

	
	public string[] eachPlayerName = new string[6];
	public string[] eachPlayerOrderName = new string[6];
	public string[] ePN = new string[6];
	public string[] fePN = new string[6];
	public string[] winLose = new string[6];
	public int[] eachPlayerDeaths = new int[6];
	public int[] eachPlayerScore = new int[6];

	public GameObject WinLosePanel;
	public GameObject scroller,	rankCalc;
	public GameObject photonmap1;
	public GameObject photonmap2;
	public GameObject SummaryListingPrefab;
	public GameObject PlayerStatus;
	public GameObject LeaveMatch;
	public GameObject DanceObject;
	public GameObject [] WinNumber = new GameObject[3];
	public GameObject[] allPlayers = new GameObject[6];
	public GameObject [] playerstatus;

	public Transform UI_Parent;
	public Transform PlayerOrderParent;

	public Text WinLoseText;
	public Text WinLoseKillCount;
	public Text WinLoseBombCount;
	public Text WinLoseBreakCount;
	public Text WinLoseStepCount;
	public Text timerText;

	public RankCalc rankCalcInstance;
	PhotonView pv;

	public float[] eachPlayerHealth = new float[6];
	public float startTime, winnerTime,	timer;
	
	public int index;
	
	public bool createStonestats = false;
	public bool EndGame_status;

	public string PlayerObjectName = "";
	public string playerObjectname = "";
	public string playername = "";
	public string playerkillCount = "";
	public string playerBombCount = "";
	public string playerBreakCount = "";
	public string playerStepCount = "";
		
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();

	
	private void Awake() {
		// j = 0;

		startTime = 200; // game play time. after 200s, create laststone.

		scroller = GameObject.FindGameObjectWithTag("Scroller");
		rankCalc = GameObject.FindGameObjectWithTag("Rank");
		timer = 20; // count time. why 20? if time is 0 when start game, destory block.
		pv = GetComponent < PhotonView > ();

		ePN = new string[PhotonNetwork.countOfPlayers];
		fePN = new string[PhotonNetwork.countOfPlayers];
		winLose = new string[PhotonNetwork.countOfPlayers];
		playerstatus = new GameObject[PhotonNetwork.room.PlayerCount];

		for (int i = 0; i < eachPlayerKills.Length; i++) {
			eachPlayerKills[i] = "0";
			eachPlayerBomb[i] = "0";
			eachPlayerBreak[i] = "0";
			eachPlayerStep[i] = "0";
			eachPlayerName[i] = "";
			eachPlayerKillOrder[i] = "";
			eachPlayerOrderName[i] = "";
			eachPlayerScore[i] = 0;
			eachPlayerDeaths[i] = 0;
			eachPlayerHealth[i] = 100;
		}
		
		for (int i = 0; i < winLose.Length; i++) {
			winLose[i] = "";
		}

		for (int i = 0; i < 3; i++) {
			WinNumber[i] = WinLosePanel.transform.GetChild(i+2).gameObject;
		}
	
		for (int i  = 0; i < PhotonNetwork.room.PlayerCount; i++ ){
			playerstatus[i] = Instantiate(SummaryListingPrefab);
			playerstatus[i].transform.SetParent(UI_Parent, false);
		}

	}

	void Start() {
		WinLosePanel.SetActive(false);
		winnerTime = 0;
		EndGame_status = false;
		createStonestats = false;
	}

	// Update is called once per frame
	void Update() {
		allPlayers = FindGameObjectsWithSameName("Monkey"); // Get All players with name including "Monkey"

		// sets right corner all player status info
		for (int i = 0; i < allPlayers.Length; i++) {
			playerstatus[i].transform.Find("Name").transform.GetComponent<Text>().text = allPlayers[i].transform.GetChild(1).GetComponent<TextMeshPro>().text;
			playerstatus[i].transform.Find("Image").transform.GetComponent<Image>().sprite = allPlayers[i].transform.GetComponent<Player_Controller>().player_image;
		}

		// if player die or creat, change right corner status.
		if (playerstatus.Length != PhotonNetwork.room.PlayerCount){
			foreach(Transform child in UI_Parent) {
				Destroy(child.gameObject);
			}
			playerstatus = new GameObject[PhotonNetwork.room.PlayerCount];
			for (int i  = 0; i < PhotonNetwork.room.PlayerCount; i++ ){
				playerstatus[i] = Instantiate(SummaryListingPrefab);
				playerstatus[i].transform.SetParent(UI_Parent, false);
			}
		}

		// when player die, player status transparency is 50%.
		for (int i = 0; i < playerstatus.Length; i++ ){
			for (int j = 0; j < allPlayers.Length; j++ ){
				if(playerstatus[i].transform.Find("Name").transform.GetComponent<Text>().text == allPlayers[j].transform.GetChild(1).GetComponent<TextMeshPro>().text && allPlayers[j].tag == "Ghost") {
					playerstatus[i].transform.GetChild(0).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 120);
					playerstatus[i].transform.GetChild(2).transform.GetComponent<Text>().color = new Color32(255, 255, 255, 120);
					playerstatus[i].transform.GetChild(3).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 120);
				}
			}
		}

		// Change the position so that all players are not visible when the game is over. Must not destroy all players.
		if (EndGame_status && winnerTime > 0.3f) {
			for (int i = 0; i < allPlayers.Length; i++) {
				allPlayers[i].transform.position = new Vector3 (50, 50, 50);
			}
		}
		
		timer = startTime - Time.timeSinceLevelLoad; // count time 200s.

		string minutes = ((int)timer / 60).ToString();
		string seconds = (timer % 60).ToString("f0");

		timerText.text = minutes + " : " + seconds;

		GameObject [] blocks = GameObject.FindGameObjectsWithTag("Breakable"); // find blocks count. because if there is no blocks, laststone start.

		// When time is up or there are no blocks, start laststone
		if (timer <= 0 && !createStonestats || !createStonestats && blocks.Length == 0 && timer < 180) {
		    timerText.text = "0" + " : " + "0"; 
			pv.RPC("CreateStone", PhotonTargets.All);
		}

		EndGame();

	}

	private void WinLose() {
		for (int i = 0; i < PhotonNetwork.countOfPlayers; i++) {
			if (rankCalcInstance.fs[i].Equals("1")) {
				winLose[i] = "Winner ! ! !";
			}
			else winLose[i] = "Loser";
		}
	}

	public GameObject[] FindGameObjectsWithSameName(string name)
	{
		GameObject[] allObjs = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		List<GameObject> likeNames = new List<GameObject>();
		foreach (GameObject obj in allObjs)
		{
			if (obj.name.Contains(name))
			{
				likeNames.Add(obj);
			}
		}
		return likeNames.ToArray();
	}

	// recieve start event.
	[PunRPC]
	private void CreateStone() {
		timer = 5000;
		StartStone();
	}

	// create laststone.
	public void StartStone() {
		Debug.Log("startStone");
		if (photonmap1.activeSelf)
			photonmap1.GetComponent<PhotonMap>().startStone();
		else 
			photonmap2.GetComponent<PhotonMap>().startStone();

		createStonestats = true;
	}

	public void EndGame() {

		if (eachPlayerKillOrder[0] != "" && GameObject.FindGameObjectsWithTag("Player").Length < 2 || EndGame_status) {
			
			// winner panel time.
			winnerTime += Time.deltaTime;
			LeaveMatch.SetActive(false);

			if (photonmap1.activeSelf)
				photonmap1.GetComponent<PhotonMap>().startStonestats = false;
			else 
				photonmap2.GetComponent<PhotonMap>().startStonestats = false;
			
			if (winnerTime > 0.3f && !EndGame_status) {
				winnerTime = 0;
				EndGame_status = true;
			}
			
			if (winnerTime > 0.3f) {
				int order = 6;
				for (int i = eachPlayerKillOrder.Length; i > 0; i--) {
					if(eachPlayerKillOrder[i-1] != "") {
						order = i;
						break;
					}
				}

				WinLosePanel.SetActive(true);
				// if (GameObject.FindGameObjectsWithTag("Player").Length == 0) {
					// when player is 3 over.
					if ( allPlayers.Length > 2 ){
						
						if (winnerTime < 5){
							WinLoseText.text = eachPlayerKillOrder[order - 3];
							WinLoseKillCount.text = "Kill : " + eachPlayerKills[order - 3];
							WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 3];
							WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 3];
							WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 3];
							WinNumber[2].SetActive(true);
							WinNumber[1].SetActive(false);
							WinNumber[0].SetActive(false);
							WinnerAnimation (eachPlayerOrderName[order - 3], eachPlayerKillOrder[order - 3], false, false, true);
						}
						if (winnerTime > 5 && winnerTime < 10){
							WinLoseText.text = eachPlayerKillOrder[order - 2];
							WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 2];
							WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 2];
							WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 2];
							WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 2];
							WinNumber[2].SetActive(false);
							WinNumber[1].SetActive(true);
							WinNumber[0].SetActive(false);
							WinnerAnimation (eachPlayerOrderName[order - 2], eachPlayerKillOrder[order - 2], false, true, false);
						}
						if (winnerTime > 10 && winnerTime < 11 ){
							WinLoseText.text = eachPlayerKillOrder[order - 1];
							WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 1];
							WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 1];
							WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 1];
							WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 1];
							WinNumber[2].SetActive(false);
							WinNumber[1].SetActive(false);
							WinNumber[0].SetActive(true);
							WinnerAnimation (eachPlayerOrderName[order - 1], eachPlayerKillOrder[order - 1], true, false, false);
						}
					}

					// when player is 2.
					if (allPlayers.Length == 2) {
						
						if (winnerTime < 5){
							WinLoseText.text = eachPlayerKillOrder[order - 2];
							WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 2];
							WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 2];
							WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 2];
							WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 2];
							WinNumber[1].SetActive(true);
							WinNumber[2].SetActive(false);
							WinNumber[0].SetActive(false);
							WinnerAnimation (eachPlayerOrderName[order - 2], eachPlayerKillOrder[order - 2], false, false, true);
						}
						if (winnerTime > 5 && winnerTime < 6 ){
							WinLoseText.text = eachPlayerKillOrder[order - 1];
							WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 1];
							WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 1];
							WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 1];
							WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 1];
							WinNumber[2].SetActive(false);
							WinNumber[1].SetActive(false);
							WinNumber[0].SetActive(true);
							WinnerAnimation (eachPlayerOrderName[order - 1], eachPlayerKillOrder[order - 1], true, false, false);
						}
					}
					
					// when player is 1.
					if (allPlayers.Length == 1) {
						
						if (winnerTime < 5){
							WinLoseText.text = eachPlayerKillOrder[order - 1];
							WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 1];
							WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 1];
							WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 1];
							WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 1];
							WinNumber[2].SetActive(false);
							WinNumber[1].SetActive(false);
							WinNumber[0].SetActive(true);
							WinnerAnimation (eachPlayerOrderName[order - 1], eachPlayerKillOrder[order - 1], true, false,false);
						}
					}
				
				// when game is over, dont show map.
				AllDeleteObjets();

			}
			
		}
		
	}

	public void AllDeleteObjets () {

		if (GameObject.Find("Grand_17") != null)
			GameObject.Find("Grand_17").SetActive(false);
		if (GameObject.Find("Grand_13") != null)
			GameObject.Find("Grand_13").SetActive(false);
		if (GameObject.Find("PowerUp2(Clone)") != null)
			GameObject.Find("PowerUp2(Clone)").SetActive(false);
		if (GameObject.Find("PowerUp(Clone)") != null)
			GameObject.Find("PowerUp(Clone)").SetActive(false);
		if (GameObject.Find("PowerUp1(Clone)") != null)
			GameObject.Find("PowerUp1(Clone)").SetActive(false);

		GameObject [] blocks = FindGameObjectsWithSameName("Breakable(Clone)");
		for (int i = 0; i < blocks.Length; i++) {
			Destroy(blocks[i]);
		}
		GameObject [] bombs = FindGameObjectsWithSameName("Bomb(Clone)");
		for (int i = 0; i < bombs.Length; i++) {
			Destroy(bombs[i]);
		}

		PlayerStatus.SetActive(false);
		GameObject.FindGameObjectWithTag("light").transform.GetChild(0).gameObject.SetActive(true);
		GameObject.FindGameObjectWithTag("light").transform.GetChild(1).gameObject.SetActive(false);
		if (DanceObject)
			DanceObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 90f, 70f);
		
	}

	public void WinnerAnimation(string playerObject, string playerOrderName, bool dance, bool clap, bool cry) {

		if (PlayerObjectName != playerObject) {
			if (PlayerOrderParent.childCount > 0)
				Destroy(PlayerOrderParent.transform.GetChild(0).gameObject);
			DanceObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/"+ playerObject), new Vector3(2, 2, 4), Quaternion.identity);
			DanceObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 90f, 70f);
			DanceObject.tag = "Untagged";
			DanceObject.transform.GetComponent<Player_Controller>().hand_status = false;
			DanceObject.transform.GetComponent<Player_Controller>().dance_status = dance;
			DanceObject.transform.GetComponent<Player_Controller>().cry_status = cry;
			DanceObject.transform.GetComponent<Player_Controller>().clap_status = clap;
			DanceObject.transform.localScale = new Vector3(5f, 5f, 5f);
			DanceObject.name = playerOrderName;
			DanceObject.transform.SetParent(PlayerOrderParent);
			PlayerObjectName = playerObject;
		}

	}
}