using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CircleFade : MonoBehaviour
{
    [SerializeField] private Image image;
    private Material mat;

    [SerializeField] private float duration = 1f;

    private void Awake()
    {
        mat = Instantiate(image.material);
        image.material = mat;
    }

    public void FadeIn(System.Action onComplete = null)
    {
        mat.SetFloat("_Radius", 1f);

        DOTween.To(() => 1f, x => mat.SetFloat("_Radius", x), 0f, duration)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void FadeOut(System.Action onComplete = null)
    {
        mat.SetFloat("_Radius", 0f);

        DOTween.To(() => 0f, x => mat.SetFloat("_Radius", x), 1f, duration)
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true)
            .OnComplete(() => onComplete?.Invoke());
    }
}