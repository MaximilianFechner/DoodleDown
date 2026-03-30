using UnityEngine;

public class PlayerSideChange : MonoBehaviour
{
    public float changeXLimit = 4.8f;

    void Update()
    {
        if (GameManager.Instance.affixSideChangeDisabled) return;

        Vector3 pos = transform.position;

        if (pos.x > changeXLimit)
        {
            pos.x = -changeXLimit;
            transform.position = pos;
        }
        else if (pos.x < -changeXLimit)
        {
            pos.x = changeXLimit;
            transform.position = pos;
        }
    }
}
