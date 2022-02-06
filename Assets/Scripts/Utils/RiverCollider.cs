using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Utils.player1Tag) || other.CompareTag(Utils.player2Tag))
        {
            other.GetComponent<PlayerMovement>().InRiver(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Utils.player1Tag) || other.CompareTag(Utils.player2Tag))
        {
            other.GetComponent<PlayerMovement>().InRiver(false);
        }
    }
}
