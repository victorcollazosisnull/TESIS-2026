using TMPro;
using System.Collections;
using UnityEngine;

public class DialogueController : MonoBehaviour
{   [Header("Components")]
    [SerializeField] private TMP_Text dialogueText;

    [Header("Settings")]
    [SerializeField] private DialogueSO dialogueSO; 
    [SerializeField] private float typingSpeed;
    [SerializeField] private float waitAfterLine;
    private void Reset()
    {
        typingSpeed = 0.08f;
        waitAfterLine = 1.2f;
    }
    void Start()
    {
        StartCoroutine(DialoguesCoroutine());
    }
    private IEnumerator DialoguesCoroutine()
    {
        for (int i = 0; i < dialogueSO.DialogueLines.Length; ++i)
        {
            dialogueText.text = "";

            foreach (char c in dialogueSO.DialogueLines[i])
            {
                dialogueText.text += c;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            yield return new WaitForSecondsRealtime(waitAfterLine); 
        }

    }
}
