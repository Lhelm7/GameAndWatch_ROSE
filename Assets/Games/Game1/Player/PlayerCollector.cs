using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCollector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    
    [Header("Display Slots")]
    [SerializeField] private SpriteRenderer slot1;
    [SerializeField] private SpriteRenderer slot2;
    
    [Header("Audio & Animation")]
    [SerializeField] private AudioEventDispatcher audioEventDispatcher;
    [SerializeField] private Animator animatorR;
    [SerializeField] private Animator animatorL;
    [SerializeField] private Animator animatorwrongR;
    [SerializeField] private Animator animatorwrongL;
    
    private ComponentColorData targetColor1;
    private ComponentColorData targetColor2;
    private bool hasColor1 = false;
    private bool hasColor2 = false;

    private void Awake()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
    
        if (slot1 == null)
        {
            Transform partLeft = transform.Find("PartLeft");
            if (partLeft != null)
            {
                slot1 = partLeft.GetComponent<SpriteRenderer>();
            }
        }
    
        if (slot2 == null)
        {
            Transform partRight = transform.Find("PartRight");
            if (partRight != null)
            {
                slot2 = partRight.GetComponent<SpriteRenderer>();
            }
        }
    
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 1.5f;
    
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        
        if (animatorR == null)
            animatorR = GetComponent<Animator>();
        
        if (animatorL == null)
            animatorL = GetComponent<Animator>();
        
        if (animatorwrongR == null)
            animatorwrongR = GetComponent<Animator>();
        
        if (animatorwrongL == null)
            animatorwrongL = GetComponent<Animator>();
    }

    public int GetCurrentPipe()
    {
        return playerMovement != null ? playerMovement.GetCurrentIndex() : 0;
    }

    public void SetTargetColors(ComponentColorData color1, ComponentColorData color2)
    {
        targetColor1 = color1;
        targetColor2 = color2;
        hasColor1 = false;
        hasColor2 = false;
        
        UpdateDisplay();
    }

    public bool TryCollect(ComponentColorData collectedColor)
    {
        if (collectedColor.colorID == targetColor1.colorID && !hasColor1)
        {
            hasColor1 = true;
           
            UpdateDisplay();
            
            StartCoroutine(PlayCollectFeedbackR());
            
            return true;
        }

        if (collectedColor.colorID == targetColor2.colorID && !hasColor2)
        {
            hasColor2 = true;
           
            UpdateDisplay();
            
            StartCoroutine(PlayCollectFeedbackL());
            
            return true;
        }
        
        StartCoroutine(PlayWrongFeedbackL());
        
        return false;
    }

    private IEnumerator PlayCollectFeedbackR()
    {
        yield return null;
        
        if (animatorR != null)
        {
            animatorR.SetTrigger("SucessL");
        }
        
        if (audioEventDispatcher != null)
        {
            audioEventDispatcher.PlayAudio(AudioType.ObjectMovement);  
        }
    }
    private IEnumerator PlayCollectFeedbackL()
    {
        yield return null;
        
        if (animatorL != null)
        {
            animatorL.SetTrigger("SuccessR");
        }
        
        if (audioEventDispatcher != null)
        {
            audioEventDispatcher.PlayAudio(AudioType.ObjectMovement);  
        }
    }
    
    private IEnumerator PlayWrongFeedbackR()
    {
        yield return null;
        
        if (animatorwrongR != null)
        {
            animatorwrongR.SetTrigger("FailR");
        }
        
        if (audioEventDispatcher != null)
        {
            audioEventDispatcher.PlayAudio(AudioType.Hurt); 
        }
    }
    private IEnumerator PlayWrongFeedbackL()
    {
        yield return null;
        
        if (animatorwrongL != null)
        {
            animatorwrongL.SetTrigger("FailL");
        }
        
        if (audioEventDispatcher != null)
        {
            audioEventDispatcher.PlayAudio(AudioType.Hurt); 
        }
    }
    public bool BothCollected()
    {
       
        return hasColor1 && hasColor2;
    }

    private void UpdateDisplay()
    {
        if (slot1 != null && targetColor1 != null)
        {
            Color c = targetColor1.displayColor;
            float targetAlpha = hasColor1 ? 0.1f : 1f;
            c.a = targetAlpha;
            slot1.color = c;
            
        }
        
        if (slot2 != null && targetColor2 != null)
        {
            Color c = targetColor2.displayColor;
            float targetAlpha = hasColor2 ? 0.1f : 1f;
            c.a = targetAlpha;
            slot2.color = c;
            
        }
    }
}
