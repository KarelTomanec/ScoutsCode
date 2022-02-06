using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCardManager : MonoBehaviour {

    public GameObject[] cards = new GameObject[3];
    public GameManager gameManager;

    public AudioSource errorSound;

    private int count = 0;    

    public void AddCard()
    {
        count++;

        ShowCards(count);
        errorSound.Play();

        if (count == 3)
        {
            gameManager.GameOver();
        }
    }

    private void ShowCards(int number)
    {
        number--;
        if (number < cards.Length)
        {
            cards[number].gameObject.SetActive(true);

        }
    }
}
