using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class ComponentCollectable : MonoBehaviour
{
    [Header("Data")]
    public ComponentColorData colorData;
    
    [Header("Spawn Info")]
    public int pipeIndex;
    
    [Header("Settings")]
    [SerializeField] private float fallSpeed = 2f;  // ← Valeur par défaut
    [SerializeField] private float autoDestroyTime = 15f;
    
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 0.6f;
    [SerializeField] private float perfectCatchRadius = 0.3f;
    
    private bool isActive = false;
    private bool isCollected = false;
    private float aliveTime = 0f;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private PlayerCollector cachedPlayer;

    // ✅ NOUVELLE MÉTHODE : Définir la vitesse de chute
    public void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
        Debug.Log($"[Component] Fall speed set to {speed:F1}");
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Forcer le scale
        transform.localScale = new Vector3(0.04f, 0.04f, 1f);
        
        // Configuration automatique Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        
        // ← PAS de collider automatique, on gère manuellement
    }

    public void Initialize(int pipe, GameManager manager)
    {
        pipeIndex = pipe;
        gameManager = manager;
        
        // Cache le player une seule fois
        cachedPlayer = FindObjectOfType<PlayerCollector>();
        
        // Appliquer le sprite du ScriptableObject
        if (colorData != null)
        {
            if (colorData.sprite != null)
            {
                spriteRenderer.sprite = colorData.sprite;
            }
            
            spriteRenderer.color = Color.white;
        }
        
        Debug.Log($"[Component] Init - Pipe:{pipeIndex}, Color:{colorData?.colorName}");
    }

    public void Activate()
    {
        isActive = true;
        Debug.Log($"[Component] ACTIVATED - Pipe:{pipeIndex}, Color:{colorData?.colorName}, ExitPosX:{transform.position.x:F2}");
    }

    private void Update()
    {
        if (!isActive || isCollected) return;

        // Tombe
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // ✅ NOUVELLE DÉTECTION : Vérifier la distance à chaque frame
        if (cachedPlayer != null)
        {
            CheckPlayerProximity();
        }

        // Auto-destruction après timeout
        aliveTime += Time.deltaTime;
        if (aliveTime > autoDestroyTime)
        {
            Debug.Log($"[Component] Timeout - Destroying");
            Destroy(gameObject);
            return;
        }
    }

    // ✅ NOUVELLE MÉTHODE : Détection progressive par distance
    private void CheckPlayerProximity()
    {
        Vector2 componentPos = transform.position;
        Vector2 playerPos = cachedPlayer.transform.position;
        
        // Calculer la distance totale (X et Y)
        float distance = Vector2.Distance(componentPos, playerPos);
        
        // Si dans la zone de détection
        if (distance < detectionRadius)
        {
            // Calculer la distance horizontale (X seulement)
            float distanceX = Mathf.Abs(componentPos.x - playerPos.x);
            
            // Vérifier si le component est proche verticalement aussi
            float distanceY = Mathf.Abs(componentPos.y - playerPos.y);
            
            Debug.Log($"[Component] Near player! Distance:{distance:F2}, X:{distanceX:F2}, Y:{distanceY:F2}");
            
            // ✅ CONDITION : Distance totale < radius OU zone parfaite
            if (distance < perfectCatchRadius)
            {
                // Zone parfaite : très proche
                CollectComponent("Perfect!");
            }
            else if (distanceX < 0.4f && distanceY < 0.8f)
            {
                // Zone acceptable : aligné horizontalement et pas trop loin verticalement
                CollectComponent("Good");
            }
        }
    }

    private void CollectComponent(string quality)
    {
        isCollected = true;
        Debug.Log($"[Component] ✅ COLLECTED ({quality})! Color:{colorData?.colorName}");
        
        // Appeler le manager
        gameManager?.OnComponentCollected(colorData);
        
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!isActive) return;
        
        // Zone de détection (jaune)
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Zone parfaite (vert)
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, perfectCatchRadius);
        
        // Point central
        Gizmos.color = isCollected ? Color.green : Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
    
}
