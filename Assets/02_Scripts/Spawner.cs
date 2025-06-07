using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] platformTiles;
    public GameObject[] collectibles;

    public float movementSpeed;
    public float spawnDelay;
    public float spawnMinX;
    public float spawnMaxX;

    public float collectibleChance;

    private void Start()
    {
        StartCoroutine(Spawning());
    }

    public IEnumerator Spawning()
    {
        while (true)
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
            
            yield return new WaitForSeconds(spawnDelay + (Random.Range(-0.5f, 0f)));
        }
    }
}
