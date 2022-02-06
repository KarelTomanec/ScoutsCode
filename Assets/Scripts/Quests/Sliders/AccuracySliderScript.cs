﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Script which is for accuraccy slider - mainly for fire extinghuision quest 
 * 
 */
public class AccuracySliderScript : MonoBehaviour {

    public Slider slider;
    public PresenceColliderScript presenceCollider;
    public Text finishText;
    
    public float slideSpeed = 100f;
    public float okRange = 20f;

    private float pos = 0;
    private bool move = true;

    private QuestManager.TreeQuest treeQuest;

    private void Start()
    {
        slider.gameObject.SetActive(false);
    }

    private void Update() {
        pos += slideSpeed * Time.deltaTime;

        if (move)
        {
            slider.value = Mathf.PingPong(pos, slider.maxValue);
        }

        // If someone is in range
        if (presenceCollider.GetStatus())
        {
            // Player 1 is in range
            if (presenceCollider.isPlayer1InRange())
            {
                int waterStatus = GameObject.FindGameObjectWithTag(Utils.player1Tag).GetComponent<PlayerInventory>().Get(Utils.InventoryObjectType.Water);
                if(slider.gameObject.active == false)
                {
                    slider.gameObject.SetActive(true);
                }
                if (Input.GetButtonDown(Utils.player1AB1) && IsValueInRange() && waterStatus > 0)
                {
                    GameObject.FindGameObjectWithTag(Utils.player1Tag).GetComponent<PlayerInventory>().Remove(Utils.InventoryObjectType.Water);
                    EventSuccessful();
                }
            }

            // Player 2 is in range
            if (presenceCollider.isPlayer2InRange())
            {
                int waterStatus = GameObject.FindGameObjectWithTag(Utils.player2Tag).GetComponent<PlayerInventory>().Get(Utils.InventoryObjectType.Water);

                if (slider.gameObject.active == false)
                {
                    slider.gameObject.SetActive(true);
                }
                if (Input.GetButtonDown(Utils.player2AB1) && IsValueInRange() && waterStatus > 0)
                {
                    GameObject.FindGameObjectWithTag(Utils.player2Tag).GetComponent<PlayerInventory>().Remove(Utils.InventoryObjectType.Water);
                    EventSuccessful();
                }
            }
        } else
        {
            if (slider.gameObject.active == true)
            {
                slider.gameObject.SetActive(false);
            }
        }
    }

    /**
     *  Checks if value on slider is in desired okRange
     * 
     */
    private bool IsValueInRange()
    {
        float edgeRange = (slider.maxValue - okRange) / 2;

        if ((slider.value > (slider.minValue + edgeRange)) && (slider.value < (slider.maxValue - edgeRange)))
            return true;
        else
            return false;
    }

    /**
     * Function for successful event ending
     *
     */
    private void EventSuccessful()
    {
        move = false;
        treeQuest.SetFinishedByUser(true);
        treeQuest.SetReadyToDestroy(true);
    }

    public void SetTreeQuest(QuestManager.TreeQuest treeQuest)
    {
        this.treeQuest = treeQuest;
    }
}

