using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public Button tapToStartBtn;

    void Update()
    {
        if (!GameManager.Instance.isLevelStarted) return;

        scoreText.text = $"{Mathf.FloorToInt(GameManager.Instance.score)}";
    }
}
