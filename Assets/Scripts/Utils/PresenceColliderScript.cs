﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script for simple collider which checks presence of player and returns Tag of 
 * player in range.
 * 
 */
public class PresenceColliderScript : MonoBehaviour {
    private bool status = false;
    private string playerTag = null;
    private bool player1InRange = false;
    private bool player2InRange = false;

    private void OnTriggerEnter(Collider other)
    {
        status = true;

        if (other.CompareTag(Utils.player1Tag))
        {
            playerTag = Utils.player1Tag;
            player1InRange = true;
        }

        if (other.CompareTag(Utils.player2Tag))
        {
            playerTag = Utils.player2Tag;
            player2InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerTag = null;
        if(other.CompareTag(Utils.player1Tag))
        {
            player1InRange = false;
        }

        if (other.CompareTag(Utils.player2Tag))
        {
            player2InRange = false;
        }

        if(player1InRange == false && player2InRange == false)
        {
            status = false;
        }
    }

    public bool GetStatus()
    {
        return status;
    }

    public string GetPlayerTag()
    {
        return playerTag;
    }

    public bool isPlayer1InRange()
    {
        return player1InRange;
    }

    public bool isPlayer2InRange()
    {
        return player2InRange;
    }
}
