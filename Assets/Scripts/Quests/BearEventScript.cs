using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BearEventScript : MonoBehaviour {

    public QuestManager questManager;
    public GameObject bear;
    public PresenceColliderScript bearRangeCollider;
    public Image feedIndicator;
    public Transform inside;
    public Transform outside;
    public AudioSource bearSound;

    public int minBerries;
    public int maxBerries;

    public float minTimeWait;
    public float maxTimeWait;

    private bool running = false;
    private bool inited = false;
    private PlayerInventory player2Inventory;
    private QuestManager.BearFeedQuest bearFeedQuest;

    private int berriesToEat;
    private int berriesEaten;
    private float timeToWait;
    private float timeWaited = 0f;


	
	void Update () {
        if (running)
        {
            if (bearRangeCollider.isPlayer2InRange())
            {
                if (Input.GetButtonDown(Utils.player2AB1) && player2Inventory.Get(Utils.InventoryObjectType.Food) > 0)
                {
                    player2Inventory.Remove(Utils.InventoryObjectType.Food);
                    berriesEaten++;
                    feedIndicator.fillAmount = GetFillAmount();
                }
            }

            if (berriesEaten == berriesToEat)
            {
                bearFeedQuest.SetFinishedByUser(true);
                Stop();
            }


            if (timeWaited > timeToWait)
            {
                Debug.Log("Waiting too long");
                bearSound.Play();
                bearFeedQuest.SetFinishedByUser(false);
                bearFeedQuest.SetExpiredTimeCap();
                Stop();
            }

            timeWaited += Time.deltaTime;
        }
	}

    private void Init()
    {
        if (!inited)
        {
            player2Inventory = GameObject.FindGameObjectWithTag(Utils.player2Tag).GetComponent<PlayerInventory>();
        }

        berriesEaten = 0;
        timeWaited = 0;
        feedIndicator.fillAmount = 0;
    }

    private float GetFillAmount()
    {
        return ((float)this.berriesEaten / (float)this.berriesToEat);
    }

    private void GoEat()
    {
        bearSound.Play();
        bear.GetComponent<AiBearController>().setTarget(outside, false);
    }

    private void GoHome()
    {
        bear.GetComponent<AiBearController>().setTarget(inside, true);
    }

    public void Run()
    {
        Init();

        this.berriesToEat = Random.Range(minBerries, maxBerries);
        this.timeToWait = Random.Range(minTimeWait, maxTimeWait);

        Debug.Log("Will wait for " + timeToWait + "and eat " + berriesToEat + "berries.");

        GoEat();

        this.running = true;
    }

    public void Stop()
    {
        feedIndicator.fillAmount = 0;
        this.running = false;
        GoHome();
    }

    public void SetBearFeedQuest(QuestManager.BearFeedQuest quest)
    {
        this.bearFeedQuest = quest;
    }
}
