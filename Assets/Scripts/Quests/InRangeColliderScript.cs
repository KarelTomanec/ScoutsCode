using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeColliderScript : MonoBehaviour {
    
    public float activationLenght = 5f;
    public int playerNumber;

    private bool isInRange;
    private bool isActivated;
    private string activationKey;

    private void Start()
    {
        isInRange = false;
        isActivated = false;
        if (playerNumber == 1)
            activationKey = Utils.player1AB1;
        else if (playerNumber == 2)
            activationKey = Utils.player2AB1;
        else
            activationKey = "";
    }

    private void Update()
    {
        StartCoroutine(activationChecker());
    }

    private IEnumerator activationChecker()
    {
        if (isInRange && Input.GetButton(activationKey))
        {
            isActivated = true;

            yield return new WaitForSeconds(activationLenght);

            isActivated = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Utils.player1Tag))
        {
            activationKey = Utils.player1AB1;
            isInRange = true;
        }
        else if (other.gameObject.CompareTag(Utils.player2Tag))
        {
            activationKey = Utils.player2AB1;
            isInRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Utils.player1Tag) || other.gameObject.CompareTag(Utils.player2Tag))
        {
            isInRange = false;
        }
    }

    public bool GetPresenceStatus()
    {
        return isInRange;
    }

    public bool GetActivatedStatus()
    {
        return isActivated;
    }

    public void Reset()
    {
        this.isActivated = false;
        this.isInRange = false;
    }
}
