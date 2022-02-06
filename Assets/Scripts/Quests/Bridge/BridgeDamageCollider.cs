using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeDamageCollider : MonoBehaviour {

    public Bridge bridge;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Utils.player1Tag) || other.gameObject.CompareTag(Utils.player2Tag))
        {
            bridge.addDamage();
        }
    }
}
