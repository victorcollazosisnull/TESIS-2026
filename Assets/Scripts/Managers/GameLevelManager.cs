using System.Collections;
using TMPro;
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private PlateStation plateStation;
    //[SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PauseManager pauseManager;

    [Header("Messages")]
    [SerializeField] private string completeMessage;

    [SerializeField] private float typingSpeed = 0.03f;

    [Header("Scene")]
    [SerializeField] private string nextSceneName = "Menu";

    private bool levelCompleted = false;

    private void Start()
    {
        StartCoroutine(LevelFlow());
    }

    IEnumerator LevelFlow()
    {
        yield return StartCoroutine(WaitForPlateComplete());

        //playerMovement.canControl = false;
        pauseManager.canPause = false;

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