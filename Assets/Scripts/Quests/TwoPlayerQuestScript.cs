using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPlayerQuestScript : MonoBehaviour
{
    public List<InRangeColliderScript> places;

    public AudioSource gateOpenSound;
    public AudioSource gateCloseSound;

    public Animator gateAnimator;

    private bool activationOfBoth = false;
    private QuestManager.TwoPlayerQuest twoPlayerQuest;

    private float rotationSpeed = 0.01f;

    private void Start()
    {
        foreach (InRangeColliderScript place in places)
        {
            place.Reset();
        }
        gateAnimator.SetBool("open", true);
        gateOpenSound.Play();
    }

    private void Update()
    {
        activationOfBoth = GetActivationStatus();
        if (activationOfBoth)
        {
            EventSuccessful();
        }
    }

    private void EventSuccessful()
    {
        twoPlayerQuest.SetFinishedByUser(true);
        activationOfBoth = false;
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        GameObject player1 = GameObject.FindGameObjectWithTag(Utils.player1Tag);
        GameObject player2 = GameObject.FindGameObjectWithTag(Utils.player2Tag);
        player1.GetComponent<PlayerMovement>().RotateTo(this.transform.position);
        player2.GetComponent<PlayerMovement>().RotateTo(this.transform.position);
        player1.GetComponent<PlayerMovement>().PlayTwoPlayerQuestAnim();
        player2.GetComponent<PlayerMovement>().PlayTwoPlayerQuestAnim();
        yield return new WaitForSeconds(1.8f);
        gateAnimator.SetBool("open", false);
        gateCloseSound.Play();
        yield return new WaitForSeconds(0.2f);
        player1.GetComponent<PlayerMovement>().StopTwoPlayerQuestAnim();
        player2.GetComponent<PlayerMovement>().StopTwoPlayerQuestAnim();
        twoPlayerQuest.SetReadyToDestroy(true);
    }


    private bool GetActivationStatus()
    {
        bool status = true;

        for (int i = 0; i < places.Count; i++)
        {
            status &= places[i].GetActivatedStatus();
        }

        return status;
    }

    public void SetTwoPlayerQuest(QuestManager.TwoPlayerQuest twoPlayerQuest)
    {
        this.twoPlayerQuest = twoPlayerQuest;
    }

    public void Init()
    {
        Start();
    }
}
