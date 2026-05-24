using UnityEngine;

public class GameplayPanelToggle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIPanelManager panelManager;

    [Header("Panel Index")]
    [SerializeField] private int panelIndex = 0;

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

        if (panelManager.IsAnimating()) return;

        if (panelManager.IsPanelVisible(panelIndex))
        {
            panelManager.HidePanel(panelIndex);
        }
        else
        {
            panelManager.ShowPanel(panelIndex);
        }
    }
}