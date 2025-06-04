using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawn : MonoBehaviour
{
    public GameObject platformPrefab;
    public int initialPlatformCount = 10;
    public float spawnDistance = 2f;
    public float minX = -2.5f, maxX = 2f;
    public float startSpeed = 2f;
    public float speedIncreasePerSecond = 0.2f;

    private float lastSpawnY = -7f;
    private List<GameObject> platforms = new List<GameObject>();
    private float currentSpeed;
    private float timeElapsed;

    void Start()
    {
        currentSpeed = startSpeed;
        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform(lastSpawnY - i * spawnDistance);
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        currentSpeed = startSpeed + speedIncreasePerSecond * timeElapsed;

        foreach (var platform in platforms)
        {
            platform.transform.position += Vector3.up * currentSpeed * Time.deltaTime;
        }

        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            if (platforms[i].transform.position.y > 6f)
            {
                Destroy(platforms[i]);
                platforms.RemoveAt(i);

                float minY = float.MaxValue;
                foreach (var p in platforms)
                {
                    if (p.transform.position.y < minY)
                        minY = p.transform.position.y;
                }
                float newY = platforms.Count > 0 ? minY - spawnDistance : -7f;
                SpawnPlatform(newY);
            }
        }
    }

    void SpawnPlatform(float y)
    {
        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, y, 0);
        GameObject platform = Instantiate(platformPrefab, pos, Quaternion.identity);
        platforms.Add(platform);
    }
}
