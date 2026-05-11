using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{   [Header("Components")]
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image dialogueImage;

    [Header("Settings")]
    private DialogueSO dialogueSO;
    [SerializeField] private float typingSpeed;
    [SerializeField] private float waitAfterLine;
    private bool isSkipping = false;
    private bool isTyping = false;
    [SerializeField] private GameObject continueButton;


    private void Reset()
    {
        typingSpeed = 0.08f;
        waitAfterLine = 1.2f;
    }
    void Start()
    {
        SceneTransitionManager.Instance.FadeOutStart();

        dialogueSO = IntroSelectionData.selectedDialogue;

        StartCoroutine(DialoguesCoroutine());
    }
    private IEnumerator DialoguesCoroutine()
    {
        int currentImageIndex = 0;

        for (int i = 0; i < dialogueSO.DialogueLines.Length; ++i)
        {
            dialogueText.text = "";

            if (dialogueSO.imageChangeIndex != null && currentImageIndex < dialogueSO.imageChangeIndex.Length && i == dialogueSO.imageChangeIndex[currentImageIndex])
            {
                if (dialogueSO.dialogueImages != null &&
                    currentImageIndex < dialogueSO.dialogueImages.Length)
                {
                    dialogueImage.sprite = dialogueSO.dialogueImages[currentImageIndex];
                    currentImageIndex++;
                }
            }

            isTyping = true;

            for (int j = 0; j < dialogueSO.DialogueLines[i].Length; j++)
            {
                if (isSkipping)
                {
                    dialogueText.text = dialogueSO.DialogueLines[i];
                    break;
                }

                dialogueText.text += dialogueSO.DialogueLines[i][j];
                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            isTyping = false;
            isSkipping = false;

            if (!isSkipping)
                yield return new WaitForSecondsRealtime(waitAfterLine);
        }

        continueButton.SetActive(true);
    }
    public void SkipIntro()
    {
        isSkipping = true;
    }
    public void ContinueGame()
    {
        SceneTransitionManager.Instance.LoadScene(IntroSelectionData.nextScene);
    }
}
