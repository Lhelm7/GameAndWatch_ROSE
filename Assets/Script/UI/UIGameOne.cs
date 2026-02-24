using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameUIManager : MonoBehaviour
{
    [Header("Lives Display")] public Image[] lifeIcons;

    [Header("Objectives")] public Image objective1Image;
    public Image objective2Image;

    [Header("Score")] public TextMeshProUGUI scoreText;

    [SerializeField] private GameObject GameOver;
    [SerializeField] private AudioEventDispatcher audioEventDispatcher;

    public void UpdateLives(int lives)
    {
        if (lifeIcons == null) return;

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (lifeIcons[i] != null)
            {
                lifeIcons[i].enabled = i < lives;
            }
        }

        if (lives <= 0)
        {
            Die();
        }
    }

    public void UpdateObjectives(Color color1, Color color2)
    {
        if (objective1Image != null)
            objective1Image.color = color1;

        if (objective2Image != null)
            objective2Image.color = color2;
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private void Die()
    {
        if (audioEventDispatcher != null)
            audioEventDispatcher.PlayAudio(AudioType.Death);
        
        SceneManager.LoadScene("GameOver");
        
        Time.timeScale = 0f;
    }
}
