using TMPro;
using UnityEngine;

public class DebugFPS : MonoBehaviour
{
    private TextMeshProUGUI fpsText;

    private int frameCount = 0;
    private float deltaTime = 0f;
    private float interval = 0.5f;

    private void Awake()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        frameCount++;
        deltaTime += Time.unscaledDeltaTime;

        if (deltaTime > interval)
        {
            float fps = frameCount / deltaTime;
            fpsText.text = $"{Mathf.RoundToInt(fps)}";
            frameCount = 0;
            deltaTime = 0f;
        }
    }
}
