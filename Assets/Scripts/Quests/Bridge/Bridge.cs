using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {
    
    public GameObject planks;
    public ParticleSystem smokeEffect;
    public GameObject[] visualisers;
    public int maximalDamage;
    public AudioSource breakSound;

    public int woodNeeded = 1;
    public int nailsNeeded = 1;
    
    private int damage;

	void Start () {
        damage = 0;
        smokeEffect.Stop();
	}
	
	void Update () {
        if(damage >= maximalDamage)
        {
            if (planks.active != false)
            {
                planks.SetActive(false);

                breakSound.Play();
                smokeEffect.Play();
                showVisualisers();
            }
        } else
        {
            if (planks.active != true)
            {
                planks.SetActive(true);
                smokeEffect.Play();
                hideVisualisers();
            }
        }
	}

    public void addDamage()
    {
        damage += 1;
    }

    public void repair()
    {
        damage = 0;
    }

    private void showVisualisers()
    {
        foreach(GameObject v in visualisers)
        {
            v.SetActive(true);
        }
    }

    private void hideVisualisers()
    {
        foreach (GameObject v in visualisers)
        {
            v.SetActive(false);
        }
    }
}
