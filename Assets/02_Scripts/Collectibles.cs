using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer coinLighting;
    [SerializeField] public int pointValue = 10;
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;

    private bool isCollected = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        SpriteRenderer coinLighting = GetComponentInChildren<SpriteRenderer>();
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
            audioSource.PlayOneShot(collectSound);
        }

        spriteRenderer.enabled = false;
        coinLighting.enabled = false;

        Destroy(gameObject, collectSound != null ? collectSound.length : 0.1f);
    }
}
