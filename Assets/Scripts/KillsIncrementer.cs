using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillsIncrementer: MonoBehaviour {

	public static KillsIncrementer Instance;

	public string[] eachPlayerKillOrder = new string[6];
	public int[] eachPlayerKills = new int[6];
	public int[] eachPlayerDeaths = new int[6];
	public int[] eachPlayerScore = new int[6];
	public string[] eachPlayerName = new string[6];
	public string[] ePN = new string[6];
	public string[] fePN = new string[6];
	public string[] winLose = new string[6];
	public GameObject WinLosePanel;
	public Text WinLoseText;
	public float[] eachPlayerHealth = new float[6];
	public GameObject[] allPlayers = new GameObject[6];
	public float startTime,
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
		}
		for (int i = 0; i < eachPlayerDeaths.Length; i++) {
			eachPlayerDeaths[i] = 0;
		}
		for (int i = 0; i < eachPlayerScore.Length; i++) {
			eachPlayerScore[i] = 0;
		}
		for (int i = 0; i < eachPlayerName.Length; i++) {
			eachPlayerName[i] = "";
		}

		for (int i = 0; i < eachPlayerName.Length; i++) {
			eachPlayerHealth[i] = 100;
		}

		for (int i = 0; i < winLose.Length; i++) {
			winLose[i] = "";
		}

		for (int i  = 0; i < PhotonNetwork.room.PlayerCount; i++ ){
			playerstatus[i] = Instantiate(SummaryListingPrefab);
			playerstatus[i].transform.SetParent(UI_Parent, false);
		}

	}

	void Start() {
		rankCalcInstance = rankCalc.GetComponent < RankCalc > ();
		Debug.Log(PhotonNetwork.countOfPlayers);

		WinLosePanel.SetActive(false);


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

		// if (allPlayers.Length != PhotonNetwork.room.PlayerCount){
		// 	foreach (Transform child in UI_Parent) {
		// 		GameObject.Destroy(child.gameObject);
		// 	}
		// 	for (int i  = 0; i < PhotonNetwork.room.PlayerCount; i++ ){
		// 		playerstatus[i] = Instantiate(SummaryListingPrefab);
		// 		playerstatus[i].transform.SetParent(UI_Parent, false);
		// 	}
		// }
		// Array.Reverse(ePN);

		timer = startTime - Time.timeSinceLevelLoad;

		string minutes = ((int)timer / 60).ToString();
		string seconds = (timer % 60).ToString("f0");

		timerText.text = minutes + " : " + seconds;

		if (timer <= 0 && !createStonestats) {
		    
		    timerText.text = "0" + " : " + "0"; 
		    Debug.Log("start");
			pv.RPC("CreateStone", PhotonTargets.All);
		    // WinLosePanel.SetActive(true);
		}

		// if (createStonestats) {
		// 	StartStone();
		// }

		// Array.Sort(eachPlayerName);

		// for (int i = 0; i < PhotonNetwork.countOfPlayers; i++) {
		//     ePN[i] = eachPlayerName[4 - i];
		// }
		// Array.Reverse(ePN);

		// for (int i = 0; i < PhotonNetwork.countOfPlayers; i++) {
		//     // fePN[i] = ePN[i].Remove(0,2);
		// }

		// for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
		// {
		//     GameObject go = scroller.transform.GetChild(i).gameObject;
		//     go.transform.GetChild(2).GetComponent<Text>().text = fePN[i];
		// }

		// for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
		// {
		//     GameObject go = scroller.transform.GetChild(i).gameObject;
		//     go.transform.GetChild(3).GetComponent<Text>().text = eachPlayerScore[i].ToString();
		// }

		// for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
		// {
		//     GameObject go = scroller.transform.GetChild(i).gameObject;
		//     go.transform.GetChild(1).GetComponent<Text>().text = rankCalcInstance.fs[i];
		// }

		// for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
		// {
		//     GameObject go = scroller.transform.GetChild(i).gameObject;
		//     go.transform.GetChild(5).GetComponent<Image>().fillAmount = eachPlayerHealth[i]/100;

		//if (allPlayers[i].GetComponent<PhotonView>().ownerId == 1) {
		//    eachPlayerHealth[0] = allPlayers[i].GetComponent<PlayerMovement>().curr_health / 100;
		//    GameObject go = scroller.transform.GetChild(i).gameObject;
		//    go.transform.GetChild(5).GetComponent<Image>().fillAmount = eachPlayerHealth[0];
		//}
		//if (allPlayers[i].GetComponent<PhotonView>().ownerId == 2)
		//{
		//    eachPlayerHealth[1] = allPlayers[i].GetComponent<PlayerMovement>().curr_health / 100;
		//    GameObject go = scroller.transform.GetChild(i).gameObject;
		//    go.transform.GetChild(5).GetComponent<Image>().fillAmount = eachPlayerHealth[1];
		//}
		//if (allPlayers[i].GetComponent<PhotonView>().ownerId == 3)
		//{
		//    eachPlayerHealth[2] = allPlayers[i].GetComponent<PlayerMovement>().curr_health / 100;
		//    GameObject go = scroller.transform.GetChild(i).gameObject;
		//    go.transform.GetChild(5).GetComponent<Image>().fillAmount = eachPlayerHealth[2];
		//}
		//if (allPlayers[i].GetComponent<PhotonView>().ownerId == 4)
		//{
		//    eachPlayerHealth[3] = allPlayers[i].GetComponent<PlayerMovement>().curr_health / 100;
		//    GameObject go = scroller.transform.GetChild(i).gameObject;
		//    go.transform.GetChild(5).GetComponent<Image>().fillAmount = eachPlayerHealth[3];
		//}
		//if (allPlayers[i].GetComponent<PhotonView>().ownerId == 5)
		//{
		//    eachPlayerHealth[4] = allPlayers[i].GetComponent<PlayerMovement>().curr_health / 100;
		//    GameObject go = scroller.transform.GetChild(i).gameObject;
		//    go.transform.GetChild(5).GetComponent<Image>().fillAmount = eachPlayerHealth[4];
		//}
		// }

		//rankScore = eachPlayerScore;
		//Array.Sort(rankScore);
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
		if (eachPlayerKillOrder[0] != "" && GameObject.FindGameObjectsWithTag("Player").Length < 2 ) {
			int order = 0;
			WinLosePanel.SetActive(true);
			string temp = "\n";
			for (int i = eachPlayerKillOrder.Length - 1; i > -1; i--) {
				if(eachPlayerKillOrder[i] != "") {
					order++;
					 temp += order.ToString() + ". " + eachPlayerKillOrder[i] +"\n"; 
				}
			}
			WinLoseText.text = temp;
		}
	}
}