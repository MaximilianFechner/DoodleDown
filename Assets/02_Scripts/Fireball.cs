using UnityEngine;
using UnityEngine.EventSystems;

public class Fireball : MonoBehaviour
{
    private GameObject player;
    private Vector2 moveDirection;

    public float speed;
    public float trackingDuration = 1.5f;

    private AudioSource audioSource;
    public AudioClip clip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Vector2 direction = (player.transform.position - transform.position).normalized;
        moveDirection = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(0.5f, 1.5f);
            audioSource.PlayOneShot(clip);
        }

        Destroy(this.gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
}
