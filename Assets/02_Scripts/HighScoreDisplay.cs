using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class HighscoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;

    void Start()
    {
        string display = "";
        string json = PlayerPrefs.GetString("Highscores", "");

        if (!string.IsNullOrEmpty(json))
        {
            var list = JsonUtility.FromJson<GameOverManager.HighscoreList>(json);
            int rank = 1;
            foreach (var entry in list.highscores)
            {
                display += $"{rank}. {entry.name} - {entry.score}\n";
                rank++;
            }
        }

        highscoreText.text = display;
    }
}
