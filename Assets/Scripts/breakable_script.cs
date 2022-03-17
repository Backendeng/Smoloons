using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class breakable_script: Photon.MonoBehaviour {

	public GameObject powerup_prefab;

	public ParticleSystem explosion;
	PhotonView photonView;
	// Use this for initialization
	void Start() {
		powerup_prefab = (GameObject) Resources.Load("PowerUp", typeof(GameObject));
		photonView = gameObject.GetComponent < PhotonView > ();

	}

	// Update is called once per frame
	void Update() {

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

			//if(PhotonNetwork.IsMasterClient){
			//	PhotonNetwork.Destroy(this.gameObject);
			//	 Destroy(gameObject); // 3  
			//}
			if (PhotonNetwork.connected == true && GetComponent<PhotonView>().isMine) {
				if (Random.Range(0.0f, 1.0f) > 0.5f) {
					PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PowerUp"), transform.position, Quaternion.identity, 0);
				}
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}
}