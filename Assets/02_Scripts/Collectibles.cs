using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public int pointValue = 10;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private GameObject collectEffect;

    [Header("Power-up Settings")]
    [SerializeField] public bool grantsInvincibility = false;
    [SerializeField] public float invincibilityDuration = 5f;
    [SerializeField] public bool grantsSlowFall = false;
    [SerializeField] public float slowFallDuration = 5f;
    [SerializeField] public float slowFallGravityScale = 0.3f;

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
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        isCollected = true;

        PlayerState playerState = null;

        if (grantsInvincibility || grantsSlowFall)
        {
            playerState = player.GetComponent<PlayerState>();
            if (playerState == null)
            {
                playerState = player.AddComponent<PlayerState>();
            }

            if (grantsInvincibility)
            {
                playerState.MakeInvincible(invincibilityDuration);
            }

            if (grantsSlowFall)
            {
                playerState.ActivateSlowFall(slowFallDuration, slowFallGravityScale);
            }
        }

        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddPoints(pointValue);
        }

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        spriteRenderer.enabled = false;

        Destroy(gameObject, collectSound != null ? collectSound.length : 0.1f);
    }
}
