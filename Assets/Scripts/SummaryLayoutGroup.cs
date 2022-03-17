using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryLayoutGroup: MonoBehaviour {

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

	public int timer;

	private void Awake() {
		timer = 1000;
		KI = GameObject.FindGameObjectWithTag("Kills");
		pv = gameObject.GetComponent < PhotonView > ();
	}

	private void Start() {
		KII = KI.GetComponent < KillsIncrementer > ();
		pv.RPC("playerDetails", PhotonTargets.All);
		// RoomListingButtons.Add(roomListing);
	}

	private void Update() {
		//timer--;
		//if(timer<=0)
		//for (int i=0;i<PhotonNetwork.countOfPlayers;i++) {
		//    GameObject go = gameObject.transform.GetChild(i).gameObject;
		//    go.transform.GetChild(1).GetComponent<Text>().text = KII.fePN[i];
		//}

	}

	[PunRPC]
	private void playerDetails() {
		GameObject summaryListingObject = Instantiate(SummaryListingPrefab);
		summaryListingObject.transform.SetParent(transform, false);

		//Text[] summaryChildren = summaryListingObject.GetComponentsInChildren<Text>();

		//summaryChildren[1].text = PhotonNetwork.player.ID.ToString();

		//switch (PhotonNetwork.player.ID % 5) {
		//    case 1: summaryChildren[1].text = KillsIncrementer.Instance.eachPlayerName[0];
		//        break;
		//    case 2: summaryChildren[1].text = KillsIncrementer.Instance.eachPlayerName[1];
		//        break;
		//    default:
		//        break;
		//}

		//RoomListing roomListing = summaryListingObject.GetComponent<RoomListing>();
	}

}