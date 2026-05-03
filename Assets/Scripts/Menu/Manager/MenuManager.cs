using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneTransitionManager.Instance.LoadScene("Introduction");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
