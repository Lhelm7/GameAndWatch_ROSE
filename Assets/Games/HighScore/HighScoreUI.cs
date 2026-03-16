using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>Affiche le top 10 des scores pour un jeu donné.</summary>
public class HighScoreUI : MonoBehaviour
{
    private const string GameChoiceSceneName = "MenuChooseGame";
    private const string MainMenuSceneName   = "MainMenu";

    [Header("Configuration")]
    [SerializeField] private string gameId; // "game1", "game2" ou "game3"

    [Header("UI — 10 lignes TMP_Text")]
    [SerializeField] private TMP_Text[] entryTexts;

    private void Start() => DisplayScores();

    private void DisplayScores()
    {
        List<HighScoreEntry> scores = HighScoreRepository.GetScores(gameId);
        for (int i = 0; i < entryTexts.Length; i++)
        {
            if (entryTexts[i] == null) continue;
            entryTexts[i].text = i < scores.Count
                ? $"{i + 1}.  {scores[i].playerName}  —  {scores[i].score}"
                : $"{i + 1}.  —";
        }
    }

    /// <summary>Retourne au menu de sélection des jeux.</summary>
    public void OnBackToGameChoice()
    {
        SceneManager.LoadScene(GameChoiceSceneName);
    }

    /// <summary>Retourne au menu principal.</summary>
    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}