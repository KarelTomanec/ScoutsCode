using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPointScript : MonoBehaviour {

    public PresenceColliderScript presenceCollider;
    public Utils.InventoryObjectType objectType;

    internal string detectedPlayer;

    void Start () {
		
	}
	
	void Update () {
        if (presenceCollider.GetStatus())
        {
            detectedPlayer = presenceCollider.GetPlayerTag();
            if (detectedPlayer == Utils.player1Tag && Input.GetButtonDown(Utils.player1AB1))
            {
                GameObject.FindGameObjectWithTag(Utils.player1Tag).GetComponent<PlayerInventory>().Add(this.objectType);
            }
            else if (detectedPlayer == Utils.player2Tag && Input.GetButtonDown(Utils.player2AB1))
            {
                GameObject.FindGameObjectWithTag(Utils.player2Tag).GetComponent<PlayerInventory>().Add(this.objectType);
            }
        }
	}
}
