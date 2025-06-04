using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float gravityScale = 2f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float maxSpeed = 5f;

    //public Sprite fallSprite;
    //public Sprite jumpSprite;

    private Rigidbody2D rb;
    private float moveInput;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        moveInput = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveInput = 1f;

        if (rb.linearVelocity.y > 0.1f)
        {
            animator.SetBool("isJumping", true);
        }
        else if (rb.linearVelocity.y < -2f)
        {
            animator.SetBool("isJumping", false);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Collectibles>() != null)
        {
            return;
        }

        PlayerState playerState = GetComponent<PlayerState>();
        if (playerState != null && playerState.IsInvincible())
        {
            return;
        }
        
        //TODO Restartlogik einbauen
        Debug.Log("you died!");
        SceneManager.LoadScene(0);
    }
}
