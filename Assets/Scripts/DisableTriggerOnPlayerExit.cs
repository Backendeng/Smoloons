
using UnityEngine;
using System.Collections;

// when player drop bomb, Trigger event.

public class DisableTriggerOnPlayerExit : MonoBehaviour
{

    public void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag ("Player"))
        { // When the player exits the trigger area
            other.gameObject.GetComponent<Player_Controller>().canDropBombs = true;  
            GetComponent<Collider> ().isTrigger = false; // Disable the trigger
        }

    }


}
