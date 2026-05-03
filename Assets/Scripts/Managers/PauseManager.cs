using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UIPanelManager panelManager;
    [SerializeField] private int pausePanelIndex = 0;

    [Header("Player")]
    public bool canPause = true;
    [SerializeField] private PlayerMovement playerMovement;

    private bool isPaused = false;

    private void OnEnable()
    {
        PlayerInputs.pauseInput += TogglePause;
    }

    private void OnDisable()
    {
        PlayerInputs.pauseInput -= TogglePause;
    }

    public void TogglePause()
    {
        if (!canPause) return;

        if (isPaused) return;

        PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;

        panelManager.ShowPanel(pausePanelIndex);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null)
            playerMovement.canControl = false;
    }

    public void ResumeGame()
    {
        isPaused = false;

        panelManager.HidePanel(pausePanelIndex);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovement != null)
            playerMovement.canControl = true;
    }
    public void GoToMenu()
    {
        Time.timeScale = 1f; 

        SceneTransitionManager.Instance.LoadScene("Menu");
    }
}