using UnityEngine;
using DG.Tweening;

public class UIPanelManager : MonoBehaviour
{
    [System.Serializable]
    public class PanelData
    {
        public RectTransform panel;
        public Vector3 hiddenPosition;
        public Vector3 visiblePosition = Vector3.zero;

        [HideInInspector] public bool isVisible;
        [HideInInspector] public Vector3 originalScale;

        public float duration = 0.4f;
        public Ease ease = Ease.OutBack;

        public bool useScaleAnimation = true;
    }

    [SerializeField] private PanelData[] panels;

    private void Start()
    {
        for (int i = 0; i < panels.Length; ++i)
        {
            panels[i].originalScale = panels[i].panel.localScale;

            panels[i].panel.anchoredPosition = panels[i].hiddenPosition;

            if (panels[i].useScaleAnimation)
            {
                panels[i].panel.localScale = Vector3.zero;
            }
            else
            {
                panels[i].panel.localScale = panels[i].originalScale;
            }

            panels[i].isVisible = false;
            panels[i].panel.gameObject.SetActive(false);
        }
    }

    public void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Length) return;

        PanelData p = panels[index];
        p.isVisible = true;

        p.panel.gameObject.SetActive(true);

        p.panel.DOKill();

        p.panel.DOAnchorPos(p.visiblePosition, p.duration).SetEase(p.ease).SetUpdate(true);
        if (p.useScaleAnimation)
        {
            p.panel.DOScale(p.originalScale, p.duration)
                .SetEase(p.ease)
                .SetUpdate(true);
        }
        else
        {
            p.panel.localScale = p.originalScale;
        }
    }

    public void HidePanel(int index)
    {
        if (index < 0 || index >= panels.Length) return;

        PanelData p = panels[index];
        p.isVisible = false;

        p.panel.DOKill();

        p.panel.DOAnchorPos(p.hiddenPosition, p.duration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                if (!p.isVisible)
                    p.panel.gameObject.SetActive(false);
            });

        if (p.useScaleAnimation)
        {
            p.panel.DOScale(Vector3.zero, p.duration)
                .SetEase(Ease.InBack)
                .SetUpdate(true);
        }
        else
        {
            p.panel.localScale = p.originalScale;
        }
    }
}