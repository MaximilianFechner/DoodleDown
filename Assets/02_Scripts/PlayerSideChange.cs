using UnityEngine;

public class PlayerSideChange : MonoBehaviour
{
    public Affix sideChangeAffix;
    private float changeXLimit;
    public float offset = 0.2f;

    private TrailRenderer trailRenderer;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        CalculateLimits();
    }

    void Update()
    {
        if (sideChangeAffix != null && sideChangeAffix.isActive) return;

        Vector3 pos = transform.position;
        bool hasTeleported = false;

        if (pos.x > changeXLimit)
        {
            pos.x = -changeXLimit + offset;
            transform.position = pos;
            hasTeleported = true;
        }
        else if (pos.x < -changeXLimit)
        {
            pos.x = changeXLimit - offset;
            transform.position = pos;
            hasTeleported = true;
        }

        if (hasTeleported && trailRenderer != null)
        {
            trailRenderer.Clear();
        }
    }

    private void CalculateLimits()
    {
        if (Camera.main != null)
        {
            changeXLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x + offset;
        }
    }

    private void OnDrawGizmos()
    {
        if (Camera.main != null)
        {
            float limit = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x + offset;

            Gizmos.color = Color.blue;

            Vector3 topRight = new Vector3(limit, 10f, 0f);
            Vector3 bottomRight = new Vector3(limit, -10f, 0f);
            Gizmos.DrawLine(topRight, bottomRight);

            Vector3 topLeft = new Vector3(-limit, 10f, 0f);
            Vector3 bottomLeft = new Vector3(-limit, -10f, 0f);
            Gizmos.DrawLine(topLeft, bottomLeft);
        }
    }
}