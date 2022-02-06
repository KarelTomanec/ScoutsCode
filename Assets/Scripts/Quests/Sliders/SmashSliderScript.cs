using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashSliderScript : MonoBehaviour {

    public Slider slider;
    public PresenceColliderScript presenceCollider;
    public GameObject animal;

    public float emptyingSpeed;
    public float hitValue;

    private float currentValue;
    private bool finished = false;

    private QuestManager.AnimalSaveQuest animalSaveQuest;

    // Use this for initialization
    private void Start ()
    {
        slider.gameObject.SetActive(false);
    }

    private void Update () {
        slider.value = Mathf.Clamp((slider.value - emptyingSpeed * Time.deltaTime), slider.minValue, slider.maxValue);

        if (presenceCollider.GetStatus())
        {
            if (presenceCollider.isPlayer1InRange() && slider.gameObject.active == false)
            {
                slider.gameObject.SetActive(true);
            }
            if (presenceCollider.isPlayer1InRange() && Input.GetButtonDown(Utils.player1AB1))
            {
                slider.value = Mathf.Clamp((slider.value + hitValue), slider.minValue, slider.maxValue);
            }
        }
        else
        {
            if (slider.gameObject.active == true)
            {
                slider.gameObject.SetActive(false);
            }
        }

        // if slider is full turn it off
        if (slider.value == slider.maxValue) {
            finished = true;
            slider.gameObject.SetActive(false);
            animalSaveQuest.SetFinishedByUser(true);
            animalSaveQuest.SetReadyToDestroy(true);
        }
	}

    internal void SetAnimalSaveQuest(QuestManager.AnimalSaveQuest animalSaveQuest)
    {
        this.animalSaveQuest = animalSaveQuest;
    }
}
