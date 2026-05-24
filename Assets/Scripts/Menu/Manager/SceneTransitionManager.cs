using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [SerializeField] private CircleFade circleFade;

    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (isTransitioning) return;

        isTransitioning = true;

        PauseManager pm = Object.FindFirstObjectByType<PauseManager>();
        if (pm != null)
        {
            pm.canPause = false;
            // También nos aseguramos de que el juego esté en Play por si acaso
            Time.timeScale = 1f;
        }

        circleFade.FadeIn(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void FadeOutStart()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return null;

        circleFade.FadeOut();
    }
}