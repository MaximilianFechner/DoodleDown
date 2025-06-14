using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class InvertedToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private GameObject checkmark;

    private void Awake()
    {
        UpdateCheckmark(toggle.isOn);
        toggle.onValueChanged.AddListener(UpdateCheckmark);
    }

    private void UpdateCheckmark(bool isOn)
    {
        if (isOn)
        {
            checkmark.SetActive(false);
        }
        else checkmark.SetActive(true);
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(UpdateCheckmark);
    }
}
