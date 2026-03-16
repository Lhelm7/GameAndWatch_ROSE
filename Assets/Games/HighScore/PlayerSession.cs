using UnityEngine;

/// <summary>Singleton persistant qui stocke le nom du joueur pour toute la session.</summary>
public class PlayerSession : MonoBehaviour
{
    public static PlayerSession Instance { get; private set; }

    public string PlayerName { get; private set; } = "Player";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Définit le nom du joueur courant.</summary>
    public void SetPlayerName(string name)
    {
        PlayerName = string.IsNullOrWhiteSpace(name) ? "Player" : name.Trim();
    }
}
