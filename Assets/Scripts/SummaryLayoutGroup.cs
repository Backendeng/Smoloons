using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryLayoutGroup: MonoBehaviour {

	public static SummaryLayoutGroup Instance;
	[SerializeField]
	private GameObject _summaryListingPrefab;
	private GameObject SummaryListingPrefab {
		get {
			return _summaryListingPrefab;
		}
	}

	PhotonView pv;
	public GameObject KI;
	public KillsIncrementer KII;

	public float timer;

	private void Awake() {
		timer = 1000;
		KI = GameObject.FindGameObjectWithTag("Kills");
		pv = gameObject.GetComponent < PhotonView > ();
	}

	private void Start() {
		KII = KI.GetComponent < KillsIncrementer > ();
		// pv.RPC("playerDetails", PhotonTargets.All);
		// RoomListingButtons.Add(roomListing);
	}

	private void Update() {
		// timer += Time.deltaTime;
		// if (timer > 10)
		// 	pv.RPC("playerDetails", PhotonTargets.All);
		//timer--;
		//if(timer<=0)
		//for (int i=0;i<PhotonNetwork.countOfPlayers;i++) {
		//    GameObject go = gameObject.transform.GetChild(i).gameObject;
		//    go.transform.GetChild(1).GetComponent<Text>().text = KII.fePN[i];
		//}

	}

	public void playerStatus (string name)
	{
		pv.RPC("playerDetails", PhotonTargets.All, name);
	}

	[PunRPC]
	private void playerDetails(string name) {
		// GameObject summaryListingObject = Instantiate(SummaryListingPrefab);
		// summaryListingObject.transform.SetParent(transform, false);

		// summaryListingObject.transform.Find("Name").transform.GetComponent<Text>().text = name;
		// summaryListingObject.transform.Find("Image").transform.GetComponent<Image>().sprite = image;


		// switch (PhotonNetwork.player.ID % 6) {
		//    case 1: name = KillsIncrementer.Instance.allPlayers[0].transform.GetComponent<Player_Controller>().playername;
		// 			image = KillsIncrementer.Instance.allPlayers[0].transform.GetComponent<Player_Controller>().player_image;
		//        break;
		// 	case 2: name = KillsIncrementer.Instance.allPlayers[1].transform.GetComponent<Player_Controller>().playername;
		// 			image = KillsIncrementer.Instance.allPlayers[1].transform.GetComponent<Player_Controller>().player_image;
		//        break;
		//    default:
		//        break;
		// }

		//RoomListing roomListing = summaryListingObject.GetComponent<RoomListing>();
	}

}