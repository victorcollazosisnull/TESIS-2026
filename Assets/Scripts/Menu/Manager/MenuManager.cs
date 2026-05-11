using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private DialogueSO lomoDialogue;
    [SerializeField] private DialogueSO cevicheDialogue;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneTransitionManager.Instance.FadeOutStart();
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