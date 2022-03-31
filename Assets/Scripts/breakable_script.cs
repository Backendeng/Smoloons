using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class breakable_script: Photon.MonoBehaviour {

	public GameObject powerup_prefab;
	Player_Controller pm = new Player_Controller();
	public ParticleSystem explosion;
	PhotonView photonView;
	public bool animation_status;
	private float liveTimer;
	private Animator animator;

	// Use this for initialization
	void Start() {
		// powerup_prefab = (GameObject) Resources.Load("PowerUp", typeof(GameObject));
		photonView = GetComponent < PhotonView > ();
		animation_status = false;
		liveTimer = 0.0f;
		animator = transform.GetComponent < Animator > ();
	}

	// Update is called once per frame
	void Update() {
		// if (animation_status) {
		// 	transform.GetComponent <Animator> ().enabled  = true;
		// 	liveTimer += Time.deltaTime;
		// 	if (liveTimer > 1.0f )
		// 		Destory(gameObject);
		// }
	}

	void OnCollisionEnter(Collision collision)

	{
		// if (collision.collider.CompareTag ("Explosion"))
		// {

		// //  Instantiate(explosion, transform.position, Quaternion.identity);

		// // 	if(Random.Range(0.0f, 1.0f)> 0.7f){

		// // 		Instantiate(powerup_prefab, transform.position, Quaternion.identity) ;
		// // 	}
		// // 	 Destroy(gameObject); // 3  

		// }
		// Debug.Log(collision.collider.gameObject.tag);
		if (collision.collider.CompareTag("Explosion")) {
			Instantiate(explosion, transform.position, Quaternion.identity);
			if (PhotonNetwork.connected == true) {
				if(photonView.isMine){
					if (Random.Range(0.0f, 1.0f) > 0.5f) {
						//  photonView.RPC("RPC_Powerup", PhotonTargets.All);
						float random = Random.Range(0, 3);
						if (random < 1 )
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"), transform.position, Quaternion.identity, 0);
						else if (random < 2)
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp1"), transform.position, Quaternion.identity, 0);
						else
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp2"), transform.position, Quaternion.identity, 0);
						// Powerups.transform.GetComponent<powerup_script>().Starts();
					}
				}
				// if (PhotonNetwork.isMasterClient)
				animator.enabled  = true;
				transform.GetComponent <BoxCollider> ().enabled = false;
				Destroy(gameObject, 2.0f);
			} else {
				Destroy(gameObject, 0.5f);
				// int viewID =  photonView.viewID;
				// photonView.RPC("DeleteBlock", PhotonTargets.MasterClient, viewID);
			}
		}
		if (collision.collider.CompareTag("LastStone")) {
			Destroy(gameObject);
		}
	}

	// [PunRPC]
    // private void DeleteBlock(int viewID)
    // {
    //     PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
    // }
	[PunRPC]
	private void RPC_Powerup() {
		if (Random.Range(0.0f, 1.0f) > 0.5f) {
			PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"), transform.position, Quaternion.identity, 0);
		}
	}
}