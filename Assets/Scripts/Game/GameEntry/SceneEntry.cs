using UnityEngine;

public class SceneEntry : MonoBehaviour
{
    private void Start()
    {
        SceneTransitionManager.Instance.FadeOutStart();
    }
}