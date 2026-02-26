using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
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
        
        Debug.Log($"[VictoryManager] Final Score: {finalScore}");
        
        // Afficher le score
        if (scoreText != null)
        {
            scoreText.text = $"Score: {finalScore}";
        }
        
        // Titre
        if (titleText != null)
        {
            titleText.text = "Victory!";
        }
    }
    
    public void OnRetryButton()
    {
        Debug.Log("[VictoryManager] Retry button pressed");
        GameData.Reset();
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void OnMainMenuButton()
    {
        Debug.Log("[VictoryManager] Main Menu button pressed");
        GameData.Reset();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}