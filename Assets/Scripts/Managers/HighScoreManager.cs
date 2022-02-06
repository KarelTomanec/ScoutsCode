using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {

    private static HighScoreManager m_instance;

    private int length = 5;

    public static HighScoreManager _instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("HighScoreManager").AddComponent<HighScoreManager>();
            }
            return m_instance;
        }
    }

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SaveHighScore(string name, int score)
    {
        List<HighScore> highScore = GetHighScores();

        int i = 1;

        if (highScore.Count == 0)
        {
            highScore.Add(new HighScore(name, score));
        }
        else
        {
            for (i = 1; i <= highScore.Count && i <= length; i++)
            {
                if (score > highScore[i - 1].score)
                {
                    highScore.Insert(i - 1, new HighScore(name, score));
                    break;
                }
                if (i == highScore.Count && i < length)
                {
                    highScore.Add(new HighScore(name, score));
                    break;
                }
            }
        }

        i = 1;
        while (i <= length && i <= highScore.Count)
        {
            PlayerPrefs.SetString("HS" + i + "name", highScore[i - 1].name);
            PlayerPrefs.SetInt("HS" + i + "score", highScore[i - 1].score);
            i++;
        }

        PlayerPrefs.Save();
    }

    public List<HighScore> GetHighScores()
    {
        List<HighScore> highScores = new List<HighScore>();

        int i = 1;
        while (i <= length && PlayerPrefs.HasKey("HS" + i + "score"))
        {
            highScores.Add(new HighScore(PlayerPrefs.GetString("HS" + i + "name"), PlayerPrefs.GetInt("HS" + i + "score")));
            i++;
        }

        return highScores;
    }

    public void DeleteHighScore()
    {
        List<HighScore> highScores = GetHighScores();

        for (int i = 1; i <= length; i++)
        {
            if (PlayerPrefs.HasKey("HS" + i + "name"))
            {
                PlayerPrefs.DeleteKey("HS" + i + "name");
                PlayerPrefs.DeleteKey("HS" + i + "score");
            }
            else
            {
                break;
            }  
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public class HighScore
    {
        public string name;
        public int score;

        public HighScore()
        {
            name = "";
            score = 0;
        }

        public HighScore(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}
