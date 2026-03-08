using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    public ColorType currentColor;

    // Assign Assets/InputSystem_Actions.inputactions in the Inspector
    public InputActionAsset inputActionAsset;

    // C# event notified when the player color changes
    public event System.Action<ColorType> OnColorChanged;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private InputAction moveAction;

    private float screenHalfWidth;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        var playerMap = inputActionAsset.FindActionMap("Player", throwIfNotFound: true);
        moveAction = playerMap.FindAction("Move", throwIfNotFound: true);
        moveAction.Enable();
    }

    void Start()
    {
        screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        SetColor(currentColor);
    }

    void Update()
    {
        Move();
        WrapHorizontal();
    }

    void Move()
    {
        float horizontal = moveAction.ReadValue<Vector2>().x;
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }

    /// <summary>
    /// Wraps the player from one side of the screen to the other.
    /// </summary>
    void WrapHorizontal()
    {
        Vector3 pos = transform.position;

        if (pos.x > screenHalfWidth)
            pos.x = -screenHalfWidth;
        else if (pos.x < -screenHalfWidth)
            pos.x = screenHalfWidth;

        transform.position = pos;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    /// <summary>
    /// Assigns a new color to the player and updates the visual.
    /// </summary>
    public void SetColor(ColorType newColor)
    {
        currentColor = newColor;
        spriteRenderer.color = ColorTypeToUnityColor(newColor);
        OnColorChanged?.Invoke(currentColor);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Platform platform = collision.gameObject.GetComponent<Platform>();

        if (platform == null) return;

        // Only bounce off the top of the platform
        bool hittingFromAbove = collision.GetContact(0).normal.y > 0.5f;
        if (!hittingFromAbove) return;

        if (platform.platformColor == currentColor)
        {
            Jump();
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");
        enabled = false;
        rb.linearVelocity = Vector2.zero;
        Time.timeScale = 0f;
    }

    void OnDisable()
    {
        moveAction?.Disable();
    }

    /// <summary>
    /// Converts a ColorType enum value to its corresponding Unity Color.
    /// </summary>
    public static Color ColorTypeToUnityColor(ColorType type)
    {
        return type switch
        {
            ColorType.Blue   => new Color(0.2f, 0.5f, 1f),
            ColorType.Red    => new Color(1f, 0.25f, 0.25f),
            ColorType.Green  => new Color(0.2f, 0.85f, 0.3f),
            ColorType.Yellow => new Color(1f, 0.9f, 0.1f),
            _                => Color.white,
        };
    }
}
