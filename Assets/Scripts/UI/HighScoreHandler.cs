using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HighScoreHandler : MonoBehaviour {

    public GameObject[] lines;

    public GameObject heading;
    public GameObject background;
    public GameObject nameInput;
    public GameObject scores;
    public TMP_InputField input;

    public void ShowInputPart()
    {
        background.SetActive(true);
        heading.SetActive(true);
        nameInput.SetActive(true);
    }

    private void HideInputPart()
    {
        nameInput.SetActive(false);
    }

    private void ShowScorePart()
    {
        background.SetActive(true);
        heading.SetActive(true);
        scores.SetActive(true);
    }

    private void AddHighScore()
    {
        string name = input.text;
        int score = GameObject.Find("GameManager").GetComponent<GameManager>().score.GetScore();
        HighScoreManager._instance.SaveHighScore(name, score);
    }

    private void FillScoreLines()
    {
        List<HighScoreManager.HighScore> highScores = HighScoreManager._instance.GetHighScores();

        for (int i = 0; i < lines.Length; i++)
        {
            if (i < highScores.Count - 1)
            {
                lines[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (i + 1).ToString();
                lines[i].transform.GetChild(1).GetComponent<TMP_Text>().text = highScores[i].name;
                lines[i].transform.GetChild(2).GetComponent<TMP_Text>().text = highScores[i].score.ToString();
            }
            else
            {
                lines[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (i + 1).ToString();
                lines[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "...";
                lines[i].transform.GetChild(2).GetComponent<TMP_Text>().text = "...";
            }
        }
    }


    public void InputNameButtonClick()
    {
        AddHighScore();
        HideInputPart();
        FillScoreLines();
        ShowScorePart();
    }

    public void ScoreButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ResetButtonClick()
    {
        HighScoreManager._instance.DeleteHighScore();
        FillScoreLines();
    }
}
