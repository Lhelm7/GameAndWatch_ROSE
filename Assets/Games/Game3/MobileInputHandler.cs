using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Gère les boutons tactiles gauche/droite pour le déplacement mobile.
/// À attacher sur chaque bouton UI (LeftButton et RightButton).
/// </summary>
public class MobileInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direction { Left, Right }

    [Tooltip("Direction assignée à ce bouton.")]
    public Direction direction;

    private PlayerController player;
    private bool isPressed;

    private const float LeftValue  = -1f;
    private const float RightValue =  1f;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        if (player == null)
            Debug.LogError("MobileInputHandler: PlayerController introuvable.");
    }

    void Update()
    {
        if (player == null || !isPressed) return;

        player.MobileHorizontalInput = direction == Direction.Left ? LeftValue : RightValue;
    }

    /// <summary>Appelé quand le doigt appuie sur le bouton.</summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    /// <summary>Appelé quand le doigt relâche le bouton.</summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        if (player != null)
            player.MobileHorizontalInput = 0f;
    }
}