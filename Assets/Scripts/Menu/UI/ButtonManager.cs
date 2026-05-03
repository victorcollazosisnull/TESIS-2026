using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;

    [Header("Button Values Scale")]
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float speed = 10f;

    [Header("Button Sound Data")]
    [SerializeField] private SoundData hoverSound;
    [SerializeField] private SoundData clickSound;

    private Vector3 targetScale;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleFactor;

        if (hoverSound != null)
            AudioManager.Instance.Play(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null)
            AudioManager.Instance.Play(clickSound);
    }
}