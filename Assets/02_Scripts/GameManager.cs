using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Values To Track")]
    public float score = 0f;
    public float highscore = 0f;
    public bool isLevelStarted = false;
    public bool isGamePaused = false;

    private AudioSource audioSource;

    [Space(20)]
    [Header("UI References")]
    public GameObject tapToStartButton;
    public GameObject tapToContinueButton;
    public GameObject title;
    public GameObject exitButton;
    public GameObject pauseButton;
    public GameObject settingButton;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI unpauseText;
    [SerializeField] private Toggle toggleMusic;
    [SerializeField] private Toggle toggleSFX;

    [Space(5)]
    [SerializeField] private Toggle[] musicChoiceToggles;
    public AudioClip[] backgroundMusicClips;

    [Space(20)]
    [Header("Options")]
    public bool isBackgroundMusicOn = true;
    public bool isSFXOn = true;

    [Space(20)]
    [Header("Script References")]
    public PlayerMovement playerMovement;
    public Spawner platformSpawner;
    public BatSpawner batSpawner;

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

        //Time.timeScale = 0f;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (!isLevelStarted) return;
        if (!Application.isFocused) Pause();

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

    public void Pause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
                pauseButton.SetActive(false);
                tapToContinueButton.SetActive(true);
            }

            if (isSFXOn)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<AudioSource>().Stop();
                }
            }

            Time.timeScale = 0f;
        }
        else
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }

            if (settingsPanel != null && settingsPanel.activeInHierarchy)
            {
                settingsPanel.SetActive(false);
            }

            StartCoroutine("ContinueCountdown");
        }

    }

    public void StartFall()
    {
        score = 0f;
        //Time.timeScale = 1f;
        isLevelStarted = true;

        tapToStartButton.SetActive(false);
        title.SetActive(false);
        exitButton.SetActive(false);
        settingButton.SetActive(false);
        pauseButton.SetActive(true);

        if (playerMovement == null) playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.ChangeRigidbodyType(false);
        }

        if (platformSpawner == null) platformSpawner = FindAnyObjectByType<Spawner>();
        if (platformSpawner != null)
        {
            platformSpawner.SpawningPlatforms(true);
        }

        if (batSpawner == null) batSpawner = FindAnyObjectByType<BatSpawner>();
        if (batSpawner != null)
        {
            batSpawner.SpawningBats(true);
        }
    }

    public void StopFall()
    {
        PlayerPrefs.SetFloat("Score", score);
        PlayerPrefs.Save();

        //Time.timeScale = 0f;
        //isLevelStarted = false;
        SceneManager.LoadScene(0);

        tapToStartButton.SetActive(true);
        title.SetActive(true);
        exitButton.SetActive(true);
        settingButton.SetActive(true);
        pauseButton.SetActive(false);

        if (pausePanel.gameObject.activeInHierarchy) pausePanel.SetActive(false);
        if (unpauseText.gameObject.activeInHierarchy) unpauseText.gameObject.SetActive(false);
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

        if (musicChoiceToggles != null && musicChoice >= 0 && musicChoice < musicChoiceToggles.Length)
        {
            musicChoiceToggles[musicChoice].isOn = true;
        }

        ChangeBackgroundMusic(musicChoice);

        if (isBackgroundMusicOn && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!isBackgroundMusicOn && audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public IEnumerator ContinueCountdown()
    {
        if (unpauseText == null) yield return null;

        unpauseText.gameObject.SetActive(true);
        unpauseText.text = string.Empty;

        for (int i = 3; i > 0; i--)
        {
            unpauseText.text = $"{i.ToString()}...";
            yield return new WaitForSecondsRealtime(1f);
        }

        unpauseText.text = string.Empty;
        unpauseText.gameObject.SetActive(false);
        pauseButton.SetActive(true);
        tapToContinueButton.SetActive(false);

        if (isSFXOn)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<AudioSource>().Play();
            }
        }

        Time.timeScale = 1f;
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