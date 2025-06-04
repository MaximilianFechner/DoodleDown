using UnityEngine;
using System.Collections.Generic;

public class BackgroundMove : MonoBehaviour
{
    public GameObject backgroundPrefab;
    public float moveSpeed = 1f;
    public int maxBackgrounds = 3;
    public float backgroundHeight = 12f; 

    private List<GameObject> backgrounds = new List<GameObject>();

    
    void Start()
    {
        for (int i = 0; i < maxBackgrounds; i++)
        {
            float y = -6 + i * backgroundHeight;
            SpawnBackground(new Vector3(0, y, 0));
        }
    }

    void Update()
    {
        foreach (var bg in backgrounds)
        {
            bg.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        if (backgrounds.Count < maxBackgrounds)
        {
            GameObject lastBg = backgrounds[backgrounds.Count - 1];
            if (lastBg.transform.position.y >= -6 + backgroundHeight - 0.1f)
            {
                Vector3 newPos = lastBg.transform.position - new Vector3(0, backgroundHeight, 0);
                SpawnBackground(newPos);
            }
        }

        for (int i = backgrounds.Count - 1; i >= 0; i--)
        {
            if (backgrounds[i].transform.position.y > 7f)
            {
                Destroy(backgrounds[i]);
                backgrounds.RemoveAt(i);
            }
        }
    }

    void SpawnBackground(Vector3 pos)
    {
        GameObject bg = Instantiate(backgroundPrefab, pos, Quaternion.identity);
        backgrounds.Add(bg);
    }
}
