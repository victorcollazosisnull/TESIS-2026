using TMPro;
using UnityEngine;
using System.Collections;

public class GameLevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject speechBubble; 
    [SerializeField] private PlateStation plateStation;
    [SerializeField] private PauseManager pauseManager;

    [Header("Messages")]
    [SerializeField] private string completeMessage;
    [SerializeField] private float typingSpeed = 0.03f;

    [Header("Scene")]
    [SerializeField] private string nextSceneName = "Menu";

    private bool levelCompleted = false;

    private void Start()
    {
        if (speechBubble != null)
            speechBubble.SetActive(false);

        StartCoroutine(LevelFlow());
    }

    IEnumerator LevelFlow()
    {
        yield return StartCoroutine(WaitForPlateComplete());

        pauseManager.canPause = false;

        if (speechBubble != null)
            speechBubble.SetActive(true);

        yield return StartCoroutine(TypeText(completeMessage));
        yield return new WaitForSeconds(2f);

        SceneTransitionManager.Instance.LoadScene(nextSceneName);
    }

    IEnumerator WaitForPlateComplete()
    {
        while (!levelCompleted)
        {
            if (plateStation.GetCurrentCount() >= plateStation.GetRequiredCount())
            {
                levelCompleted = true;
                break;
            }

            yield return null;
        }
    }

    IEnumerator TypeText(string message)
    {
        levelText.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            levelText.text += message[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}