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
    [SerializeField] private float fallSpeed = 2f;  
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

    public void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        transform.localScale = new Vector3(0.04f, 0.04f, 1f);
       
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        
    }

    public void Initialize(int pipe, GameManager manager)
    {
        pipeIndex = pipe;
        gameManager = manager;
        
        cachedPlayer = FindObjectOfType<PlayerCollector>();
        
        if (colorData != null)
        {
            if (colorData.sprite != null)
            {
                spriteRenderer.sprite = colorData.sprite;
            }
            
            spriteRenderer.color = Color.white;
        }
        
    }

    public void Activate()
    {
        isActive = true;
    }

    private void Update()
    {
        if (!isActive || isCollected) return;
        
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (cachedPlayer != null)
        {
            CheckPlayerProximity();
        }

        aliveTime += Time.deltaTime;
        if (aliveTime > autoDestroyTime)
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void CheckPlayerProximity()
    {
        Vector2 componentPos = transform.position;
        Vector2 playerPos = cachedPlayer.transform.position;
        
        float distance = Vector2.Distance(componentPos, playerPos);
        
        if (distance < detectionRadius)
        {
            float distanceX = Mathf.Abs(componentPos.x - playerPos.x);
            
            float distanceY = Mathf.Abs(componentPos.y - playerPos.y);
            
            if (distance < perfectCatchRadius)
            {
                CollectComponent("Perfect!");
            }
            else if (distanceX < 0.4f && distanceY < 0.8f)
            {
                CollectComponent("Good");
            }
        }
    }

    private void CollectComponent(string quality)
    {
        isCollected = true;
        
        gameManager?.OnComponentCollected(colorData);
        
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!isActive) return;
        
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, perfectCatchRadius);
        
        Gizmos.color = isCollected ? Color.green : Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
    
}
