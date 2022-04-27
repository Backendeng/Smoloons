using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class breakable_script: Photon.MonoBehaviour {

	public GameObject powerup_prefab;
	public GameObject blockAnimation;
	Player_Controller pm = new Player_Controller();
	public ParticleSystem explosion;
	PhotonView photonView;
	public bool animation_status, delete_status;
	private float liveTimer;
	private Animator animator;
	private float random, delete_time;
	private int PlayerID = 0;
	public bool flag;  //for once call

	// Use this for initialization
	void Start() {
		photonView = GetComponent < PhotonView > ();
		animation_status = false;
		delete_status = false;
		liveTimer = 0.0f;
		animator = transform.GetComponent < Animator > ();
		random = -1f;
		delete_time = 0;
		flag = false;
	}

	// Update is called once per frame
	void Update() {

		if (flag) {
			// block count and break.
			PhotonView.Find(PlayerID).GetComponent<Player_Controller> ().count_break++;
			PhotonView.Find(PlayerID).transform.GetChild(6).GetComponent<TextMesh>().text = PhotonView.Find(PlayerID).GetComponent<Player_Controller> ().count_break.ToString();
			photonView.RPC("LocalDestroy", PhotonTargets.AllBuffered, photonView.viewID); 
			flag = false;
		}

	}

	void OnCollisionEnter(Collision collision)

	{
		
		if (collision.collider.CompareTag("Explosion")) {
			
			Instantiate(explosion, transform.position, Quaternion.identity);
			transform.GetComponent <BoxCollider> ().enabled = false;
			transform.GetChild(0).GetComponent <BoxCollider> ().enabled = true;
			
			Instantiate(blockAnimation, transform.position, Quaternion.identity);
			animator.enabled = true;
			
			if (PhotonNetwork.connected == true) {
				if(photonView.isMine){

					// powerups probability of occurrence
					if (Random.Range(0.0f, 1.0f) > 0.5f && random == -1f) {
						
						random = Random.Range(0, 3); // random - bomb, explosion, speed
						if (random == 0 ) {
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"), transform.position, Quaternion.identity, 0);
						} 
						if (random == 1) {
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp1"), transform.position, Quaternion.identity, 0);
						}
						if (random == 2) {
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp2"), transform.position, Quaternion.identity, 0);
						}
					}
				}

				flag = true;
				// convert string to int for get playerID
				int.TryParse(collision.gameObject.name.Split(char.Parse("("))[0], out PlayerID);
				
			} else {
				Destroy(gameObject, 0.5f);
			}
		}

		// when hit Laststone
		if (collision.collider.CompareTag("LastStone")) {
			Destroy(gameObject);
		}
	}

	// when creation two powerups at same time, destroy powerups
	[PunRPC]
	private void RPC_Powerup() {
		if (Random.Range(0.0f, 1.0f) > 0.5f) {
			PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"), transform.position, Quaternion.identity, 0);
		}
	}
	
	// delete blocks. why? block made by master and scene
    [PunRPC]
    private void LocalDestroy(int viewId)
    {
        GameObject.Destroy(PhotonView.Find(viewId).gameObject);
    }
}