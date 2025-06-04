using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("Invincibility Settings")]
    [SerializeField] public bool flashWhenInvincible = true;
    [SerializeField] public float invincibilityFlashInterval = 0.1f;
    [SerializeField] public float invincibilityAlpha = 0.5f;

    private bool isInvincible = false;
    private bool isSlowFalling = false;
    private SpriteRenderer playerSprite;
    private PlayerMovement playerMovement;
    private float originalGravityScale;
    private Coroutine invincibilityCoroutine;
    private Coroutine slowFallCoroutine;

    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        if (playerSprite == null)
        {
            playerSprite = GetComponentInChildren<SpriteRenderer>();
        }

        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            originalGravityScale = playerMovement.gravityScale;
        }
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void MakeInvincible(float duration)
    {
        if (invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);
        }

        invincibilityCoroutine = StartCoroutine(InvincibilityRoutine(duration));
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;

        if (flashWhenInvincible && playerSprite != null)
        {
            Color originalColor = playerSprite.color;
            Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, invincibilityAlpha);

            float endTime = Time.time + duration;
            while (Time.time < endTime)
            {
                playerSprite.color = (playerSprite.color.a == originalColor.a) ? transparentColor : originalColor;
                yield return new WaitForSeconds(invincibilityFlashInterval);
            }

            playerSprite.color = originalColor;
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }

        isInvincible = false;
        invincibilityCoroutine = null;
    }

    public bool IsSlowFalling()
    {
        return isSlowFalling;
    }

    public void ActivateSlowFall(float duration, float slowFallScale)
    {
        if (slowFallCoroutine != null)
        {
            StopCoroutine(slowFallCoroutine);
        }

        slowFallCoroutine = StartCoroutine(SlowFallRoutine(duration, slowFallScale));
    }

    private IEnumerator SlowFallRoutine(float duration, float slowFallScale)
    {
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogWarning("Player has no PlayerMovement component. Slow fall will not work.");
                yield break;
            }
            originalGravityScale = playerMovement.gravityScale;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("Player has no Rigidbody2D component. Slow fall will not work.");
            yield break;
        }

        isSlowFalling = true;

        float originalGravity = playerMovement.gravityScale;

        playerMovement.gravityScale = slowFallScale;
        rb.gravityScale = slowFallScale;

        yield return new WaitForSeconds(duration);

        playerMovement.gravityScale = originalGravity;
        rb.gravityScale = originalGravity;

        isSlowFalling = false;
        slowFallCoroutine = null;
    }
}
