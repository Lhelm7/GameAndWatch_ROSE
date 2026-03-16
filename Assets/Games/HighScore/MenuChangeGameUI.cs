using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>Gère la saisie du nom du joueur avant d'accéder à la sélection des jeux.</summary>
public class MenuChangeGameUI : MonoBehaviour
{
    private const string GameChoiceSceneName = "MenuChooseGame"; // adapte au nom réel

    [SerializeField] private TMP_InputField nameInputField;

    private void Start()
    {
        // Pré-remplir avec le nom déjà saisi si le singleton existe
        if (PlayerSession.Instance != null)
            nameInputField.text = PlayerSession.Instance.PlayerName;
    }

    /// <summary>Valide le nom et charge la scène de sélection des jeux.</summary>
    public void OnConfirmName()
    {
        EnsureSessionExists();
        PlayerSession.Instance.SetPlayerName(nameInputField.text);
        SceneManager.LoadScene(GameChoiceSceneName);
    }

    /// <summary>Retourne au menu principal.</summary>
    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void EnsureSessionExists()
    {
        if (PlayerSession.Instance == null)
        {
            GameObject go = new GameObject("PlayerSession");
            go.AddComponent<PlayerSession>();
        }
    }
}