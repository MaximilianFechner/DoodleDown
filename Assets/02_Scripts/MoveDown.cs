using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y > -12) return;
        Destroy(gameObject);
    }
}
