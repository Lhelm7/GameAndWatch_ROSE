using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Tooltip("Vitesse de suivi de la caméra.")]
    public float smoothSpeed = 5f;

    [Tooltip("Décalage vertical au-dessus du joueur.")]
    public float yOffset = 2f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            target.position.y + yOffset,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}