using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillsIncrementer: MonoBehaviour {

	public static KillsIncrementer Instance;

	public string[] eachPlayerKillOrder = new string[6];
	public int[] eachPlayerKills = new int[6];
	public int[] eachPlayerBreak = new int[6];
	public int[] eachPlayerBomb = new int[6];
	public int[] eachPlayerStep = new int[6];
	public int[] eachPlayerDeaths = new int[6];
	public int[] eachPlayerScore = new int[6];
	public string[] eachPlayerName = new string[6];
	public string[] eachPlayerOrderName = new string[6];
	public string[] ePN = new string[6];
	public string[] fePN = new string[6];
	public string[] winLose = new string[6];
	public GameObject WinLosePanel;
	public GameObject [] WinNumber = new GameObject[3];
	public Text WinLoseText;
	public Text WinLoseKillCount;
	public Text WinLoseBombCount;
	public Text WinLoseBreakCount;
	public Text WinLoseStepCount;
	public float[] eachPlayerHealth = new float[6];
	public GameObject[] allPlayers = new GameObject[6];
	public float startTime, winnerTime,
	timer;
	public Text timerText;
	// Use this for initialization
	public int j;
	int index;
	public GameObject scroller,
	rankCalc;
	public RankCalc rankCalcInstance;
	public bool createStonestats = false;
	public GameObject photonmap1;
	public GameObject photonmap2;
	public GameObject [] playerstatus;
	public Transform UI_Parent;
	public GameObject SummaryListingPrefab;
	public GameObject PlayerStatus;
	public bool EndGame_status;
	public string PlayerObjectName = "";
	public Transform PlayerOrderParent;
	public string playerObjectname = "";
	public string playername = "";
	public int playerkillCount = 0;
	public int playerBombCount = 0;
	public int playerBreakCount = 0;
	public int playerStepCount = 0;
	
	private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();

	PhotonView pv;
	private void Awake() {
		j = 0;

		startTime = 200;

		scroller = GameObject.FindGameObjectWithTag("Scroller");
		rankCalc = GameObject.FindGameObjectWithTag("Rank");
		timer = 20;
		pv = GetComponent < PhotonView > ();
		ePN = new string[PhotonNetwork.countOfPlayers];
		fePN = new string[PhotonNetwork.countOfPlayers];
		winLose = new string[PhotonNetwork.countOfPlayers];
		playerstatus = new GameObject[PhotonNetwork.room.PlayerCount];

		for (int i = 0; i < eachPlayerKillOrder.Length; i++) {
			eachPlayerKillOrder[i] = "";
		}
		for (int i = 0; i < eachPlayerKills.Length; i++) {
			eachPlayerKills[i] = 0;
			eachPlayerBomb[i] = 0;
			eachPlayerBreak[i] = 0;
			eachPlayerStep[i] = 0;
		}
		for (int i = 0; i < eachPlayerDeaths.Length; i++) {
			eachPlayerDeaths[i] = 0;
		}
		for (int i = 0; i < eachPlayerScore.Length; i++) {
			eachPlayerScore[i] = 0;
		}
		for (int i = 0; i < eachPlayerName.Length; i++) {
			eachPlayerName[i] = "";
			eachPlayerOrderName[i] = "";
		}

		for (int i = 0; i < eachPlayerName.Length; i++) {
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
	}

	// Update is called once per frame
	void Update() {
		allPlayers = FindGameObjectsWithSameName("Monkey");

		for (int i = 0; i < allPlayers.Length; i++) {
			playerstatus[i].transform.Find("Name").transform.GetComponent<Text>().text = allPlayers[i].transform.GetChild(1).GetComponent<TextMeshPro>().text;
			playerstatus[i].transform.Find("Image").transform.GetComponent<Image>().sprite = allPlayers[i].transform.GetComponent<Player_Controller>().player_image;
		}

		for (int i = 0; i < playerstatus.Length; i++ ){
			for (int j = 0; j < allPlayers.Length; j++ ){
				if(playerstatus[i].transform.Find("Name").transform.GetComponent<Text>().text == allPlayers[j].transform.GetChild(1).GetComponent<TextMeshPro>().text && allPlayers[j].tag == "Ghost") {
					playerstatus[i].transform.GetChild(0).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 120);
					playerstatus[i].transform.GetChild(2).transform.GetComponent<Text>().color = new Color32(255, 255, 255, 120);
					playerstatus[i].transform.GetChild(3).transform.GetComponent<Image>().color = new Color32(255, 255, 255, 120);
				}
			}
		}

		if (EndGame_status) {
			for (int i = 0; i < allPlayers.Length; i++) {
				allPlayers[i].transform.position = new Vector3 (50, 50, 50);
			}
		}

		timer = startTime - Time.timeSinceLevelLoad;

		string minutes = ((int)timer / 60).ToString();
		string seconds = (timer % 60).ToString("f0");

		timerText.text = minutes + " : " + seconds;

		GameObject [] blocks = FindGameObjectsWithSameName("Breakable(Clone)");

		if (timer <= 0 && !createStonestats || !createStonestats && blocks.Length == 0) {
		    
		    timerText.text = "0" + " : " + "0"; 
			pv.RPC("CreateStone", PhotonTargets.All);
		    // WinLosePanel.SetActive(true);
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

	[PunRPC]
	private void CreateStone() {
		timer = 5000;
		StartStone();
	}

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
			winnerTime += Time.deltaTime;
			WinLosePanel.SetActive(true);
			// if (winnerTime < 1){
			// 	_playerCustomProperties["PlayerReady"] = false;
        	// 	PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);
			// }
			
			
			int order = 6;
			for (int i = eachPlayerKillOrder.Length; i > 0; i--) {
				if(eachPlayerKillOrder[i-1] != "") {
					order = i;
					break;
				}
			}
			
			if (GameObject.FindGameObjectsWithTag("Player").Length == 0) {
				if ( allPlayers.Length > 2 ){
					
					if (winnerTime < 5 && winnerTime > .1f){
						WinLoseText.text = eachPlayerKillOrder[order - 3];
						WinLoseKillCount.text = "Kill : " + eachPlayerKills[order - 3].ToString();
						WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 3].ToString();
						WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 3].ToString();
						WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 3].ToString();
						WinNumber[2].SetActive(true);
						WinNumber[1].SetActive(false);
						WinNumber[0].SetActive(false);
						WinnerAnimation (eachPlayerOrderName[order - 3], eachPlayerKillOrder[order - 3], false, false, true);
					}
					if (winnerTime > 5 && winnerTime < 10){
						WinLoseText.text = eachPlayerKillOrder[order - 2];
						WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 2].ToString();
						WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 2].ToString();
						WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 2].ToString();
						WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 2].ToString();
						WinNumber[2].SetActive(false);
						WinNumber[1].SetActive(true);
						WinNumber[0].SetActive(false);
						WinnerAnimation (eachPlayerOrderName[order - 2], eachPlayerKillOrder[order - 2], false, true, false);
					}
					if (winnerTime > 10 && winnerTime < 11 ){
						WinLoseText.text = eachPlayerKillOrder[order - 1];
						WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 1].ToString();
						WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 1].ToString();
						WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 1].ToString();
						WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 1].ToString();
						WinNumber[2].SetActive(false);
						WinNumber[1].SetActive(false);
						WinNumber[0].SetActive(true);
						WinnerAnimation (eachPlayerOrderName[order - 1], eachPlayerKillOrder[order - 1], true, false, false);
					}
				}
				if (allPlayers.Length == 2) {
					
					if (winnerTime < 5 && winnerTime > .1f){
						WinLoseText.text = eachPlayerKillOrder[order - 2];
						WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 2].ToString();
						WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 2].ToString();
						WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 2].ToString();
						WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 2].ToString();
						WinNumber[1].SetActive(true);
						WinNumber[2].SetActive(false);
						WinNumber[0].SetActive(false);
						WinnerAnimation (eachPlayerOrderName[order - 2], eachPlayerKillOrder[order - 2], false, false, true);
					}
					if (winnerTime > 5 && winnerTime < 6 ){
						WinLoseText.text = eachPlayerKillOrder[order - 1];
						WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 1].ToString();
						WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 1].ToString();
						WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 1].ToString();
						WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 1].ToString();
						WinNumber[2].SetActive(false);
						WinNumber[1].SetActive(false);
						WinNumber[0].SetActive(true);
						WinnerAnimation (eachPlayerOrderName[order - 1], eachPlayerKillOrder[order - 1], true, false, false);
					}
				}
				if (allPlayers.Length == 1) {
					
					if (winnerTime < 5){
						WinLoseText.text = eachPlayerKillOrder[order - 1];
						WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 1].ToString();
						WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 1].ToString();
						WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 1].ToString();
						WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 1].ToString();
						WinNumber[2].SetActive(false);
						WinNumber[1].SetActive(false);
						WinNumber[0].SetActive(true);
						WinnerAnimation (eachPlayerOrderName[order - 1], eachPlayerKillOrder[order - 1], true, false,false);
					}
				}

				
			} else {

				if ( allPlayers.Length > 2 ){
					
					if (winnerTime < 5 && winnerTime > .1f){
						WinLoseText.text = eachPlayerKillOrder[order - 2];
						WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 2].ToString();
						WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 2].ToString();
						WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 2].ToString();
						WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 2].ToString();
						WinNumber[2].SetActive(true);
						WinNumber[1].SetActive(false);
						WinNumber[0].SetActive(false);
						playername = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetComponent<TextMeshPro>().text;
						playerObjectname = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3).GetComponent<TextMesh>().text;
						playerkillCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_kill;
						playerBombCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_bomb;
						playerBreakCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_break;
						playerStepCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_step;
						WinnerAnimation (eachPlayerOrderName[order - 2], eachPlayerKillOrder[order - 2], false, false, true);
					}
					if (winnerTime > 5 && winnerTime < 10){
						WinLoseText.text = eachPlayerKillOrder[order - 1];
						WinLoseKillCount.text = "Kill : " +  eachPlayerKills[order - 1].ToString();
						WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 1].ToString();
						WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 1].ToString();
						WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 1].ToString();
						WinNumber[2].SetActive(false);
						WinNumber[1].SetActive(true);
						WinNumber[0].SetActive(false);
						WinnerAnimation (eachPlayerOrderName[order - 1], eachPlayerKillOrder[order - 1], false, true, false);
					}
					if (winnerTime > 10 && winnerTime < 11){
						WinLoseText.text = playername;
						WinLoseKillCount.text = "Kill : " +  playerkillCount;
						WinLoseBombCount.text = "Bombs placed : " + playerBombCount;
						WinLoseBreakCount.text = "Boxes broken : " + playerBreakCount;
						WinLoseStepCount.text = "Steps taken : " + playerStepCount;
						WinNumber[1].SetActive(false);
						WinNumber[0].SetActive(true);
						WinNumber[2].SetActive(false);
						
						WinnerAnimation (playerObjectname, playername, true, false, false);
					}
				}
				if (allPlayers.Length == 2) {
					
					if (winnerTime < 5 && winnerTime > .1f){
						WinLoseText.text = eachPlayerKillOrder[order - 1];
						WinLoseKillCount.text = "Kill : " + eachPlayerKills[order - 1].ToString();
						WinLoseBombCount.text = "Bombs placed : " + eachPlayerBomb[order - 1].ToString();
						WinLoseBreakCount.text = "Boxes broken : " + eachPlayerBreak[order - 1].ToString();
						WinLoseStepCount.text = "Steps taken : " + eachPlayerStep[order - 1].ToString();
						WinNumber[0].SetActive(false);
						WinNumber[1].SetActive(true);
						WinNumber[2].SetActive(false);
						playername = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetComponent<TextMeshPro>().text;
						playerObjectname = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3).GetComponent<TextMesh>().text;
						playerkillCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_kill;
						playerBombCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_bomb;
						playerBreakCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_break;
						playerStepCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_step;
						WinnerAnimation (eachPlayerOrderName[order - 1], eachPlayerKillOrder[order - 1], false, false, true);
					}
					if (winnerTime > 5 && winnerTime < 6 ){
						WinLoseText.text = playername;
						WinLoseKillCount.text = "Kill : " +  playerkillCount;
						WinLoseBombCount.text = "Bombs placed : " + playerBombCount;
						WinLoseBreakCount.text = "Boxes broken : " + playerBreakCount;
						WinLoseStepCount.text = "Steps taken : " + playerStepCount;
						WinNumber[1].SetActive(false);
						WinNumber[0].SetActive(true);
						WinNumber[2].SetActive(false);
						WinnerAnimation (playerObjectname, playername, true, false, false);
						
					}
				}
				if (allPlayers.Length == 1) {
					
					if (winnerTime < 5){
						WinLoseText.text = playername;
						WinLoseKillCount.text = "Kill : " +  playerkillCount;
						WinLoseBombCount.text = "Bombs placed : " + playerBombCount;
						WinLoseBreakCount.text = "Boxes broken : " + playerBreakCount;
						WinLoseStepCount.text = "Steps taken : " + playerStepCount;
						WinNumber[0].SetActive(true);
						WinNumber[2].SetActive(false);
						WinNumber[1].SetActive(false);
						playername = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetComponent<TextMeshPro>().text;
						playerObjectname = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3).GetComponent<TextMesh>().text;
						playerkillCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_kill;
						playerBombCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_bomb;
						playerBreakCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_break;
						playerStepCount = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player_Controller>().count_step;
						WinnerAnimation (playerObjectname, playername, true, false, false);
					}
				}
				if (winnerTime > 11) {
					for (int i = 0; i < allPlayers.Length; i++) {
						Destroy(allPlayers[i]);
					}
				}
				
			}
			AllDeleteObjets();
			
		}
		
	}

	public void AllDeleteObjets () {
		if (GameObject.Find("Grand_17") != null)
			GameObject.Find("Grand_17").SetActive(false);
		if (GameObject.Find("Grand_13") != null)
			GameObject.Find("Grand_13").SetActive(false);
		// if (GameObject.Find("Breakable(Clone)") != null)
		// 	GameObject.Find("Breakable(Clone)").SetActive(false);
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
		EndGame_status = true;
		GameObject.FindGameObjectWithTag("light").transform.GetChild(0).gameObject.SetActive(true);
		GameObject.FindGameObjectWithTag("light").transform.GetChild(1).gameObject.SetActive(false);
		// PlayerOrderParent.position = new Vector3 (PlayerOrderParent.position.x, 5, 5.15f);
	}

	public void WinnerAnimation(string playerObject, string playerOrderName, bool dance, bool clap, bool cry) {
		if (PlayerObjectName != playerObject) {
			if (PlayerOrderParent.childCount > 0)
				Destroy(PlayerOrderParent.transform.GetChild(0).gameObject);
			// Instantiate(Resources.Load("Prefabs/"+ playerObject), new Vector3(2, 0, 5), Quaternion.Euler(0f, 90f, 70f));
			GameObject temp = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/"+ playerObject), new Vector3(2, 0, 5), Quaternion.Euler(0f, 90f, 70f));
			temp.tag = "Untagged";
			temp.transform.GetComponent<Player_Controller>().hand_status = false;
			temp.transform.GetComponent<Player_Controller>().dance_status = dance;
			temp.transform.GetComponent<Player_Controller>().cry_status = cry;
			temp.transform.GetComponent<Player_Controller>().clap_status = clap;
			temp.transform.localScale = new Vector3(5f, 5f, 5f);
			temp.name = playerOrderName;
			temp.transform.SetParent(PlayerOrderParent);
			PlayerObjectName = playerObject;
		}
	}
}