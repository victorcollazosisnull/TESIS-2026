using UnityEngine;

public class GameplayPanelToggle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIPanelManager panelManager;

    [Header("Panel Index")]
    [SerializeField] private int panelIndex = 0;

    private bool isOpen = false;

    private void OnEnable()
    {
        PlayerInputs.activatePanelInput += TogglePanel;
    }

    private void OnDisable()
    {
        PlayerInputs.activatePanelInput -= TogglePanel;
    }

    private void TogglePanel()
    {
        if (Time.timeScale == 0f) return;

        isOpen = !isOpen;

        if (isOpen)
        {
            panelManager.ShowPanel(panelIndex);
        }
        else
        {
            panelManager.HidePanel(panelIndex);
        }
    }
}