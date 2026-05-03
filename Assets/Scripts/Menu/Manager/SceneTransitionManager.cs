using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [SerializeField] private CircleFade circleFade;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadScene(string sceneName)
    {
        circleFade.FadeIn(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void FadeOutStart()
    {
        circleFade.FadeOut();
    }
}