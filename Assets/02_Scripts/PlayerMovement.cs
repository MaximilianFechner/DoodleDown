using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float gravityScale = 2f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float maxSpeed = 5f;
    private bool isAlive = true;

    public float screenGameOverTolerance = 0.1f;

    private Rigidbody2D rb;
    private float moveInput;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float screenTopY;
    private float screenBottomY;

    [Space(20)]
    private AudioSource audioSource;
    public AudioClip[] jumpSounds;
    public AudioClip[] hitSounds;
    public GameObject playerDeathPS;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = gravityScale;

        screenBottomY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0)).y - screenGameOverTolerance;
        screenTopY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y + screenGameOverTolerance;
    }

    void Update()
    {
        if (Time.timeScale != 1) return;

        HandleJumpInput();

        if (rb.linearVelocity.y > 0.1f)
        {
            animator.SetBool("isJumping", true);
        }
        else if (rb.linearVelocity.y < -3f)
        {
            animator.SetBool("isJumping", false);
        }

        if (transform.position.y > screenTopY || transform.position.y < screenBottomY)
        {
            if (!isAlive) return;
            Die();
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float movement = accelRate * speedDif;

        rb.AddForce(Vector2.right * movement);

        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxSpeed, rb.linearVelocity.y);
        }
    }

    private void HandleJumpInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x < Screen.width / 2f)
                {
                    moveInput = -1f;
                }
                else
                {
                    moveInput = 1f;
                }

                Jump();
            }
        }
    }

    private void Jump()
    {
        if (GameManager.Instance.isSFXOn && audioSource != null && jumpSounds != null)
        {
            audioSource.pitch = Random.Range(1.1f, 1.4f);
            audioSource.PlayOneShot(jumpSounds[0]);
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;
        Die();
    }

    public void Die()
    {
        if (GameManager.Instance.isSFXOn && audioSource != null && hitSounds != null)
        {
            audioSource.pitch = Random.Range(1.2f, 1.5f);
            audioSource.PlayOneShot(hitSounds[0]);
            StartCoroutine(WaitThenStopFall(hitSounds[0].length));
            spriteRenderer.enabled = false;
            Instantiate(playerDeathPS, transform.position, Quaternion.identity);
            isAlive = false;
        }

        else
        {
            GameManager.Instance.StopFall();
            spriteRenderer.enabled = false;
            isAlive = false;
        }
    }
    
    private IEnumerator WaitThenStopFall(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        GameManager.Instance.StopFall();
    }
}
