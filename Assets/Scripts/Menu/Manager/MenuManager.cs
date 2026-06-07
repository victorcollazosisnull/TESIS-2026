using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private DialogueSO lomoDialogue;
    [SerializeField] private DialogueSO cevicheDialogue;

    [Header("Level Buttons")]
    [SerializeField] private Button lomoButton;
    [SerializeField] private Button cevicheButton;

    private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneTransitionManager.Instance.FadeOutStart();

        bool tutorialDone = PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 0) == 1;
        lomoButton.interactable = tutorialDone;
        cevicheButton.interactable = tutorialDone;
    }

    public void PlayTutorial()
    {
        SceneTransitionManager.Instance.LoadScene("Tutorial");
    }

    public void PlayLomoSaltado()
    {
        IntroSelectionData.selectedDialogue = lomoDialogue;
        IntroSelectionData.nextScene = "Game_LomoSaltado";
        SceneTransitionManager.Instance.LoadScene("Introduction");
    }

    public void PlayCeviche()
    {
        IntroSelectionData.selectedDialogue = cevicheDialogue;
        IntroSelectionData.nextScene = "Game_Ceviche";
        SceneTransitionManager.Instance.LoadScene("Introduction");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}