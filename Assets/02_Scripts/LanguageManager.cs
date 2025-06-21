using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum Language { English, German }
    public Language currentLanguage;

    private Dictionary<string, string> localizedText;

    public TextAsset germanJson;
    public TextAsset englishJson;

    public GameObject deImage;
    public GameObject enImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            string language = PlayerPrefs.GetString("language");

            if (language != null && language == Language.German.ToString())
            {
                currentLanguage = Language.German;
            }
            else
            {
                currentLanguage = Language.English;
            }

            LoadLocalizedText(currentLanguage);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLanguage()
    {
        if (currentLanguage == Language.English)
        {
            currentLanguage = Language.German;

            PlayerPrefs.SetString("language", currentLanguage.ToString());
            PlayerPrefs.Save();

            if (deImage != null && enImage != null)
            {
                deImage.SetActive(true);
                enImage.SetActive(false);
            }
        }
        else
        {
            currentLanguage = Language.English;

            PlayerPrefs.SetString("language", currentLanguage.ToString());
            PlayerPrefs.Save();

            if (deImage != null && enImage != null)
            {
                deImage.SetActive(false);
                enImage.SetActive(true);
            }
        }

        if (currentLanguage == Language.German)
            LanguageManager.Instance.LoadLocalizedText(LanguageManager.Language.German);
        else
            LanguageManager.Instance.LoadLocalizedText(LanguageManager.Language.English);
    }

    public void LoadLocalizedText(Language language)
    {
        currentLanguage = language;

        string data = (language == Language.German) ? germanJson.text : englishJson.text;
        localizedText = JsonUtility.FromJson<LocalizationData>(data).ToDictionary();

        // Aktualisiere alle Texte
        foreach (LocalizedText lt in FindObjectsOfType<LocalizedText>())
        {
            lt.UpdateText();
        }

        if (currentLanguage == Language.English)
        {
            deImage.SetActive(false);
            enImage.SetActive(true);
        }
        else
        {
            deImage.SetActive(true);
            enImage.SetActive(false);
        }
    }

    public string GetLocalizedValue(string key)
    {
        if (localizedText.TryGetValue(key, out string value))
            return value;

        return "[Missing Text: " + key + "]";
    }
}

[System.Serializable]
public class LocalizationData
{
    public LocalizationEntry[] entries;

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (var entry in entries)
        {
            dict[entry.key] = entry.value;
        }
        return dict;
    }
}

[System.Serializable]
public class LocalizationEntry
{
    public string key;
    public string value;
}
