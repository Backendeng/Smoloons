using UnityEngine;
using UnityEngine.SceneManagement;

public class Initiator : MonoBehaviour {

    private void Awake(){

        Invoke("EnteringLobby", 2f);

    }

    private void EnteringLobby() {

        PhotonNetwork.LoadLevel(1);

    }

}
