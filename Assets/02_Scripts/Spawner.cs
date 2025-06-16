using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] platformTiles;
    public GameObject[] collectibles;

    public float movementSpeed;
    public float spawnDelayMin;
    public float spawnDelayMax;
    public float spawnMinX;
    public float spawnMaxX;

    public float collectibleChance;

    public void SpawningPlatforms(bool isSpawning)
    {
        StartCoroutine(Spawning(isSpawning));
    }

    public IEnumerator Spawning(bool isSpawning)
    {
        while (isSpawning)
        {
            float random = Random.Range(1, 100);
            float spawnX = Random.Range(spawnMinX, spawnMaxX);

            Vector2 spawnPosition = new Vector2(spawnX, transform.position.y);

            if (random <= collectibleChance)
            {
                GameObject collectibleToSpawn = Instantiate(collectibles[0], spawnPosition, Quaternion.identity, transform);
                collectibleToSpawn.GetComponent<MoveUp>().speed = movementSpeed;
                collectibleChance = 0f;
            }

            else
            {
                GameObject platformToSpawn = Instantiate(platformTiles[Random.Range(0, platformTiles.Length)], spawnPosition, Quaternion.identity, transform);
                platformToSpawn.GetComponent <MoveUp>().speed = movementSpeed;
                collectibleChance++;
            }
            
            yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));
        }
    }
}
