using UnityEngine;
using UnityEngine.UI;

public class SFXToggleLoad : MonoBehaviour
{
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        toggle.isOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
    }
}
