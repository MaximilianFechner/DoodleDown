using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public int pointValue = 10;
    [SerializeField] private AudioClip collectSound;

    private bool isCollected = false;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            Collect();
        }
    }

    private void Collect()
    {
        isCollected = true;

        GameManager.Instance.AddPoints(pointValue);

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        spriteRenderer.enabled = false;

        Destroy(gameObject, collectSound != null ? collectSound.length : 0.1f);
    }
}
