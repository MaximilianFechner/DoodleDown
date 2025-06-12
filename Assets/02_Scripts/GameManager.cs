using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float score = 0f;
    public float highscore = 0f;

    public bool isLevelStarted = false;
    private AudioSource audioSource;

    [Space(20)]
    public GameObject tapToStartButton;
    public GameObject title;
    public GameObject exitButton;
    public GameObject settingButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    [SerializeField] private Toggle toggleMusic;
    [SerializeField] private Toggle toggleSFX;
    [SerializeField] private Toggle[] musicChoiceToggles;
    public AudioClip[] backgroundMusicClips;

    [Space(20)]
    [Header("Options")]
    public bool isBackgroundMusicOn = true;
    public bool isSFXOn = true;

    private int musicChoice;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        LoadPlayerPrefs();
        UpdateSettingToggles();

        Time.timeScale = 0f;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (isBackgroundMusicOn && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!isBackgroundMusicOn && audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void Update()
    {
        if (!isLevelStarted || Time.timeScale != 1) return;

        score += Time.deltaTime;
        scoreText.text = $"{Mathf.FloorToInt(score)}";

        //PlayerPrefs.SetFloat("Score", score);
        //PlayerPrefs.Save();

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
        settingButton.SetActive(false);
    }

    public void StopFall()
    {
        PlayerPrefs.SetFloat("Score", score);
        PlayerPrefs.Save();

        Time.timeScale = 0f;
        isLevelStarted = false;
        SceneManager.LoadScene(0);

        tapToStartButton.SetActive(true);
        title.SetActive(true);
        exitButton.SetActive(true);
        settingButton.SetActive(true);
    }

    public void LoadPlayerPrefs()
    {
        score = PlayerPrefs.GetFloat("Score", 0f);
        scoreText.text = $"{Mathf.FloorToInt(score)}";

        highscore = PlayerPrefs.GetFloat("Highscore", 0f);
        highScoreText.text = $"{Mathf.FloorToInt(highscore)}";

        isSFXOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
        isBackgroundMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        musicChoice = PlayerPrefs.GetInt("SelectedMusicIndex", 0);
        ChangeBackgroundMusic(musicChoice);
    }

    #region Settings_UI_Toggles
    public void UpdateSettingToggles()
    {
        if (toggleSFX != null)
        {
            toggleSFX.isOn = isSFXOn;
        }

        if (toggleMusic != null)
        {
            toggleMusic.isOn = isBackgroundMusicOn;
        }

        if (musicChoiceToggles != null && musicChoice >= 0 && musicChoice < musicChoiceToggles.Length)
        {
            musicChoiceToggles[musicChoice].isOn = true;
        }
    }

    public void SFXToggle()
    {
        isSFXOn = toggleSFX.isOn;

        PlayerPrefs.SetInt("SFXOn", isSFXOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateSettingToggles();
    }

    public void BackgroundMusicToggle()
    {
        isBackgroundMusicOn = toggleMusic.isOn;

        if (isBackgroundMusicOn)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        PlayerPrefs.SetInt("MusicOn", isBackgroundMusicOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateSettingToggles();
    }

    public void ChangeBackgroundMusic(int choice)
    {
        if (audioSource != null && backgroundMusicClips != null && choice < backgroundMusicClips.Length)
        {
            audioSource.clip = backgroundMusicClips[choice];

            if (audioSource.clip != null && isBackgroundMusicOn)
            {
                audioSource.Play();
            }

            foreach (Toggle toggle in musicChoiceToggles)
            {
                if (toggle.isOn) toggle.interactable = false;
                else toggle.interactable = true;
            }


            PlayerPrefs.SetInt("SelectedMusicIndex", choice);
            PlayerPrefs.Save();
        }
    }
    #endregion
}