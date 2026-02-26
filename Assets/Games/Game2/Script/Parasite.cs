using UnityEngine;

public class Parasite : MonoBehaviour
{
    public ParasiteData data;
    private ParasiteGameManager gameManager;

    public void Init(ParasiteData parasiteData, ParasiteGameManager manager)
    {
        data = parasiteData;
        gameManager = manager;
        GetComponent<SpriteRenderer>().sprite = data.sprite;
    }

    private void OnMouseDown()
    {
        gameManager.OnParasiteTouched(data.colorType);
        Destroy(gameObject);
    }
}