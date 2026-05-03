using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        SceneTransitionManager.Instance.FadeOutStart();
    }
    public void PlayGame()
    {
        SceneTransitionManager.Instance.LoadScene("Introduction");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
