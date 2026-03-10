using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CameraBackground : MonoBehaviour
{
    void Start()
    {
        FitToScreen();
    }

    /// <summary>
    /// Redimensionne le sprite pour couvrir exactement la vue de la caméra.
    /// </summary>
    void FitToScreen()
    {
        Camera cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr.sprite == null)
        {
            Debug.LogError("CameraBackground: aucun sprite assigné.");
            return;
        }

        float worldHeight = cam.orthographicSize * 2f;
        float worldWidth  = worldHeight * cam.aspect;

        Vector2 spriteSize = sr.sprite.bounds.size;

        // "Cover" : on prend le scale le plus grand pour ne jamais laisser de bord vide
        float scaleX = worldWidth  / spriteSize.x;
        float scaleY = worldHeight / spriteSize.y;
        float scale  = Mathf.Max(scaleX, scaleY);

        transform.localScale    = new Vector3(scale, scale, 1f);
        transform.localPosition = new Vector3(0f, 0f, 10f);
    }
}