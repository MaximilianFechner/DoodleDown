using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float score = 0f;
    public float highscore = 0f;

    public bool isLevelStarted = false;

    [Space(20)]
    public GameObject tapToStartButton;
    public GameObject title;
    public GameObject exitButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    [Space(20)]
    [Header("Options")]
    public bool isJumpSFXOn = true;
    public bool isBackgroundSFXOn = true;
    public bool isHitSFXOn = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 0f;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        score = 0f;

        highscore = PlayerPrefs.GetFloat("Highscore", 0f);
        highScoreText.text = $"{Mathf.FloorToInt(highscore)}";
    }

    void Update()
    {
        if (!isLevelStarted || Time.timeScale != 1) return;

        score += Time.deltaTime;
        scoreText.text = $"{Mathf.FloorToInt(score)}";

        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetFloat("Highscore", highscore);
            PlayerPrefs.Save();
            highScoreText.text = $"{Mathf.FloorToInt(highscore)}";
        }
    }

    public void AddPoints(int value)
    {
        score += value;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void StartFall()
    {
        score = 0f;
        Time.timeScale = 1f;
        isLevelStarted = true;

        tapToStartButton.SetActive(false);
        title.SetActive(false);
        exitButton.SetActive(false);
    }

    public void StopFall()
    {
        Time.timeScale = 0f;
        isLevelStarted = false;
        SceneManager.LoadScene(0);

        tapToStartButton.SetActive(true);
        title.SetActive(true);
        exitButton.SetActive(true);
    }
}