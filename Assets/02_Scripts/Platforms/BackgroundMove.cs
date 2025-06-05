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
            float y = -20 + i * backgroundHeight;
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
            GameObject lowestBg = backgrounds[0];
            for (int i = 1; i < backgrounds.Count; i++)
            {
                if (backgrounds[i].transform.position.y < lowestBg.transform.position.y)
                {
                    lowestBg = backgrounds[i];
                }
            }

            if (lowestBg.transform.position.y >= -backgroundHeight -12f)
            {
                Vector3 newPos = lowestBg.transform.position - new Vector3(0, backgroundHeight, 0);
                SpawnBackground(newPos);
            }
        }

        for (int i = backgrounds.Count - 1; i >= 0; i--)
        {
            if (backgrounds[i].transform.position.y > 13f)
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
