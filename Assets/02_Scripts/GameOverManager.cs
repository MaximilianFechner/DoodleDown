using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public InputField nameInput;
    public Highscore highScore;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void OnPlayerDeath()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void SubmitScore()
    {
        string playerName = nameInput.text;
        int finalScore = Mathf.FloorToInt(highScore.score);

        SaveHighscore(playerName, finalScore);

    }

    void SaveHighscore(string name, int score)
    {
        List<HighscoreEntry> entries = LoadHighscores();

        entries.Add(new HighscoreEntry { name = name, score = score });
        entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (entries.Count > 10) entries = entries.GetRange(0, 10);

        string json = JsonUtility.ToJson(new HighscoreList { highscores = entries });
        PlayerPrefs.SetString("Highscores", json);
        PlayerPrefs.Save();
    }

    List<HighscoreEntry> LoadHighscores()
    {
        if (PlayerPrefs.HasKey("Highscores"))
        {
            string json = PlayerPrefs.GetString("Highscores");
            return JsonUtility.FromJson<HighscoreList>(json).highscores;
        }
        return new List<HighscoreEntry>();
    }

    [System.Serializable]
    public class HighscoreEntry
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class HighscoreList
    {
        public List<HighscoreEntry> highscores;
    }
}
