using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    public string localizationKey;

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        string value = LanguageManager.Instance.GetLocalizedValue(localizationKey);
        GetComponent<TextMeshProUGUI>().text = value;
    }
}
