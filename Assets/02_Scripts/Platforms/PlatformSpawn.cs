using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawn : MonoBehaviour
{
    public GameObject platformPrefabA;
    public GameObject platformPrefabB;
    public GameObject powerupPrefabA;
    public GameObject powerupPrefabB;
    public int initialPlatformCount = 10;
    public float spawnDistance = 2f;
    public float minX = -2.5f, maxX = 2f;
    public float startSpeed = 2f;
    public float speedIncreasePerSecond = 0.2f;
    public int maxSamePrefabInRow = 3; 

    private float lastSpawnY = -13f;
    private List<GameObject> platforms = new List<GameObject>();
    private float currentSpeed;
    private float timeElapsed;

    private int lastPrefabIndex = -1; 
    private int samePrefabCount = 0;

    private int platformsSinceLastPowerup = 0;
    private int powerupInterval = 20; 

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
            if (platform != null)
                platform.transform.position += Vector3.up * currentSpeed * Time.deltaTime;
        }

        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            if (platforms[i] == null)
            {
                platforms.RemoveAt(i);
                continue;
            }

            if (platforms[i].transform.position.y > 13f)
            {
                Destroy(platforms[i]);
                platforms.RemoveAt(i);

                float minY = float.MaxValue;
                foreach (var p in platforms)
                {
                    if (p != null && p.transform.position.y < minY)
                        minY = p.transform.position.y;
                }

                float newY = platforms.Count > 0 ? minY - spawnDistance : -7f;
                SpawnPlatform(newY);
            }
        }
    }

    void SpawnPlatform(float y)
    {
        GameObject prefabToSpawn = null;

        platformsSinceLastPowerup++;
        bool spawnPowerup = false;

        if (platformsSinceLastPowerup >= powerupInterval || Random.value < 0.05f)
        {
            spawnPowerup = true;
            platformsSinceLastPowerup = 0;
        }

        if (spawnPowerup)
        {
            prefabToSpawn = Random.Range(0, 2) == 0 ? powerupPrefabA : powerupPrefabB;
        }
        else
        {
            int prefabIndex;
            if (lastPrefabIndex == -1)
            {
                prefabIndex = Random.Range(0, 2);
                samePrefabCount = 1;
            }
            else
            {
                if (samePrefabCount >= maxSamePrefabInRow)
                {
                    prefabIndex = 1 - lastPrefabIndex;
                    samePrefabCount = 1;
                }
                else
                {
                    prefabIndex = Random.Range(0, 2);
                    if (prefabIndex == lastPrefabIndex)
                        samePrefabCount++;
                    else
                        samePrefabCount = 1;
                }
            }
            lastPrefabIndex = prefabIndex;
            prefabToSpawn = prefabIndex == 0 ? platformPrefabA : platformPrefabB;
        }

        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, y, 0);
        GameObject platform = Instantiate(prefabToSpawn, pos, Quaternion.identity);
        platforms.Add(platform);
    }
}
