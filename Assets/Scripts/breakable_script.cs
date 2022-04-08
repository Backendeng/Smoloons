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
	private float random;

	// Use this for initialization
	void Start() {
		// powerup_prefab = (GameObject) Resources.Load("PowerUp", typeof(GameObject));
		photonView = GetComponent < PhotonView > ();
		animation_status = false;
		liveTimer = 0.0f;
		animator = transform.GetComponent < Animator > ();
		random = -1f;
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
			transform.GetComponent <BoxCollider> ().enabled = false;
			transform.GetChild(0).GetComponent <BoxCollider> ().enabled = true;
			animator.enabled = true;
			if (PhotonNetwork.connected == true) {
				if(photonView.isMine){
					if (Random.Range(0.0f, 1.0f) > 0.5f && random == -1f) {
						Debug.Log("hit");
						//  photonView.RPC("RPC_Powerup", PhotonTargets.All);
						random = Random.Range(0, 3);
						Debug.Log(random);
						if (random == 0 ) {
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"), transform.position, Quaternion.identity, 0);
							break;
						} 
						if (random == 1) {
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp1"), transform.position, Quaternion.identity, 0);
							break;
						}
						if (random == 2) {
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp2"), transform.position, Quaternion.identity, 0);
							break;
						}
						// Powerups.transform.GetComponent<powerup_script>().Starts();
						break;
					}
					break;
				}
				// if (PhotonNetwork.isMasterClient)
				Destroy(gameObject.transform.GetChild(0).gameObject, 0.7f);
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