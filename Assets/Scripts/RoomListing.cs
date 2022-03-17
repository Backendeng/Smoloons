using UnityEngine;
using UnityEngine.UI;
public class RoomListing : MonoBehaviour {

    [SerializeField]
    private Text _roomNameText;
    private Text RoomNameText {
        get { return _roomNameText; }
    }

    public string RoomName { get; private set; }
    public bool Updated { get; set; }

	// Use this for initialization
	void Start () {

        GameObject lobbyCanvasObject = MainCanvasManager.Instance.LobbyCanvas.gameObject;
        if (lobbyCanvasObject == null)
            return;

        LobbyCanvas lobbyCanvas = lobbyCanvasObject.GetComponent<LobbyCanvas>();

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => lobbyCanvas.OnJoinRoom(RoomNameText.text));

	}

    private void OnDestroy() {

        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();

    }

    public void SetRoomNameText(string text) {

        RoomName = text;
        RoomNameText.text = RoomName;

    }

}
