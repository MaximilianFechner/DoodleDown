using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float targetWidth = 9f;
    public float baseOrthographicSize = 5f;

    void Start()
    {
        Camera cam = Camera.main;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = targetWidth / (baseOrthographicSize * 2f);

        if (screenRatio >= targetRatio)
        {
            cam.orthographicSize = baseOrthographicSize;
        }
        else
        {
            cam.orthographicSize = targetWidth / (2f * screenRatio);
        }
    }
}
