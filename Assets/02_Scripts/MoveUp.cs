using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}
