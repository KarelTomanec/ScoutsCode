using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    private Image timeBar;
    private float time;
    private bool active = false;

    public float timeCap = 60f;

    public GameObject timesUp;
    public GameManager gameManager;
    public Score score;


    void Start ()
    {
        timesUp.SetActive(false);
        timeBar = GetComponent<Image>();
        time = timeCap;
    }
	
	void Update ()
    {
        if (active == true)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                timeBar.fillAmount = time / timeCap;
                timeBar.color = new Color(1 - timeBar.color.g * timeBar.color.g, time / timeCap, 0);
            }
            else
            {
                timesUp.SetActive(true);
                gameManager.GameOver();
                //Time.timeScale = 0.7f;
            }
        }
    }

    public void StartTimer()
    {
        active = true;
        timeBar = GetComponent<Image>();
        timeBar.enabled = true;
    }

    public void StopTimer()
    {
        active = false;
        timeBar.enabled = false;
    }
}
