﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillSliderScript : MonoBehaviour
{

    public Slider slider;
    public PresenceColliderScript presenceCollider;

    public AudioSource pickaxeSound;
    public AudioSource rockDestroy;


    public float hitValue;
    private bool finished = false;
    private bool pressed = false;
    private bool inProgress = false;

    private QuestManager.RockDestroyQuest rockDestroyQuest;

    // Use this for initialization
    private void Start()
    {
    }

    private void Update()
    {
        if (inProgress)
        {
            //Debug.Log(presenceCollider.GetStatus());
            if (!pressed)
            {
                if (presenceCollider.GetStatus())
                {
                    if (presenceCollider.isPlayer1InRange() && slider.gameObject.active == false)
                    {
                        slider.gameObject.SetActive(true);
                    }
                    if (presenceCollider.isPlayer1InRange() && Input.GetButtonDown(Utils.player1AB1))
                    {
                        pressed = true;
                        StartCoroutine(PlayAnimation());
                    }
                } else
                {
                    if (slider.gameObject.active == true)
                    {
                        slider.gameObject.SetActive(false);
                    }
                }

                // if slider is full turn it off
                if (slider.value == slider.maxValue)
                {
                    finished = true;
                    slider.gameObject.SetActive(false);
                    rockDestroyQuest.SetFinishedByUser(true);
                    rockDestroyQuest.SetReadyToDestroy(true);
                }
            }
        }
    }

    IEnumerator PlayAnimation()
    {
        GameObject player1 = GameObject.FindGameObjectWithTag(Utils.player1Tag);
        player1.GetComponent<PlayerMovement>().RotateTo(this.transform.position);
        player1.GetComponent<PlayerMovement>().PlayRockQuestAnim();

        yield return new WaitForSeconds(0.8f);

        slider.value += hitValue;

        if (slider.value == slider.maxValue)
        {
            rockDestroy.Play();
            pickaxeSound.Play();
        } else {
            pickaxeSound.Play();
        }


        yield return new WaitForSeconds(0.2f);
        
        player1.GetComponent<PlayerMovement>().StopRockQuestAnim();
        pressed = false;
    }

    public void SetRockDestroyQuest(QuestManager.RockDestroyQuest rockDestroyQuest)
    {
        this.rockDestroyQuest = rockDestroyQuest;
    }

    public void SetRunning(bool v)
    {
        this.inProgress = v;
    }

    public void Clear()
    {
        this.slider.value = 0;
    }
}

