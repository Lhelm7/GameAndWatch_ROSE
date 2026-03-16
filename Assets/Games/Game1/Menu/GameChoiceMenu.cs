using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameChoiceMenu : MonoBehaviour
{
    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;
    [SerializeField] private Animator animator3;
    private static readonly int Blink = Animator.StringToHash("Blink");

    private void Start()
    {
        
    }

    public void FirstGame(string sceneName)
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void SecondGame(string sceneName)
    {
        SceneManager.LoadScene("2ndGame");
    }
    public void ThirdGame(string sceneName)
    {
        SceneManager.LoadScene("3rdGame");
    }
}
