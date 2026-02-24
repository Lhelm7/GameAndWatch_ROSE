using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI titleText;
    
    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameSceneName = "SampleScene";
    
    private void Start()
    {
        int finalScore = GameData.GetFinalScore();
        
        Debug.Log($"[GameOverManager] Final Score: {finalScore}");
        
        // Afficher le score
        if (scoreText != null)
        {
            scoreText.text = $"Score: {finalScore}";
        }
        
        // Titre
        if (titleText != null)
        {
            titleText.text = "Game Over";
        }
    }
    
    public void OnRetryButton()
    {
        Debug.Log("[GameOverManager] Retry button pressed");
        GameData.Reset();
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void OnMainMenuButton()
    {
        Debug.Log("[GameOverManager] Main Menu button pressed");
        GameData.Reset();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}