using UnityEngine;
using UnityEngine.EventSystems;

public class Parasite : MonoBehaviour, IPointerClickHandler
{
    public ParasiteData data;
    private ParasiteGameManager gameManager;

    /// <summary>Initialise le parasite avec ses données et le gestionnaire de jeu.</summary>
    public void Init(ParasiteData parasiteData, ParasiteGameManager manager)
    {
        data = parasiteData;
        gameManager = manager;

        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = data.sprite;
        sr.sortingLayerName = "forward";
        sr.sortingOrder = 100;

        var col = GetComponent<BoxCollider2D>();
        col.size = sr.sprite != null ? sr.sprite.bounds.size : Vector2.one;

        Debug.LogWarning($">>> PARASITE SPAWNÉ : {data.colorType} | pos={transform.position} | sprite={( sr.sprite != null ? sr.sprite.name : "NULL")} | sortingLayer={sr.sortingLayerName}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameManager.OnParasiteTouched(data.colorType);
        var anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Touch");
            // Détruit après la durée de l'animation
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}