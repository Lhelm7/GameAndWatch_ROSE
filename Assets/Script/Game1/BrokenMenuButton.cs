using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using TMPro;
using IEnumerator = System.Collections.IEnumerator;

public class BrokenMenuButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private string sceneToLoad = "SampleScene";
    [SerializeField] private TextMeshProUGUI dialogueText;

    private bool firstClickDone = false;
    private bool dialogueFinished = false;

    [SerializeField] private Image menuImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite blinkSprite;

    [SerializeField] private string[] dialogueLines;
    [SerializeField] private float delayBetweenLines = 2f;

    private Sprite originalSprite;

    private static readonly int Blink = Animator.StringToHash("Blink");

    private void Start()
    {
        if (menuImage != null)
            originalSprite = menuImage.sprite;
    }

    public void OnButtonClick()
    {
        // Premier clic
        if (!firstClickDone)
        {
            firstClickDone = true;
            FirstClick();
            return;
        }

        // Deuxième clic seulement si dialogue terminé
        if (dialogueFinished)
        {
            LoadGame();
        }
    }

    private void FirstClick()
    {
        firstClickDone = true;
        if (menuImage != null)
            menuImage.sprite = blinkSprite;

        StartCoroutine(PlayDialogue());

        animator.SetTrigger(Blink);
    }

    private IEnumerator PlayDialogue()
    {
        dialogueText.gameObject.SetActive(true);

        for (int i = 0; i < dialogueLines.Length; i++)
        {
            dialogueText.text = dialogueLines[i];
            yield return new WaitForSeconds(delayBetweenLines);
        }

        dialogueText.gameObject.SetActive(false);

        dialogueFinished = true; 

}
    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
    }
    private void LoadGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}