using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Tutorial Texts")]
    [TextArea]
    [SerializeField] private string[] tutorialMessages;

    [SerializeField] private float typingSpeed = 0.03f;
    [SerializeField] private float delayBetweenMessages = 1.5f;

    [Header("Gameplay")]
    [SerializeField] private PlateStation plateStation;
    private bool tutorialDone = false;

    [Header("UI")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private string nextSceneName = "Game";

    private void Start()
    {
        StartCoroutine(TutorialFlow());
    }

    IEnumerator TutorialFlow()
    {
        playerMovement.canControl = false;

        yield return StartCoroutine(FadeIn());

        for (int i = 0; i < tutorialMessages.Length; i++)
        {
            yield return StartCoroutine(TypeText(tutorialMessages[i]));
            yield return new WaitForSeconds(delayBetweenMessages);
        }

        playerMovement.canControl = true;

        yield return StartCoroutine(WaitForPlateComplete());

        yield return StartCoroutine(TypeText("¡Felicidades! Completaste el tutorial"));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("Prepárate para cocinar..."));
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(nextSceneName);
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
    IEnumerator FadeIn()
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = 1f - (t / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 0f;
    }

    IEnumerator FadeOut()
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = t / fadeDuration;
            yield return null;
        }

        fadePanel.alpha = 1f;
    }
}