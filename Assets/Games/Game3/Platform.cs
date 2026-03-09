using UnityEngine;

public class Platform : MonoBehaviour
{
    public ColorType platformColor;
    public bool isNeutral = false;

    private static readonly Color NeutralColor = new Color(0.75f, 0.75f, 0.75f);

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Assigne une couleur à la plateforme et met à jour le visuel.
    /// </summary>
    public void SetColor(ColorType color)
    {
        isNeutral     = false;
        platformColor = color;
        spriteRenderer.color = PlayerController.ColorTypeToUnityColor(color);
    }

    /// <summary>
    /// Passe la plateforme en mode neutre : tout le monde peut rebondir dessus.
    /// </summary>
    public void SetNeutral()
    {
        isNeutral = true;
        spriteRenderer.color = NeutralColor;
    }
}