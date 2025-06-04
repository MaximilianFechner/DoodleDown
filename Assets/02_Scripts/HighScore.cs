using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class Highscore : MonoBehaviour
{
    public float score = 0f;
    public float multiplier = 1f;
    public float multiplierIncreaseInterval = 5f;
    private float multiplierTimer = 0f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;

    void Update()
    {
        score += Time.deltaTime * multiplier;
        multiplierTimer += Time.deltaTime;

        if (multiplierTimer >= multiplierIncreaseInterval)
        {
            multiplier += 0.1f;
            multiplierTimer = 0f;
        }

        scoreText.text = $"Score: {Mathf.FloorToInt(score)}";
        multiplierText.text = $"x{multiplier:F1}";
    }
}
