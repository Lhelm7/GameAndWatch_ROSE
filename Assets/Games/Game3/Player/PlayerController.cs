using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Color")]
    public ColorType currentColor;

    [Header("Input")]
    public InputActionAsset inputActionAsset;

    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite jumpSprite;

    public event System.Action<ColorType> OnColorChanged;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private InputAction moveAction;

    [SerializeField] private AudioEventDispatcher audioEventDispatcher;

    [SerializeField] private GameObject music;
    public event System.Action<int> OnPlayerDied;

    public float deathHeight = -10f;
    bool isDead = false;
    private float screenHalfWidth;
    private bool isGrounded;

    // Input mobile fourni par MobileInputHandler
    public float MobileHorizontalInput { get; set; }

    void Awake()
    {
        rb             = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        var playerMap = inputActionAsset.FindActionMap("Player", throwIfNotFound: true);
        moveAction    = playerMap.FindAction("Move", throwIfNotFound: true);
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
        UpdateSprite();  
        
        if(!isDead && transform.position.y < deathHeight)
        {
            Die();
        }
    }

    void Move()
    {
        // Fusionne input clavier/gamepad et input mobile
        float horizontal = moveAction.ReadValue<Vector2>().x;
        if (Mathf.Approximately(horizontal, 0f))
            horizontal = MobileHorizontalInput;

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }

    /// <summary>
    /// Wrap horizontal : sortir d'un côté = entrer de l'autre.
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

    /// <summary>
    /// Change le sprite selon si le joueur est en l'air ou au sol.
    /// </summary>
    void UpdateSprite()
    {
        if (idleSprite == null || jumpSprite == null) return;

        spriteRenderer.sprite = isGrounded ? idleSprite : jumpSprite;
    }

    void Jump()
    {
        isGrounded = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    /// <summary>
    /// Assigne une nouvelle couleur au joueur et met à jour le visuel.
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

        bool hittingFromAbove = collision.GetContact(0).normal.y > 0.5f;
        if (!hittingFromAbove) return;

        if (platform.isNeutral || platform.platformColor == currentColor)
            
        {
            isGrounded = true;
            Jump();
            audioEventDispatcher.PlayAudio(AudioType.Jump);
        }
        else
        {
            Die();
        }
    }
    
    void Die()
    {
        if (music != null)
            music.SetActive(false);
        audioEventDispatcher.PlayAudio(AudioType.Death);
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Dead");

        enabled = false;
        rb.linearVelocity = Vector2.zero;

        int finalScore = FindFirstObjectByType<ScoreManager>()?.CurrentMeters ?? 0;
        OnPlayerDied?.Invoke(finalScore);

        StartCoroutine(FreezeAfterDelay(0.6f));
    }

    System.Collections.IEnumerator FreezeAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0f;
    }


    void OnDisable()
    {
        moveAction?.Disable();
    }

    /// <summary>
    /// Convertit un ColorType en Color Unity.
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
