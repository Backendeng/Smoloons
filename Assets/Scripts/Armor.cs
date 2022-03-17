//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Armor : MonoBehaviour {

//    private void Awake() {

//    }

//    private void OnTriggerEnter(Collider collider) {

//        //if (!PhotonNetwork.isMasterClient)
//        //    return;
        
//        PhotonView photonView = collider.GetComponent<PhotonView>();
//        if (photonView != null && photonView.isMine) {
//            PlayerMovement.Instance.Health = -10;
//            //PlayerManagement.Instance.ModifyHealth(photonView.owner, -10);
//        }
            

//    }
//}
