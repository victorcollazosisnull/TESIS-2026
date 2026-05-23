using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class DialogueController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image dialogueImage;

    [Header("Settings")]
    private DialogueSO dialogueSO;

    [SerializeField] private float typingSpeed = 0.08f;

    [SerializeField] private GameObject continueButton;

    private bool isTyping = false;
    private bool skipLine = false;
    [SerializeField] private bool dialogueFinished = false;

    private int currentLine = 0;
    private Coroutine typingCoroutine;

    private void Start()
    {
        SceneTransitionManager.Instance.FadeOutStart();

        dialogueSO = IntroSelectionData.selectedDialogue;

        ShowCurrentImage();

        StartTyping();
    }

    void Update()
    {
        if (dialogueFinished) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleDialogueInput();
        }
    }

    void HandleDialogueInput()
    {
        if (isTyping)
        {
            skipLine = true;
            return;
        }

        currentLine++;

        if (currentLine >= dialogueSO.DialogueLines.Length)
        {
            dialogueFinished = true;
            continueButton.SetActive(true);
            return;
        }

        ShowCurrentImage();

        StartTyping();
    }

    void StartTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        skipLine = false;

        dialogueText.text = "";

        string line = dialogueSO.DialogueLines[currentLine];

        for (int i = 0; i < line.Length; i++)
        {
            if (skipLine)
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += line[i];

            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    void ShowCurrentImage()
    {
        if (dialogueSO.imageChangeIndex == null) return;

        for (int i = 0; i < dialogueSO.imageChangeIndex.Length; i++)
        {
            if (currentLine == dialogueSO.imageChangeIndex[i])
            {
                if (i < dialogueSO.dialogueImages.Length)
                {
                    dialogueImage.sprite = dialogueSO.dialogueImages[i];
                }
            }
        }
    }

    public void SkipIntro()
    {
        ContinueGame();
    }

    public void ContinueGame()
    {
        //Debug.Log(IntroSelectionData.nextScene);
        Debug.Log(SceneTransitionManager.Instance);

        SceneTransitionManager.Instance.LoadScene(IntroSelectionData.nextScene);
    }
}