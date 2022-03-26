using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillsIncrementer: MonoBehaviour {

	public static KillsIncrementer Instance;

	public int[] eachPlayerKills = new int[5];
	public int[] eachPlayerDeaths = new int[5];
	public int[] eachPlayerScore = new int[5];
	public string[] eachPlayerName = new string[5];
	public string[] ePN = new string[5];
	public string[] fePN = new string[5];
	public string[] winLose = new string[5];
	public GameObject WinLosePanel;
	public Text WinLoseText;
	public float[] eachPlayerHealth = new float[5];
	public GameObject[] allPlayers = new GameObject[5];
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

	PhotonView pv;
	private void Awake() {
		j = 0;

		startTime = 10;

		scroller = GameObject.FindGameObjectWithTag("Scroller");
		rankCalc = GameObject.FindGameObjectWithTag("Rank");
		timer = 20;
		pv = GetComponent < PhotonView > ();
		ePN = new string[PhotonNetwork.countOfPlayers];
		fePN = new string[PhotonNetwork.countOfPlayers];
		winLose = new string[PhotonNetwork.countOfPlayers];

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

	}

	void Start() {
		rankCalcInstance = rankCalc.GetComponent < RankCalc > ();
		Debug.Log(PhotonNetwork.countOfPlayers);

		WinLosePanel.SetActive(false);

	}

	// Update is called once per frame
	void Update() {
		allPlayers = FindGameObjectsWithSameName("Monkey(Clone)");

		for (int i = 0; i < allPlayers.Length; i++) {
			if (PhotonNetwork.isMasterClient)
		    	allPlayers[i].transform.GetChild(1).GetComponent<TextMeshPro>().text = eachPlayerName[i];
			else
				allPlayers[i].transform.GetChild(1).GetComponent<TextMeshPro>().text = eachPlayerName[allPlayers.Length - i - 1];
		}
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
			if (obj.name == name)
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
}