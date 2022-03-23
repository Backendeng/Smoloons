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
		// photonView = gameObject.GetComponent < PhotonView > ();
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
		Debug.Log(1);
		// if (collision.collider.CompareTag ("Explosion"))
		// {

		// //  Instantiate(explosion, transform.position, Quaternion.identity);

		// // 	if(Random.Range(0.0f, 1.0f)> 0.7f){

		// // 		Instantiate(powerup_prefab, transform.position, Quaternion.identity) ;
		// // 	}
		// // 	 Destroy(gameObject); // 3  

		// }
		if (collision.collider.CompareTag("Explosion")) {
			Debug.Log(2);
			Instantiate(explosion, transform.position, Quaternion.identity);
			Debug.Log(3);
			if (PhotonNetwork.connected == true) {
				Debug.Log(4);
				if (Random.Range(0.0f, 1.0f) > 0.5f) {
					PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"), transform.position, Quaternion.identity, 0);
				}
				Debug.Log(5);
				// if (PhotonNetwork.isMasterClient)
				animator.enabled  = true;
				Debug.Log(7);
				Destroy(gameObject, 2.0f);
				Debug.Log(8);
			} else {
				Debug.Log(6);
				Destroy(gameObject);
				// int viewID =  photonView.viewID;
				// photonView.RPC("DeleteBlock", PhotonTargets.MasterClient, viewID);
			}
		}
	}

	// [PunRPC]
    // private void DeleteBlock(int viewID)
    // {
    //     PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
    // }
}