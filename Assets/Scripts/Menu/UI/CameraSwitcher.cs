using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Camaras")]
    public CinemachineCamera camMain; 
    public CinemachineCamera camOptions;
    public CinemachineCamera camCredits;

    private void SetActiveCamera(CinemachineCamera cam)
    {
        camMain.Priority = 0;
        camOptions.Priority = 0;
        camCredits.Priority = 0;

        cam.Priority = 10;
    }

    public void GoToMenu()
    {
        SetActiveCamera(camMain);
    }

    public void GoToTienda()
    {
        SetActiveCamera(camOptions);
    }

    public void GoToMapa()
    {
        SetActiveCamera(camCredits);
    }
}