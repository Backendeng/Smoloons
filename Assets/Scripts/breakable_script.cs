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
	public bool flag;

	// Use this for initialization
	void Start() {
		// powerup_prefab = (GameObject) Resources.Load("PowerUp", typeof(GameObject));
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
		// if (animation_status) {
		// 	transform.GetComponent <Animator> ().enabled  = true;
		// 	liveTimer += Time.deltaTime;
		// 	if (liveTimer > 1.0f )
		// 		Destory(gameObject);
		// }
		if (flag) {
			
			PhotonView.Find(PlayerID).GetComponent<Player_Controller> ().count_break++;
			photonView.RPC("LocalDestroy", PhotonTargets.AllBuffered, photonView.viewID); 
			flag = false;
		}

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
		if (collision.collider.CompareTag("Explosion")) {
			
			Instantiate(explosion, transform.position, Quaternion.identity);
			transform.GetComponent <BoxCollider> ().enabled = false;
			transform.GetChild(0).GetComponent <BoxCollider> ().enabled = true;
			Instantiate(blockAnimation, transform.position, Quaternion.identity);
			animator.enabled = true;
			if (PhotonNetwork.connected == true) {
				if(photonView.isMine){
					if (Random.Range(0.0f, 1.0f) > 0.5f && random == -1f) {
						//  photonView.RPC("RPC_Powerup", PhotonTargets.All);
						random = Random.Range(0, 3);
						if (random == 0 ) {
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"), transform.position, Quaternion.identity, 0);
						} 
						if (random == 1) {
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp1"), transform.position, Quaternion.identity, 0);
						}
						if (random == 2) {
							PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp2"), transform.position, Quaternion.identity, 0);
						}
						// Powerups.transform.GetComponent<powerup_script>().Starts();
					}
				}
				// if (PhotonNetwork.isMasterClient)
				// Destroy(gameObject.transform.GetChild(0).gameObject, 0.7f);
				// DestroySceneObject(photonView);
				flag = true;
				int.TryParse(collision.gameObject.name.Split(char.Parse("("))[0], out PlayerID);
				
				// PhotonNetwork.Destroy(PhotonView.Find(photonView.viewID).gameObject);
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

	[PunRPC]
	private void RPC_Powerup() {
		if (Random.Range(0.0f, 1.0f) > 0.5f) {
			PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"), transform.position, Quaternion.identity, 0);
		}
	}
	
    [PunRPC]
    private void LocalDestroy(int viewId)
    {
        GameObject.Destroy(PhotonView.Find(viewId).gameObject);
    }
}