using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public void RetryGame(string sceneName)
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void QuitGame(string sceneName)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
