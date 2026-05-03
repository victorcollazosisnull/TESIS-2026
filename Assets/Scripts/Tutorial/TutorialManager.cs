using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private PauseManager pauseManager;

    [Header("Tutorial Texts")]
    [TextArea]
    [SerializeField] private string[] tutorialMessages;

    [SerializeField] private float typingSpeed = 0.03f;
    [SerializeField] private float delayBetweenMessages = 1.5f;

    [Header("Gameplay")]
    [SerializeField] private PlateStation plateStation;
    private bool tutorialDone = false;

    [Header("Scene")]
    [SerializeField] private string nextSceneName = "Game";

    private void Start()
    {
        StartCoroutine(TutorialFlow());
    }

    IEnumerator TutorialFlow()
    {
        pauseManager.canPause = false;
        playerMovement.canControl = false;

        SceneTransitionManager.Instance.FadeOutStart();

        yield return new WaitForSeconds(1f); 

        for (int i = 0; i < tutorialMessages.Length; i++)
        {
            yield return StartCoroutine(TypeText(tutorialMessages[i]));
            yield return new WaitForSeconds(delayBetweenMessages);
        }

        playerMovement.canControl = true;
        pauseManager.canPause = true;

        yield return StartCoroutine(WaitForPlateComplete());

        yield return StartCoroutine(TypeText("¡Felicidades! Completaste el tutorial"));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("Prepárate para cocinar..."));
        yield return new WaitForSeconds(1.5f);

        SceneTransitionManager.Instance.LoadScene(nextSceneName);
    }

    IEnumerator TypeText(string message)
    {
        tutorialText.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            tutorialText.text += message[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator WaitForPlateComplete()
    {
        while (!tutorialDone)
        {
            if (plateStation.GetCurrentCount() >= plateStation.GetRequiredCount())
            {
                tutorialDone = true;
                break;
            }

            yield return null;
        }
    }
}