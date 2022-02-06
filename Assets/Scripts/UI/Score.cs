using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private int score = 0;
    public Text scoreText;
    
	void Start () {
        scoreText.text = "Score: " + score;
	}

    public void ShowScore()
    {
        scoreText.enabled = true;
    }

    public void HideScore()
    {
        scoreText.enabled = false;
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }
}
