using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerriesPickupPointScript : PickupPointScript {

    public GameObject bushWithBerries;
    public int berriesAvailable;

    private QuestManager questManager;
    private bool eaten = false;


    // Update is called once per frame
    void Update () {
        if (!eaten)
        {
            if (presenceCollider.GetStatus())
            {
                detectedPlayer = presenceCollider.GetPlayerTag();
                if (detectedPlayer == Utils.player2Tag && Input.GetButtonDown(Utils.player2AB1))
                {
                    GameObject.FindGameObjectWithTag(Utils.player2Tag).GetComponent<PlayerInventory>().Add(this.objectType);
                    eaten = true;
                    RemoveBerriesFromBush();
                    questManager.AddEatenBushes(this);
                }
            }
        }
    }

    private void RemoveBerriesFromBush()
    {
        if (questManager == null)
        {
            questManager = GameObject.FindGameObjectWithTag(Utils.questManagerTag).GetComponent<QuestManager>() ;
        }

        foreach (Transform child in bushWithBerries.transform)
        {
            if (child.gameObject.CompareTag(Utils.berriesTag))
            {
                child.gameObject.SetActive(false);
                questManager.AddDeletedBerries(child.gameObject);
            }
        }
    }

    public void SetEaten(bool b)
    {
        this.eaten = b;
    }
}
