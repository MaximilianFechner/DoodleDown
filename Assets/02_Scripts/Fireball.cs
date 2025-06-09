using UnityEngine;
using UnityEngine.EventSystems;

public class Fireball : MonoBehaviour
{
    private GameObject player;
    private Vector2 moveDirection;

    public float speed;
    public float trackingDuration = 1.5f;

    private float trackingTimer;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Vector2 direction = (player.transform.position - transform.position).normalized;
        moveDirection = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        Destroy(this.gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
