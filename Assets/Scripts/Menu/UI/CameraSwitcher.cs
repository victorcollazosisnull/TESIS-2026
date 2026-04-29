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

    [Header("Settings")]
    public float transitionTime = 1.5f;

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
        SetActiveCamera(camMain);
    }

    public void GoToOptions()
    {
        SetActiveCamera(camOptions);
        StartCoroutine(ShowPanelDeferred(panelOptions));
    }

    public void GoToCredits()
    {
        SetActiveCamera(camCredits);
        StartCoroutine(ShowPanelDeferred(panelCredits));
    }

    private IEnumerator ShowPanelDeferred(GameObject panel)
    {
        yield return new WaitForSeconds(transitionTime);
        panel.SetActive(true);
    }
}