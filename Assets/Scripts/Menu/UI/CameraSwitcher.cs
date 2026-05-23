using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Camaras")]
    public CinemachineCamera camMain;
    public CinemachineCamera camOptions;
    public CinemachineCamera camCredits;

    [Header("Paneles UI")]
    public GameObject panelOptions;
    public GameObject panelCredits;
    public CanvasGroup mainMenu;

    [Header("Settings")]
    public float transitionTime = 1.5f;

    private bool isTransitioning = false;

    private void SetActiveCamera(CinemachineCamera targetCam)
    {
        camMain.Priority = 0;
        camOptions.Priority = 0;
        camCredits.Priority = 0;

        panelOptions.SetActive(false);
        panelCredits.SetActive(false);

        targetCam.Priority = 10;
    }

    public void GoToMenu()
    {
        if (isTransitioning) return;

        StartCoroutine(TransitionRoutine(camMain, null));
    }

    public void GoToOptions()
    {
        if (isTransitioning) return;

        StartCoroutine(TransitionRoutine(camOptions, panelOptions));
    }

    public void GoToCredits()
    {
        if (isTransitioning) return;

        StartCoroutine(TransitionRoutine(camCredits, panelCredits));
    }

    private IEnumerator TransitionRoutine(CinemachineCamera targetCam, GameObject targetPanel)
    {
        isTransitioning = true;

        mainMenu.interactable = false;
        mainMenu.blocksRaycasts = false;

        SetActiveCamera(targetCam);

        yield return new WaitForSeconds(transitionTime);

        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }

        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;

        isTransitioning = false;
    }
}