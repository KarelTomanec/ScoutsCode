using System;
using UnityEngine;

[Serializable]
public class PlayerManager
{
    public Transform spawnPoint;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public GameObject instance;

    private PlayerMovement movement; 

    public void Setup()
    {
        movement = instance.GetComponent<PlayerMovement>();
        movement.playerNumber = playerNumber;
    }
}
