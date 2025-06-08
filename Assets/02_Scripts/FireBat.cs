using System.Collections;
using UnityEngine;

public class FireBat : MonoBehaviour
{
    public GameObject fireBall;
    public float fireBallSpeed;
    public float attackDelay;
    public float batLifeTime;
    public bool isInPosition;

    private void Start()
    {
        StartCoroutine(SpawnFireBalls());
    }

    public IEnumerator SpawnFireBalls()
    {
        while (true)
        {
            Instantiate(fireBall, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(attackDelay);
        }
    }
}
