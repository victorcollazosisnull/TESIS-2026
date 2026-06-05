using System.Collections;
using TMPro;
using UnityEngine;

public class GameFeedbackUI : MonoBehaviour
{
    public static GameFeedbackUI Instance;

    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Config")]
    [SerializeField] private float typingSpeed = 0.03f;
    [SerializeField] private float displayDuration = 2.5f;

    private Coroutine currentRoutine;
    private bool isBusy = false; 

    private void Awake()
    {
        Instance = this;
        speechBubble.SetActive(false);
    }

    public void Show(string message, bool force = false)
    {
        if (isBusy && !force) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        isBusy = true;
        speechBubble.SetActive(true);
        feedbackText.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            feedbackText.text += message[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(displayDuration);

        speechBubble.SetActive(false);
        feedbackText.text = "";
        isBusy = false;
        currentRoutine = null;
    }
}