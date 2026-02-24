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
    
        // Auto-détecter les slots
        if (slot1 == null)
        {
            Transform partLeft = transform.Find("PartLeft");
            if (partLeft != null)
            {
                slot1 = partLeft.GetComponent<SpriteRenderer>();
                Debug.Log($"[PlayerCollector] Auto-assigned slot1 to PartLeft");
            }
        }
    
        if (slot2 == null)
        {
            Transform partRight = transform.Find("PartRight");
            if (partRight != null)
            {
                slot2 = partRight.GetComponent<SpriteRenderer>();
                Debug.Log($"[PlayerCollector] Auto-assigned slot2 to PartRight");
            }
        }
    
        // Configuration collider
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
        
        Debug.Log($"[PlayerCollector] 🎨 SetTargetColors: {color1.colorName}(RGB:{color1.displayColor}) & {color2.colorName}(RGB:{color2.displayColor})");
        Debug.Log($"[PlayerCollector] Flags reset: hasColor1={hasColor1}, hasColor2={hasColor2}");
        
        UpdateDisplay();
        
        Debug.Log($"[PlayerCollector] New targets: {color1.colorName} & {color2.colorName}");
    }

    public bool TryCollect(ComponentColorData collectedColor)
    {
        Debug.Log($"[PlayerCollector] TryCollect called! Collected:{collectedColor.colorName}(ID:{collectedColor.colorID}), Target1:{targetColor1?.colorName}(ID:{targetColor1?.colorID}), Target2:{targetColor2?.colorName}(ID:{targetColor2?.colorID})");
    
        // Comparer par colorID
        if (collectedColor.colorID == targetColor1.colorID && !hasColor1)
        {
            hasColor1 = true;
            Debug.Log($"[PlayerCollector] ✅ Collected Slot1: {collectedColor.colorName}, hasColor1 = true");
            
            UpdateDisplay();
            
            // ✅ JOUER ANIMATION/SON DE MANIÈRE ASYNCHRONE
            StartCoroutine(PlayCollectFeedbackR());
            
            return true;
        }

        if (collectedColor.colorID == targetColor2.colorID && !hasColor2)
        {
            hasColor2 = true;
            Debug.Log($"[PlayerCollector] ✅ Collected Slot2: {collectedColor.colorName}, hasColor2 = true");
            
            UpdateDisplay();
            
            // ✅ JOUER ANIMATION/SON DE MANIÈRE ASYNCHRONE
            StartCoroutine(PlayCollectFeedbackL());
            
            return true;
        }
        
        // ❌ MAUVAISE COULEUR
        Debug.Log($"[PlayerCollector] ❌ Wrong color: {collectedColor.colorName} (ID:{collectedColor.colorID}), Expected: {targetColor1.colorName}(ID:{targetColor1.colorID}) or {targetColor2.colorName}(ID:{targetColor2.colorID})");
        
        // ✅ JOUER ANIMATION/SON D'ERREUR DE MANIÈRE ASYNCHRONE
        StartCoroutine(PlayWrongFeedbackL());
        
        return false;
    }

    // ✅ COROUTINE pour jouer l'animation/son sans bloquer
    private IEnumerator PlayCollectFeedbackR()
    {
        // Attendre la prochaine frame pour ne pas bloquer la détection
        yield return null;
        
        // Jouer l'animation
        if (animatorR != null)
        {
            animatorR.SetTrigger("SucessL");
        }
        
        // Jouer le son
        if (audioEventDispatcher != null)
        {
            audioEventDispatcher.PlayAudio(AudioType.Point);  // Changez en AudioType approprié
        }
    }
    private IEnumerator PlayCollectFeedbackL()
    {
        // Attendre la prochaine frame pour ne pas bloquer la détection
        yield return null;
        
        // Jouer l'animation
        if (animatorL != null)
        {
            animatorL.SetTrigger("SuccessR");
        }
        
        // Jouer le son
        if (audioEventDispatcher != null)
        {
            audioEventDispatcher.PlayAudio(AudioType.Point);  // Changez en AudioType approprié
        }
    }
    
    // ✅ COROUTINE pour jouer l'animation/son d'erreur
    private IEnumerator PlayWrongFeedbackR()
    {
        // Attendre la prochaine frame
        yield return null;
        
        // Jouer l'animation
        if (animatorwrongR != null)
        {
            animatorwrongR.SetTrigger("FailR");
        }
        
        // Jouer le son
        if (audioEventDispatcher != null)
        {
            audioEventDispatcher.PlayAudio(AudioType.Hurt);  // Changez en AudioType approprié
        }
    }
    private IEnumerator PlayWrongFeedbackL()
    {
        // Attendre la prochaine frame
        yield return null;
        
        // Jouer l'animation
        if (animatorwrongL != null)
        {
            animatorwrongL.SetTrigger("FailL");
        }
        
        // Jouer le son
        if (audioEventDispatcher != null)
        {
            audioEventDispatcher.PlayAudio(AudioType.Hurt);  // Changez en AudioType approprié
        }
    }
    public bool BothCollected()
    {
       
        return hasColor1 && hasColor2;
    }

    private void UpdateDisplay()
    {
        // Slot 1
        if (slot1 != null && targetColor1 != null)
        {
            Color c = targetColor1.displayColor;
            float targetAlpha = hasColor1 ? 0.5f : 1f;
            c.a = targetAlpha;
            slot1.color = c;
            
            Debug.Log($"[PlayerCollector] 🎨 Slot1 updated: Color={targetColor1.colorName}, RGB=({c.r:F2},{c.g:F2},{c.b:F2}), Alpha={c.a:F2}, Collected={hasColor1}");
        }
        else
        {
            Debug.LogWarning($"[PlayerCollector] ⚠️ Slot1 update failed! slot1={slot1}, targetColor1={targetColor1}");
        }
    
        // Slot 2
        if (slot2 != null && targetColor2 != null)
        {
            Color c = targetColor2.displayColor;
            float targetAlpha = hasColor2 ? 0.5f : 1f;
            c.a = targetAlpha;
            slot2.color = c;
            
            Debug.Log($"[PlayerCollector] 🎨 Slot2 updated: Color={targetColor2.colorName}, RGB=({c.r:F2},{c.g:F2},{c.b:F2}), Alpha={c.a:F2}, Collected={hasColor2}");
        }
        else
        {
            Debug.LogWarning($"[PlayerCollector] ⚠️ Slot2 update failed! slot2={slot2}, targetColor2={targetColor2}");
        }
    }
}
