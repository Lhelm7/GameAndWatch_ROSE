using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Tooltip("How quickly the camera moves to follow the player.")]
    public float smoothSpeed = 5f;

    [Tooltip("Vertical offset above the player.")]
    public float yOffset = 2f;

    private float highestY;

    void Start()
    {
        if (target != null)
            highestY = target.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Camera only moves upward, never downward
        if (target.position.y > highestY)
            highestY = target.position.y;

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            highestY + yOffset,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}