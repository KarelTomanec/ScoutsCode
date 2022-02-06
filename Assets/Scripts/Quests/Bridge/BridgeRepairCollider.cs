using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRepairCollider : MonoBehaviour {

    public Bridge bridge;
    private bool isInRange;

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown(Utils.player1AB1) && other.gameObject.CompareTag(Utils.player1Tag))
        {
            PlayerInventory inventory = GameObject.FindGameObjectWithTag(Utils.player1Tag).GetComponent<PlayerInventory>();
            if (inventory.Get(Utils.InventoryObjectType.Wood) >= bridge.woodNeeded && inventory.Get(Utils.InventoryObjectType.Nails) >= bridge.nailsNeeded)
            {
                bridge.repair();
                RemoveMaterials(inventory);
            }
        }

        if(Input.GetButtonDown(Utils.player2AB1) && other.gameObject.CompareTag(Utils.player2Tag))
        {
            PlayerInventory inventory = GameObject.FindGameObjectWithTag(Utils.player2Tag).GetComponent<PlayerInventory>();
            if (inventory.Get(Utils.InventoryObjectType.Wood) >= bridge.woodNeeded && inventory.Get(Utils.InventoryObjectType.Nails) >= bridge.nailsNeeded)
            {
                bridge.repair();
                RemoveMaterials(inventory);
            }
        }
    }

    private void RemoveMaterials(PlayerInventory inv)
    {
        for (int i = 0; i < bridge.woodNeeded; i++)
        {
            inv.Remove(Utils.InventoryObjectType.Wood);
        }

        for (int i = 0; i < bridge.nailsNeeded; i++)
        {
            inv.Remove(Utils.InventoryObjectType.Nails);
        }
    }
}
