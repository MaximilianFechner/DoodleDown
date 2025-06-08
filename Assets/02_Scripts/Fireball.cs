using UnityEngine;
using UnityEngine.EventSystems;

public class Fireball : MonoBehaviour
{
    private GameObject player;
    private Vector2 target;

    public float speed;
    public float trackingDuration = 1.5f;

    private float trackingTimer;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        trackingTimer = trackingDuration;
        UpdateDirection();
    }

    void Update()
    {
        if (trackingTimer > 0f)
        {
            trackingTimer -= Time.deltaTime;

            if (player != null)
            {
                UpdateDirection();
                UpdateRotation();
            }
        }

        transform.Translate(target * speed * Time.deltaTime);
    }

    private void UpdateDirection()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        target = direction;
    }

    private void UpdateRotation()
    {
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
