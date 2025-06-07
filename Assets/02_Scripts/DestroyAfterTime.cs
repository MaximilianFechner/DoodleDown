using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < 12) return;
        Destroy(gameObject);
    }
}
