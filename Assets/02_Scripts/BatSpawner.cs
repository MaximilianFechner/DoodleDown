using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;

public class BatSpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public AudioClip[] spawnSound;

    public float enemySpeedMin;
    public float enemySpeedMax;
    public float spawnDelay;
    public float spawnMinX;
    public float spawnMaxX;

    public float batSpawnChance;
    public float fireBatSpawnChance;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(BatSpawning());
    }

    public IEnumerator BatSpawning()
    {
        while (true)
        {
            float random = Random.Range(1, 100);
            float spawnX = Random.Range(spawnMinX, spawnMaxX);

            Vector2 spawnPosition = new Vector2(spawnX, transform.position.y);

            if (random <= fireBatSpawnChance)
            {
                GameObject enemy = Instantiate(enemies[1], spawnPosition, Quaternion.identity, transform);
                enemy.GetComponent<MoveDown>().speed = 1f;

                if (audioSource != null && GameManager.Instance.isSFXOn)
                {
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(spawnSound[0]);
                }

                fireBatSpawnChance = 1f;
                batSpawnChance += 10f;

                yield return new WaitForSeconds(spawnDelay);
                continue;
            }

            if (random <= batSpawnChance)
            {
                GameObject enemy = Instantiate(enemies[0], spawnPosition, Quaternion.identity, transform);
                enemy.GetComponent<MoveDown>().speed = Random.Range(enemySpeedMin, enemySpeedMax);

                if (audioSource != null && GameManager.Instance.isSFXOn)
                {
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(spawnSound[0]);
                }

                batSpawnChance = 0f;
            }

            fireBatSpawnChance += 1f;
            batSpawnChance += 10f;


            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
